using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float damage;
	public float knockbackForce;
	public float knockbackArea;

	public static float mana;

	private Transform myTransform;
	private SpriteRenderer spriteRenderer;

	public Elements element;

	private int minX;
	private int maxX;

	public Sprite[] animations;

	private static Player instance;
	public static Player Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		myTransform = transform;
		instance = this;
		element = Elements.Neutral;

		CameraFollow cameraFollow = GameObject.Find ("Main Camera").GetComponent<CameraFollow> ();

		spriteRenderer = transform.FindChild ("Body").GetComponent<SpriteRenderer> ();

		minX = (int)cameraFollow.minXAndY.x - 7;
		maxX = (int)cameraFollow.maxXAndY.x + 7;
	}

	void Start()
	{
		StartCoroutine (WaitForGameController ());
	}

	IEnumerator WaitForGameController()
	{
		while(GameController.Instance == null) yield return null;

		mana = GameController.Instance.maxMana;
	}

	//called at GesturesRecognition.OnCustomGesture()
	public void UsePower(Elements element)
	{
		Debug.Log ("Used: " + element.ToString ());

		this.element = element;

		mana -= GetSkillCost(element);

		HUD.Instance.ShowSkillTime (element);

		SoundController.SoundFX sFX;

		if(element == Elements.Fire)
			sFX = SoundController.SoundFX.Urith;
		else if(element == Elements.Water)
			sFX = SoundController.SoundFX.Inna;
		else if(element == Elements.Earth)
			sFX = SoundController.SoundFX.Damek;
		else
			sFX = SoundController.SoundFX.Raiden;

		SoundController.Instance.PlaySound (sFX);

		//animation
		spriteRenderer.sprite = animations [(int)element];

		StartCoroutine (Zoom ());
	}

	private IEnumerator Zoom()
	{
		CameraZoom.Instance.ZoomIn ();

		yield return new WaitForSeconds (1.5f);

		CameraZoom.Instance.ZoomOut ();
	}

	//calles when time is over (at HUD.cs)
	public void BackToNeutral()
	{
		element = Elements.Neutral;

		//animation
		spriteRenderer.sprite = animations [(int)element];
	}

	//called when user tap on screen and on an Enemy (at GesturesRecognition.OnTap)
	public void Teleport(GameObject enemyGO)
	{
		if(element == Elements.Neutral) return;

		Vector3 pos = new Vector3(Mathf.Clamp(enemyGO.transform.position.x, minX, maxX), myTransform.position.y);

		KnockbackNearbyEnemies (pos);

		//give damage only to the enemy tapped
		enemyGO.GetComponent<Enemy> ().TakeDamage(damage * Element.Multiplier(element, enemyGO.GetComponent<Enemy> ().element));

		//move player
		myTransform.position = pos;

		for(int i = 0; i < SpawnController.enemiesInGame.Count; i++)
		{
			Transform e = SpawnController.enemiesInGame[i] as Transform;
			Enemy enemy = e.GetComponent<Enemy>();

			enemy.RecalculateDirection();
		}

		GameController.Instance.TrySendNextWave ();
	}

	private void KnockbackNearbyEnemies(Vector3 pos)
	{
		//knockback near enemies
		for(int i = SpawnController.enemiesInGame.Count - 1; i >= 0; i--)
		{
			Transform e = SpawnController.enemiesInGame[i] as Transform;
			float dist = Mathf.Abs(pos.x - e.position.x);
			
			if(dist < knockbackArea)
			{
				int dir = (e.position.x - pos.x == 0) ? (int)((e.position.x - myTransform.position.x) / Mathf.Abs(e.position.x - myTransform.position.x)) : (int)((e.position.x - pos.x) / Mathf.Abs(e.position.x - pos.x));
				
				Enemy enemy = e.GetComponent<Enemy>();
				enemy.Knockback(knockbackForce, dir);
				
				//Debug.Log(element.ToString() + " against " + enemy.element + " = x" + Element.Multiplier(element, enemy.element));
				
				//enemy.TakeDamage(damage * Element.Multiplier(element, enemy.element));
			}
		}
	}

	private float GetSkillCost(Elements element)
	{
		float cost = 0;

		if(element == Elements.Earth || element == Elements.Water || element == Elements.Fire || element == Elements.Energy)
			cost = GameController.Instance.lvl1SkillCost;

		return cost;
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if(LayerMask.LayerToName(col.gameObject.layer) == "Enemy")
		{
			Player.mana -= GameController.Instance.manaLostOnCollision * GameController.Instance.maxMana;

			KnockbackNearbyEnemies(myTransform.position);
		}
	}
}
