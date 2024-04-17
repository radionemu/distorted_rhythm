using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class circle : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isactive;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
    }

    private void Awake() {
        isactive = false;
    }
}