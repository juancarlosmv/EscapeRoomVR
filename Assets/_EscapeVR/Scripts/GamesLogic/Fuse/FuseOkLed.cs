using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseOkLed : MonoBehaviour
{
    private bool on = false;
    public Material ok;
    public Material notok;
    public FuseBox fuseBox;
    AudioSource _as;

    private void Awake() 
    {
        _as = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().material = notok;
    }

    void Update()
    {
        bool onaux = fuseBox.IsCorrect();
        if(onaux != on)
        {
            on = onaux;
            if (on == true) 
            {
                GetComponent<MeshRenderer>().material = ok;
                if (_as != null) _as.Play();
            }
            else GetComponent<MeshRenderer>().material = notok;
        }
    }
}
