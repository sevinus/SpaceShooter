using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Anim
{
    public AnimationClip m_idle;
    public AnimationClip m_runForward;
    public AnimationClip m_runBackward;
    public AnimationClip m_runRight;
    public AnimationClip m_runLeft;

    public bool IsValid()
    {
        if (m_idle == null ||
            m_runForward == null ||
            m_runBackward == null ||
            m_runRight == null ||
            m_runLeft == null)
            return false;

        return true;
    }
}

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    public float m_moveSpeed = 10.0f;
    public float m_rotateSpeed = 100.0f;
    public Anim m_anim;
    public int m_hp;
    public Image m_hpBarImage;
    
    private Transform m_cahcedTransform;
    private Animation m_animation;
    private int m_startHp;    

    void Awake()
    {
        m_cahcedTransform = gameObject.GetComponent<Transform>();
        m_animation = gameObject.GetComponentInChildren<Animation>();
        m_hp = 100;
        m_startHp = m_hp;
    }

	// Use this for initialization
	void Start ()
    {
        if (m_animation == null ||
            m_anim.m_idle == null)
            return;

        m_animation.Play(m_anim.m_idle.name);
    }
	
	// Update is called once per frame
	void Update ()
    {
        PositionControl();
        AnimationContol();
    }

    void PositionControl()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.right * xAxis) + (Vector3.forward * yAxis);
        m_cahcedTransform.Translate(moveDir.normalized * m_moveSpeed * Time.deltaTime, Space.Self);

        float rotate = m_rotateSpeed * mouseX * 2.0f;
        m_cahcedTransform.Rotate(Vector3.up * rotate * Time.deltaTime, Space.Self);
    }

    void AnimationContol()
    {
        if (m_animation == null)
            return;

        if (m_anim == null ||
            m_anim.IsValid() == false)
            return;

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        if (xAxis < 0.0f)
        {
            m_animation.CrossFade(m_anim.m_runLeft.name, 0.3f);
        }
        else if (xAxis > 0.0f)
        {
            m_animation.CrossFade(m_anim.m_runRight.name, 0.3f);
        }
        else if (yAxis > 0.0f)
        {
            m_animation.CrossFade(m_anim.m_runForward.name, 0.3f);
        }
        else if (yAxis < 0.0f)
        {
            m_animation.CrossFade(m_anim.m_runBackward.name, 0.3f);
        }
        else
        {
            m_animation.CrossFade(m_anim.m_idle.name, 0.3f);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (m_hpBarImage == null)
            return;

        if (collider.tag == "Punch")
        {
            m_hp -= 10;
            m_hpBarImage.fillAmount = (float)m_hp / m_startHp;

            if (m_hp <= 0)
            {
                PlayerDie();
                m_hp = 0;
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die.");

        OnPlayerDie();

        GameManager.instance.m_isGameOver = true;

        //GameObject[] monsterList = GameObject.FindGameObjectsWithTag("Monster");

        //for (int i = 0; i < monsterList.Length; ++i)
        //{
        //    monsterList[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }
}
