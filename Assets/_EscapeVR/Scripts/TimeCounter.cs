using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

    Text _counterText;

    void Awake()
    {
        _counterText = GetComponent<Text>();
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.GetInstance().bEndGame) _counterText.text = GameManager.GetInstance().GetGameTimeString();
        else _counterText.text = GameManager.GetInstance().SetEndText(GameManager.GetInstance().bEndGame);
    }
}
