using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FireController : MonoBehaviour
{
    public GameObject m_bullet;
    public Transform m_firePos;
    public MeshRenderer m_muzzleFlash;
    public AudioClip m_fireSfx;

    AudioSource m_audioSource;

    void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

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
        Debug.DrawRay(m_firePos.position, m_firePos.forward * 10.0f, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();

            RaycastHit hit;
            if (Physics.Raycast(m_firePos.position, m_firePos.forward, out hit, 10.0f))
            {
                if (hit.collider.tag == "Monster")
                {
                    object[] _params = new object[2];
                    _params[0] = hit.point;
                    _params[1] = 20;

                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
                else if (hit.collider.tag == "Barrel")
                {
                    object[] _params = new object[2];
                    _params[0] = m_firePos.position;
                    _params[1] = hit.point;

                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
	}

    void Fire()
    {
        //CreateBullet();
        PlaySound();
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

    void PlaySound()
    {
        if (m_audioSource == null)
            return;

        if (m_fireSfx == null)
            return;

        GameManager.instance.PlaySfx(m_firePos.position, m_fireSfx);
        //m_audioSource.PlayOneShot(m_fireSfx);
    }
}
