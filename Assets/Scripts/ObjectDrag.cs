using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ObjectDrag : MonoBehaviour /*,IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler*/
{
    // Image Components...
    /*[SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    public float DragXPosition;
    public float DragYPosition;*/
    /*private void Awake() {
        rectTransform = GetComponent<RectTransform>(); 
    }*/
    // Sprite Drag....
    private Vector3 _dragoffset;
    [SerializeField] private Camera _cam;
    public float clampX = 1.6f;
    public float clampY = 3.6f;

    void Awake()
    {
        _cam = Camera.main;
    }

    private void Start()
    {
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.6f, 1.6f),
            Mathf.Clamp(transform.position.y, -3.6f, 3.6f), 0);
    }

    void OnMouseDown()
    {
        AudioManager.instance.Play("Tap");
        Vibration.Vibrate(20);
        _dragoffset = transform.position - GetMousePos();
    }

    private void OnMouseUp()
    {
        transform.DOScale(.6f, .15f);
    }

    void OnMouseDrag()
    {
        transform.position = GetMousePos() + _dragoffset;
        transform.DOScale(1f, .15f);
    }

    Vector3 GetMousePos()
    {

        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    void CheckOption()
    {
    }
}

// Image Drag...
/*public void OnBeginDrag (PointerEventData eventData) {
    
    Debug.Log("OnBeginDrag");
    transform.DOScale( 1.5f, .15f);

}
public void OnEndDrag(PointerEventData eventData) {
    Debug.Log("OnEndDrag");
    transform.DOScale( 1f, .15f);

}
public void OnPointerDown (PointerEventData eventData) {
    Debug. Log("OnPointerDown");
}

public void OnDrag(PointerEventData eventData)
{
    Debug.Log("OnDrag"); 
    // rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    ClampingImages(eventData);
}

void ClampingImages(PointerEventData eventData)
{
    DragXPosition = (rectTransform.anchoredPosition.x + eventData.delta.x) / canvas.scaleFactor;
    DragYPosition = (rectTransform.anchoredPosition.y + eventData.delta.y) / canvas.scaleFactor;


    if (DragXPosition <= -350)
    {
        DragXPosition = -350;
    }

    if (DragXPosition >= 350f)
    {
        DragXPosition = 350;
    }

    if (DragYPosition <= -628)
    {
        DragYPosition = -628;
    }

    if (DragYPosition >= 628)
    {
        DragYPosition = 628;
    }


    rectTransform.anchoredPosition = new Vector2(DragXPosition, DragYPosition);
}*/
