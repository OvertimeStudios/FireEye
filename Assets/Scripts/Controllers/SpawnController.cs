using UnityEngine;
using System.Collections;

public enum Lifestream
{
	Small = 1,
	Medium = 5,
	Big = 25
}
public class SpawnController : MonoBehaviour 
{

	#region enemies
	public float minionsSpread;
	
	public Transform[] spawnPoints;
	
	public GameObject[] enemiesPrefab;
	private static ArrayList enemiesPool;
	public static ArrayList enemiesInGame;
	#endregion


	#region lifestream
	public Sprite[] lifestreamSprites;
	public GameObject lifestreamPrefab;
	private ArrayList lifestreamsPool;
	public float lifestreamSpread;
	public float lifestreamValueMultiplier;
	#endregion lifestream

	#region damage
	public GameObject damagePrefab;
	private ArrayList damagesPool;
	#endregion

	[HideInInspector]
	public static Transform uiRoot;
	
	private static SpawnController instance;
	public static SpawnController Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		enemiesPool = new ArrayList ();
		enemiesInGame = new ArrayList ();
		lifestreamsPool = new ArrayList ();
		damagesPool = new ArrayList ();
		instance = this;

		uiRoot = GameObject.Find ("UI Root").transform;
	}

	#region enemies
	public void SpawnEnemies(byte quantity)
	{
		for(byte i = 0; i < quantity; i++)
		{
			Transform enemy = GetEnemyFromPool();
			Debug.Log("spawn");
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

					return e;
				}
			}
		}

		//if reach this line, no enemies in pool matched criteria
		//so, spawn new enemie
		e = (Instantiate(enemiesPrefab[(int)element], spawnPoint.position + new Vector3(Random.Range((float)-minionsSpread, (float)+minionsSpread), 0), spawnPoint.rotation) as GameObject).transform;

		enemiesInGame.Add(e);
		enemiesPool.Add(e);
		return e;
	}

	#endregion

	#region lifestream
	public void SpawnLifestream(Vector3 pos)
	{
		int quantity = GameController.Instance.GetCoinDrop ();

		//SMALL LIFESTREAMS
		int ls = GetSmallLifestream(quantity);
		for(byte i = 0; i < ls; i++)
		{
			Transform lifestream = GetLifestreamFromPool();
			lifestream.GetComponent<SpriteRenderer>().sprite = lifestreamSprites[0];
			lifestream.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			lifestream.position = pos + new Vector3(Random.Range(-(float)lifestreamSpread, +(float)lifestreamSpread), 0);
		}

		//MEDIUM LIFESTREAMS
		ls = GetMediumLifestream(quantity);

		for(byte i = 0; i < ls; i++)
		{
			Transform lifestream = GetLifestreamFromPool();
			lifestream.GetComponent<SpriteRenderer>().sprite = lifestreamSprites[1];
			lifestream.localScale = new Vector3(0.4f, 0.4f, 0.4f);
			lifestream.position = pos + new Vector3(Random.Range(-(float)lifestreamSpread, +(float)lifestreamSpread), 0);
		}

		//BIG LIFESTREAMS
		ls = GetBigLifestream(quantity);

		for(byte i = 0; i < ls; i++)
		{
			Transform lifestream = GetLifestreamFromPool();
			lifestream.GetComponent<SpriteRenderer>().sprite = lifestreamSprites[2];
			lifestream.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			lifestream.position = pos + new Vector3(Random.Range(-(float)lifestreamSpread, +(float)lifestreamSpread), 0);
		}
	}

	private int GetSmallLifestream(int quantity)
	{
		return (int)((quantity - (GetMediumLifestream(quantity) * (int)Lifestream.Medium) - (GetBigLifestream(quantity) * (int)Lifestream.Big))/(int)Lifestream.Small);
	}

	private int GetMediumLifestream(int quantity)
	{
		return (int)((quantity - (GetBigLifestream(quantity) * (int)Lifestream.Big))/(int)Lifestream.Medium);
	}

	private int GetBigLifestream(int quantity)
	{
		return (int)(Mathf.Floor(quantity / (int)Lifestream.Big));
	}

	private Transform GetLifestreamFromPool()
	{
		Transform lifestream = null;

		for(byte i = 0; i < lifestreamsPool.Count; i++)
		{
			if(!((Transform)lifestreamsPool[i]).gameObject.activeInHierarchy)
			{
				lifestream = lifestreamsPool[i] as Transform;
				lifestream.gameObject.SetActive(true);
			}
		}

		if(lifestream == null)
		{
			GameObject go = Instantiate(lifestreamPrefab) as GameObject;
			
			lifestream = go.transform;
			
			lifestreamsPool.Add(lifestream);
		}


		return lifestream;
	}
	#endregion

	#region damage
	public void SpawnDamage(float damage, Vector3 position)
	{
		Transform dmg = GetDamageFromPool();
		dmg.position = position;

		dmg.GetComponent<UILabel> ().text = damage.ToString ();

		dmg.GetComponent<Damage> ().SetAnimation ();
	}

	private Transform GetDamageFromPool()
	{
		Transform d = null;

		for(byte i = 0; i < damagesPool.Count; i++)
		{
			Transform damage = damagesPool[i] as Transform;

			if(!damage.gameObject.activeInHierarchy)
			{
				d = damage;
				d.gameObject.SetActive(true);
				break;
			}
		}

		if(d == null)
		{
			d = (Instantiate(damagePrefab) as GameObject).transform;
			d.parent = uiRoot;

			damagesPool.Add(d);
		}

		return d;
	}
	#endregion
}