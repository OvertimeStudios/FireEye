using UnityEngine;
using System.Collections;

public enum Elements
{
	Water,
	Earth,
	Energy,
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
			e = Elements.Energy;
		
		if (element == Elements.Energy)
			e = Elements.Earth;
		
		if (element == Elements.Earth)
			e = Elements.Fire;
		
		return e;
	}
	
	public static float Multiplier(Elements element, Elements elementAgainst)
	{
		if (element == WeakAgainst (elementAgainst)) return 1;

		if(element != elementAgainst) return GameController.Instance.damageDealOnNeutralElements;

		return 0f;
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
			return Elements.Energy;
	}
}
