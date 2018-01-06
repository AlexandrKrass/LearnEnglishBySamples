using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDropDown : MonoBehaviour {
	/// <summary>
	/// Controls button "change active group"
	/// </summary>
	public GroupsContainer container;
	public Dropdown dropDown; 

	// public GameObject inputFieldGO;
	// private InputField inputFieldData;

	// void OnEnable () {	}

	public void ChangeGroupClicked() {
		string newActiveGroup = dropDown.options[dropDown.value].text; 
		container.activeGroup = newActiveGroup;

		container.OutputGroup (newActiveGroup);
	}
}
