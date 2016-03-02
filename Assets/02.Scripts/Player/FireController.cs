using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour
{
    public GameObject m_bullet;
    public Transform m_firePos;
    public MeshRenderer m_muzzleFlash;

	// Use this for initialization
	void Start ()
    {
        if (m_muzzleFlash != null)
        {
            m_muzzleFlash.enabled = false;
        }
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

        if (m_firePos == null)
            return;

        Instantiate(m_bullet, m_firePos.position, m_firePos.rotation);

        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        if (m_muzzleFlash == null)
            yield break;

        m_muzzleFlash.enabled = true;

        float scale = Random.Range(1.0f, 2.0f);
        m_muzzleFlash.transform.localScale = Vector3.one * scale;

        float angle = Random.Range(0.0f, 360.0f);
        m_muzzleFlash.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        m_muzzleFlash.enabled = false;
    }
}
