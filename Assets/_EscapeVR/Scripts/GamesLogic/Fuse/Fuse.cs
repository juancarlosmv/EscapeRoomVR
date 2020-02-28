using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public enum FuseColor { Red, Green, Blue };
    [SerializeField]
    private FuseColor color = FuseColor.Red;
    private FuseLocation location = null;
    public FuseColor Color => color;
}
