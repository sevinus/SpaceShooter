using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform m_targetTransform;
    public float m_distance = 7.0f;    

    private Transform m_cachedTransform;
    private float m_curHeight;
    private float m_angle;

    void Awake()
    {
        m_cachedTransform = gameObject.GetComponent<Transform>();
        m_curHeight = 1.0f;
        m_angle = 10.0f;
    }

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void LateUpdate()
    {
        if (m_targetTransform == null)
            return;

        // 첫번째 방법
        float mouseY = Input.GetAxis("Mouse Y");
        if (mouseY != 0.0f)
        {
            m_angle -= mouseY * Time.deltaTime * 90.0f;
            m_angle = Mathf.Clamp(m_angle, 10.0f, 85.0f);
        }

        Vector3 targetPosition = m_targetTransform.position;
        Quaternion rotation = Quaternion.Euler(m_angle, m_targetTransform.rotation.y, m_targetTransform.rotation.z);        
        m_cachedTransform.position = targetPosition + (rotation * (-m_targetTransform.forward * m_distance));

        Vector3 result = (rotation * (-m_targetTransform.forward * m_distance));
        Debug.Log(string.Format("rotation x :{0}, y{1}, z{2}", rotation.x, rotation.y, rotation.z));
        Debug.Log(string.Format("result x :{0}, y{1}, z{2}", result.x, result.y, result.z));
        Debug.Log(string.Format("position x :{0}, y{1}, z{2}", m_cachedTransform.position.x, m_cachedTransform.position.y, m_cachedTransform.position.z));

        m_cachedTransform.LookAt(m_targetTransform.position);


        //// 두번째 방법
        //float mouseY = Input.GetAxis("Mouse Y");
        //if (mouseY != 0.0f)
        //{
        //    m_curHeight -= mouseY * Time.deltaTime * 10.0f;
        //    m_curHeight = Mathf.Clamp(m_curHeight, 1.0f, 10.0f);
        //}

        //Vector3 targetPosition = m_targetTransform.position;
        //Vector3 position = targetPosition - (m_targetTransform.forward * m_distance) + (Vector3.up * m_curHeight);
        //Vector3 movePosition = position - targetPosition;
        //Vector3 resultPosition = targetPosition + (movePosition.normalized * m_distance);

        ////RaycastHit hit;

        ////if (Physics.Raycast(transform.position, resultPosition, out hit, 10.0f))
        ////{
        ////    resultPosition = targetPosition + (movePosition.normalized * hit.distance);
        ////}

        //m_cachedTransform.position = resultPosition;
        //m_cachedTransform.LookAt(m_targetTransform.position);
    }
}
