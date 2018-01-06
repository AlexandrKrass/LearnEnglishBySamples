using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSettings : MonoBehaviour {

	public Toggle toggleWater;
	public GameObject bgWater;


	public Toggle[] toggleFontSize;
	public Text[] changableFontSize;
	public Toggle[] toggleBGColor;
	public Material bgMaterial;
	public Material bgWaterMaterial;


	private int[] fontSizes = new int[4] {24,26,28,30}; 
	public void ChangeBGWater() {

		bgWater.SetActive (toggleWater.isOn);

	}

	public void ChangeFontSize() {
		for (int i = 0; i < toggleFontSize.Length; i++) {
			if (toggleFontSize [i].isOn) {
				foreach(Text txt in changableFontSize)
					txt.fontSize =	fontSizes [i];		
			}		
		}
	}

	public void ChangeBGColor() {
		foreach (Toggle tog in toggleBGColor) {
			if (tog.isOn) {
				ColorBlock cols = tog.colors;
				bgMaterial.color = cols.normalColor;
				bgWaterMaterial.color = cols.normalColor;
			}		
		}	
	}




}
