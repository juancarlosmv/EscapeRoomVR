using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOkLed : MonoBehaviour
{
    private bool on = false;
    public Material ok;
    public Material notok;
    public PipeGridController gridController;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = notok;
    }

    // Update is called once per frame
    void Update()
    {
        bool onaux = gridController.PathOk(id);
        if (onaux != on)
        {
            on = onaux;
            if (on == true) GetComponent<MeshRenderer>().material = ok;
            else GetComponent<MeshRenderer>().material = notok;
        }
    }
}
