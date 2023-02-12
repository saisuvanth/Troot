using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float movementSpeed;
	public float movementTime;

	public Vector3 newPosition;

	public Vector3 dragStartPosition;
	public Vector3 dragCurrentPosition;

	void Start()
	{
		if (GameManager.currentPlayer == (int)Player.P2)
		{
			newPosition = new Vector3(34f, 27f, 33f);
			transform.rotation = new Quaternion(-0.0990457683801651f, 0.8923991322517395f, -0.23911765217781068f, -0.36964380741119387f);
		}
		else
		{
			transform.rotation = new Quaternion(0.239117652f, 0.369643807f, -0.0990457684f, 0.892399132f);
			newPosition = new Vector3(-34f, 27f, -33f);
		}
	}

	public void Update()
	{
		HandleMouseInput();
		HandleTouchInput();
	}

	void HandleMouseInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			float entry;

			if (plane.Raycast(ray, out entry))
			{
				dragStartPosition = ray.GetPoint(entry);
			}
		}

		if (Input.GetMouseButton(0))
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			float entry;

			if (plane.Raycast(ray, out entry))
			{
				dragCurrentPosition = ray.GetPoint(entry);

				newPosition = transform.position + dragStartPosition - dragCurrentPosition;
			}
		}

		if (Input.mouseScrollDelta.y != 0)
		{
			Camera.main.fieldOfView -= Input.mouseScrollDelta.y * 2;
		}


		transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
	}

	void HandleTouchInput()
	{
		if (Input.touchCount == 1)
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);

			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

			float entry;

			if (plane.Raycast(ray, out entry))
			{
				dragStartPosition = ray.GetPoint(entry);
			}
		}

		if (Input.touchCount == 2)
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);

			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

			float entry;

			if (plane.Raycast(ray, out entry))
			{
				dragCurrentPosition = ray.GetPoint(entry);

				newPosition = transform.position + dragStartPosition - dragCurrentPosition;
			}
		}

		// Handle zoom on pinch
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			Camera.main.fieldOfView -= difference * 0.05f;
		}

		transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
	}
}
