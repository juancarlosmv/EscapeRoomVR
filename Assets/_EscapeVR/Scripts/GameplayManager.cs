using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameplayManager : MonoBehaviour
{
    #region singleton
    private static GameplayManager _instance;
    public static GameplayManager GetInstance() { return _instance; }
    #endregion

    [SerializeField] GameObject _game;
    [SerializeField] GameObject _navigationPanel;
    [SerializeField] Text _instructions;
    AudioSource _as;
    
    void Awake() 
    {
        if (_instance == null)
            _instance = this;
        _as = GetComponent<AudioSource>();
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name=="SalaCentral"){GameManager.GetInstance().InGame = true;}
    }

    void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "SalaCentral":
                GameManager.GetInstance().InGame = true;
                if (_game.GetComponent<FuseBox>().IsCorrect()) 
                {
                    _navigationPanel.SetActive(true);
                    _instructions.text = "Go to the Machine Room to active the flotation turbines before the submarine sinks";
                    _as.Play();
                }
                break;
            case "SalaMaquinas":
                break;
            case "SalaAlmacen":
                break;
            case "Taller":
                break;
        }
    }
    public void NextScene()
    {
        string next="";
        switch (SceneManager.GetActiveScene().name)
        {
          case "SalaCentral":
              next = "SalaMaquinas";
                break;
            case "SalaMaquinas":
                next = "Taller";
                break;
            case "Taller":
                next = "Almacen";
                break;
        }
        GameManager.GetInstance().LoadScene(next);
    }
}
