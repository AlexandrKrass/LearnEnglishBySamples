using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearningEnglishBySamples
{

public class FetchNewWords : MonoBehaviour {
	/// <summary>
	/// Loads sentences for words opened in the list
	/// </summary>

	public GameObject listContainer;


	public void FetchWordsInLists() {
	
		foreach (Transform wordTrans in listContainer.transform) {
			//RespondTouchEvents w = wordTrans.gameObject.GetComponent<RespondTouchEvents> (); 
			//if (w.isSelected) {
			FetchSamples wr =  wordTrans.gameObject.GetComponent<FetchSamples> ();
			string wd = wr.inText.text;
			string cd = wr.outTextCnt.text;
			if (cd == "0") {
				wr.LoadFromHtmlPage ();
			//	Animation anim = wordTrans.gameObject.GetComponent<Animation> ();
			//	anim.Play ("LoadFromHtml");	
							
			}
			//}	
		}	
	}
}

}
