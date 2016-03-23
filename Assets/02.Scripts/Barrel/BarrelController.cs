using UnityEngine;
using System.Collections;

public class BarrelController : MonoBehaviour {

    public GameObject m_sparkEffect;
    public GameObject m_explosionEffect;
    public Texture[] m_textureList;

    Transform m_cachedTransform;
    int m_hitCount;

    void Awake()
    {
        m_hitCount = 0;        
        m_cachedTransform = gameObject.transform;
    }

	// Use this for initialization
	void Start ()
    {
        int idx = Random.Range(0, m_textureList.Length);
        MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.mainTexture = m_textureList[idx];
    }
	
	// Update is called once per frame
	void Update ()
    {        
	}
    
    void OnCollisionEnter(Collision collision)
    {
        if (m_sparkEffect == null)
            return;

        if (collision.collider.tag == "Bullet")
        {
            GameObject spark = (GameObject)Instantiate(m_sparkEffect, collision.transform.position, Quaternion.identity);

            Destroy(collision.gameObject);
            Destroy(spark, 0.5f);

            if (++m_hitCount >= 3)
            {
                ExplosionBarrel();
            }
        }
    }

    void ExplosionBarrel()
    {
        if (m_explosionEffect == null)
            return;

        GameObject explosionEffect = (GameObject)Instantiate(m_explosionEffect, m_cachedTransform.position, Quaternion.identity);
        Destroy(explosionEffect, 1.0f);

        Collider[] colliderList = Physics.OverlapSphere(m_cachedTransform.position, 10.0f);

        for (int i = 0; i < colliderList.Length; ++i)
        {
            Collider collider = colliderList[i];
            if (collider == null)
                continue;

            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody == null)
                continue;

            rigidbody.mass = 1.0f;
            rigidbody.AddExplosionForce(1000.0f, m_cachedTransform.position, 10.0f, 300.0f);
        }
    }

    void OnDamage(object[] _params)
    {
        Vector3 firePos = (Vector3)_params[0];
        Vector3 hitPos = (Vector3)_params[1];

        Vector3 incomeVector = hitPos - firePos;
        incomeVector.Normalize();

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForceAtPosition(incomeVector * 1000.0f, hitPos);
        
        if (++m_hitCount >= 3)
        {
            ExplosionBarrel();
        }
    }
}
