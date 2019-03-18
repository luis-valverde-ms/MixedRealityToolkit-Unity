using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input2
{
    public class MouseController : Controller
    {
        public delegate void TickHandler(MouseController c);
        public event TickHandler TickEvent;

        public struct MouseState
        {
            // Current mouse position in pixel coordinates
            public Vector3 position;
            public bool present;
            public bool leftButton;
            public bool rightButton;
        }

        private MouseState state;
        public MouseState State { get { return state; } }

        private bool present = true;

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                present = !present;
            }

            state.present = present; // Input.mousePresent;

            if (state.present)
            {
                state.position = Input.mousePosition;
                state.leftButton = Input.GetMouseButton(0);
                state.rightButton = Input.GetMouseButton(1);
            }

            TickEvent.Invoke(this);
        }
    }
}