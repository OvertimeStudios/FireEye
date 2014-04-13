using UnityEngine;
using System.Collections;

public enum Elements
{
	Water,
	Earth,
	Electric,
	Fire,
	Neutral,
	None
}

public class Element
{

	public static Elements WeakAgainst(Elements element)
	{
		Elements e = Elements.None;
		
		if (element == Elements.Fire)
			e = Elements.Water;
		
		if (element == Elements.Water)
			e = Elements.Electric;
		
		if (element == Elements.Electric)
			e = Elements.Earth;
		
		if (element == Elements.Earth)
			e = Elements.Fire;
		
		return e;
	}
	
	public static float Multiplier(Elements element, Elements elementAgainst)
	{
		if (element == elementAgainst) return 0;
		
		if (element == WeakAgainst (elementAgainst)) return 1;
		
		return 0.5f;
	}

	public static Elements GetRandomElement()
	{
		float rnd = Random.Range (0, 4);

		if(rnd == 0)
			return Elements.Earth;
		else if(rnd == 1)
			return Elements.Fire;
		else if(rnd == 2)
			return Elements.Water;
		else
			return Elements.Electric;
	}
}
