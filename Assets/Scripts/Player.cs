using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private static Player instance;
	public static Player Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		instance = this;
	}

	public void UsePower(Element element)
	{
		Debug.Log ("Used: " + element.ToString ());
	}
}
