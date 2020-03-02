using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiTrollSystem : MonoBehaviour
{
    AudioSource _as;
    void Awake() 
    {
        _as = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other) 
    {
        _as.Play();
        if (other.gameObject.layer == 9)GameManager.GetInstance().EndGame(false, "Why do you kill yourself. You have drowned");
    }
}
