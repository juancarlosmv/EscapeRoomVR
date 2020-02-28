using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

    GameplayManager _gameManager;

    Text _counterText;

    void Awake()
    {
        _gameManager = FindObjectOfType<GameplayManager>();
        _counterText = GetComponent<Text>();
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        _counterText.text = _gameManager.GetGameTimeString(); ;
       
	}
}
