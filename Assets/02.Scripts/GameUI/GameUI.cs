using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    public Text m_scoreText;
    private int m_totalScore = 0;

	// Use this for initialization
	void Start ()
    {
        m_totalScore = PlayerPrefs.GetInt("totalScore", 0);
        AddScore(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AddScore(int score)
    {
        if (m_scoreText == null)
            return;
        m_totalScore += score;
        m_scoreText.text = string.Format("SCORE <color=#FF0000>{0}</color>", m_totalScore);
        PlayerPrefs.SetInt("totalScore", m_totalScore);
    }
}
