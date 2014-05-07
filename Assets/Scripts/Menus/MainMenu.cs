using UnityEngine;
using System.Collections;

public class MainMenu : MenuBase 
{
	public void OnPlayClick()
	{
		Application.LoadLevel ("FireEye");
	}

	public void OnShopClick()
	{
		NavigationController.Instance.Navigate(typeof(ShopMenu));
	}
}
