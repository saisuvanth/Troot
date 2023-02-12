using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
	public GameManager gameManager;
	public TextMeshProUGUI scoreText;
	public GameObject scoreBarMask;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.currentPlayer == (int)Player.P1)
		{
			scoreText.text = gameManager.P1score.ToString() + "/" + gameManager.P2score.ToString();
			scoreBarMask.GetComponent<Image>().fillAmount = (float)gameManager.P1score / ((float)gameManager.P1score + (float)gameManager.P2score);
		}
		else
		{
			scoreText.text = gameManager.P2score.ToString() + "/" + gameManager.P1score.ToString();
			scoreBarMask.GetComponent<Image>().fillAmount = (float)gameManager.P2score / ((float)gameManager.P1score + (float)gameManager.P2score);
		}

	}
}
