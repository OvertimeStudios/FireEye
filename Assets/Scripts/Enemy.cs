using UnityEngine;
using System.Collections;

public enum Element
{
	Water,
	Earth,
	Electric,
	Fire
}

public class Enemy : MonoBehaviour 
{
	public Element element;
	public Vector2 vel;

	private int dir;

	private Transform player;

	//hold components
	private Rigidbody2D myRigidbody;

	void Awake()
	{
		myRigidbody = rigidbody2D;
	}

	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Howl").transform;
		dir = (int)((player.position.x - transform.position.x) / Mathf.Abs (player.position.x - transform.position.x));
	}
	
	// Update is called once per frame
	void Update () 
	{
		myRigidbody.AddForce (vel * dir);
	}
}
