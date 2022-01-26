using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DropDownTranslate : MonoBehaviour
{
	[SerializeField] string[] myLangs;

	Dropdown drp;
	int index;

	void Awake ()
	{
		drp = this.GetComponent <Dropdown> ();
		int v = PlayerPrefs.GetInt ("_language_index", 0);
		drp.value = v;

		drp.onValueChanged.AddListener (delegate {
			index = drp.value;
			PlayerPrefs.SetInt ("_language_index", index);
			PlayerPrefs.SetString ("_language", myLangs [index]);
			Debug.Log ("language changed to " + myLangs [index]);
			//apply changes
			ApplyLanguageChanges ();
		});
	}

	void ApplyLanguageChanges ()
	{
		if(SceneManager.GetActiveScene().buildIndex == 0)
			Loader.LoadGame(); //Pas sûr de pouvoir garder les thèmes quand on recharge une toute nouvelle scène
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void OnDestroy ()
	{
		drp.onValueChanged.RemoveAllListeners ();
	}
}
