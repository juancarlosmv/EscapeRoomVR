using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscotillaManagement : MonoBehaviour
{
    [SerializeField]
    WeighingMachine[] weightMachines;
    bool ok = false;

    private void Update()
    {
        if (!ok)
        {
            bool aux = true;
            for (int i = 0; i < weightMachines.Length; i++) aux = aux && weightMachines[i].IsOk;
            if (aux)
            {
                ok = true;
                GetComponent<GrabableObj>().CanBeGrabbed = true;
            }
        }
    }
}
