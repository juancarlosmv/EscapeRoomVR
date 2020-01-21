using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnOverEvent;
    public UnityEvent OnOffEvent;
    public UnityEvent OnClickEvent;
    private Interacter interacter = null;
    

    bool m_bDown;
    bool m_bLastDown;

    
    // Llamado desde un Interacter cuando el objeto esta seleccionado pero no pulsado
    public void Down(Interacter it)
    {
        interacter = it;
        m_bDown = true;
    }
    

    // Llamado desde un Interacter al pulsar en el objeto
    public void Clicked(Interacter it)
    {
        interacter = it;
        OnClickEvent.Invoke();
    }


    // El código de la clase para los eventos de Over y Off
    void Update()
    {
        if(m_bDown && !m_bLastDown)
        {
            OnOverEvent.Invoke();
        }
        if(!m_bDown && m_bLastDown)
        {
            OnOffEvent.Invoke();
        }

        m_bLastDown = m_bDown;
        m_bDown = false;
    }

    public Interacter GetInteracter() => interacter;
}
