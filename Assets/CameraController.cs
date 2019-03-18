using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float linearSpeed = 10.0f;
    public float mouseSensitivity = 10.0f;
    public float axisSensitivity = 2.0f;

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

        {
            position += transform.right * Input.GetAxis("Horizontal") * linearSpeed * Time.deltaTime;
            position += transform.forward * Input.GetAxis("Vertical") * linearSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Assert(Cursor.lockState == CursorLockMode.None);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector3 forward = transform.forward;

        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            forward = Quaternion.Euler(-v * mouseSensitivity, h * mouseSensitivity, 0) * forward;
        }

        {
            float h = Input.GetAxis("AXIS_4");
            float v = Input.GetAxis("AXIS_5");
            forward = Quaternion.Euler(v * axisSensitivity, h * axisSensitivity, 0) * forward;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Assert(Cursor.lockState == CursorLockMode.Locked);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        transform.SetPositionAndRotation(position, Quaternion.LookRotation(forward));
    }
}
