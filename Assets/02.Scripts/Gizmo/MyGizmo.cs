using UnityEngine;
using System.Collections;

public class MyGizmo : MonoBehaviour
{
    public Color m_color = Color.yellow;
    public float m_radius = 0.1f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = m_color;
        Gizmos.DrawSphere(transform.position, m_radius);
    }
}
