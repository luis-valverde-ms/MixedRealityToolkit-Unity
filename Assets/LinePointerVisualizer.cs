using Input2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Pointer))]
public class LinePointerVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Pointer pointer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Assert(lineRenderer != null);
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = .01f;
        lineRenderer.endWidth = .01f;

        pointer = GetComponent<Pointer>();
        Debug.Assert(pointer != null);
    }

    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, pointer.TargetPose.position);
    }
}
