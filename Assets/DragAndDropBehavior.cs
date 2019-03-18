using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input2
{

    public class DragAndDropBehavior : MonoBehaviour, IPointerEventHandler
    {
        public void OnPointerEnter(PointerEventData data) { }

        public void OnPointerExit(PointerEventData data) { }

        private bool isBeingDragged = false;
        private Vector3 posInPointer;
        private Vector3 targetPosInPointer;

        public void OnPointerPressed(PointerEventData data)
        {
            isBeingDragged = true;

            Pointer p = data.pointer;
            p.Locked = true;

            posInPointer = p.transform.InverseTransformPoint(transform.position);
            targetPosInPointer = p.transform.InverseTransformPoint(p.TargetPose.position);
        }

        public void OnPointerMoved(PointerEventData data)
        {
            if (isBeingDragged)
            {
                Pointer p = data.pointer;
                transform.position = p.transform.TransformPoint(posInPointer);

                Pose newTargetPose = p.TargetPose;
                newTargetPose.position = p.transform.TransformPoint(targetPosInPointer);
                p.TargetPose = newTargetPose;
            }
        }

        public void OnPointerReleased(PointerEventData data)
        {
            data.pointer.Locked = false;
            isBeingDragged = false;
        }
    }
}