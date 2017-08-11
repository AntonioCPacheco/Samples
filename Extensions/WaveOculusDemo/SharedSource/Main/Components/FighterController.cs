﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Sound;
using WaveOculusDemoProject.Audio;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// This class control a space fighter
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class FighterController : Behavior
    {
        private List<Vector3> trailPositions;
        private List<TrailSetting> trails;
        private List<ProjectileEmitter> guns;
        private SoundType engineAudio;
        private SoundType shootAudio;

        [RequiredComponent]
        private Transform3D transform3D;

        [RequiredComponent]
        private TrailManager trailManager;

        [RequiredComponent]
        private AnimatedParamBehavior laserTrigger;

        private SoundEmitter3D engineSoundEmitter;
        private SoundEmitter3D gunsSoundEmitter;

        private bool lastVisible;
        private SoundInstance engineInstance;
        private int nShoots = 0;

        private SoundManager soundManager;

        [DataMember]
        public FighterState State { get; set; }
        
        protected override void DefaultValues()
        {
            base.DefaultValues();

        }

        /// <summary>
        /// Initializes the fighter controller
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Init engine trails
            this.trails = new List<TrailSetting>();

            foreach(var trailEntity in this.Owner.FindChildrenByTag("Trail"))
            {
                Transform3D trailTransform = trailEntity.FindComponent<Transform3D>();
                TrailSetting trail = this.trailManager.GetFreeTrail(trailTransform);

                trail.Thickness = 1;
                trail.ExpirationTime = 1f;
                trail.TimeStep = 0.02;
            }

            // Register fighter in screenplay manager
            var screenplay = this.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();
            screenplay.RegisterFighter(new FighterSetting()
                {
                    State = this.State,
                    Transform = transform3D
                });

            this.soundManager = this.EntityManager.Find("SoundManager").FindComponent<SoundManager>();

            // Gets all fighter guns 
            this.guns = new List<ProjectileEmitter>();
            var gunEntities = this.Owner.ChildEntities.Where(e => e.Tag == "Gun");
            foreach (var gunEntity in gunEntities)
            {
                var projectileEmitter = gunEntity.FindComponent<ProjectileEmitter>();
                this.guns.Add(projectileEmitter);

                projectileEmitter.OnShoot += projectileEmitter_OnShoot;
            }

            this.laserTrigger.OnActionChange += laserTrigger_OnActionChange;


            // Load sound emitters
            var engineSoundEntity = this.Owner.FindChild("engineSoundEmitter");
            if (engineSoundEntity == null)
            {
                engineSoundEntity = this.Owner;
            }

            this.engineSoundEmitter = engineSoundEntity.FindComponent<SoundEmitter3D>();

            var gunsSoundEntity = this.Owner.FindChild("gunsSoundEmitter");
            if (gunsSoundEntity == null)
            {
                gunsSoundEntity = this.Owner;
            }

            this.gunsSoundEmitter = gunsSoundEntity.FindComponent<SoundEmitter3D>(); 

        }

        /// <summary>
        /// A projectile has been emitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectileEmitter_OnShoot(object sender, EventArgs e)
        {
            if (nShoots % 2 == 0)
            {
                // Play shoot sound 
                this.gunsSoundEmitter.SoundPath = this.soundManager.GetSoundPath(SoundType.Shoot);
                this.gunsSoundEmitter.Play();
            }

            nShoots++;
        }

        /// <summary>
        /// The laser trigger value has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void laserTrigger_OnActionChange(object sender, bool e)
        {
            if (e)
            {
                foreach (var emitter in this.guns)
                {
                    emitter.StarFiring();
                }
            }
            else
            {
                foreach (var emitter in this.guns)
                {
                    emitter.StopFiring();
                }
            }
        }

        /// <summary>
        /// Update visibility of the fighter
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.lastVisible != this.Owner.IsVisible)
             {
                if (this.Owner.IsVisible)
                {  
                    // TODO: this.engineInstance = this.engineSoundEmitter.Play(this.engineAudio, 1, true);
                }
                else if (this.engineInstance != null)
                {
                    this.engineInstance.Stop();
                }

                this.lastVisible = this.Owner.IsVisible;
            }
        }
    }
}
