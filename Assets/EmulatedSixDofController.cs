using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Input2
{
    public class EmulatedSixDofController : SixDofController
    {
        public KeyCode toggleKey = KeyCode.LeftShift;
        public KeyCode rotationKey = KeyCode.LeftControl;
        public float maxToggleDelay = 0.2f;
        public float mouseSensitivity = 10.0f;
        public float distanceToCamera = 0.4f;

        private Stopwatch toggleTimer = new Stopwatch();

        public EmulatedSixDofController()
        {
            IsPresent = true;
        }

        private bool isManipulating;

        public override void Tick()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                toggleTimer.Start();
            }

            if (toggleTimer.IsRunning && toggleTimer.ElapsedMilliseconds > maxToggleDelay * 1000.0f)
            {
                isManipulating = true;
            }

            if (Input.GetKeyUp(toggleKey))
            {
                if (!isManipulating)
                {
                    IsPresent = !IsPresent;
                }
                isManipulating = false;
                toggleTimer.Reset();
            }

            if (isManipulating)
            {
                if (Input.GetKey(rotationKey))
                {
                    float h = Input.GetAxis("Mouse X");
                    float v = Input.GetAxis("Mouse Y");
                    Rotation = Quaternion.Euler(-v * mouseSensitivity, h * mouseSensitivity, 0) * Rotation;
                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Position = ray.GetPoint(distanceToCamera);
                }
            }

            base.Tick();
        }
    }
}