using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    enum Controller { Left, Right };
    public enum Mode { Close, Far };

    [SerializeField]
    private Transform headPosition;
    [SerializeField]
    private float thPointer = 0.5f;
    [SerializeField]
    private Controller controller = Controller.Left;
    [SerializeField]
    private float laserRadius = 0.1f;
    private Mode interactMode = Mode.Close;
    private LineRenderer lr;
    private Grabber grabber;
    private bool bFirstHeldClicked = false;
    private bool bFirstPointerClicked = false;
    private OVRInput.Button bHandTrigger, bIndexTrigger;
    private Dictionary<int, Interactable> closeObjects;

    // Botones segun si es el control derecho o izquierdo
    public OVRInput.Button BHandTrigger => bHandTrigger;
    public OVRInput.Button BIndexTrigger => bIndexTrigger;
    public Grabber GetGrabber() => grabber;
    public Mode InteractMode => interactMode;


    void Start()
    {
        closeObjects = new Dictionary<int, Interactable>();
        lr = GetComponent<LineRenderer>();
        grabber = GetComponent<Grabber>();
        lr.enabled = false;
        SetButtons();
    }


    void Update()
    {
        // Solo podemos hace opciones de apuntar y coger si no hay nada agarrado,
        // en caso contrario el control lo tiene el Grabber
        if(grabber.GetGrabbedObject() == null)
        {
            UpdateClicks();
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
            closeObjects[interactableObject.GetInstanceID()] = interactableObject;
            Debug.Log(other.gameObject);
        }
    }


    // Desregistramos los objetos que salgan de rango del interacter
    private void OnTriggerExit(Collider other)
    {
        Interactable interactableObject = other.gameObject.GetComponent<Interactable>();
        if (interactableObject != null)
        {
            closeObjects.Remove(interactableObject.GetInstanceID());
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
        foreach(KeyValuePair<int, Interactable> value in closeObjects)
        {
            float daux = (transform.position - value.Value.gameObject.transform.position).magnitude;
            if(daux < distance)
            {
                distance = daux;
                closest = value.Value;
            }
        }
        return closest;
    }


    // Actualiza los botones pulsados en el mando
    private void UpdateClicks()
    {
        bFirstHeldClicked = false;
        bFirstPointerClicked = false;

        if (OVRInput.GetDown(BHandTrigger) || Input.GetMouseButtonDown(0))
        {
            bFirstHeldClicked = true;
        }
        if (OVRInput.GetDown(BIndexTrigger) || Input.GetMouseButtonDown(0))
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


    // Inicializar los botones usados dependiendo del mando
    private void SetButtons()
    {
        if(controller == Controller.Left)
        {
            bHandTrigger = OVRInput.Button.PrimaryHandTrigger;
            bIndexTrigger = OVRInput.Button.PrimaryIndexTrigger;
        }
        else if(controller == Controller.Right)
        {
            bHandTrigger = OVRInput.Button.SecondaryHandTrigger;
            bIndexTrigger = OVRInput.Button.SecondaryIndexTrigger;
        }
    }
}
