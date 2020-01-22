using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseOkLed : MonoBehaviour
{
    private bool on = false;
    public Material ok;
    public Material notok;
    public FuseBox fuseBox;

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
            if (on == true) GetComponent<MeshRenderer>().material = ok;
            else GetComponent<MeshRenderer>().material = notok;
        }
    }
}
