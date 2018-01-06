using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RespondTouchEvents : MonoBehaviour {
	
	public GameObject container;
	public Image imageBackground;
	public FetchSamples samples;
	public NavigateSentences navigate;

	public bool isSelected = false;


	public void ClickTap() {
		isSelected = !isSelected;

		Color c = imageBackground.color;
		if (isSelected) {
			c.a = 0.5f;
		} else {
			c.a = 0.333f;
		} 	
		imageBackground.color = c;

		navigate.SetSamples (samples);

	}


}
