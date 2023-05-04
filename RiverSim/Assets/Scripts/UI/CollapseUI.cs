using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollapseUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject content;
    [SerializeField] private RawImage icon;
    [SerializeField] private float angle;
    private bool contentActive;

    private void OnEnable()
    {
        content.SetActive(true);
        icon.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        contentActive = !contentActive;
        content.SetActive(contentActive);
        if (contentActive) icon.transform.rotation = Quaternion.Euler(0, 0, 0);
        else icon.transform.rotation = Quaternion.Euler(0, 0, angle);        
    }
}
