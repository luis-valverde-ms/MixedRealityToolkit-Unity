# Pointers

Pointers are instanced automatically at runtime when a new controller is detected. You can have more than one pointer attached to a controller; for example, with the default pointer profile, WMR controllers get both a line and a parabolic pointer for normal selection and teleportation respectively. Pointers communicate with each other to decide which one is active. The pointers created for each controller are set up in the **Pointer Profile**, under the *Input System Profile*.

<img src="../../Documentation/Images/Input/PointerProfile.png" style="max-width:100%;">

MRTK provides a set of pointer prefabs in *Assets/MixedRealityToolkit.SDK/Features/UX/Prefabs/Pointers*. You can use your own prefabs as long as they contain one of the pointer scripts in *Assets/MixedRealityToolkit.SDK/Features/UX/Scripts/Pointers* or any other script implementing [`IMixedRealityPointer`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityPointer).

## Pointer Events

To receive pointer events, implement one of the following interfaces in your script:

Handler | Events | Description
--- | --- | ---
[`IMixedRealityFocusChangedHandler`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityFocusChangedHandler) | Before Focus Changed / Focus Changed | Raised on both the game object losing focus and the one gaining it every time a pointer changes focus.
[`IMixedRealityFocusHandler`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityFocusHandler) | Focus Enter / Exit | Raised on the game object gaining focus when the first pointer enters it and on the one losing focus when the last pointer leaves it.
[`IMixedRealityPointerHandler`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityPointerHandler) | Pointer Down / Dragged / Up / Clicked | Raised to report pointer press, drag and release.
[`IMixedRealityTouchHandler`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityTouchHandler) | Touch Started / Updated / Completed | Raised by touch-aware pointers like [**PokePointer**](xref:Microsoft.MixedReality.Toolkit.Input.PokePointer) to report touch activity.

## Primary Pointer

In some situations it can be useful to have a notion of *primary* pointer, e.g. to indicate the pointer that should be considered as the primary source of input in application logic.

### Accessing the Primary Pointer

Changes to the current primary pointer are reported by the focus provider via  **primary pointer changed** events. To subscribe, call [`SubscribeToPrimaryPointerChanged`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityFocusProvider.SubscribeToPrimaryPointerChanged*) on the focus provider. Unsubscribe via [`UnsubscribeFromPrimaryPointerChanged`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityFocusProvider.UnsubscribeFromPrimaryPointerChanged*). See `MixedRealityToolkit.Examples\Demos\Input\Scenes\PrimaryPointer` for an example.

<img src="../../Documentation/Images/Input/PrimaryPointerExample.png" style="max-width:100%;">

You can also poll the current value of the primary pointer via the [`PrimaryPointer`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityFocusProvider.PrimaryPointer) read-only property in the focus provider.

### The Primary Pointer Selector

The logic used to select the primary pointer is given by the **Primary Pointer Selector** in use. This is setup in the **Pointer Profile**, *Primary Pointer Selector* drop down, which allows you to choose between all available implementations of the [`IMixedRealityPrimaryPointerSelector`](xref:Microsoft.MixedReality.Toolkit.Input.IMixedRealityPrimaryPointerSelector) interface.

<img src="../../Documentation/Images/Input/PrimaryPointerSelector.png" style="max-width:100%;">

The default implementation, [`DefaultPrimaryPointerSelector`](xref:Microsoft.MixedReality.Toolkit.Input.DefaultPrimaryPointerSelector), chooses the primary pointer among all the interaction enabled ones (i.e. active) using the following rules in order:
1. Currently pressed pointer that has been pressed for the longest.
2. Pointer that was released most recently.
3. Pointer that became interaction enabled most recently.

You are encouraged to play with the PrimaryPointer example scene to get a feeling for the way this logic works in practice.

You can customize this logic deriving from the default pointer selector or creating your own interface implementation from scratch and selecting it in the pointer profile.