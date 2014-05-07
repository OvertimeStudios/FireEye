using UnityEngine;
using System.Collections;

public class InkEffect : MonoBehaviour 
{
	public GameObject inkStain;
	public float inkScaleVariation = 0.5f;

	private ParticleSystem pSystem;

	private ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[16];

	void OnParticleCollision(GameObject other) 
	{
		int safeLength = pSystem.safeCollisionEventSize;
		if (collisionEvents.Length < safeLength)
			collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
		
		int numCollisionEvents = pSystem.GetCollisionEvents(other, collisionEvents);
		int i = 0;
		while (i < numCollisionEvents) 
		{
			Vector3 pos = collisionEvents[i].intersection;
			Vector3 scale = Vector3.one * Mathf.Max(0.5f, (Random.Range(0f, 1f) * (inkScaleVariation)) + inkScaleVariation);
			SpawnController.Instance.SpawnInkStain(pos, scale);

			i++;

			break;
		}
	}

	void Start()
	{
		pSystem = GetComponent<ParticleSystem> ();
	}
}
