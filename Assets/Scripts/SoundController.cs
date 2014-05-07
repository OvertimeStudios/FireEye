using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour 
{
	public enum Music
	{
		Game
	}

	public AudioClip[] notes;
	private int currentNote;

	public AudioClip gameMusic;

	private AudioSource audioSource;

	private static SoundController instance;
	public static SoundController Instance
	{
		get { return instance; }
	}

	void Start()
	{
		instance = this;
		currentNote = 0;

		audioSource = GetComponent<AudioSource> ();
	}

	public void PlayMusic(Music music)
	{
		AudioClip clip = null;

		if(music == Music.Game)
			clip = gameMusic;

		audioSource.clip = clip;

		audioSource.Stop ();
		audioSource.Play ();
	}

	public void PlayNextNote()
	{
		audio.PlayOneShot (notes [currentNote]);

		StopCoroutine ("ResetNote");
		StartCoroutine ("ResetNote", notes [currentNote].length);

		currentNote++;

		if(currentNote > notes.Length - 1)
			currentNote = notes.Length - 1;
	}

	public IEnumerator ResetNote(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);

		currentNote = 0;
	}
}
