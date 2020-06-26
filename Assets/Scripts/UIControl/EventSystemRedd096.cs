using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("redd096/Event System redd096")]
public class EventSystemRedd096 : EventSystem
{
    #region variables

    [Tooltip("For every menu, from what object start?")]
    [SerializeField] GameObject[] firstSelectedGameObjects = default;

    [Header("When this object is active, can navigate only in its menu")]
    [SerializeField] GameObject overrideObject = default;

    [Header("Can't navigate to these objects")]
    [SerializeField] GameObject[] notInteractables = default;

    GameObject lastSelected;

    #endregion

    protected override void Update()
    {
        base.Update();

        GameObject selected = current.currentSelectedGameObject;

        //if there is something selected and active
        if (selected && selected.activeInHierarchy)
        {
            //if active override, can't go to another menu
            if(overrideObject && overrideObject.activeInHierarchy && selected.transform.parent != overrideObject.transform.parent)
            {
                //if last selected was in override menu, select it - otherwise select override button
                if (lastSelected && lastSelected.activeInHierarchy && lastSelected.transform.parent == overrideObject.transform.parent)
                    current.SetSelectedGameObject(lastSelected);
                else
                    current.SetSelectedGameObject(overrideObject);

                return;
            }

            //if there are no interactables
            if(notInteractables.Length > 0)
            {
                foreach(GameObject go in notInteractables)
                {
                    //if selected a not interactable object
                    if(go != null && selected == go)
                    {
                        //back to last selected or select null
                        if (lastSelected && lastSelected.activeInHierarchy)
                            current.SetSelectedGameObject(lastSelected);
                        else
                            current.SetSelectedGameObject(null);
                    }
                }
            }

            //if != from last selected, set last selected
            if (lastSelected != selected)
                lastSelected = selected;
        }
        else
        {
            //if selected nothing or is no active, if is active the override button, select it
            if (overrideObject && overrideObject.activeInHierarchy)
            {
                current.SetSelectedGameObject(overrideObject);
                return;
            }

            //else, if last selected is active, select it
            if (lastSelected && lastSelected.activeInHierarchy)
            {
                current.SetSelectedGameObject(lastSelected);
            }
            else
            {
                //else check which firstSelectedGameObject is active, and select it
                foreach (GameObject firstSelect in firstSelectedGameObjects)
                {
                    if (firstSelect && firstSelect.activeInHierarchy)
                    {
                        current.SetSelectedGameObject(firstSelect);
                        break;
                    }
                }
            }
        }

        //if selected something not active, select null
        if (selected && selected.activeInHierarchy == false)
            current.SetSelectedGameObject(null);
    }
}