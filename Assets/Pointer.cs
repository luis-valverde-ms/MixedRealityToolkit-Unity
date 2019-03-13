using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// May make sense to split this in several interfaces (e.g. one just for pointer moved). 
// Keeping it simple here
interface IPointerEventHandler : IEventSystemHandler
{
    void OnPointerEnter(PointerEventData data);
    void OnPointerPressed(PointerEventData data);
    void OnPointerReleased(PointerEventData data);
    void OnPointerExit(PointerEventData data);
    //void OnPointerMoved(PointerEventData data);
}

public class PointerEventData : BaseEventData
{
    public Pointer pointer;

    public PointerEventData(Pointer pointer) : base(EventSystem.current)
    {
        this.pointer = pointer;
    }
}

// TODO Sort out execution order: move pointer and cursor via events
// TODO Implement pointer locking and setting the target pose
// TODO Expose target pose via a UnityEvent?
public class Pointer : MonoBehaviour
{
    public float maxDistance = 10.0f;
    bool debugDisplay = true;

    private Pose targetPose = Pose.identity;
    public Pose TargetPose { get { return targetPose; } }

    public bool Locked { get; set; }

    private GameObject currentTarget = null;
    private bool isPressed = false;
    private PointerEventData eventData;

    void Start()
    {
        eventData = new PointerEventData(this);
    }

    void Update()
    {
        if (Locked)
        {
            Vector3 dir = transform.forward * 0.1f;
            Debug.DrawRay(transform.position, dir, Color.green);
            dir = TargetPose.forward * 0.1f;
            Debug.DrawRay(TargetPose.position, dir, Color.red);
        }
        else
        {
            Raycast();
        }
    }

    private void Raycast()
    { 
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
        Debug.Assert(!isPressed);
        isPressed = true;

        if (currentTarget)
        {
            ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerPressedEventFunction);
        }
    }

    public void Released()
    {
        Debug.Assert(isPressed);
        isPressed = false;

        if (currentTarget)
        {
            ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerReleasedEventFunction);
        }
    }


    //
    // Pointer event functions

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

    private static readonly ExecuteEvents.EventFunction<IPointerEventHandler> onPointerPressedEventFunction =
        delegate (IPointerEventHandler handler, BaseEventData eventData)
        {
            var casted = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            handler.OnPointerPressed(casted);
        };

    private static readonly ExecuteEvents.EventFunction<IPointerEventHandler> onPointerReleasedEventFunction =
        delegate (IPointerEventHandler handler, BaseEventData eventData)
        {
            var casted = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            handler.OnPointerReleased(casted);
        };
}
