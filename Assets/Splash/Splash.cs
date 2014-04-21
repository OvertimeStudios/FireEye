using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour 
{
	private float duration;

	// Use this for initialization
	void Start () 
	{
		//MovieTexture mTexture = renderer.material.mainTexture as MovieTexture;
		//mTexture.Play ();
		audio.Play ();

		//duration = mTexture.duration;

		StartCoroutine (NextScene(duration));
	}

	IEnumerator NextScene(float seconds)
	{
		yield return new WaitForSeconds (seconds);

		Application.LoadLevelAsync ("FireEye");
	}
}
