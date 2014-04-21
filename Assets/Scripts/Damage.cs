using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour 
{
	TweenPosition tweenPosition;
	TweenAlpha tweenAlpha;

	Transform myTransform;

	public void Hide()
	{
		gameObject.SetActive (false);
	}

	void Awake()
	{
		tweenPosition = GetComponent<TweenPosition> ();
		tweenAlpha = GetComponent<TweenAlpha> ();
		myTransform = transform;
	}

	public void SetAnimation()
	{
		tweenPosition.ResetToBeginning ();
		tweenPosition.from = myTransform.position;
		Vector3 to = new Vector3 (myTransform.position.x, myTransform.position.y, myTransform.position.z);
		tweenPosition.to = to + new Vector3(0, 2, 0);
		tweenPosition.PlayForward ();

		tweenAlpha.ResetToBeginning ();
		tweenAlpha.PlayForward ();
	}
}
