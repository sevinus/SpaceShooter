using UnityEngine;
using System.Collections;

public class MonsterContoroller : MonoBehaviour {

    public enum MonsterState
    {
        idle,
        trace,
        attack,
        die
    }

    public float m_traceDist = 10.0f;
    public float m_attackDist = 2.0f;
    public MonsterState m_monsterState = MonsterState.idle;
    public GameObject m_bloodEffect;
    public GameObject m_bloodDecal;

    Transform m_monsterTransform;
    Transform m_playerTransform;
    NavMeshAgent m_naviMeshAgent;
    Animator m_animator;
    GameUI m_gameUI;
    bool m_isDie = false;
    int m_hp = 100;

    void Awake()
    {
        m_monsterTransform = gameObject.GetComponent<Transform>();
        m_playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_naviMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_animator = gameObject.GetComponent<Animator>();
        m_gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();

        //m_naviMeshAgent.destination = m_playerTransform.position;
    }

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    void OnEnable()
    {
        PlayerController.OnPlayerDie += OnPlayerDie;
    }

    void OnDisable()
    {
        PlayerController.OnPlayerDie -= OnPlayerDie;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            var contoller = collision.gameObject.GetComponent<BulletController>();
            m_hp -= contoller.m_damage;

            if (m_hp <= 0)
            {
                MonsterDie();
            }

            CreateBloodEffect(collision.transform.position);

            Destroy(collision.gameObject);
            m_animator.SetTrigger("IsHit");            
        }
    }

    void CreateBloodEffect(Vector3 position)
    {
        if (m_bloodEffect == null)
            return;

        GameObject bloodEffect = GameObject.Instantiate(m_bloodEffect, position, Quaternion.identity) as GameObject;
        Destroy(bloodEffect, 2.0f);

        Vector3 decalPos = m_monsterTransform.position + (Vector3.up * 0.05f);
        Quaternion decalRotate = Quaternion.Euler(90.0f, 0.0f, Random.Range(0, 360));

        GameObject bloodDecal = GameObject.Instantiate(m_bloodDecal, decalPos, decalRotate) as GameObject;
        float scale = Random.Range(1.0f, 3.0f);
        bloodDecal.transform.localScale *= scale;
        Destroy(bloodDecal, 5.0f);
    }

    void MonsterDie()
    {
        if (m_gameUI == null)
            return;

        StopAllCoroutines();

        m_isDie = true;
        m_monsterState = MonsterState.die;
        m_naviMeshAgent.Stop();
        m_animator.SetTrigger("IsMonsterDie");

        Collider[] colliderList = gameObject.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliderList.Length; ++i)
        {
            colliderList[i].enabled = false;
        }

        m_gameUI.AddScore(50);
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.tag);
    }

    IEnumerator CheckMonsterState()
    {
        while (m_isDie == false)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(m_playerTransform.position, m_monsterTransform.position);

            if (dist <= m_attackDist)
            {
                m_monsterState = MonsterState.attack;
            }
            else if (dist <= m_traceDist)
            {
                m_monsterState = MonsterState.trace;
            }
            else
            {
                m_monsterState = MonsterState.idle;
            }
        }
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        m_naviMeshAgent.Stop();
        m_animator.SetTrigger("IsPlayerDie");
    }

    IEnumerator MonsterAction()
    {
        while (m_isDie == false)
        {
            switch (m_monsterState)
            {
                case MonsterState.idle:
                    {
                        m_naviMeshAgent.Stop();
                        m_animator.SetBool("IsTrace", false);

                        break;
                    }

                case MonsterState.trace:
                    {
                        m_naviMeshAgent.destination = m_playerTransform.position;
                        m_naviMeshAgent.Resume();
                        m_animator.SetBool("IsTrace", true);
                        m_animator.SetBool("IsAttack", false);

                        break;
                    }

                case MonsterState.attack:
                    {
                        m_naviMeshAgent.Stop();
                        m_animator.SetBool("IsAttack", true);
                        break;
                    }

                case MonsterState.die:
                    {
                        break;
                    }
            }

            yield return null;
        }
    }
}
