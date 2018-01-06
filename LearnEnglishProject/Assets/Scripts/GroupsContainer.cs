using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;

public class GroupsContainer : MonoBehaviour {

	public GameObject container;     // UI panel where to put the group list of words
	public GameObject prefabElement;  // word's UI and component containnig sentences 
	public Dropdown dropDown;
	public Dropdown dropDownActions;

	public NavigateSentences navigate;

	public Dictionary<string, List<string> > groups;        // group -> words
	public Dictionary<string, List<string> > sentences;     // word -> sentences
//	public Dictionary<string, GameObject> 	 panels;        // word -> panel with sentences 

	public string activeGroup = ""; //"All Groups";


	public void OutputFirstScreen () {
		
			OutputGroupsDropdown ();
			// active group at the begining
		if(dropDown.options.Count>0) { 
			SwitchToGroup(0);
		}
	}

	public void SwitchToGroup(int groupIndex) {
		
		dropDown.value = groupIndex;
		dropDown.RefreshShownValue ();
		dropDownActions.value = groupIndex;
		dropDownActions.RefreshShownValue ();
		var grs = new List<string> (groups.Keys);  
		activeGroup = grs [groupIndex];

		OutputGroup (activeGroup);	
	}

	public void RemoveGroup(string groupDeleted) {
	
		groups.Remove (groupDeleted);

		foreach (Dropdown.OptionData opt in dropDown.options)
			if (opt.text == groupDeleted) { 
				dropDown.options.Remove (opt);
				break;
			}
			
		foreach (Dropdown.OptionData opt in dropDownActions.options)
			if (opt.text == groupDeleted) {
				dropDownActions.options.Remove (opt);
				break;
			}
		
		SwitchToGroup (0);
	}

	public void RemoveWords(string group, List<string> wordsDeleted) {

		foreach(string w in wordsDeleted)
			groups [group].Remove (w);

		OutputGroup (activeGroup);
	}



	public void AddWords(string newWords) {

		List<string> wordsOfActiveGroup = groups [activeGroup];

		foreach (string wr in newWords.Split('\n')) {
			if(wr.Trim() != "")
				wordsOfActiveGroup.Add(wr.Trim()); 
		}
		OutputGroup (activeGroup);
	}

	public void OutputGroup(string groupName) {
		
		// drop previous words if present
		// add words from groupName in the form of wordPanel
		foreach (Transform childTrans in container.transform) {
			Destroy (childTrans.gameObject);
		}

		/*if (groupName == "All Groups") {

			List<string> wordsAllGroups = new List<string> ();
			foreach (List<string> group in groups.Values)
				wordsAllGroups.Union(group);
			
			foreach (string selem in wordsAllGroups) { //wordsOfOneGroup) {
				GameObject go = (GameObject)Instantiate (prefabElement);
				go.GetComponent<FetchSamples> ().inText.text = selem;
				go.GetComponent<FetchSamples> ().outTextCnt.text = "0";
				go.transform.SetParent (container.transform);
			}

		} else {*/

			if (!groups.ContainsKey (groupName)) {
				Debug.Log ("No group with name " + groupName);	
			} else {		
				foreach (string selem in groups[groupName]) { //wordsOfOneGroup) {
					GameObject go = (GameObject)Instantiate (prefabElement);
					FetchSamples fs = go.GetComponent<FetchSamples> ();
					fs.groupsContainer = this;
					fs.inText.text = selem;					
					if (sentences.ContainsKey (selem)) {
						fs.sentences = sentences [selem];
						fs.outTextCnt.text = (fs.sentences.Count / 2).ToString();
					} else 
						fs.outTextCnt.text = "0";
				
					RespondTouchEvents rte = go.GetComponent<RespondTouchEvents> ();
					rte.container = container;
					rte.navigate = navigate;

					go.transform.SetParent (container.transform);
				}
			}
		// }
	}

	public void TransferWords(string toGroup, List<string> wordsTransfered) {

		if (!groups.ContainsKey (toGroup)) {
			Debug.Log ("!!! Cannot transfer words to noexisting group");
		} else
			foreach (string word in	wordsTransfered) {
				if (groups [activeGroup].Contains (word)) {
					groups [activeGroup].Remove (word);
					groups [toGroup].Add (word);				
				}			
			}

		OutputGroup (activeGroup);	
	}

	public void AddGroup(string newGroupName) {

		if (groups.ContainsKey (newGroupName)) {
			Debug.Log ("There is alredy a group with name " + newGroupName);
		} else if (newGroupName.Trim() == "") {
				Debug.Log ("Group name should not be empty");
		} else {			
			groups.Add (newGroupName, new List<string> ());
			dropDown.options.Add (new Dropdown.OptionData (newGroupName));
			dropDown.RefreshShownValue ();
			dropDownActions.options.Add (new Dropdown.OptionData (newGroupName));

			// SwitchToGroup ();
			// activeGroup = newGroupName;
		}

	}


	public void OutputGroupsDropdown() {

	//	dropDown.ClearOptions(); // predefined are always there
		foreach (string s in groups.Keys) {
			dropDown.options.Add (new Dropdown.OptionData (s));
			dropDownActions.options.Add (new Dropdown.OptionData(s));		
		}
	}
}
