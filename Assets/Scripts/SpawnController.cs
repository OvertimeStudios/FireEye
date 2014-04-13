using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour 
{
	public float spread;
	public Transform[] spawnPoints;
	
	public GameObject[] enemiesPrefab;

	private static ArrayList enemiesPool;
	public static ArrayList enemiesInGame;

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
			Transform enemy = GetEnemyFromPool();
		}
	}

	private Transform GetEnemyFromPool()
	{
		Transform e;

		int index = Random.Range(0, spawnPoints.Length);
		Transform spawnPoint = spawnPoints[index];

		Elements element = Element.GetRandomElement ();

		for(byte i = 0; i < enemiesPool.Count; i++)
		{
			e = enemiesPool[i] as Transform;

			if(!e.gameObject.activeInHierarchy)
			{
				Enemy enemy = e.GetComponent<Enemy>();

				if(enemy.element == element)
				{
					e.gameObject.SetActive(true);
					enemiesInGame.Add(e);

					Debug.Log("reused");

					return e;
				}
			}
		}

		//if reach this line, no enemies in pool matched criteria
		//so, spawn new enemie
		Debug.Log("create new");
		e = (Instantiate(enemiesPrefab[(int)element], spawnPoint.position + new Vector3(Random.Range((float)-spread, (float)+spread), 0), spawnPoint.rotation) as GameObject).transform;
		enemiesInGame.Add(e);
		enemiesPool.Add(e);
		return e;
	}
}