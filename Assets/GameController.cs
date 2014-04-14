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
		SpawnController.Instance.SpawnEnemies (GetWaveMonstersNumber());
	}

	private byte GetWaveMonstersNumber()
	{
		return (byte)Mathf.Min(10, wave + Mathf.Floor(wave * 0.4f));
	}

	public byte GetCoinDrop()
	{
		return (byte)(Mathf.Round ((Random.Range (0f, 1f) * (wave * 3))) + wave);
	}

	public void TrySendNextWave()
	{
		if (SpawnController.enemiesInGame.Count > 0) return;
		
		wave++;

		SpawnController.Instance.SpawnEnemies (GetWaveMonstersNumber());
	}

	public void CollectLifestream(Transform lifestream)
	{
		lifestream.gameObject.SetActive (false);
	}
}
