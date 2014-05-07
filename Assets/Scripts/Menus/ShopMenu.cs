using UnityEngine;
using System.Collections;

public class ShopMenu : MenuBase 
{
	public Line[] lines;

	public Transform trailRenderer;

	void Start()
	{
		trailRenderer = GameObject.Find ("Trail Renderer").transform;
	}

	public void ClickedChangeLine()
	{
		foreach(Line line in lines)
		{
			if(line.name == UIButton.current.gameObject.name)
			{
				trailRenderer.GetComponent<TrailRenderer>().material.mainTexture = line.line;
				trailRenderer.GetComponent<ParticleSystemRenderer>().material.mainTexture = line.particle;

				Global.lineTexture = line.line;
				Global.particleTexture = line.particle;
				break;
			}
		}
	}

	public void OnBackClicked()
	{
		NavigationController.Instance.Navigate(typeof(MainMenu));
	}
}

[System.Serializable]
public class Line
{
	public string name;
	public Texture line;
	public Texture particle;
}