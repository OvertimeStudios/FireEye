using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public byte wave = 1;

	// Use this for initialization
	void Start () 
	{
		SpawnController.Instance.SpawnEnemies (GetWaveMonstersNumber(wave));
	}

	private byte GetWaveMonstersNumber(byte wave)
	{
		return (byte)Mathf.Min(10, wave + Mathf.Floor(wave * 0.4f));
	}
}
