using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Intro: MonoBehaviour
{
    [SerializeField] GameObject BlackPanel;
    [SerializeField] CinemachineDollyCart _cart;

    int _delay = 5;

    void Update() 
    {
        Debug.Log(_cart.m_Position);
        if (_cart.m_Position > 290.0 && !BlackPanel.active) 
        {
            BlackPanel.SetActive(true);
            GameManager.GetInstance().LoadScene("Menu",_delay);
        }
    }
}
