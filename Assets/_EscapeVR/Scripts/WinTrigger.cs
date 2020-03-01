using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == 9) 
        {
            GameManager.GetInstance().EndGame(true);
        }
    }
}
