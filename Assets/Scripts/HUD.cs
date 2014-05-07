using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
	public UISprite timeSprite;

	private float maxTime;
	private float time;

	public GameObject heartPrefab;
	public UISprite manaFill;
	public UILabel lifestreamLabel;

	private static HUD instance;
	public static HUD Instance
	{
		get { return instance; }
	}
	// Use this for initialization
	void Start () 
	{
		instance = this;

		StartCoroutine (WaitForGameController ());
		timeSprite.gameObject.SetActive (false);

		lifestreamLabel.text = "0";
	}

	IEnumerator WaitForGameController()
	{
		while(GameController.Instance == null) yield return null;

		maxTime = GameController.Instance.skillTime;

		UIGrid grid = GetComponentInChildren<UIGrid> ();

		for(int i = 0; i < GameController.Instance.initialLife; i++)
		{
			NGUITools.AddChild(grid.gameObject, heartPrefab);
		}

		grid.Reposition ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(time > 0)
		{
			time -= Time.deltaTime;

			timeSprite.fillAmount = time / maxTime;

			if(time <= 0)
			{
				timeSprite.gameObject.SetActive (false);
				Player.Instance.BackToNeutral();
			}
		}

		if(Player.mana > 0)
		{
			Player.mana -= Time.deltaTime * GameController.Instance.manaConsumedPerSecond;

			if(Player.mana <= 0)
			{
				if(GameController.life > 0)
				{
					Player.mana += GameController.Instance.maxMana * GameController.Instance.manaRecoveredPerLife;
					LoseLife();
				}
				else
				{
					//GameOver();
				}
			}

			manaFill.fillAmount = Player.mana / GameController.Instance.maxMana;
		}
	}

	public void LoseLife()
	{
		GameController.life--;

		Transform grid = GetComponentInChildren<UIGrid> ().transform;

		UISprite heart = grid.GetChild (GameController.life).GetComponent<UISprite> ();

		heart.alpha = 0.5f;
	}

	public void UpdateLifestreamHUD()
	{
		lifestreamLabel.text = GameController.Instance.lifestreamCollected.ToString ();
	}

	public void ShowSkillTime(Elements element)
	{
		timeSprite.spriteName = element.ToString ().ToLower () + "_time";
		time = maxTime;

		timeSprite.gameObject.SetActive (true);
	}

	public void PauseClick()
	{
		//temp
		Application.LoadLevel ("Menus");
	}
}
