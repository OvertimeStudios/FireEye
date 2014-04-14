using UnityEngine;
using System.Collections;

public class GesturesRecognition : MonoBehaviour 
{
	public Transform trailRenderer;

	void OnCustomGesture( PointCloudGesture gesture ) 
	{
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
				Player.Instance.UsePower(Elements.Electric);
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
			Vector3 pos = Camera.main.ScreenToWorldPoint(e.Position);
			pos.z = 0;

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
		Debug.Log ("oooi");
		// check the hover event phase to check if we're entering or exiting the object
		if( e.Phase == FingerHoverPhase.Enter )
		{
			GameController.Instance.CollectLifestream(e.Selection.transform);
			//Debug.Log( e.Finger + " entered object: " + e.Selection );
		}
		/*else if( e.Phase == FingerHoverPhase.Exit )
		{
			Debug.Log( e.Finger + " exited object: " + e.Selection );
		}*/
	}

}
