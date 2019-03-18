using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input2
{
    public class PointerTargetAttachment : MonoBehaviour
    {
        public Pointer pointer;

        private void OnEnable()
        {
            if (pointer != null)
            {
                pointer.TickEvent += Tick;
            }
        }

        private void OnDisable()
        {
            if (pointer != null)
            {
                pointer.TickEvent -= Tick;
            }
        }

        public void Tick(Pointer pointer)
        {
            transform.SetPositionAndRotation(pointer.TargetPose.position, pointer.TargetPose.rotation);
        }
    }
}
