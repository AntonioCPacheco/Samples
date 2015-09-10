﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;

namespace CameraRengerTarget
{
    public class SecurityCameraBehavior : Behavior
    {
        private const double Period = 16.0;
        private const double StartPeriod = 4;
        private const double EaseInPeriod = 8;
        private const double EndPeriod = 12;
        private const double EaseOutPeriod = 16;

        private Vector3 startLookAt;
        private Vector3 endLookAt;
        private double timeAcumulator;

        private static CubicEase easing = new CubicEase() { EasingMode = EasingMode.EaseInOut };

        [RequiredComponent]
        public Camera3D Camera;

        public SecurityCameraBehavior(Vector3 startLookAt, Vector3 endLookAt, TimeSpan timeOffset)
        {
            this.startLookAt = startLookAt;
            this.endLookAt = endLookAt;
            this.timeAcumulator = timeOffset.TotalSeconds;
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.timeAcumulator += gameTime.TotalSeconds;

            double phase = this.timeAcumulator % Period;

            if (phase < StartPeriod)
            {
                this.Camera.Transform.LookAt(this.startLookAt);
            }
            else if (phase < EaseInPeriod)
            {
                float lerp = (float)((phase - StartPeriod) / StartPeriod);
                this.Camera.Transform.LookAt(Vector3.Lerp(this.startLookAt, this.endLookAt, lerp));
            }
            else if (phase < EndPeriod)
            {
                this.Camera.Transform.LookAt(this.endLookAt);
            }
            else if (phase < EaseOutPeriod)
            {
                float lerp = (float)((phase - EndPeriod) / StartPeriod);
                lerp = (float)easing.Ease(lerp);
                this.Camera.Transform.LookAt(Vector3.Lerp(this.endLookAt, this.startLookAt, lerp));
            }
        }
    }
}
