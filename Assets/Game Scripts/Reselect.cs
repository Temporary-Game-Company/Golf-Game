using UnityEngine;
using UnityEngine.EventSystems;

public class Reselect : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject lastSelected;
    
    
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = gameObject.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }
        else
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
    }
}
