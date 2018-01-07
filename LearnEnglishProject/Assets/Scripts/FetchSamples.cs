using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using RestSharp.Contrib;

namespace LearningEnglishBySamples
{

public class FetchSamples : MonoBehaviour {
	/// <summary>
	/// Connects to linguiee server and fetches sentences.
	/// 
	/// Read html document by Reader
	/// Mathes startPattern0 with start of english sentence	
	/// Matces startPattern1 with start of translated sentence
	/// Matces endingPattern0 with end of sentence
	/// Excules any additional tags <, > inside the sentence
	/// 
	/// </summary>
	public Text inText;
    public Text outTextCnt; 
	public Text outTextDate;
	public GroupsContainer groupsContainer;
//	public Animation anim;

	public List<string> sentences; 

	private string wwwLink = "https://www.linguee.ru/english-russian/search?source=auto&query=";
	private string startPattern0  = "<td class='sentence left'>";
	private string startAltPattern0  = "<td class='sentence left warn'>";
	private string startPattern1  = "<td class='sentence right2'>";
	private string startAltPattern1  = "<td class='sentence right2 warn'>";
	private string endingPattern0 = "<div class='source_url_spacer'>";

	private char[] startP0;
	private char[] startP1;
	private char[] startAltP0;
	private char[] startAltP1;
	private char[] endP0;
	private char[] sentenceRead; // = new char[1024];
	private int cntSentenceRead = 0;

	public void LoadFromHtmlPage() {
		string lnk = wwwLink + inText.text.Trim();
		StartCoroutine (GetStringFromWebPage (lnk));
	}

	private IEnumerator GetStringFromWebPage(string url) {
		WWW webpage = new WWW (url);
		while (!webpage.isDone)
			yield return false;

		using (StringReader reader = new StringReader (webpage.text)) {
			sentenceRead = new char[2048];
			ParseSentences (reader);
			sentenceRead = null;
		}	



		// adding new word to dictionary async

		string newWord = inText.text;
		if(!groupsContainer.sentences.ContainsKey(newWord))
			groupsContainer.sentences.Add (newWord, sentences);		

		outTextCnt.text = (sentences.Count/2).ToString();

		// stop anim
		//if(anim!= null)
		//	anim.Stop();
	}

	private int indStartSub0 = 0;
	/// <summary>
	/// Determines whether a fixed subStr appears in the sequence on characters.
	/// </summary>
	/// <returns><c>true</c> if subStr read in the sequence on characters; otherwise, <c>false</c>.</returns>
	/// 
	/// <param name="c">c </param>
	/// <param name="subStr">subStr is reference to a fixed </param>
	bool IsStartOfSent0(char c, char[] subStr) {
		if (c != subStr [indStartSub0]) {
			indStartSub0 = 0;
			return false;
		} else if (c == subStr [indStartSub0] && indStartSub0 < subStr.Length-1) {
			indStartSub0++;
			return false;
		} else if (c == subStr [indStartSub0] && indStartSub0 == subStr.Length-1) {
			indStartSub0 = 0;
			return true;	
		}  
		return false;	
	} 


	private int indSubstr = 0;
	bool IsAltStartSubstr(char c, char[] subStr) {
		if (c != subStr [indSubstr]) {
			indSubstr = 0;
			return false;
		} else if (c == subStr [indSubstr] && indSubstr < subStr.Length-1) {
			indSubstr++;
			return false;
		} else if (c == subStr [indSubstr] && indSubstr == subStr.Length-1) {
			indSubstr = 0;
			return true;	
		}  
		return false;	
	}

	private bool isTagStarted = false;  // exclude everything tagged < >
	private bool isShortenedStarted = false;  // exclude [...] 

	private bool CanApennd(char c) {
		if (c == '<') {
			isTagStarted = true;
			return false;
		} else if (c == '>') {
			isTagStarted = false;
			return false;
		} else if (isTagStarted)
			return false;
		else if (c == '[') {
			isShortenedStarted = true; 
			return false;
		} else if (c == ']') {
			isShortenedStarted = false; 
			return false;
		} if (isShortenedStarted)
			return false; 
		else if (c == '\n')
			return false;
		else
			return true;
	}

	private void DoApennd(char c) {
		sentenceRead [cntSentenceRead] = c;
		cntSentenceRead++;
		if (cntSentenceRead == sentenceRead.Length)
			Debug.Log ("!!! Too long sentence !!!");
	}


	private void AddWordToList () {
		string s = new string (sentenceRead, 0, cntSentenceRead);
		cntSentenceRead = 0;

		sentences.Add ( HttpUtility.HtmlDecode(s.Trim()));
	}

	private void ParseSentences (StringReader sreader) {		

		char[] part = new char[256];  // buffer
		char chr;
		bool isEnglish = true; // first sentence is in English
		bool isReadingSent = false;

		int cntRead = sreader.ReadBlock (part, 0, 256);
		while (cntRead > 0) {

			for (int i = 0; i < cntRead; i++) {
				chr = part [i];	

				if (isReadingSent) {
					if (IsStartOfSent0 (chr, endP0)) {      // checks if ending found      
						isEnglish = !isEnglish;
						isReadingSent = false;
						AddWordToList ();
					} else if (CanApennd (chr)) {		
						DoApennd (chr);				
					}
				} else {					
					if (isEnglish) {
						if (IsStartOfSent0 (chr, startP0)  || IsAltStartSubstr(chr, startAltP0))   // begining found from left column 
							isReadingSent = true;
					} else 
						if (IsStartOfSent0 (chr, startP1)  || IsAltStartSubstr(chr, startAltP1))   // begining found from right column
							isReadingSent = true;	
				}
			}
			cntRead = sreader.ReadBlock (part, 0, 256);
		}
	}


	void Start() {
		if(sentences == null)
			sentences = new List<string>();
		startP0 = startPattern0.ToCharArray();
		startP1 = startPattern1.ToCharArray();

		startAltP0 = startAltPattern0.ToCharArray();
		startAltP1 = startAltPattern1.ToCharArray();
		endP0  = endingPattern0.ToCharArray();
	//	anim = GetComponent<Animation> ();
	}
}

}
