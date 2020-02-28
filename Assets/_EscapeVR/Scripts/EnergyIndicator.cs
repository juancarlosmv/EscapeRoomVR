using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyIndicator : MonoBehaviour
{
    [SerializeField] Material _mat;
    #region Singleton
    private static EnergyIndicator _instance;
    public static EnergyIndicator GetInstance() { return _instance; }
    #endregion
    void Awake() 
    {
        _instance = this;
    }
    public void EnergyOn() 
    {
        this.GetComponent<Renderer>().material = _mat;
        this.GetComponent<AudioSource>().Play();
    }
}
