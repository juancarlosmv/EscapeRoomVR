using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeighingMachine : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] int _sum;

    int _content = 0;

    void Update()
    {   
     _text.text = _content.ToString();
        if (_content == _sum)
            CorrectWeight();
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weight"))
        {
            _content += other.gameObject.GetComponent<Weight>().WeightValue;
            other.gameObject.GetComponent<Weight>().OnMachine = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Weight"))
        {
            _content -= other.gameObject.GetComponent<Weight>().WeightValue;
            other.gameObject.GetComponent<Weight>().OnMachine = false;
        }
    }
    void CorrectWeight() 
    {
        _text.color = Color.green;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
