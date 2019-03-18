using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// TODO
// - Fix six DOF controller
// - Add support for left and right hand
// - Two handed manipulation
// - Pointer display
// - Hand controller
// - Touch pointer?
// - Two handed manipulation

namespace Input2
{
    // May make sense to split this in several interfaces (e.g. one just for pointer moved). 
    // Keeping it simple here
    interface IPointerEventHandler : IEventSystemHandler
    {
        void OnPointerEnter(PointerEventData data);

        // Too nosy? Implement this as UnityEvent instead?
        void OnPointerMoved(PointerEventData data);

        void OnPointerPressed(PointerEventData data);
        void OnPointerReleased(PointerEventData data);
        void OnPointerExit(PointerEventData data);
    }

    public class PointerEventData : BaseEventData
    {
        public Pointer pointer;

        public PointerEventData(Pointer pointer) : base(EventSystem.current)
        {
            this.pointer = pointer;
        }
    }

    public class Pointer : MonoBehaviour
    {
        public float maxRaycastDistance = 10.0f;
        public bool debugDisplay = true;

        // Set to true to stop the pointer from raycasting. Used to manually control the target pose.
        public bool Locked { get; set; }

        private Pose targetPose = Pose.identity;
        public Pose TargetPose
        {
            get { return targetPose; }
            set
            {
                targetPose = value;
                //TickEvent?.Invoke(this);
            }
        }

        public delegate void TickHandler(Pointer p);
        public event TickHandler TickEvent;

        private GameObject currentTarget = null;
        private bool isPressed = false;
        private PointerEventData eventData;

        void Awake()
        {
            eventData = new PointerEventData(this);
        }

        private void OnEnable()
        {
            var attachment = GetComponent<ControllerAttachmentBase>();
            Debug.Assert(attachment != null);
            attachment.TickEvent += Tick;
        }

        private void OnDisable()
        {
            var attachment = GetComponent<ControllerAttachmentBase>();
            Debug.Assert(attachment != null);
            attachment.TickEvent += Tick;
        }

        private void Tick(ControllerAttachmentBase attachment)
        {
            GameObject prevTarget = currentTarget;

            if (Locked)
            {
                if (debugDisplay)
                {
                    Vector3 dir = transform.forward * 0.1f;
                    Debug.DrawRay(transform.position, dir, Color.green);
                    dir = TargetPose.forward * 0.1f;
                    Debug.DrawRay(TargetPose.position, dir, Color.red);
                }
            }
            else
            {
                Raycast();
            }

            // Move event
            if (transform.hasChanged && prevTarget == currentTarget)
            {
                ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerMovedEventFunction);
            }

            // Press/released event
            bool newIsPressed = attachment.Controller.PrimaryButton;
            if (newIsPressed != isPressed && currentTarget)
            {
                isPressed = newIsPressed;
                if (isPressed)
                {
                    ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerPressedEventFunction);
                }
                else
                {
                    ExecuteEvents.Execute<IPointerEventHandler>(currentTarget, eventData, onPointerReleasedEventFunction);
                }
            }

            TickEvent?.Invoke(this);
        }

        private void Raycast()
        {
            Vector3 direction = transform.rotation * Vector3.forward;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance))
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

                targetPose.position = transform.position + direction * maxRaycastDistance;
                targetPose.rotation = Quaternion.LookRotation(-direction);
            }

            if (debugDisplay)
            {
                Debug.DrawLine(transform.position, targetPose.position);
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

        private static readonly ExecuteEvents.EventFunction<IPointerEventHandler> onPointerMovedEventFunction =
        delegate (IPointerEventHandler handler, BaseEventData eventData)
        {
            var casted = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            handler.OnPointerMoved(casted);
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
}