using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{

    RectTransform rt;

    Vector2 originalAnchored;

    //test
    public Vector2 axisValue;
    
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        originalAnchored = rt.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        var parent = rt.parent.GetComponent<RectTransform>();

        var parentSize = parent.rect.size;
        var parentPoint = eventData.position - parentSize;

        Vector2 localPoint = parent.InverseTransformPoint(parentPoint);
        Vector2 newAnchorPos = localPoint - originalAnchored;

        newAnchorPos = Vector2.ClampMagnitude(newAnchorPos, parentSize.x / 2);

        rt.anchoredPosition = newAnchorPos;

        //test
        axisValue = newAnchorPos / (parentSize.x/2);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rt.anchoredPosition = Vector3.zero;

        axisValue = Vector2.zero;
    }
}
