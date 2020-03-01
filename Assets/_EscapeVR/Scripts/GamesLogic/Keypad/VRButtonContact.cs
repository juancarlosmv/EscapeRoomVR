﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRButtonContact : MonoBehaviour
{
    [SerializeField]
    bool NavigationPanel;
    [SerializeField]
    private char code;
    [SerializeField]
    private GameObject pushButton;
    [SerializeField]
    private VRButtonController buttonController;

    private AudioSource _as;

    void Awake() 
    {
        _as = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Activate button puch only if the button enters the trigger
        if (SceneManager.GetActiveScene().name != "Menu" && other.gameObject.GetInstanceID() == pushButton.GetInstanceID())
        {
            buttonController.ProcessPush(code);
        }
        else 
        {
            if (code == 4) GameManager.GetInstance().LoadScene("TestMechanics", 2);
            else if (code == 9) GameManager.GetInstance().QuitGame();
            else
            {
                GameManager.GetInstance().Difficulty = code;
                GameManager.GetInstance().SetDifficultyTimming();
                GameManager.GetInstance().LoadScene("SalaCentral", 2);
            }
        }
        if (NavigationPanel)  GameplayManager.GetInstance().NextScene();
        _as.Play();
    }
}
