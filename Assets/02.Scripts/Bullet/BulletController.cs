using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    Rigidbody m_cachedRigidbody;

    int m_damage = 20;
    float m_speed = 1000.0f;

	// Use this for initialization
	void Start ()
    {
        m_cachedRigidbody = gameObject.GetComponent<Rigidbody>();
        m_cachedRigidbody.AddForce(transform.forward * m_speed);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
