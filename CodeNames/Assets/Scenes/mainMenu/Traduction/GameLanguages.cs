using System;
using UnityEngine;
using System.Collections.Generic;

public class GameLanguages : MonoBehaviour
{
	public static GameLanguages Instance;
	public static Dictionary<String, String> Champs;
	[SerializeField] string LangueParDefault = "en";


	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

		ChargementLanguage ();
	}


	void ChargementLanguage ()
	{
		if (Champs == null)
			Champs = new Dictionary<string, string> ();
		
		Champs.Clear ();

		string lang = PlayerPrefs.GetString ("_language", LangueParDefault);

		if (PlayerPrefs.GetInt ("_language_index", -1) == -1)
			PlayerPrefs.SetInt ("_language_index", 0);

		string allTexts = (Resources.Load (@"Traduction/" + lang) as TextAsset).text;

		string[] lines = allTexts.Split (new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		string key, value;

		for (int i = 0; i < lines.Length; i++) {
			if (lines [i].IndexOf ("=") >= 0 && !lines [i].StartsWith ("#")) {
				key = lines [i].Substring (0, lines [i].IndexOf ("="));
				value = lines [i].Substring (lines [i].IndexOf ("=") + 1,
				lines [i].Length - lines [i].IndexOf ("=") - 1).Replace ("\\n", Environment.NewLine);
				Champs.Add (key, value);
			}
		}
	}

	public static string Traduction (string key)
	{
		if (!Champs.ContainsKey (key)) {
			Debug.LogError ("pas de clé a ce nom [" + key);
			return null;
		}

		return Champs [key];
	}


}
