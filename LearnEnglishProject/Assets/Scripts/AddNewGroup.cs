using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewGroup : MonoBehaviour {
	/// <summary>
	/// Controls button "add words"
	/// </summary>
	public GroupsContainer container;
	public GameObject inputFieldGO;
	private InputField inputFieldData;

	void OnEnable () {
		inputFieldData = inputFieldGO.GetComponent<InputField> ();		
	}

	public void AddGroupButtonClicked() {

		if (inputFieldGO.activeSelf) {
			// adds words to proper group in container

			container.AddGroup(inputFieldData.text);
			inputFieldGO.SetActive (false);
		} else {
			// clean inputField
			inputFieldData.text = "";
			inputFieldGO.SetActive (true);
		}	
	}
}
