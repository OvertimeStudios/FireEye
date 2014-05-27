using UnityEngine;
using System.Collections;

public class MainMenu : MenuBase 
{
	public UILabel lifestreamCollected;

	void Start()
	{
		lifestreamCollected.text = PlayerPrefs.GetInt ("lifestream").ToString();
	}

	public void OnPlayClick()
	{
		Application.LoadLevel ("FireEye");
	}

	public void OnShopClick()
	{
		NavigationController.Instance.Navigate(typeof(ShopMenu));
	}
}
