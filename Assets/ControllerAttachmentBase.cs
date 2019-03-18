using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Input2;

namespace Input2
{
    public class ControllerAttachmentBase : MonoBehaviour
    {
        public delegate void TickHandler(ControllerAttachmentBase c);
        public event TickHandler TickEvent;

        public Controller Controller { get; protected set; }

        protected void Tick()
        {
            TickEvent?.Invoke(this);
        }
    }
}