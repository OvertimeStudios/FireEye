using UnityEngine;
using System.Collections;


public enum Lifestreams
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
	#endregion lifestream

	#region damage
	public GameObject damagePrefab;
	private ArrayList damagesPool;
	#endregion

	#region ink stain
	public GameObject inkStainPrefab;
	private ArrayList inkStainPool;
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
		inkStainPool = new ArrayList ();
		instance = this;

		uiRoot = GameObject.Find ("UI Root").transform;
	}

	#region enemies
	public void SpawnEnemies(Wave wave)
	{
		byte quantity = 0;
		float rnd = Random.Range (0f, 1f);
		float quantityPercent = 0f;

		for(int i = 0; i < wave.monsterWaves.Length; i++)
		{
			MonsterWave mw = wave.monsterWaves[i];

			quantityPercent += mw.percent;
			if(rnd <= quantityPercent)
			{
				quantity = mw.quantity;
				break;
			}
		}

		Debug.Log ("Quantity: " + quantity);

		for(byte i = 0; i < quantity; i++)
		{
			Elements rndElement = wave.GetRandomElement();
			Transform enemy = GetEnemyFromPool(rndElement);
			int eIndex = wave.GetElementIndex(rndElement);

			Debug.Log("Spawning " + enemy.GetComponent<Enemy>().element + " monster");

			int index = Random.Range(0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints[index];

			enemy.position = spawnPoint.position + new Vector3((Random.Range(0f, 1f) * 2 * minionsSpread) - minionsSpread, 0);

			enemy.GetComponent<Enemy>().ChangeVelocity(wave.elements[eIndex].velocity);

			rnd = Random.Range(0f, 1f);

			if(rnd < wave.elements[eIndex].chanceToIncreaseVelocity)
			{
				enemy.GetComponent<Enemy>().ChangeVelocityMultiplier(wave.elements[eIndex].newVelocity);
			}
		}

		Debug.Log ("----- End Spawn Wave ----");
	}

	private Transform GetEnemyFromPool(Elements elem)
	{
		Transform e;

		Elements element = elem;

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
					enemy.Respawn();

					return e;
				}
			}
		}

		//if reach this line, no enemies in pool matched criteria
		//so, spawn new enemy

		e = (Instantiate(enemiesPrefab[(int)element]) as GameObject).transform;

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
			lifestream.GetComponent<Lifestream>().value = Lifestreams.Small;
			lifestream.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			lifestream.position = pos + new Vector3(Random.Range(-(float)lifestreamSpread, +(float)lifestreamSpread), 0);
		}

		//MEDIUM LIFESTREAMS
		ls = GetMediumLifestream(quantity);

		for(byte i = 0; i < ls; i++)
		{
			Transform lifestream = GetLifestreamFromPool();
			lifestream.GetComponent<SpriteRenderer>().sprite = lifestreamSprites[1];
			lifestream.GetComponent<Lifestream>().value = Lifestreams.Medium;
			lifestream.localScale = new Vector3(0.4f, 0.4f, 0.4f);
			lifestream.position = pos + new Vector3(Random.Range(-(float)lifestreamSpread, +(float)lifestreamSpread), 0);
		}

		//BIG LIFESTREAMS
		ls = GetBigLifestream(quantity);

		for(byte i = 0; i < ls; i++)
		{
			Transform lifestream = GetLifestreamFromPool();
			lifestream.GetComponent<SpriteRenderer>().sprite = lifestreamSprites[2];
			lifestream.GetComponent<Lifestream>().value = Lifestreams.Big;
			lifestream.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			lifestream.position = pos + new Vector3(Random.Range(-(float)lifestreamSpread, +(float)lifestreamSpread), 0);
		}
	}

	private int GetSmallLifestream(int quantity)
	{
		return (int)((quantity - (GetMediumLifestream(quantity) * (int)Lifestreams.Medium) - (GetBigLifestream(quantity) * (int)Lifestreams.Big))/(int)Lifestreams.Small);
	}

	private int GetMediumLifestream(int quantity)
	{
		return (int)((quantity - (GetBigLifestream(quantity) * (int)Lifestreams.Big))/(int)Lifestreams.Medium);
	}

	private int GetBigLifestream(int quantity)
	{
		return (int)(Mathf.Floor(quantity / (int)Lifestreams.Big));
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
				break;
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

	#region ink stain
	public void SpawnInkStain(Vector3 position, Vector3 scale)
	{
		Transform ink = GetInkStainFromPool();
		ink.position = position;
		
		ink.localScale = scale;
	}

	private Transform GetInkStainFromPool()
	{
		Transform ink = null;

		for(byte i = 0; i < inkStainPool.Count; i++)
		{
			if(!(inkStainPool[i] as Transform).gameObject.activeInHierarchy)
			{
				ink = inkStainPool[i] as Transform;
				ink.gameObject.SetActive(true);
				break;
			}
		}
		
		if(ink == null)
		{
			GameObject go = Instantiate(inkStainPrefab) as GameObject;
			
			ink = go.transform;
			
			inkStainPool.Add(ink);
		}
		
		
		return ink;
	}
	#endregion
}