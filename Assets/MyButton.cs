using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input2
{
    public class MyButton : MonoBehaviour, IPointerEventHandler
    {
        public float travelDistance = 0.1f;
        private Color originalColor;

        public void OnPointerEnter(PointerEventData data)
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                originalColor = meshRenderer.material.color;
                meshRenderer.material.color = Color.Lerp(originalColor, Color.white, 0.5f);
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = originalColor;
            }
        }

        public void OnPointerMoved(PointerEventData data) { }

        public void OnPointerPressed(PointerEventData data)
        {
            transform.position = transform.position - transform.forward * travelDistance;
            data.pointer.Locked = true;
        }

        public void OnPointerReleased(PointerEventData data)
        {
            data.pointer.Locked = false;
            transform.position = transform.position + transform.forward * travelDistance;
        }
    }
}