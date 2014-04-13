using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public byte wave = 1;

	private static GameController instance;
	public static GameController Instance
	{
		get { return instance; }
	}

	// Use this for initialization
	void Start () 
	{
		instance = this;
		SpawnController.Instance.SpawnEnemies (GetWaveMonstersNumber(wave));
	}

	private byte GetWaveMonstersNumber(byte wave)
	{
		return (byte)Mathf.Min(10, wave + Mathf.Floor(wave * 0.4f));
	}

	public void TrySendNextWave()
	{
		if (SpawnController.enemiesInGame.Count > 0) return;
		
		wave++;

		SpawnController.Instance.SpawnEnemies (GetWaveMonstersNumber(wave));
	}
}
