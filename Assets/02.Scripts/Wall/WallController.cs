using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

    public GameObject m_sparkEffect;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            GameObject spark = Instantiate(m_sparkEffect, collision.transform.position, Quaternion.identity) as GameObject;

            Destroy(collision.gameObject);
            Destroy(spark, 0.5f);
        }
    }
}
