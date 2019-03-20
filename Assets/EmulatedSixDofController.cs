using System.Diagnostics;
using UnityEngine;

namespace Input2
{
    public class EmulatedSixDofController : SixDofController
    {
        public KeyCode toggleKey = KeyCode.LeftShift;
        public KeyCode movementKey = KeyCode.LeftControl;
        public float maxToggleDelay = 0.2f;
        public float mouseSensitivity = 0.2f;
        public float distanceToCamera = 0.4f;

        private Stopwatch toggleTimer = new Stopwatch();

        public EmulatedSixDofController()
        {
            IsPresent = true;
        }

        private bool isManipulating;

        private Vector3 localPosition = new Vector3(0.2f, 0.2f, 1.0f);
        private Quaternion localRotation = Quaternion.identity;

        public override void Tick()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                toggleTimer.Start();
            }

            if (toggleTimer.IsRunning && toggleTimer.ElapsedMilliseconds > maxToggleDelay * 1000.0f)
            {
                isManipulating = true;
                toggleTimer.Reset();
            }

            if (Input.GetKeyUp(toggleKey))
            {
                if (isManipulating)
                {
                    isManipulating = false;
                }
                else
                { 
                    IsPresent = !IsPresent;
                    toggleTimer.Reset();
                }
            }

            if (isManipulating)
            {
                if (Input.GetKey(movementKey))
                {
                    float x = Input.GetAxis("Mouse X");
                    float y = Input.GetAxis("Mouse Y");
                    localPosition += new Vector3(x * mouseSensitivity, y * mouseSensitivity, 0);
                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    localRotation = Quaternion.LookRotation(ray.direction);
                }

                PrimaryButton = Input.GetMouseButton(0);
            }

            Rotation = localRotation * Camera.main.transform.rotation;
            Position = Camera.main.transform.TransformPoint(localPosition);

            base.Tick();
        }
    }
}