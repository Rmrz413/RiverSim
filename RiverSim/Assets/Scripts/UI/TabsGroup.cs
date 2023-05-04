using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsGroup : MonoBehaviour
{
    public List<TabsButton> tabButtons;
    public List<GameObject> objectsToSwap;

    [Header("Colors")]
    public Color NormalColor;
    public Color HighlightedColor;
    public Color SelectedColor;

    private TabsButton selected;

    public void Subscribe(TabsButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabsButton>();
            tabButtons.Add(button);
            OnTabSelected(button);
        }
        else tabButtons.Add(button);
    }

    public void OnTabEnter(TabsButton button)
    {
        ResetTabs();
        if(selected == null || button != selected) 
            button.background.color = HighlightedColor;
    }

    public void OnTabExit(TabsButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabsButton button)
    {
        selected = button;
        ResetTabs();
        button.background.color = SelectedColor;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)            
                objectsToSwap[i].SetActive(true);
            else
                objectsToSwap[i].SetActive(false);            
        }
    }

    public void ResetTabs()
    {
        foreach(TabsButton button in tabButtons)
        {
            if(selected != null && button == selected) continue;
            button.background.color = NormalColor;
        }
    }
}
