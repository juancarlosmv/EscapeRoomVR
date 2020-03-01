using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOkLed : MonoBehaviour
{
    private bool on = false;
    public Material ok;
    public Material notok;
    public PipeGridController gridController;
    int id=0;

    AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        GetComponent<MeshRenderer>().material = notok;
    }

    // Update is called once per frame
    void Update()
    {
        bool onaux = gridController.PathOk(id);
        if (onaux != on)
        {
            on = onaux;
            if (on == true) 
            {
                GetComponent<MeshRenderer>().material = ok;
                _as.Play();
            }
            else GetComponent<MeshRenderer>().material = notok;
        }
    }
}
