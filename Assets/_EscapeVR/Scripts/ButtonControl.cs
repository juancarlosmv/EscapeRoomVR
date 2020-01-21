using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour
{
    public EventSystem eventSystem;
    UnityEngine.UI.Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
    }

    // Con esta forma, llamamod al sistema de unity para que gestione los clicks
    public void Click()
    {
        UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject,
            new UnityEngine.EventSystems.BaseEventData(eventSystem),
            UnityEngine.EventSystems.ExecuteEvents.submitHandler);
    }

    public void Over()
    {
        Debug.Log("Over");
        UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject,
            new UnityEngine.EventSystems.PointerEventData(eventSystem),
            UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
    }

    public void Off()
    {
        UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject,
            new UnityEngine.EventSystems.PointerEventData(eventSystem),
            UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
    }
}
