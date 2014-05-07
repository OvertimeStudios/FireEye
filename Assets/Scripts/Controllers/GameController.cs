using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	[HideInInspector]
	public byte wave = 1;
	public float skillTime = 6f;
	public float manaConsumedPerSecond = 1;
	public float maxMana = 100f;
	public int initialLife = 3;
	public float manaRecoveredPerLife = 0.3f;
	public float manaRecoveredPerKill = 5f;
	public float damageDealOnNeutralElements = 0.5f;
	public static int life;
	[HideInInspector]
	public int lifestreamCollected = 0;
	public float manaPerLifestreamCollected = 0.2f;
	public float lifestreamValueMultiplier = 1f;

	public float lvl1SkillCost = 8;
	public float lvl2SkillCost = 20;
	public float lvl3SkillCost = 50;
	public float lvl4SkillCost = 120;

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
		SpawnController.Instance.SpawnEnemies (GetWaveMonstersNumber());

		life = initialLife;

		if(FingerGestures.Instance == null)
		{
			Instantiate(FingerGesturePrefab);
		}
		FingerGestures.Instance.transform.GetComponent<ScreenRaycaster> ().Cameras [0] = GameObject.Find ("Main Camera").camera;

		SoundController.Instance.PlayMusic (SoundController.Music.Game);
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
		int lsValue = (int)lifestream.GetComponent<Lifestream> ().value;
		lifestreamCollected += (int)(lsValue * lifestreamValueMultiplier);
		Player.mana += lsValue * manaPerLifestreamCollected;

		lifestream.gameObject.SetActive (false);

		HUD.Instance.UpdateLifestreamHUD ();
	}
}