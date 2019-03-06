using Microsoft.MixedReality.Toolkit.Core.Definitions.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem.Handlers;
using Microsoft.MixedReality.Toolkit.Core.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAttachment : MonoBehaviour, IMixedRealityInputHandler<MixedRealityPose>
{
    [SerializeField]
    private Handedness hand;

    [SerializeField]
    private MixedRealityInputAction poseAction;

    public void OnInputChanged(InputEventData<MixedRealityPose> eventData)
    {
        if (eventData.Handedness == hand && eventData.MixedRealityInputAction == poseAction)
        {
            transform.position = eventData.InputData.Position;
            transform.rotation = eventData.InputData.Rotation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MixedRealityToolkit.InputSystem.Register(gameObject);
    }

    private void OnDisable()
    {
        MixedRealityToolkit.InputSystem.Unregister(gameObject);
    }
}
