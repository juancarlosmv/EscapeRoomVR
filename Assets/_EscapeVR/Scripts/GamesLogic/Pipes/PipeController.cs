using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField]
    List<Vector3Int> positions;
    [SerializeField]
    List<Vector3Int> exits;
    float t;
    bool attached = false;
    Vector3 destinyP;
    Quaternion destinyR;
    static short numPipes = 0;
    public short id { get; private set; }

    void Start()
    {
        id = numPipes;
        numPipes++;
    }

    void Update()
    {
        if (attached)
        {
            transform.position = Vector3.Lerp(transform.position, destinyP, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, destinyR, t);
            t += Time.deltaTime;
        }
        else
        {
            t = 0.0f;
        }
    }

    public void Attach(Vector3 desP, Quaternion desR)
    {
        destinyP = desP;
        destinyR = desR;
        attached = true;
    }

    public void Detach()
    {
        attached = false;
    }

    public List<Vector3Int> Positions => positions;
    public List<Vector3Int> Exits => exits;
}
