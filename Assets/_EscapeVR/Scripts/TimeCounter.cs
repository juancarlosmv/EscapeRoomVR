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
	void Update () {

        _counterText.text = GameManager.GetInstance().GetGameTimeString();
    }
}
