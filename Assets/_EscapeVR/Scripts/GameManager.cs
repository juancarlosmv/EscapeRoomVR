using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    GameObject _canvas;
    [SerializeField]
    Text _text;


    #region Singleton
    private static GameManager _instance;
    public static GameManager GetInstance() { return _instance; }
    #endregion

    #region PrivateVariables
    float _gameTiming = 0;
    float _gameDuration = 900;
    char _difficulty = '1';
    string _scene;
    int _delay;
    Camera mainCamera;
    #endregion

    public char Difficulty { set { _difficulty = value; } }
    public string Scene { set { _scene = value; } }
    public int Delay{ set { _delay = value; } }
    public Camera MainCamera { set { mainCamera = value; } }
    public bool InGame;

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update() 
    {
        if (InGame) 
        {
            _gameTiming += Time.deltaTime;
            if (_gameTiming > _gameDuration) EndGame(false);
        }
    }

    public void LoadScene(string scene, int delay=0) 
    {
        Scene= scene;
        //if (scene == "Menu") _gameTiming = 0;
        Delay=delay;
        //StartCoroutine("ChangeScene");
        Invoke("ChangeScene", delay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    void ChangeScene() 
    {
        //if (_delay > 0) yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_scene);
    }
    public void SetDifficultyTimming() 
    {
        switch (_difficulty) 
        {
            case '1':
                _gameDuration = 2700;
                break;
            case '2':
                _gameDuration = 1800;
                break;
            case '3':
                _gameDuration = 1200;
                break;
        }
    }
    public string GetGameTimeString()
    {
        float _timeRemaining = _gameDuration - _gameTiming;
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining) % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void EndGame(bool win, string message="")
    {
        if (message == "")
            _text.text = win ? "You Win" : "GameOver";
        else
            _text.text = message;

        _canvas.SetActive(true);
        _canvas.GetComponent<Canvas>().worldCamera = mainCamera;
        _gameTiming = 0;
        InGame = false;
        Invoke("CerrarCanvas", 2);
        LoadScene("Menu",2);
    }
 
    public void CerrarCanvas()
    {
        _canvas.SetActive(false);
    }
}
