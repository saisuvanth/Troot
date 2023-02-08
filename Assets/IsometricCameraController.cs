using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
  private bool isRotating;
  private bool isPanning;
  private float velocity;
  private float zoomVelocity;

  private float deltaX = 0f;
  private float deltaY = 0f;

  public new Camera camera;

  public float zoomSpeed = 1f;
  public float panSpeed = 1f;
  public float rotateSpeed = 1f;
  public float maxZoom = 80f;
  public float minZoom = 15f;

  public Vector3 minPanLimit = new Vector3(-100f, 0, -100f);
  public Vector3 maxPanLimit = new Vector3(100f, 0, 100f);
  public Transform targetAnchor;

  void Update()
  {
    if (Input.GetMouseButton(1))
    {
      isRotating = true;
      velocity = 0f;
    }
    else
    {
      isRotating = false;
    }

    if (Input.GetMouseButton(2))
    {
      isPanning = true;
      deltaX = 0f;
      deltaY = 0f;
    }
    else
    {
      isPanning = false;
    }

    if (isRotating == true)
    {
      RotateCameraTarget();
    }
    else if (isPanning == true)
    {
      PanCameraTarget();
    }

    CalculateRotation();
    CalculateZoom();
    CalculatePan();
  }

  void CalculateRotation()
  {
    Vector3 rot = Camera.main.transform.parent.eulerAngles;
    rot.y += velocity;
    Camera.main.transform.parent.eulerAngles = rot;
    velocity = Mathf.Lerp(velocity, 0f, Time.deltaTime * 5f);
  }



  void CalculateZoom()
  {
    if (Camera.main.orthographicSize < minZoom)
    {
      Camera.main.orthographicSize = minZoom;
      zoomVelocity = 0f;
      return;
    }

    if (Camera.main.orthographicSize > maxZoom)
    {
      Camera.main.orthographicSize = maxZoom;
      zoomVelocity = 0f;
      return;
    }

    if (
        Camera.main.orthographicSize + zoomVelocity >= minZoom &&
        Camera.main.orthographicSize + zoomVelocity <= maxZoom
    )
    {
      Camera.main.orthographicSize += zoomVelocity;
    }

    zoomVelocity -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    zoomVelocity = Mathf.Lerp(zoomVelocity, 0f, Time.deltaTime * 5f);
  }

  void CalculatePan()
  {
    Vector3 pos = Camera.main.transform.parent.position;
    if (deltaX < 0 && pos.x > minPanLimit.x)
    {
      Camera.main.transform.parent.Translate(-deltaX, 0, deltaX);
    }

    if (deltaX > 0 && pos.x < maxPanLimit.x)
    {
      Camera.main.transform.parent.Translate(-deltaX, 0, deltaX);
    }

    if (deltaY < 0 && pos.z > minPanLimit.z)
    {
      Camera.main.transform.parent.Translate(-deltaY, 0, -deltaY);
    }

    if (deltaY > 0 && pos.z < maxPanLimit.z)
    {
      Camera.main.transform.parent.Translate(-deltaY, 0, -deltaY);
    }

    deltaY = Mathf.Lerp(deltaY, 0f, Time.deltaTime * 5f);
    deltaX = Mathf.Lerp(deltaX, 0f, Time.deltaTime * 5f);
  }

  void RotateCameraTarget()
  {
    velocity += Input.GetAxis("Mouse X") * rotateSpeed;
  }

  void PanCameraTarget()
  {
    deltaX = Input.GetAxisRaw("Mouse X") * panSpeed;
    deltaY = Input.GetAxisRaw("Mouse Y") * panSpeed;
  }
}