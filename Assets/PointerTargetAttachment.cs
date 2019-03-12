using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Change this to be a Pose Attachment instead that can be subscribed to a UnityEvent that produces a Pose?
public class PointerTargetAttachment : MonoBehaviour
{
    void Update()
    {
        var pointer = GetComponentInParent<Pointer>();
        if (pointer != null)
        {
            transform.SetPositionAndRotation(pointer.TargetPose.position, pointer.TargetPose.rotation);
        }
    }
}
