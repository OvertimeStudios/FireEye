using UnityEngine;
using System.Collections;

public class NavigationController : MonoBehaviour {

	public MenuBase[] allMenus;

	public Transform uiRoot;

	private static MenuBase currentMenu;

	public static MenuBase CurrentMenu
	{
		get { return currentMenu; }
	}

	private static NavigationController instance;
	public static NavigationController Instance
	{
		get { return instance; }
	}

	void Start()
	{
		instance = this;

		//activate all children
		for(int j = 0; j < uiRoot.childCount; j++)
			uiRoot.GetChild(j).gameObject.SetActive(true);

		allMenus = uiRoot.GetComponentsInChildren<MenuBase> ();

		//deactivate all children
		for(int i = 0; i < allMenus.Length; i++)
			allMenus[i].gameObject.SetActive(false);

		Navigate (typeof(MainMenu));
	}

	public void Navigate(System.Type type)
	{
		if(currentMenu != null)
		{
			currentMenu.gameObject.SetActive(false);

			currentMenu = null;
		}

		for(int i = 0; i < allMenus.Length; i++)
		{
			if(allMenus[i].GetType() == type)
			{
				allMenus[i].gameObject.SetActive(true);
				currentMenu = allMenus[i];
				break;
			}
		}
	}
}
