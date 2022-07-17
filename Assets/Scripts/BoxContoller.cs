using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;

public class BoxContoller : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealitySpeechHandler, IMixedRealityFocusHandler
{
    private bool isFocus;
    private Handedness previousHand = Handedness.None;

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealitySpeechHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityFocusHandler>(this);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityFocusHandler>(this);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (isFocus)
        {
            Vector3 pos = eventData.Pointer.Position;
            this.gameObject.transform.position = new Vector3(pos.x, pos.y, 5f);
            //eventData.Use();
        }
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (isFocus)
        {
            Vector3 initialPosition = this.gameObject.transform.position;
            Vector3 scale = this.gameObject.transform.localScale;
            Vector3 finalPositionRight = initialPosition + new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 finalPositionLeft = initialPosition - new Vector3(0.5f, 0.5f, 0.5f);

            if ((eventData.Handedness == Handedness.Left && previousHand == Handedness.Right) || (eventData.Handedness == Handedness.Right && previousHand == Handedness.Left))
            {
                if (eventData.Handedness == Handedness.Right)
                    finalPositionRight = eventData.Pointer.Position;
                if (eventData.Handedness == Handedness.Left)
                    finalPositionLeft = eventData.Pointer.Position;

                float sx = finalPositionRight.x - finalPositionLeft.x;
                float sy = finalPositionRight.y - finalPositionLeft.y;
                float sz = finalPositionRight.z - finalPositionLeft.z;
                this.gameObject.transform.localScale = new Vector3(sx, sy, sz);
            }

            // Drag
            Vector3 pos = eventData.Pointer.Position;
            this.gameObject.transform.position = new Vector3(pos.x, pos.y, 5f);
            previousHand = eventData.Handedness;
            //eventData.Use();
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Down - {eventData.Pointer.Position}");
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Up - {eventData.Pointer.Position}");
    }

    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (string.Equals(eventData.Command.Keyword, "hello", System.StringComparison.OrdinalIgnoreCase))
            Debug.Log("Hi There!!!");
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        if (eventData.NewFocusedObject.name == this.gameObject.name)
            isFocus = true;
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        if (eventData.OldFocusedObject.name == this.gameObject.name)
            isFocus = false;
    }
}