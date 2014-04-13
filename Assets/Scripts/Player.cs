using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float damage;
	public float attackArea;
	public float knockbackForce;
	public float knockbackArea;

	private Transform myTransform;

	private static Player instance;
	public static Player Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		myTransform = transform;
		instance = this;
	}

	public void UsePower(Elements element)
	{
		Debug.Log ("Used: " + element.ToString ());

		Transform closestEnemy = GetClosestEnemy (element);

		if (closestEnemy == null) return;

		Enemy enemy = closestEnemy.GetComponent<Enemy> ();
		int dir = (int)((closestEnemy.position.x - myTransform.position.x) / Mathf.Abs(closestEnemy.position.x - myTransform.position.x));

		enemy.TakeDamage (damage * Element.Multiplier(element, enemy.element));
		enemy.Knockback (knockbackForce, 1);

		float newPosX = closestEnemy.position.x;

		//apply knockback and damage on all near enemies
		for (byte i = 0; i < SpawnController.enemiesInGame.Count; i++) 
		{
			Transform e = SpawnController.enemiesInGame[i] as Transform;

			if(e == closestEnemy) continue;

			if(Mathf.Abs(e.position.x - newPosX) < knockbackArea)
			{
				enemy = e.GetComponent<Enemy>();

				dir = (int)((e.position.x - newPosX) / Mathf.Abs(e.position.x - newPosX));

				enemy.TakeDamage (damage * Element.Multiplier(element, enemy.element));
				enemy.Knockback (knockbackForce, dir);
			}
		}

		myTransform.position = new Vector3 (newPosX, myTransform.position.y, myTransform.position.z);
	}

	public Transform GetClosestEnemy(Elements element)
	{
		Transform closestEnemy = null;

		for(byte i = 0; i < SpawnController.enemiesInGame.Count; i++)
		{
			Transform e = SpawnController.enemiesInGame[i] as Transform;
			Enemy enemy = e.GetComponent<Enemy>();

			if(closestEnemy == null)
			{
				closestEnemy = e;
			}
			else 
			{
				if(element == Element.WeakAgainst(enemy.element))
				{
					if(enemy.element == Element.WeakAgainst(element))
					{
						if(Mathf.Abs(myTransform.position.x - e.position.x) < Mathf.Abs(myTransform.position.x - closestEnemy.position.x))
						{
							closestEnemy = e;
						}
					}
				}
				else
				{
					if(element == Element.WeakAgainst(enemy.element))
					{
						closestEnemy = e;
					}
					else
					{
						if(Mathf.Abs(myTransform.position.x - e.position.x) < Mathf.Abs(myTransform.position.x - closestEnemy.position.x))
						{
							closestEnemy = e;
						}
					}
				}
			}
		}

		return closestEnemy;
	}
}
