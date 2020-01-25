using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseLocation : MonoBehaviour
{
    [SerializeField]
    private Fuse.FuseColor color = Fuse.FuseColor.Red;
    private Fuse fuse = null;
    private bool isMoving = false;
    private float t = 0.0f;

    // BIG TODO cmprobar que cada vez que obtengo un componente, este exista
    void Update()
    {
        if (isMoving)
        {
            t += Time.deltaTime;
            if (t > 1.0f)
            {
                isMoving = false;
                // Al llegar a su destino se puede agarrar de nuevo
                GrabableObj gro = fuse.gameObject.GetComponent<GrabableObj>();
                gro.CanBeGrabbed = true;
            }
                
            GameObject go = fuse.gameObject;
            go.transform.position = Vector3.Lerp(go.transform.position, transform.position, t);
            go.transform.rotation = Quaternion.Lerp(go.transform.rotation, transform.rotation, t);
        }
        // Si aun tenemos fusible y no esta cogido
        else if(fuse != null && fuse.GetComponent<GrabableObj>().GetGrabber() == null)
        {
            // Comprobar si se ha movido
            // En caso de ser asi, reactivar el movimiento automatico
            float movementP = Vector3.Distance(transform.position, fuse.gameObject.transform.position);
            float movementR = Mathf.Abs(Quaternion.Angle(transform.rotation, fuse.gameObject.transform.rotation));
            bool moved = movementP > 0.05f || movementR > 0.5f;
            if (moved)
            {
                isMoving = true;
                t = 0.0f;
                GrabableObj gro = fuse.gameObject.GetComponent<GrabableObj>();
                gro.CanBeGrabbed = false;
                gro.IsDistanceGrabbable = false;
                fuse.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public bool IsCorrect()
    {
        if (fuse == null) return false;
        if (fuse.Color == color) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (fuse != null) return;
        Fuse f = other.gameObject.GetComponent<Fuse>();
        if(f != null)
        {
            fuse = f;
            isMoving = true;
            t = 0.0f;
            // Ungrab si es un grabbable
            GrabableObj grabbable = fuse.gameObject.GetComponent<GrabableObj>();
            if (grabbable != null)
            {
                Grabber hand = grabbable.GetGrabber();
                if (hand != null) hand.Ungrab(); // No es force ungrab porque no lo cogemos con la otra mano
            }
            // Desactivar rigidbody (recordar que el ungrab lo activa)
            fuse.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            // Impedir que pueda ser agarrado de nuevo
            GrabableObj gro = fuse.gameObject.GetComponent<GrabableObj>();
            gro.CanBeGrabbed = false;
            gro.IsDistanceGrabbable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ver si el que ha salido es el fusible que tiene
        if(fuse != null && other.gameObject.GetInstanceID() == fuse.gameObject.GetInstanceID())
        {
            GrabableObj gro = fuse.gameObject.GetComponent<GrabableObj>();
            // Activar rigidbody solo si no se esta cogiendo
            if(gro.GetGrabber() == null)
                fuse.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gro.IsDistanceGrabbable = gro.DG;
            fuse = null;
        }
    }
}
