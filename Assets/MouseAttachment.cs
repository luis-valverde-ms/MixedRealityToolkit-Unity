using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseAttachment : MonoBehaviour
{
    public UnityEvent primaryButtonPressed;
    public UnityEvent primaryButtonReleased;

    private void Start()
    {
        if (primaryButtonPressed == null)
        {
            primaryButtonPressed = new UnityEvent();
        }

        if (primaryButtonReleased == null)
        {
            primaryButtonReleased = new UnityEvent();
        }
    }

    void Update()
    {
        var camera = GetComponentInParent<Camera>();
        if (camera != null)
        {
            // Set transform relative to camera
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            transform.localPosition = camera.transform.InverseTransformPoint(ray.origin);
            transform.localRotation = Quaternion.FromToRotation(Vector3.forward, ray.direction);

            // Button events
            if (Input.GetMouseButtonDown(0))
            {
                primaryButtonPressed.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                primaryButtonReleased.Invoke();
            }
        }
    }
}
