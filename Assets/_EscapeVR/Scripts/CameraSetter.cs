using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    private void Start()
    {
        GameManager.GetInstance().MainCamera = GetComponent<Camera>();
    }
}
