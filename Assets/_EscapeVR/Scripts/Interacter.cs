using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    //enum Controller { Left, Right };
    [SerializeField]
    private OVRInput.Controller controller = OVRInput.Controller.LTouch;
    public enum Mode { Close, Far };

    [SerializeField]
    private Transform headPosition;
    [SerializeField]
    private float thPointer = 0.5f;
    [SerializeField]
    private float laserRadius = 0.1f;
    private Mode interactMode = Mode.Close;
    private LineRenderer lr;
    private Grabber grabber;
    public bool bFirstHeldClicked { get; private set; } = false;
    public bool bFirstPointerClicked { get; private set; } = false;
    private Dictionary<int, Tuple<Interactable, int>> closeObjects;
    public Grabber GetGrabber() => grabber;
    public OVRInput.Controller Controller => controller;
    public Mode InteractMode => interactMode;


    void Start()
    {
        closeObjects = new Dictionary<int, Tuple<Interactable, int>>();
        lr = GetComponent<LineRenderer>();
        grabber = GetComponent<Grabber>();
        lr.enabled = false;
    }


    void Update()
    {
        // Solo podemos hace opciones de apuntar y coger si no hay nada agarrado,
        // en caso contrario el control lo tiene el Grabber
        UpdateClicks();
        if (grabber.GetGrabbedObject() == null)
        {
            // Si distancia con head > th, laser
            if ((headPosition.position - transform.position).sqrMagnitude >= thPointer)
            {
                interactMode = Mode.Far;
                LaserBehaviour();
            }
            // Si no, comportamiento a corta distancia
            else
            {
                interactMode = Mode.Close;
                CloseBehaviour();
            }
        }
    }


    // Registramos los objetos que entran a rango del interacter
    private void OnTriggerEnter(Collider other)
    {
        Interactable interactableObject = other.gameObject.GetComponent<Interactable>();
        if(interactableObject != null)
        {
            if (closeObjects.ContainsKey(interactableObject.GetInstanceID()))
                closeObjects[interactableObject.GetInstanceID()] = new Tuple<Interactable, int>(interactableObject, closeObjects[interactableObject.GetInstanceID()].Item2 + 1);
            else
                closeObjects[interactableObject.GetInstanceID()] = new Tuple<Interactable, int>(interactableObject, 1);
        }
    }


    // Desregistramos los objetos que salgan de rango del interacter
    private void OnTriggerExit(Collider other)
    {
        Interactable interactableObject = other.gameObject.GetComponent<Interactable>();
        if (interactableObject != null)
        {
            int instances = closeObjects[interactableObject.GetInstanceID()].Item2;
            if (instances == 1)
                closeObjects.Remove(interactableObject.GetInstanceID());
            else
                closeObjects[interactableObject.GetInstanceID()] = new Tuple<Interactable, int>(interactableObject, closeObjects[interactableObject.GetInstanceID()].Item2 - 1);
        }
    }


    // Define el comportamiento del Interacter cuando esta en modo larga distancia
    private void LaserBehaviour()
    {
        TurnOnLaser();
        lr.SetPosition(0, transform.position);

        RaycastHit raycastHit;
        //if (Physics.Raycast(new Ray(transform.position, transform.forward), out raycastHit, Mathf.Infinity))
        if (Physics.SphereCast(new Ray(transform.position, transform.forward), laserRadius, out raycastHit, Mathf.Infinity))
            {
            // Posicion final del laser y objeto colisionado
            lr.SetPosition(1, raycastHit.point);
            GameObject target = raycastHit.collider.gameObject;

            // En caso de que el objeto sea interactable, hacer acciones especiales
            Interactable interactable = target.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Por pasar por encima, activamos el Down para los OnOverEvents
                interactable.Down(this);
                // En caso de que pulsemos el boton lateral y este en un layer que pueda cogerse,
                // lo clickamos
                if ((target.layer == LayerMask.NameToLayer("grabbable") ||
                    target.layer == LayerMask.NameToLayer("grabbed")) && bFirstHeldClicked) {
                    interactable.Clicked(this);
                }
                // Si pertenece al menu, se activa con el dedo indice en vez de el lateral
                else if (target.layer == LayerMask.NameToLayer("UI") && bFirstPointerClicked) {
                    interactable.Clicked(this);
                }
            }
        }
        // Si no colisiona, establecer a mano un punto de fin del laser
        else lr.SetPosition(1, transform.position + transform.forward * 1000);
    }


    // TODO
    // Comportamiento para coger de cerca
    private void CloseBehaviour()
    {
        TurnOffLaser();
        Interactable closestInteractableObject = GetClosestInteractable();
        if(closestInteractableObject != null)
        {
            GameObject target = closestInteractableObject.gameObject;
            closestInteractableObject.Down(this);
            // En caso de que pulsemos el boton lateral y este en un layer que pueda cogerse,
            // lo clickamos
            if ((target.layer == LayerMask.NameToLayer("grabbable") ||
                target.layer == LayerMask.NameToLayer("grabbed")) && bFirstHeldClicked)
            {
                closestInteractableObject.Clicked(this);
            }
            // Si pertenece al menu, se activa con el dedo indice en vez de el lateral
            // TODO cambiar a simplemente apretar el objeto (Colision directa con la mano)
            else if (target.layer == LayerMask.NameToLayer("UI") && bFirstPointerClicked)
            {
                closestInteractableObject.Clicked(this);
            }
        }
    }


    // Obtiene el interactable mas cercano de entre los registrados
    private Interactable GetClosestInteractable()
    {
        float distance = 100000.0f;
        Interactable closest = null;
        foreach(KeyValuePair<int, Tuple<Interactable, int>> value in closeObjects)
        {
            float daux = (transform.position - value.Value.Item1.gameObject.transform.position).magnitude;
            if(daux < distance)
            {
                distance = daux;
                closest = value.Value.Item1;
            }
        }
        return closest;
    }


    // Actualiza los botones pulsados en el mando
    private void UpdateClicks()
    {
        bFirstHeldClicked = false;
        bFirstPointerClicked = false;
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller))
        {
            bFirstHeldClicked = true;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            bFirstPointerClicked = true;
        }
    }


    // Encender laser
    public void TurnOnLaser()
    {
        lr.enabled = true;
    }


    // Apagar laser
    public void TurnOffLaser()
    {
        lr.enabled = false;
    }
}
