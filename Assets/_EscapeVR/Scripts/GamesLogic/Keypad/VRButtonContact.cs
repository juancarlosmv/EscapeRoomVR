using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRButtonContact : MonoBehaviour
{
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
            GameManager.GetInstance().Difficulty = code;
            GameManager.GetInstance().LoadScene("SalaCentral",2);
        }
        _as.Play();
    }
}
