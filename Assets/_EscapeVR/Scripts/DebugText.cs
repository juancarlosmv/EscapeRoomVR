using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    Interacter left, right;
    TextMesh tx;
    // Start is called before the first frame update
    void Start()
    {
        tx = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        tx.text = "Debug\n" +
            "Left triggered: " + left.bFirstHeldClicked.ToString() + "\n" +
            "Left object: " + left.GetGrabber().GetGrabbedObject() + "\n" +
            "Right triggered: " + right.bFirstHeldClicked.ToString() + "\n" +
            "Right object: " + right.GetGrabber().GetGrabbedObject() + "\n";
    }
}
