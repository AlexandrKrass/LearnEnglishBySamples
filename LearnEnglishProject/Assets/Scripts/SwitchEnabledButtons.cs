using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LearningEnglishBySamples {
	
	public class SwitchEnabledButtons : MonoBehaviour {

		/// <summary>
		/// Switch buttons to interactive/noninteractive states
		/// </summary>

		public Button btnTrash;  // 
		public Button btnTransfer;
		public Button btnNavigateLeft;
		public Button btnNavigateRight;
		public Button btnAddWords;
		public Button btnFetchNew;

		public Transform container;
		public Dropdown mainDropDown;

		private bool isSomeWordSelected;
		private bool isContainerEmpty;
		private bool isNoGroup;
		private bool isOneGroup;
		private bool isNoNewWord;
		private bool isKnownWordSelected;

		public void CheckEnabledButtons() {

			SetBoolStates ();

			btnTrash.interactable = CheckTrashEnabled ();

			btnTransfer.interactable = CheckTransferEnabled();

			bool isNav = CheckNavigateEnabled ();

			btnNavigateLeft.interactable = isNav;
			btnNavigateRight.interactable = isNav;

			btnAddWords.interactable = CheckAddWordsEnabled ();
			btnFetchNew.interactable = CheckFetchNewEnabled ();		
		}

		private void SetBoolStates () {
			isContainerEmpty = (container.childCount == 0);
			isNoGroup = (mainDropDown.options.Count == 0);
			isOneGroup =  (mainDropDown.options.Count == 1);

			isSomeWordSelected = false;
			isNoNewWord = true;
			isKnownWordSelected = false;
			foreach (Transform t in container) {
				FetchSamples sample = t.gameObject.GetComponent<FetchSamples> ();
				RespondTouchEvents tch = t.gameObject.GetComponent<RespondTouchEvents> ();

				if (sample.outTextCnt.text == "0")
					isNoNewWord = false;
				else if(tch.isSelected)
					isKnownWordSelected = true;
				
				if (tch.isSelected)
					isSomeWordSelected = true;
				
			}		
		} 

		private bool CheckTrashEnabled () {			
			return isContainerEmpty || isSomeWordSelected;
		}

		private bool CheckTransferEnabled () {
			return (!isOneGroup) && isSomeWordSelected;
		}

		private bool CheckNavigateEnabled () {  // check if last selected word has samples
			return isKnownWordSelected;
		}

		private bool CheckAddWordsEnabled () {
			return !isNoGroup;
		}


		private bool CheckFetchNewEnabled () {
			return !isNoNewWord;
		}
	}
}
