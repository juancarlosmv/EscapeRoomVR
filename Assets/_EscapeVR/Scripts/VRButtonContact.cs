using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButtonContact : MonoBehaviour
{
    [SerializeField]
    private char code;
    [SerializeField]
    private GameObject pushButton;
    [SerializeField]
    private VRButtonController buttonController;

    private void OnTriggerEnter(Collider other)
    {
        // Activate button puch only if the button enters the trigger
        if(other.gameObject.GetInstanceID() == pushButton.GetInstanceID())
        {
            buttonController.ProcessPush(code);
        }
    }
}
