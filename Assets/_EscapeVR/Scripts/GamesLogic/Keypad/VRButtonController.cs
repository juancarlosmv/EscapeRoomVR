using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButtonController : MonoBehaviour
{
    public virtual void ProcessPush(char code)
    {
        Debug.Log($"Button '{code}' pushed");
    }
}
