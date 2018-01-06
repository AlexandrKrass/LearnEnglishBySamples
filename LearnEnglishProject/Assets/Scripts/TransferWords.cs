using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferWords : MonoBehaviour {

	public GameObject dropDown;
	public GameObject listContainer;
	public GroupsContainer container;
	public  Dropdown dropDownMain;

	private Dropdown dropDownElem;


	void OnEnable () {
		dropDownElem = dropDown.GetComponent<Dropdown> ();		
	}

	public void TransferBtnClicked() {
		
		if(!dropDown.activeSelf) {

			dropDownElem.value = dropDownMain.value;
			dropDownElem.RefreshShownValue ();
			dropDown.SetActive (true);
		} else {			
			string toGroup = dropDownElem.options [dropDownElem.value].text;
			List<string> selectedWords = new List<string> ();

			foreach (Transform wordTrans in listContainer.transform) {

				RespondTouchEvents wr =  wordTrans.gameObject.GetComponent<RespondTouchEvents> ();
				if (wr.isSelected) {

					FetchSamples fs =  wordTrans.gameObject.GetComponent<FetchSamples> ();
					string word = fs.inText.text;
					selectedWords.Add (word);
				}
			}

			container.TransferWords (toGroup, selectedWords);
			dropDown.SetActive (false);

		}	
	}

	public void DropdownGroupChanged() {

		dropDown.SetActive (false);

	}


}
