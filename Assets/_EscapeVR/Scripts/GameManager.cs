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
    float _gameTiming;
    float _gameDuration;
    int _difficulty = 0;
    string _scene;
    int _delay;
    bool flag = false;
    public int Difficulty { set { _difficulty = value; } }
    public string Scene { set { _scene = value; } }
    public int Delay{ set { _delay = value; } }
    public bool InGame;
    public bool bEndGame;

    void Awake(){_instance = this;}
    void Start(){DontDestroyOnLoad(this.gameObject);}
    void Update() 
    {
        if (InGame && !flag) 
        {
            flag = true;
            _gameTiming += Time.deltaTime;
            if (_gameTiming > _gameDuration)EndGame(false);
        }
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
    public void SetDifficultyTimming() 
    {
        switch (_difficulty) 
        {
            case 0:
                _gameDuration = 900;
                break;
            case 1:
                _gameDuration = 600;
                break;
            case 2:
                _gameDuration = 300;
                break;
        }
    }
    public string GetGameTimeString()
    {
        int minutes = Mathf.FloorToInt(_gameTiming / 60);
        int seconds = Mathf.FloorToInt(_gameTiming) % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void EndGame(bool win) 
    {
        bEndGame = win;
    }
    public string SetEndText(bool win) 
    {
       return  win ? "You win" : "Game Over";
    }
}
