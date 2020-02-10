using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOkLed : MonoBehaviour
{
    public Material ok;
    public Material error;
    public Material baseMat;

    public void SetBase()
    {
        GetComponent<MeshRenderer>().material = baseMat;
    }

    public void SetOk()
    {
        GetComponent<MeshRenderer>().material = ok;
    }

    public void SetError()
    {
        GetComponent<MeshRenderer>().material = error;
    }
}
