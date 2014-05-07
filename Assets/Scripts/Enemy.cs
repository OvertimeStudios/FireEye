using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public Elements element;
	public float vel;
	public float velOffScreen;

	[HideInInspector]
	public bool knockbacking;

	private Vector2 velV2;
	private Vector2 velOffScreenV2;

	private float dir;

	private Transform player;

	public float maxLife;
	[HideInInspector]
	public float life;

	private Camera mainCamera;

	//hold components
	private Rigidbody2D myRigidbody;
	private Transform myTransform;

	void Awake()
	{
		myRigidbody = rigidbody2D;
		myTransform = transform;

		velV2 = new Vector2 (vel, 0);
		velOffScreenV2 = new Vector2 (velOffScreen, 0);
	}

	// Use this for initialization
	void Start () 
	{
		knockbacking = false;

		life = maxLife;
		player = GameObject.Find ("Numees").transform;
		dir = Mathf.Round((player.position.x - myTransform.position.x) / Mathf.Abs (player.position.x - myTransform.position.x));

		mainCamera = GameObject.Find ("Main Camera").camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (knockbacking) 
		{
			if(Mathf.Abs(myRigidbody.velocity.x) < 0.5f)
			{
				knockbacking = false;
				RecalculateDirection();
			}
			else
				return;
		}

		if(IsOffscreen())
			myRigidbody.AddForce (velOffScreenV2 * dir);
		else
			myRigidbody.AddForce (velV2 * dir);
	}

	public void Respawn()
	{
		life = maxLife;
		RecalculateDirection ();
	}

	public void RecalculateDirection()
	{
		dir = Mathf.Round((player.position.x - myTransform.position.x) / Mathf.Abs (player.position.x - myTransform.position.x));

		if(float.IsNaN(dir))
			dir = 1;
	}

	public void TakeDamage(float damage)
	{
		life -= damage;

		if(damage > 0)
			SoundController.Instance.PlayNextNote ();

		//SpawnController.Instance.SpawnDamage (damage, myTransform.position);

		if (life <= 0)
			Dead ();
	}

	public void Knockback(float force, int direction)
	{
		knockbacking = true;

		myRigidbody.AddForce (new Vector2 (force * direction * 100, 0));
	}

	private bool IsOffscreen()
	{
		Vector3 size = renderer.bounds.size;
		
		// Here is the definition of the boundary in world point
		var distance = (myTransform.position - mainCamera.transform.position).z;
		
		var leftBorder = mainCamera.ViewportToWorldPoint (new Vector3 (0, 0, distance)).x - (size.x);
		var rightBorder = mainCamera.ViewportToWorldPoint (new Vector3 (1, 0, distance)).x + (size.x);

		if(myTransform.position.x < leftBorder || myTransform.position.x > rightBorder)
			return true;

		return false;
	}

	private void Dead()
	{
		gameObject.SetActive (false);

		Player.mana += GameController.Instance.manaRecoveredPerKill;

		SpawnController.enemiesInGame.Remove (myTransform);
		SpawnController.Instance.SpawnLifestream (myTransform.position);
	}
}
