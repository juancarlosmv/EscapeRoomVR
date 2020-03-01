using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    public int WeightValue = 10;
    public bool OnMachine = false;

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Weight") && OnMachine) 
        {
            WeightValue += other.gameObject.GetComponent<Weight>().WeightValue;
        }
    }
}
