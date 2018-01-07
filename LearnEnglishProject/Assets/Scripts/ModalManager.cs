using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LearningEnglishBySamples
{

public class ModalManager : MonoBehaviour {
		/// <summary>
		/// Displays modal panel in order to confirm words/group deletion.
		/// </summary>
	public GameObject panelModal;
	public GameObject listContainer;

	public Text question;
	public Text questionParam;

	public GroupsContainer container;

	private bool isGroupDelete = false;
	private bool isWordsDelete = false;
	private List<string> wordDeleting = new List<string> ();


	public void VisualizeModal () {

		if (container.groups [container.activeGroup].Count == 0) {
			
			question.text = "Are you sure want to delete group";
			questionParam.text = container.activeGroup;
			isGroupDelete = true;

			panelModal.SetActive (true);
		} else {
			wordDeleting.Clear ();
			string wordsDelFlat = "";


			foreach (Transform wordTrans in listContainer.transform) {
				RespondTouchEvents w = wordTrans.gameObject.GetComponent<RespondTouchEvents> (); 
				if (w.isSelected) {
					FetchSamples wr =  wordTrans.gameObject.GetComponent<FetchSamples> ();

					string wd = wr.inText.text;
					wordDeleting.Add (wd);
					wordsDelFlat += "  " + wd;

				}			
			}

			if (wordDeleting.Count > 0) {
				question.text = "Are you sure want to delete words";
				questionParam.text = wordsDelFlat;
				isWordsDelete = true;	

				panelModal.SetActive (true);						
			}		
		}
	}


	public void BtnConfirmClicked() {
	
		if (isGroupDelete) {

			container.RemoveGroup (container.activeGroup);
					
		} else if (isWordsDelete) {
			container.RemoveWords (container.activeGroup, wordDeleting);		
		} 

		isGroupDelete = false;
		isWordsDelete = false;

		panelModal.SetActive (false);	
	}

	public void BtnCancelClicked() {

		isGroupDelete = false;
		isWordsDelete = false;
		panelModal.SetActive (false);	
	}
}

}
