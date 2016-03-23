using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {

    private Transform m_cachedTransform;
    private LineRenderer m_lineRenderer;
    private RaycastHit m_hit;

    void Awake()
    {
        m_cachedTransform = gameObject.transform;
        m_lineRenderer = gameObject.GetComponent<LineRenderer>();

        m_lineRenderer.useWorldSpace = false;
        m_lineRenderer.enabled = false;
        m_lineRenderer.SetWidth(0.3f, 0.1f);
    }

	// Use this for initialization
	void Start ()
    {	
	}
	
	// Update is called once per frame
	void LateUpdate()
    {
        if (m_lineRenderer == null)
            return;
        
        Ray ray = new Ray(m_cachedTransform.position + (Vector3.up * 0.02f), m_cachedTransform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            m_lineRenderer.SetPosition(0, m_cachedTransform.InverseTransformPoint(ray.origin));

            if (Physics.Raycast(ray, out m_hit, 100.0f))
            {
                m_lineRenderer.SetPosition(1, m_cachedTransform.InverseTransformPoint(m_hit.point));
            }
            else
            {
                m_lineRenderer.SetPosition(1, m_cachedTransform.InverseTransformPoint(ray.direction * 100.0f));
            }

            StartCoroutine(ShowLaserBeam());
        }
	}

    IEnumerator ShowLaserBeam()
    {
        if (m_lineRenderer == null)
            yield break;

        m_lineRenderer.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        m_lineRenderer.enabled = false;
    }
}
