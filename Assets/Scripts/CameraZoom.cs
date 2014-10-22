using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour 
{
	private enum States
	{
		Idle,
		ZoomIn,
		ZoomOut
	}

	public float length = 0.5f;

	private float cameraSizeZoomIn = 3f;
	private float cameraSizeZoomOut = 5f;
	private float vel;

	private States state;
	private Camera myCamera;
	private Transform myTransform;
	private CameraFollow cameraFollow;
	private Transform numees;

	private static CameraZoom instance;
	public static CameraZoom Instance
	{
		get { return instance; }
	}

	// Use this for initialization
	void Start () 
	{
		instance = this;

		state = States.Idle;

		myCamera = GetComponent<Camera> ();
		cameraFollow = GetComponent<CameraFollow> ();

		myTransform = transform;

		numees = GameObject.Find ("Numees").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(state != States.Idle)
		{
			myCamera.orthographicSize += vel * Time.deltaTime;

			float toY = 0f;
			if(state == States.ZoomIn)
				toY = numees.position.y * 0.5f;

			myTransform.position = Vector3.Slerp(myTransform.position, new Vector3(myTransform.position.x, toY, myTransform.position.z), 0.1f);

			if(myCamera.orthographicSize < cameraSizeZoomIn || myCamera.orthographicSize > cameraSizeZoomOut)
			{
				if(state == States.ZoomOut)
					cameraFollow.enabled = true;

				state = States.Idle;
			}
		}
	}

	public void ZoomIn()
	{
		vel = (cameraSizeZoomIn - cameraSizeZoomOut) / length;
		state = States.ZoomIn;

		cameraFollow.enabled = false;
	}

	public void ZoomOut()
	{
		vel = (cameraSizeZoomOut - cameraSizeZoomIn) / length;
		state = States.ZoomOut;
	}
}
