using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private static int m_maxMonster = 10;

    public GameObject m_monsterPrefab = null;
    public bool m_isGameOver = false;
    public float m_sfxVolume = 1.0f;
    public bool m_isSfxMute = false;

    private Transform[] m_spawnPointList = null;    
    private float m_createTime = 2.0f;
    private List<GameObject> m_monsterPool = null;
    
    // Use this for initialization
    void Start ()
    {
        instance = this;
        m_monsterPool = new List<GameObject>();

        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        m_spawnPointList = spawnPoint.GetComponentsInChildren<Transform>();

        CreateMonsterObjectPool();
        StartCoroutine(CreateMonster());
    }
	
	// Update is called once per frame
	void Update ()
    {
        	
	}

    void CreateMonsterObjectPool()
    {
        for (int i = 0; i < m_maxMonster; ++i)
        {
            GameObject monsterObj = (GameObject)GameObject.Instantiate(m_monsterPrefab);
            monsterObj.name = string.Format("Monster_{0}", i);
            monsterObj.SetActive(false);
            m_monsterPool.Add(monsterObj);
        }
    }

    IEnumerator CreateMonster()
    {
        if (m_monsterPrefab == null)
            yield break;

        while (m_isGameOver == false)
        {
            if (m_isGameOver == true)
                yield break;

            for (int i = 0; i < m_monsterPool.Count; ++i)
            {
                GameObject monsterObj = m_monsterPool[i];
                if (monsterObj == null)
                    continue;

                if (monsterObj.activeSelf == true)
                    continue;

                int random = Random.Range(1, m_spawnPointList.Length);
                Quaternion rotation= Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
                
                monsterObj.transform.position = m_spawnPointList[random].position;
                monsterObj.transform.rotation = rotation;
                monsterObj.SetActive(true);
                break;
            }

            yield return new WaitForSeconds(m_createTime);
        }
    }

    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (m_isSfxMute == true)
            return;

        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.minDistance = 10.0f;
        audioSource.maxDistance = 30.0f;
        audioSource.volume = m_sfxVolume;
        audioSource.Play();

        GameObject.Destroy(soundObj, sfx.length);
    }
}
