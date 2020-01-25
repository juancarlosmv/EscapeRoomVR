using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCharacterControl : MonoBehaviour {
    public float m_force;
    public Transform m_movementDirection;
    Rigidbody m_rigidBody;

	// Use this for initialization
	void Start () {
        m_rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 primaryAxix = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 secondaryAxix = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Vector3 trueFordward = m_movementDirection.transform.forward;
        Vector3 trueRight = m_movementDirection.transform.right;
        trueFordward.y = 0;
        trueFordward.Normalize();
        m_rigidBody.AddForce(trueFordward * m_force * primaryAxix.y);
        trueRight.y = 0;
        trueRight.Normalize();
        m_rigidBody.AddForce(trueRight * m_force * primaryAxix.x);

        Vector3 trueUp = m_movementDirection.transform.up;
        trueUp.x = 0;
        trueUp.z = 0;
        trueUp.Normalize();
        Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles + trueUp * m_force * secondaryAxix.x / 3.0f);
        m_rigidBody.MoveRotation(rot);
    }
}
