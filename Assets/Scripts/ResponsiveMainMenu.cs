using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveMainMenu : MonoBehaviour
{
	// [SerializeField]
	// public AudioSource audioSource;

	public RectTransform image;

	// void Awake()
	// {
	// 	audioSource.Play();
	// 	GameObject.DontDestroyOnLoad(audioSource);
	// }

	void Start()
	{
		if (Screen.width < Screen.height)
		{
			image.anchoredPosition = new Vector2(0, -300);

		}
	}
}
