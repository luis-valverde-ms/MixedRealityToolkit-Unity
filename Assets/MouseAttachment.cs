using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Input2;

namespace Input2
{
    public class MouseAttachment : ControllerAttachment
    {
        private void Awake()
        {
            var inputManager = Input2.InputManager.Instance;
            Debug.Assert(inputManager != null);

            var mouse = inputManager.GetOrCreateController<MouseController>();
            mouse.TickEvent += Tick;

            gameObject.SetActive(mouse.State.present);
        }

        private void OnDestroy()
        {
            var inputManager = Input2.InputManager.Instance;
            if (inputManager)
            {
                var mouse = inputManager.GetOrCreateController<MouseController>();
                mouse.TickEvent -= Tick;
            }
        }

        private void Tick(MouseController mouse)
        {
            var mouseState = mouse.State;
            gameObject.SetActive(mouseState.present);

            if (mouseState.present)
            {
                Ray ray = Camera.main.ScreenPointToRay(mouseState.position);
                transform.SetPositionAndRotation(ray.origin, Quaternion.LookRotation(ray.direction));
                PrimaryButton = mouseState.leftButton;
                base.Tick();
            }
        }
    }
}