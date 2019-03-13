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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        transform.SetPositionAndRotation(ray.origin, Quaternion.LookRotation(ray.direction));

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
