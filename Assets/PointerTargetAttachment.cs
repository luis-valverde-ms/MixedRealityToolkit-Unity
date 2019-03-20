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
                pointer.TargetPoseChangedEvent += OnTargetPoseChanged;
            }
        }

        private void OnDisable()
        {
            if (pointer != null)
            {
                pointer.TargetPoseChangedEvent -= OnTargetPoseChanged;
            }
        }

        public void OnTargetPoseChanged(Pose targetPose)
        {
            transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
        }
    }
}
