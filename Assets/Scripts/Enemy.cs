using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public Elements element;
	public float vel;

	[HideInInspector]
	public bool knockbacking;

	private Vector2 velV2;

	private int dir;

	private Transform player;

	public float maxLife;
	[HideInInspector]
	public float life;

	//hold components
	private Rigidbody2D myRigidbody;
	private Transform myTransform;

	void Awake()
	{
		myRigidbody = rigidbody2D;
		myTransform = transform;

		velV2 = new Vector2 (vel, 0);
	}

	// Use this for initialization
	void Start () 
	{
		knockbacking = false;

		life = maxLife;
		player = GameObject.Find ("Howl").transform;
		dir = (int)((player.position.x - myTransform.position.x) / Mathf.Abs (player.position.x - myTransform.position.x));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (knockbacking) 
		{
			if(myRigidbody.velocity.x < 1f)
				knockbacking = false;
			else
				return;
		}

		myRigidbody.AddForce (velV2 * dir);
	}

	public void TakeDamage(float damage)
	{
		life -= damage;

		if (life <= 0)
			Dead ();
	}

	public void Knockback(float force, int direction)
	{
		knockbacking = true;

		myRigidbody.AddForce (new Vector2 (force * direction * 100, 0));
	}

	private void Dead()
	{
		gameObject.SetActive (false);
		SpawnController.enemiesInGame.Remove (myTransform);

		GameController.Instance.TrySendNextWave ();
	}
}
