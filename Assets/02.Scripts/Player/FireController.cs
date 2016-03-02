using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour
{
    public GameObject m_bullet;
    public Transform m_firePosTransform;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
	}

    void Fire()
    {
        CreateBullet();
    }

    void CreateBullet()
    {
        if (m_bullet == null)
            return;

        if (m_firePosTransform == null)
            return;

        Instantiate(m_bullet, m_firePosTransform.position, m_firePosTransform.rotation);
    }
}
