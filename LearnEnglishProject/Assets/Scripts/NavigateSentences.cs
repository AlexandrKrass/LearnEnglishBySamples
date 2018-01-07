using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LearningEnglishBySamples
{

public class NavigateSentences : MonoBehaviour {
		/// <summary>
		/// Controls for buttons assigned to change sample sentences.
		/// </summary>

	public GroupsContainer groupContainer;

	public Text upSentenceWin;
	public Text downSentenceWin;
	private int indexSentence = 0;
	private int countSentences = 0;
	private FetchSamples currentSentences = null;


	public void SetSamples(FetchSamples samples) {
		currentSentences = samples;
		indexSentence = 0;
		countSentences = samples.sentences.Count;

		if (countSentences > 1) {
			upSentenceWin.text = samples.sentences [0];
			downSentenceWin.text = samples.sentences [1];
		}
	}


	public void NextSentence() {


		if (currentSentences != null && countSentences > 1) {
			indexSentence+=2;
			indexSentence = indexSentence % countSentences;
			upSentenceWin.text = currentSentences.sentences [indexSentence];
			downSentenceWin.text = currentSentences.sentences [indexSentence+1];
		}
	}

	public void PreviousSentence() {
		if (currentSentences != null && countSentences > 1) {
			
			indexSentence-=2;
		if(indexSentence<0) 
			indexSentence = countSentences - 2;
			
			upSentenceWin.text = currentSentences.sentences [indexSentence];
			downSentenceWin.text = currentSentences.sentences [indexSentence+1];
		}		
	}
}

}
