using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [SerializeField]
    private FuseLocation[] locations;

    public bool IsCorrect()
    {
        bool correct = true;
        foreach(FuseLocation fuseLocation in locations)
        {
            correct = correct && fuseLocation.IsCorrect();
        }
        return correct;
    }
}
