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
    public GameObject bloodDecal;

    Transform m_monsterTransform;
    Transform m_playerTransform;
    NavMeshAgent m_naviMeshAgent;
    Animator m_animator;
    bool m_isDie = false;

    void Awake()
    {
        m_monsterTransform = gameObject.GetComponent<Transform>();
        m_playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_naviMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_animator = gameObject.GetComponent<Animator>();

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
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
