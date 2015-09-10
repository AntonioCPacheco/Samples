﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace LensFlare
{
    /// <summary>
    /// Orbiting Behavior
    /// </summary>
    public class OrbitBehavior : Behavior
    {
        [RequiredComponent]
        public Transform3D Transform { get; set; }

        /// <summary>
        /// The orbit center
        /// </summary>
        private Vector3 orbitCenter;

        /// <summary>
        /// The orbit time in seconds
        /// </summary>
        private float yearTime;

        /// <summary>
        /// The day time in seconds
        /// </summary>
        private float dayTime;

        /// <summary>
        /// The angle
        /// </summary>
        private float orbitAngle;

        /// <summary>
        /// The rotation angle
        /// </summary>
        private float rotationAngle;

        /// <summary>
        /// The angle speed
        /// </summary>
        private float orbitSpeed;

        /// <summary>
        /// The rotation speed
        /// </summary>
        private float rotationSpeed;

        /// <summary>
        /// The radius
        /// </summary>
        private float radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitBehavior" /> class.
        /// </summary>
        /// <param name="dayTime">The day time.</param>
        /// <param name="orbitCenter">The orbit center.</param>
        /// <param name="yearTime">The year speed.</param>
        public OrbitBehavior(float dayTime, Vector3 orbitCenter, float yearTime)
        {
            this.orbitCenter = orbitCenter;
            this.yearTime = yearTime;
            this.orbitAngle = 0;
            this.rotationAngle = (float)Math.PI;

            if (yearTime > 0)
            {
                this.orbitSpeed = (2 * (float)Math.PI) / yearTime;
            }

            if (dayTime > 0)
            {
                this.rotationSpeed = (2 * (float)Math.PI) / dayTime;
            }
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.radius = (this.Transform.Position - this.orbitCenter).Length();
        }

        /// <summary>
        /// Allows this instance to execute custom logic during its <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if the <see cref="T:WaveEngine.Framework.Component" />, or the <see cref="T:WaveEngine.Framework.Entity" />
        /// owning it are not <c>Active</c>.
        /// </remarks>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.orbitSpeed > 0)
            {
                this.orbitAngle = this.orbitAngle + ((float)gameTime.TotalSeconds * this.orbitSpeed);                
            }

            Vector3 auxPosition = Vector3.Zero;
            auxPosition.X = this.radius * ((float)Math.Sin(this.orbitAngle));
            auxPosition.Z = this.radius * ((float)Math.Cos(this.orbitAngle));
            this.Transform.Position = auxPosition;

            if (this.rotationSpeed > 0)
            {
                this.rotationAngle = this.rotationAngle + ((float)gameTime.TotalSeconds * this.rotationSpeed);                
            }

            Vector3 auxRotation = this.Transform.Rotation;
            auxRotation.Y = this.rotationAngle;
            this.Transform.Rotation = auxRotation;
        }
    }
}
