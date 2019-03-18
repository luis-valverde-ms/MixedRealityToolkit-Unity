using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Input2;

namespace Input2
{
    public class ControllerAttachment<T> : ControllerAttachmentBase where T : Controller, new()
    {
        private void Awake()
        {
            var inputManager = Input2.InputManager.Instance;
            Debug.Assert(inputManager != null);

            Controller = inputManager.GetOrCreateController<T>();
            Controller.TickEvent += Tick;

            gameObject.SetActive(Controller.IsPresent);
        }

        private void OnDestroy()
        {
            if (Controller != null)
            {
                Controller.TickEvent -= Tick;
            }
        }

        private void Tick(Controller notUsed)
        {
            gameObject.SetActive(Controller.IsPresent);

            if (Controller.IsPresent)
            {
                transform.SetPositionAndRotation(Controller.Position, Controller.Rotation);
                base.Tick();
            }
        }
    }
}