using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LearningEnglishBySamples
{

public class AddNewWords : MonoBehaviour {
	/// <summary>
	/// Controls button "add words"
	/// </summary>
	public GroupsContainer container;
	public GameObject inputFieldGO;
	private InputField inputFieldData;


	void OnEnable () {
		inputFieldData = inputFieldGO.GetComponent<InputField> ();		
	}


	public void AddWordsButtonClicked() {

		if (inputFieldGO.activeSelf) {
			// adds words to proper group in container

			container.AddWords (inputFieldData.text);
			inputFieldGO.SetActive (false);
		} else {
			// clean inputField
			inputFieldData.text = "";
			inputFieldGO.SetActive (true);
		}	
	}
}

}