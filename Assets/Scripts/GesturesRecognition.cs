using UnityEngine;
using System.Collections;

public class GesturesRecognition : MonoBehaviour 
{
	public Transform trailRenderer;
	ParticleSystem particleSystem;

	private int lifestreamLayer;
	private int enemyLayer;

	void Start()
	{
		particleSystem = trailRenderer.GetComponent<ParticleSystem> ();

		particleSystem.enableEmission = false;

		lifestreamLayer = LayerMask.NameToLayer ("Lifestream");
		enemyLayer = LayerMask.NameToLayer ("Enemy");
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
			particleSystem.enableEmission = true;

		if(Input.GetMouseButtonUp(0))
			particleSystem.enableEmission = false;
	}

	void OnCustomGesture( PointCloudGesture gesture ) 
	{
		if(Application.loadedLevelName != "FireEye") return;

		switch (gesture.RecognizedTemplate.name) 
		{
			case "Fire":
				Player.Instance.UsePower(Elements.Fire);
			break;
			
			case "Water":
				Player.Instance.UsePower(Elements.Water);
			break;
			
			case "Earth":
				Player.Instance.UsePower(Elements.Earth);
			break;
			
			case "Electric":
				Player.Instance.UsePower(Elements.Energy);
			break;
			
			default:
			
			break;
		}
		/*Debug.Log( "Recognized custom gesture: " + gesture.RecognizedTemplate.name + 
		          ", match score: " + gesture.MatchScore + 
		          ", match distance: " + gesture.MatchDistance );*/
	}

	void OnFingerMove( FingerMotionEvent e ) 
	{
		if(e.Phase == FingerMotionPhase.Updated)
		{
			Vector3 pos = (camera == null) ? Camera.main.ScreenToWorldPoint(e.Position) : camera.ScreenToWorldPoint(e.Position);
			pos.z = -10;

			trailRenderer.position = pos;
		}

		/*float elapsed = e.ElapsedTime;
		
		if( e.Phase == FingerMotionPhase.Started )
			Debug.Log( e.Finger + " started moving at " + e.Position);
		else if( e.Phase == FingerMotionPhase.Updated )
			Debug.Log( e.Finger + " moving at " + e.Position );
		else if( e.Phase == FingerMotionPhase.Ended )
			Debug.Log( e.Finger + " stopped moving at " + e.Position );*/
	}

	void OnFingerHover( FingerHoverEvent e ) 
	{
		// check the hover event phase to check if we're entering or exiting the object
		if( e.Phase == FingerHoverPhase.Enter )
		{
			if(e.Selection.layer == lifestreamLayer)
				GameController.Instance.CollectLifestream(e.Selection.transform);
			//Debug.Log( e.Finger + " entered object: " + e.Selection );
		}
		/*else if( e.Phase == FingerHoverPhase.Exit )
		{
			Debug.Log( e.Finger + " exited object: " + e.Selection );
		}*/
	}

	void OnTap( TapGesture gesture)
	{
		if(gesture.Selection != null && gesture.Selection.layer == enemyLayer)
			Player.Instance.Teleport (gesture.Selection);
	}
}
