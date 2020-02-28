using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayManager : MonoBehaviour
{
    #region singleton
    private static GameplayManager _instance;
    public static GameplayManager GetInstance() { return _instance; }
    #endregion
    [SerializeField] float _gameDuration = 60;
    float _gameTiming;

    void Start()
    {
        //if (InstructionsReaded)
        _gameTiming = Time.realtimeSinceStartup;
        if (_instance == null)
            _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        _gameTiming += Time.deltaTime;
        if (_gameTiming > _gameDuration)
        {
            //GameOver(true);
            Debug.Log("Se terminó");
        }
    }
    public string GetGameTimeString()
    {
        int minutes = Mathf.FloorToInt(_gameTiming / 60);
        int seconds = Mathf.FloorToInt(_gameTiming) % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
