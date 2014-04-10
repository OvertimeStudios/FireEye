using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour 
{
	private float duration;

	// Use this for initialization
	void Start () 
	{
		MovieTexture mTexture = (MovieTexture)renderer.material.mainTexture;
		mTexture.Play ();
		audio.Play ();

		duration = mTexture.duration;

		StartCoroutine (NextScene(duration));
	}

	IEnumerator NextScene(float seconds)
	{
		yield return new WaitForSeconds (seconds);

		Application.LoadLevelAsync ("FireEye");
	}
}
