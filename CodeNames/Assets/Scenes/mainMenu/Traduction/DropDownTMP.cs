using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DropDownTMP : MonoBehaviour
{
	[SerializeField] string[] myLangs;
	TMP_Dropdown drp;
	int index;

	void Awake ()
	{
		drp = this.GetComponent <TMP_Dropdown> ();
		int v = PlayerPrefs.GetInt ("_language_index", 0);
		drp.value = v;

		drp.onValueChanged.AddListener (delegate {
			index = drp.value;
			PlayerPrefs.SetInt ("_language_index", index);
			PlayerPrefs.SetString ("_language", myLangs [index]);
			Debug.Log ("langue changé " + myLangs [index]);
			ApplyLanguageChanges ();
		});
	}

	void ApplyLanguageChanges ()
	{
		SceneManager.LoadScene (0);
	}

	void OnDestroy ()
	{
		drp.onValueChanged.RemoveAllListeners ();
	}
}
