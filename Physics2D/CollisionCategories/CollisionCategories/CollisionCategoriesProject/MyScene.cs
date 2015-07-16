// Copyright (C) 2014 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
#endregion

namespace CollisionCategoriesProject
{
    public class MyScene : Scene
    {
        // Consts
        private const string CRATEA_TEXTURE = "Content/CrateA.wpk";
        private const string CRATEB_TEXTURE = "Content/CrateB.wpk";
        private const string CIRCLE_TEXTURE = "Content/CircleSprite.wpk";
        private const string GROUND_TEXTURE = "Content/GroundSprite.wpk";

        // Box Instance count
        private long instances = 0;

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all 
        /// <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            // Create a 2D camera
            var camera2D = new FixedCamera2D("Camera2D")
            {
                BackgroundColor = Color.CornflowerBlue
            }; 
            EntityManager.Add(camera2D);

            // Create Border limits with "Category.All" CollisionGroup (colliding bodies no matter the body collision category)
            this.CreateClosures();


            // Create other collision group. Category 3
            for (int i = 0; i < 5; i++)
            {
                EntityManager.Add(this.CreateCircle(200 + i * 100, 200, CIRCLE_TEXTURE, Physic2DCategory.Cat3));
            }

            // Create other collision group. Category 2
            for (int i = 0; i < 10; i++)
            {
                EntityManager.Add(this.CreateCrate(200 + i * 50, 200, CRATEB_TEXTURE, Physic2DCategory.Cat2));
            }

            // Create a Collision Group. Category 1
            for (int i = 0; i < 10; i++)
            {
                EntityManager.Add(this.CreateCrate(200 + i * 50, 300, CRATEA_TEXTURE, Physic2DCategory.Cat1));
            }

            // Mouse drag controller
            this.AddSceneBehavior(new MouseBehavior(), SceneBehavior.Order.PostUpdate);
        }

        /// <summary>
        /// Allows to perform custom code when this instance is started.
        /// </summary>
        /// <remarks>
        /// This base method perfoms a layout pass.
        /// </remarks>
        protected override void Start()
        {
            base.Start();

            // This method is called after the CreateScene and Initialize methods and before the first Update.
        }

        /// <summary>
        /// Creates a Physic Crate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Entity CreateCrate(float x, float y, string texture, Physic2DCategory category)
        {
            Entity box = new Entity("Crate" + this.instances++)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite(texture))
                .AddComponent(new JointMap2D())
                .AddComponent(new RigidBody2D() { PhysicBodyType = PhysicBodyType.Dynamic, CollisionCategories = category, CollidesWith = category })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            return box;
        }

        /// <summary>
        /// Creates a Physic Circle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Entity CreateCircle(float x, float y, string texture, Physic2DCategory category)
        {
            Entity box = new Entity("Circle" + this.instances++)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center })
                .AddComponent(new CircleCollider())
                .AddComponent(new Sprite(texture))
                .AddComponent(new JointMap2D())
                .AddComponent(new RigidBody2D() { PhysicBodyType = PhysicBodyType.Dynamic, CollisionCategories = category })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            return box;
        }

        /// <summary>
        /// Creates Physic Borders
        /// </summary>
        private void CreateClosures()
        {
            EntityManager.Add(this.CreateGround("top1", 150, -10, 0));
            EntityManager.Add(this.CreateGround("top2", 650, -10, 0));

            EntityManager.Add(this.CreateGround("bottom1", 150, 610, 0));
            EntityManager.Add(this.CreateGround("bottom2", 650, 610, 0));

            EntityManager.Add(this.CreateGround("left", 0, 300, MathHelper.ToRadians(90)));
            EntityManager.Add(this.CreateGround("right", 800, 300, MathHelper.ToRadians(90)));
        }

        /// <summary>
        /// Creates a Physic Ground.
        /// </summary>
        /// <param name="name">Entity Name.</param>
        /// <param name="x">X Position.</param>
        /// <param name="y">Y Position.</param>
        /// <param name="angle">Ground Angle.</param>
        /// <returns>Ground Entity.</returns>
        private Entity CreateGround(string name, float x, float y, float angle)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, Origin = Vector2.Center, Rotation = angle })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite(GROUND_TEXTURE))
                .AddComponent(new RigidBody2D() { PhysicBodyType = PhysicBodyType.Kinematic, Friction = 1, CollisionCategories = Physic2DCategory.All })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));

            return sprite;
        }
    }
}
