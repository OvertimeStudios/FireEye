using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public Elements element;
	public float vel;
	public float velOffScreen;
	private float velMultiplier = 1f;

	[HideInInspector]
	public bool knockbacking;

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
			myRigidbody.velocity = new Vector2(velOffScreen * dir * velMultiplier, myRigidbody.velocity.y);
		else
			myRigidbody.velocity = new Vector2(vel * dir * velMultiplier, myRigidbody.velocity.y);
	}

	public void Respawn()
	{
		velMultiplier = 1f;

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

	public void ChangeVelocity(float newVel)
	{
		vel = 1f;
		velOffScreen = 1f;

		vel *= newVel;
		velOffScreen *= newVel;
	}

	public void ChangeVelocityMultiplier(float factor)
	{
		velMultiplier = factor;
	}

	private void Dead()
	{
		gameObject.SetActive (false);

		Player.mana += GameController.Instance.manaRecoveredPerKill;

		SpawnController.enemiesInGame.Remove (myTransform);
		SpawnController.Instance.SpawnLifestream (myTransform.position);
	}
}
