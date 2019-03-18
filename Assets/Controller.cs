using UnityEngine;

namespace Input2
{
    public abstract class Controller
    {
        public bool IsPresent { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        public bool PrimaryButton { get; protected set; }

        public delegate void TickHandler(Controller c);
        public event TickHandler TickEvent;

        public virtual void Tick()
        {
            TickEvent?.Invoke(this);
        }
    }
}
