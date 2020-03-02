using System.Collections;
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
        if (other.gameObject.GetInstanceID() == pushButton.GetInstanceID())
        {
            // Activate button puch only if the button enters the trigger
            if (SceneManager.GetActiveScene().name != "Menu" && other.gameObject.GetInstanceID() == pushButton.GetInstanceID() && !NavigationPanel)
            {
                buttonController.ProcessPush(code);
            }
            else if (NavigationPanel)
            {
                if (code == '9') GameManager.GetInstance().QuitGame();
                else if (code == 'm') GameManager.GetInstance().LoadScene("Menu");
                else GameplayManager.GetInstance().NextScene();
            }
            else if (SceneManager.GetActiveScene().name == "Menu")
            {
                if (code == '4') GameManager.GetInstance().LoadScene("TestMechanics", 2);
                else
                {
                    GameManager.GetInstance().Difficulty = code;
                    GameManager.GetInstance().SetDifficultyTimming();
                    GameManager.GetInstance().LoadScene("SalaCentral", 2);
                }
            }
            _as.PlayOneShot(_as.clip);
        }
    }
}
