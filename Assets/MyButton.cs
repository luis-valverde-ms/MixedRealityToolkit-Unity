using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour, IPointerEventHandler
{
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log("OnPointerExit");
    }

    public void OnPointerPressed(PointerEventData data)
    {
        Debug.Log("OnPointerPressed");
    }

    public void OnPointerReleased(PointerEventData data)
    {
        Debug.Log("OnPointerReleased");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
