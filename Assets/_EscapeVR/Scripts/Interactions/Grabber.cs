using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private Interacter interacter;
    private GameObject grabbed = null;
    private Vector3 grabbedPositionOffset;
    private Quaternion initialHandRotation, grabbedRotationOffset; // TODO posOffset para close grab
    private Vector3 speed, position0;
    private float timeToInteractAgain = 0.1f;
    float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        interacter = GetComponent<Interacter>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si hay algo agarrado, realiza operaciones para atraerlo a la mano, mantener la
        // rotacion relativa y calcular la velocidad a la que se esta moviendo
        if(grabbed != null)
        {
            float dt = Time.deltaTime;
            t += dt;
            t = Mathf.Min(t, 1.0f);
            // Si cogemos de lejos, atraemos con Lerp
            if(interacter.InteractMode == Interacter.Mode.Far)
            {
                grabbed.transform.position = Vector3.Lerp(grabbed.transform.position,
                transform.position, t);
            }
            // Si cogemos de cerca, mantenemos la posicion relativa
            else if(interacter.InteractMode == Interacter.Mode.Close)
            {
                grabbed.transform.position = transform.position;
                Quaternion rot = transform.rotation * Quaternion.Inverse(initialHandRotation);
                Vector3 rotatedPositionOffset = rot * grabbedPositionOffset;
                grabbed.transform.position = transform.position + rotatedPositionOffset;
            }
            // Mantenemos la rotacion relativa y calculamos la velocidad
            grabbed.transform.rotation = transform.rotation * grabbedRotationOffset;
            speed = (grabbed.transform.position - position0) / dt;
            position0 = grabbed.transform.position;
            
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, interacter.Controller))
            {
                Ungrab();
            }
        }
    }


    // Agarra un objeto. Es llamado desde un GrabableObj
    public void Grab(GrabableObj grabbable)
    {
        if(grabbed != null)
        {
            Debug.Log("Ya hay un objeto agarrado!");
            return;
        }
        grabbed = grabbable.gameObject;
        // Guardar posicion y rotacion relativa
        grabbedPositionOffset = grabbed.transform.position - transform.position;
        initialHandRotation = transform.rotation;
        grabbedRotationOffset = Quaternion.Inverse(transform.rotation) * grabbed.transform.rotation;
        // Desactivar fuerzas sobre el objeto cogido
        grabbed.GetComponent<Rigidbody>().isKinematic = true;
        // Cambiar el layer a objeto agarrado (no colisiona con el jugador)
        grabbed.layer = LayerMask.NameToLayer("grabbed");
        position0 = transform.position;
        speed = Vector3.zero;
        // No queremos laser en una mano con algo cogido
        interacter.TurnOffLaser();
        t = 0.0f;
    }


    // Suelta un objeto. Con forceUngrab true no hace espera para volver a poner el
    // objeto en su layer original, puesto que se usa cuando una mano le quita el objeto
    // a la otra
    public void Ungrab(bool forceUngrab = false)
    {
        // Activar fuerzas en el objeto
        grabbed.GetComponent<Rigidbody>().isKinematic = false;
        // Darle usa fuerza en la direccion de la velocidad actual, para que
        // no caiga directamente al suelo y que salga de forma realista
        grabbed.GetComponent<Rigidbody>().AddForce(speed, ForceMode.Impulse);
        // Cambiar el layer al original tras una espera en paralelo
        if (!forceUngrab) StartCoroutine(WaitUntilChangeLayer(grabbed));
        // Decirle al objeto que ya no tiene nada que lo esta agarrando
        grabbed.GetComponent<GrabableObj>().GetUngrabbed();
        grabbed = null;
    }


    // Simple corutina para devolver el layer de un objeto cogido al original
    // Usa una pequeña espera porque, si no, colisiona contigo en el mismo instante
    // de soltarlo
    IEnumerator WaitUntilChangeLayer(GameObject go)
    {
        yield return new WaitForSeconds(timeToInteractAgain);
        go.layer = go.GetComponent<GrabableObj>().OriginalLayer;
    }

    public GameObject GetGrabbedObject() => grabbed;
    //public Interacter GetInteracter() => interacter;
}
