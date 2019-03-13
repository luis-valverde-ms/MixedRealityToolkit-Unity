using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float linearSpeed = 10.0f;
    public float mouseSensitivity = 10.0f;

    void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            position += transform.forward * linearSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            position -= transform.forward * linearSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            position -= transform.right * linearSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            position += transform.right * linearSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Assert(Cursor.lockState == CursorLockMode.None);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Quaternion rotation = transform.rotation;

        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            rotation = Quaternion.Euler(0, h * mouseSensitivity, 0) * rotation;
            float v = Input.GetAxis("Mouse Y");
            rotation *= Quaternion.Euler(-v * mouseSensitivity, 0, 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Assert(Cursor.lockState == CursorLockMode.Locked);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        transform.SetPositionAndRotation(position, rotation);
    }
}
