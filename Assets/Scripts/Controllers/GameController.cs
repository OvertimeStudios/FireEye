using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	private int wave = 0;
	public float skillTime = 6f;
	public float manaConsumedPerSecond = 1;
	public float maxMana = 100f;
	public int initialLife = 3;
	public float manaRecoveredPerLife = 0.3f;
	public float manaRecoveredPerKill = 5f;
	public float damageDealOnNeutralElements = 0.5f;
	public float manaLostOnCollision = 0.5f;
	public static int life;
	[HideInInspector]
	public int lifestreamCollected = 0;
	public float manaPerLifestreamCollected = 0.2f;
	public float lifestreamValueMultiplier = 1f;

	public float lvl1SkillCost = 8;
	public float lvl2SkillCost = 20;
	public float lvl3SkillCost = 50;
	public float lvl4SkillCost = 120;

	public Wave[] waves;
	public byte infinityMonsterQuantity = 10;
	private int infinityMonstersValue = 11;
	public float infinityVel = 1f;
	public float infinityVelIncrement = 0.1f;
	public float infinityVelPercent = 0.3f;
	public float infinityVelPercentIncrement = 0.1f;

	public GameObject FingerGesturePrefab;

	private static GameController instance;
	public static GameController Instance
	{
		get { return instance; }
	}
	
	// Use this for initialization
	void Start () 
	{
		instance = this;
		SpawnController.Instance.SpawnEnemies (GetNextWave());

		life = initialLife;

		if(FingerGestures.Instance == null)
		{
			Instantiate(FingerGesturePrefab);
		}
		FingerGestures.Instance.transform.GetComponent<ScreenRaycaster> ().Cameras [0] = GameObject.Find ("Main Camera").camera;

		SoundController.Instance.PlayMusic (SoundController.Music.Game);
	}
	
	private Wave GetNextWave()
	{
		Debug.Log ("----- Sending wave " + wave + " ------");

		Wave w;


		if(wave == 0) 
		{
			w = new Wave();
			w.Init();
		}
		else if(wave - 1 < waves.Length)
		{
			w = waves[wave - 1];
		}
		else
		{
			int waveInfinity = wave - waves.Length;

			ElementsMonsters[] e = new ElementsMonsters[4];
			e [0] = new ElementsMonsters ();
			e [0].Init (Elements.Fire, infinityVel + (waveInfinity * infinityVelIncrement), infinityVelPercent + (waveInfinity * infinityVelPercentIncrement));
			e [1] = new ElementsMonsters ();
			e [1].Init (Elements.Water, infinityVel + (waveInfinity * infinityVelIncrement), infinityVelPercent + (waveInfinity * infinityVelPercentIncrement));
			e [2] = new ElementsMonsters ();
			e [2].Init (Elements.Earth, infinityVel + (waveInfinity * infinityVelIncrement), infinityVelPercent + (waveInfinity * infinityVelPercentIncrement));
			e [3] = new ElementsMonsters ();
			e [3].Init (Elements.Energy, infinityVel + (waveInfinity * infinityVelIncrement), infinityVelPercent + (waveInfinity * infinityVelPercentIncrement));

			MonsterWave[] mw = new MonsterWave[1];
			mw[0] = new MonsterWave();
			mw[0].Init(infinityMonsterQuantity);

			w = new Wave();
			w.Init(e, mw);
		}

		return w;
	}
	
	public byte GetCoinDrop()
	{
		return (byte)(Mathf.Round ((Random.Range (0f, 1f) * (wave * 3))) + wave);
	}
	
	public void TrySendNextWave()
	{
		if (SpawnController.enemiesInGame.Count > 0) return;

		wave++;
		
		SpawnController.Instance.SpawnEnemies (GetNextWave());
	}
	
	public void CollectLifestream(Transform lifestream)
	{
		int lsValue = (int)lifestream.GetComponent<Lifestream> ().value;
		lifestreamCollected += (int)(lsValue * lifestreamValueMultiplier);
		Player.mana += lsValue * manaPerLifestreamCollected;

		lifestream.gameObject.SetActive (false);

		HUD.Instance.UpdateLifestreamHUD ();
	}
}

[System.Serializable]
public class Wave
{
	public string name;
	public ElementsMonsters[] elements;
	public MonsterWave[] monsterWaves;

	public Wave()
	{

	}

	public void Init()
	{
		ElementsMonsters[] e = new ElementsMonsters[4];
		e [0] = new ElementsMonsters ();
		e [0].Init (Elements.Fire);
		e [1] = new ElementsMonsters ();
		e [1].Init (Elements.Water);
		e [2] = new ElementsMonsters ();
		e [2].Init (Elements.Earth);
		e [3] = new ElementsMonsters ();
		e [3].Init (Elements.Energy);
		
		MonsterWave[] mw = new MonsterWave[1];
		mw [0] = new MonsterWave ();
		mw [0].Init ();
		
		elements = e;
		monsterWaves = mw;
	}

	public void Init(MonsterWave[] mw)
	{
		ElementsMonsters[] e = new ElementsMonsters[4];
		e [0] = new ElementsMonsters ();
		e [0].Init (Elements.Fire);
		e [1] = new ElementsMonsters ();
		e [1].Init (Elements.Water);
		e [2] = new ElementsMonsters ();
		e [2].Init (Elements.Earth);
		e [3] = new ElementsMonsters ();
		e [3].Init (Elements.Energy);

		elements = e;
		monsterWaves = mw;
	}

	public void Init(ElementsMonsters[] e, MonsterWave[] mw)
	{
		elements = e;
		monsterWaves = mw;
	}

	public Elements GetRandomElement()
	{
		float rnd = Random.Range (0f, 1f);
		float percent = 0f;
		Elements e = Elements.None;

		for(byte i = 0; i < elements.Length; i++)
		{
			percent += 1f / elements.Length;

			if(rnd <= percent)
			{
				e = elements[i].element;
				break;
			}
		}

		return e;
	}
}

[System.Serializable]
public class MonsterWave
{
	public string name;
	public byte quantity;
	public float percent;

	public MonsterWave()
	{

	}

	public void Init()
	{
		quantity = 1;
		percent = 1f;
	}

	public void Init(byte q)
	{
		quantity = q;
		percent = 1f;
	}

	public void Init(byte q, float p)
	{
		quantity = q;
		percent = p;
	}
}

[System.Serializable]
public class ElementsMonsters
{
	public string name;
	public Elements element;
	public float velocity;
	public float chanceToIncreaseVelocity;

	public ElementsMonsters()
	{

	}

	public void Init()
	{
		element = Element.GetRandomElement();
		velocity = 1f;
		chanceToIncreaseVelocity = 0;
	}

	public void Init(Elements e)
	{
		element = e;
		velocity = 1f;
		chanceToIncreaseVelocity = 0;
	}

	public void Init(Elements e, float v, float c)
	{
		element = e;
		velocity = v;
		chanceToIncreaseVelocity = c;
	}

	public void Init(float v, float c)
	{
		element = Element.GetRandomElement();
		velocity = v;
		chanceToIncreaseVelocity = c;
	}
}