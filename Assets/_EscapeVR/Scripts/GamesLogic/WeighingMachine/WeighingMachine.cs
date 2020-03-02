using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeighingMachine : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] int _sum;
    bool ok = false;

    int _content = 0;

    void Update()
    {   
     _text.text = (_sum - _content).ToString();
        if (_content == _sum)
            CorrectWeight();
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weight"))
        {
            _content += other.gameObject.GetComponent<Weight>().WeightValue;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Weight"))
        {
            _content -= other.gameObject.GetComponent<Weight>().WeightValue;
        }
    }
    void CorrectWeight() 
    {
        ok = true;
        _text.color = Color.green;
        GetComponent<GrabableObj>().CanBeGrabbed = true;
        GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
    public bool IsOk => ok;
}
