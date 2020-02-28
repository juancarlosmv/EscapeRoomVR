using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager GetInstance() { return _instance; }
    #endregion


    float _gameDuration = 60;
    int _difficulty = 0;
    string _scene;
    int _delay;
    
    public int Difficulty { set { _difficulty = value; } }
    public string Scene { set { _scene = value; } }
    public int Delay{ set { _delay = value; } }
    void Awake() 
    {
        _instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string scene, int delay=0) 
    {
        Scene= scene;
        Delay=delay;
        StartCoroutine("ChangeScene");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator ChangeScene() 
    {
        if (_delay > 0) yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_scene);
    }
}
