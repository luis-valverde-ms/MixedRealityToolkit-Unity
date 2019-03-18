using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Input2;

namespace Input2
{
    public abstract class ControllerAttachment : MonoBehaviour
    {
        public delegate void TickHandler(ControllerAttachment c);
        public event TickHandler TickEvent;

        public bool PrimaryButton { get; protected set; }

        protected void Tick()
        {
            TickEvent.Invoke(this);
        }
    }
}