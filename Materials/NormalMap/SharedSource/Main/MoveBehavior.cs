﻿// Copyright (C) 2012-2015 Weekend Game Studio
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace NormalMap
{
    [DataContract]
    public class MoveBehavior : Behavior
    {
        [RequiredComponent]
        public Transform3D transform3D;

        [DataMember]
        public float Speed { get; set; }

        public MoveBehavior()
            : base("MoveBehavior")
        {
            Speed = 10;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var keyboard = WaveServices.Input.KeyboardState;

            Vector3 auxPosition = this.transform3D.Position;
            if (keyboard.Left == ButtonState.Pressed)
            {
                auxPosition.X -= Speed;
            }

            if (keyboard.Right == ButtonState.Pressed)
            {
                auxPosition.X += Speed;
            }

            if (keyboard.O == ButtonState.Pressed)
            {
                auxPosition.Z -= Speed;
            }

            if (keyboard.P == ButtonState.Pressed)
            {
                auxPosition.Z += Speed;
            }

            if (keyboard.Up == ButtonState.Pressed)
            {
                auxPosition.Y += Speed;
            }

            if (keyboard.Down == ButtonState.Pressed)
            {
                auxPosition.Y -= Speed;
            }

            this.transform3D.Position = auxPosition;
        }
    }
}
