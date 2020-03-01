using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOkLed : MonoBehaviour
{
    public Material ok;
    public Material error;
    public Material baseMat;
    AudioSource _as;

    void Awake() 
    {
        _as = GetComponent<AudioSource>();
    }
    public void SetBase()
    {
        GetComponent<MeshRenderer>().material = baseMat;
    }

    public void SetOk()
    {
        GetComponent<MeshRenderer>().material = ok;
        _as.Play();
    }

    public void SetError()
    {
        GetComponent<MeshRenderer>().material = error;
    }
    
}
