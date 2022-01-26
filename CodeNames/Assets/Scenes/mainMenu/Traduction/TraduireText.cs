using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]
public class TraduireText : MonoBehaviour
{
	[SerializeField] string key;

	void Start ()
	{
		GetComponent <Text> ().text = GameLanguages.Traduction (key);
	}
}
