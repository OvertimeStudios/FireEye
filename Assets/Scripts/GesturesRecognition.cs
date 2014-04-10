using UnityEngine;
using System.Collections;

public class GesturesRecognition : MonoBehaviour 
{
	void OnCustomGesture( PointCloudGesture gesture ) 
	{
		switch (gesture.RecognizedTemplate.name) 
		{
			case "Fire":
				Player.Instance.UsePower(Element.Fire);
			break;
			
			case "Water":
				Player.Instance.UsePower(Element.Water);
			break;
			
			case "Earth":
				Player.Instance.UsePower(Element.Earth);
			break;
			
			case "Electric":
				Player.Instance.UsePower(Element.Electric);
			break;
			
			default:
			
			break;
		}
		/*Debug.Log( "Recognized custom gesture: " + gesture.RecognizedTemplate.name + 
		          ", match score: " + gesture.MatchScore + 
		          ", match distance: " + gesture.MatchDistance );*/
	}

}
