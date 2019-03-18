using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input2
{
    public class MouseController : Controller
    {
        public MouseController()
        {
            IsPresent = true;
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                IsPresent = !IsPresent;
            }

            if (IsPresent)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Position = ray.origin;
                Rotation = Quaternion.LookRotation(ray.direction);
                PrimaryButton = Input.GetMouseButton(0);
            }

            base.Tick();
        }
    }
}