using UnityEngine;
using TMPro;

[RequireComponent (typeof(TMP_Text))]
public class TraduireTMP : MonoBehaviour
{
	[SerializeField] string key;

	void Start ()
	{
		GetComponent <TMP_Text> ().text = GameLanguages.Traduction (key);
	}
}
