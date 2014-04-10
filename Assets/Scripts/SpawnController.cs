using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour 
{
	public float spread;
	public Transform[] spawnPoints;
	
	public GameObject enemyPrefab;

	private ArrayList enemiesPool;
	private ArrayList enemiesInGame;

	private static SpawnController instance;
	public static SpawnController Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		enemiesPool = new ArrayList ();
		enemiesInGame = new ArrayList ();
		instance = this;
	}

	public void SpawnEnemies(byte quantity)
	{
		for(byte i = 0; i < quantity; i++)
		{
			int index = Random.Range(0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints[index];

			GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + new Vector3(Random.Range((float)-spread, (float)+spread), 0), spawnPoint.rotation) as GameObject;
		}
	}
}