using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    Rigidbody m_cachedRigidbody;
    Transform m_cachedTransform;

    public int m_damage = 20;
    float m_speed = 1000.0f;
    Vector3 m_startPos;

	// Use this for initialization
	void Start ()
    {
        m_cachedRigidbody = gameObject.GetComponent<Rigidbody>();
        m_cachedRigidbody.AddForce(transform.forward * m_speed);

        m_cachedTransform = gameObject.transform;
        m_startPos = m_cachedTransform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 movePos = m_cachedTransform.position - m_startPos;
        if (movePos.magnitude > 100.0f)
            Destroy(this.gameObject);
	}
}
