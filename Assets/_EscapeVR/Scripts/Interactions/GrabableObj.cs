using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObj : MonoBehaviour
{
    Grabber hand;
    Interactable interactable;
    private int originalLayer;
    public bool CanBeGrabbed = true;
    public bool IsDistanceGrabbable = true;
    public bool DG { get; private set; }

    public int OriginalLayer => originalLayer;
    

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        originalLayer = gameObject.layer;
        DG = IsDistanceGrabbable;
    }


    // Le dice a un Grabber que lo agarre. Generalmente se invoca desde un Interactable
    // en el OnClickEvent
    public void GetGrabbed()
    {
        if (CanBeGrabbed)
        {
            // Si no puede ser agarrado a corta distancia
            if (interactable.GetInteracter().InteractMode == Interacter.Mode.Far && !IsDistanceGrabbable) return;
            // Si ya lo tenemos cogido con la otra mano, lo "robamos" a la fuerza
            if (hand != null) hand.Ungrab(forceUngrab: true);
            hand = interactable.GetInteracter().GetGrabber();
            if (hand == null)
            {
                Debug.Log("No hay un grabber");
                return;
            }
            hand.Grab(this);
        }
    }


    // Cambia a null la referencia de quien le esta agarrando
    public void GetUngrabbed()
    {
        hand = null;
    }

    public Grabber GetGrabber() => hand;
}
