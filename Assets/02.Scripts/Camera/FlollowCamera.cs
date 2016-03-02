using UnityEngine;
using System.Collections;

public class FlollowCamera : MonoBehaviour
{
    public Transform m_targetTransform;
    public float m_distance = 5.0f;
    public float m_height = 3.0f;
    public float m_dampTrace = 1.0f;

    private Transform m_cachedTransform;
    private Vector3 m_offset;

    void Awake()
    {
        m_cachedTransform = gameObject.GetComponent<Transform>();
        m_offset = m_targetTransform.position - transform.position;
    }
    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (m_targetTransform == null)
            return;

        m_cachedTransform.position = Vector3.Lerp(m_cachedTransform.position,
            m_targetTransform.position -
            (m_targetTransform.forward * m_distance) +
            (Vector3.up * m_height),
            Time.deltaTime * m_dampTrace);

        m_cachedTransform.LookAt(m_targetTransform.position);

        //float currentAngle = transform.eulerAngles.y;
        //float desiredAngle = m_targetTransform.eulerAngles.y;
        //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * m_dampTrace);

        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
        //transform.position = m_targetTransform.position - (rotation * m_offset);

        //transform.LookAt(m_targetTransform);
    }
}
