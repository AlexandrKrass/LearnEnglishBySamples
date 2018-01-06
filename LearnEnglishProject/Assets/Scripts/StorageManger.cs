using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;

public class StorageManger : MonoBehaviour {
	/// <summary>
	/// Loads and stores interested language words in PlayerPrefs. 
	/// Keeps track of newly added and removes words.
	/// 
	/// OnStart it loads from PlayerPrefs to GroupsContainer words stored on disc. 
	/// OnApplicationFocus(false), OnApplicationPause(true) updates PlayerPrefs by last changes 
	/// </summary>

	public  GroupsContainer groupsContainer; 
	private TimeSpan span;
	private bool isProcessingSL = false;


	//private List<string> groupNames;                         								 		   // lately added groups 
	private Dictionary<string, List<string> > group2Words = new Dictionary<string, List<string> >();    // lately added words 
	private Dictionary<string, List<string> > word2Sentences = new Dictionary<string, List<string> >(); // lately fetched sentences 


	void Start () {		
		
		if (!isProcessingSL)
			LoadPlayerPrefs ();
		else
			Debug.Log (" Still processing save/load on AWAKE");
		
		span = TimeSpan.FromSeconds (Time.time);
	}


	private void LoadPlayerPrefs ()
	{   

		if (isProcessingSL)
			Debug.Log ("load occures while processing");
		
		isProcessingSL = true;

		List<string> groupNames = Serializer.Load<List<string>>("LE_GroupNames");

		foreach (string groupName in groupNames) {
			List<string> group = Serializer.Load<List<string>>("GN_"+groupName);
			group2Words.Add (groupName, group);
		}

		List<string> wordsOnly = Serializer.Load<List<string>>("WO_Words");

		foreach (string word in wordsOnly) {
			List<string> sentences = Serializer.Load<List<string>>("SW_"+word);
			if(sentences!= null)
				word2Sentences.Add (word, sentences);
		}


		groupsContainer.groups = group2Words;
		groupsContainer.sentences = word2Sentences;
		groupsContainer.OutputFirstScreen ();
		isProcessingSL = false;

		//group2Words.Add ("All Groups", new List<string> ());
		//group2Words.Add ("Garbage", new List<string> ());
	}

	private void SavePlayerPrefs ()
	{
		if (isProcessingSL)
			Debug.Log ("save occures while processing");
		isProcessingSL = true;
	

		List<string> groupNames = new List<string>(group2Words.Keys);
		Serializer.Save("LE_GroupNames", groupNames);

		foreach (string groupName in groupNames) {
			Serializer.Save ("GN_"+groupName, group2Words [groupName]);						
		}

		List<string> wordsOnly = new List<string> ();

		foreach (List<string> group in group2Words.Values)
			wordsOnly.AddRange (group);   // check duplictes

		Serializer.Save("WO_Words", wordsOnly);

		foreach (string word in wordsOnly) {
			if (word2Sentences.ContainsKey (word)) {
				if(word2Sentences [word]!=null)
					Serializer.Save ("SW_" + word, word2Sentences [word]);
			}
		}
	
		isProcessingSL = false;
	
	}

	private string TimeFormated() {
		return string.Format ("{0:D2}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds);

	}

	void OnApplicationFocus(bool hasFocus) {
		span = TimeSpan.FromSeconds (Time.time);	
//		Debug.Log("hasFocus "+ TimeFormated() + " " + hasFocus);
		if (!hasFocus) { 
			Debug.Log("saved on focus lost at "+ TimeFormated());
			SavePlayerPrefs ();
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		span = TimeSpan.FromSeconds (Time.time);	
		if (pauseStatus) {
			Debug.Log ("saved on pause called at " + TimeFormated ());
			SavePlayerPrefs ();	
		}
	}
		
	void OnApplicationQuit() {
		span = TimeSpan.FromSeconds (Time.time);	
		Debug.Log ("saved on quit at " + TimeFormated ());
		SavePlayerPrefs ();
	}
}


/// <summary>
/// Store objects in PlayerPrefs
/// </summary>
static class Serializer {
	/// <summary>
	/// Serialize the <paramref name="item"/> and save it to PlayerPrefs under the <paramref name="key"/>.
	/// <paramref name="item"/> class must have the [Serializable] attribute. Use the
	/// [NonSerialized] attribute on fields you do not want serialized with the class.
	/// </summary>
	/// <param name="key">The key</param>
	/// <param name="item">The object</param>
	internal static void Save(string key, object item) {
		using (var stream = new MemoryStream()) {
			formatter.Serialize(stream, item);
			var bytes = stream.ToArray();
			var serialized = Convert.ToBase64String(bytes);
			PlayerPrefs.SetString(key, serialized);
		}
	}

	/// <summary>
	/// Load the <paramref name="key"/> from PlayerPrefs and deserialize it.
	/// </summary>
	/// <param name="key">The key</param>
	internal static T Load<T>(string key) {
		if (!PlayerPrefs.HasKey(key)) return default(T);

		var serialized = PlayerPrefs.GetString(key);
		var bytes = Convert.FromBase64String(serialized);

		T deserialized;
		using (var stream = new MemoryStream(bytes)) {
			deserialized = (T)formatter.Deserialize(stream);
		}
		return deserialized;
	}
	static readonly BinaryFormatter formatter = new BinaryFormatter();
}