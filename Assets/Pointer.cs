using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

interface IPointerEventHandler : IEventSystemHandler
{
    void OnPointerEnter(PointerEventData data);
    void OnPointerPressed(PointerEventData data);
    void OnPointerReleased(PointerEventData data);
    void OnPointerExit(PointerEventData data);
}

public class PointerEventData : BaseEventData
{
    public PointerEventData(EventSystem eventSystem) : base(eventSystem) { }
}

// TODO Expose target pose via a UnityEvent
public class Pointer : MonoBehaviour
{
    public float maxDistance = 10.0f;
    bool debugDisplay = true;

    private Pose targetPose = Pose.identity;
    public Pose TargetPose { get { return targetPose; } }

    private GameObject currentTarget = null;
    
    private static readonly ExecuteEvents.EventFunction<IPointerEventHandler> onPointerEnterEventFunction =
        delegate (IPointerEventHandler handler, BaseEventData eventData)
        {
            var casted = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            handler.OnPointerEnter(casted);
        };

    private static readonly ExecuteEvents.EventFunction<IPointerEventHandler> onPointerExitEventFunction =
        delegate (IPointerEventHandler handler, BaseEventData eventData)
        {
            var casted = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            handler.OnPointerExit(casted);
        };

    void Update()
    {
        var eventData = new PointerEventData(EventSystem.current);
        Vector3 direction = transform.rotation * Vector3.forward;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            if (hitInfo.collider.gameObject != currentTarget)
            {
                if (currentTarget != null)
                {
                    ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerExitEventFunction);
                }
                currentTarget = hitInfo.collider.gameObject;
                ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerEnterEventFunction);
            }

            targetPose.position = hitInfo.point;
            targetPose.rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
        }
        else
        {
            if (currentTarget != null)
            {
                ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerExitEventFunction);
                currentTarget = null;
            }

            // Fixe pose rotation
            targetPose.position = transform.position + direction * maxDistance;
            targetPose.rotation = Quaternion.LookRotation(-direction);
        }

        if (debugDisplay)
        {
            Debug.DrawLine(transform.position, targetPose.position);
        }
    }

    public void Pressed()
    {
        Debug.Log("Pointer pressed");
    }

    public void Released()
    {
        Debug.Log("Pointer released");
    }
}
