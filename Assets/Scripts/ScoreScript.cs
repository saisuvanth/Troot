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
    scoreText.text = gameManager.P1score.ToString() + "/" + gameManager.P2score.ToString();
    scoreBarMask.GetComponent<Image>().fillAmount = (float)gameManager.P1score / ((float)gameManager.P1score + (float)gameManager.P2score);
  }
}
