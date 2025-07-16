using UnityEngine;

public class CameraRatioController : MonoBehaviour
{
  public float refWidth = 1920f;
  public float refHeight = 1080f;

  public float refOrthoSize = 9f;

  private Camera mainCamera;

  void Awake()
  {
    mainCamera = GetComponent<Camera>();
    if (mainCamera == null)
    {
      enabled = false;
      return;
    }

    AdjustCameraSize(); // Adjust once when starting
  }

  void Update()
  {
    AdjustCameraSize();
  }

  void AdjustCameraSize()
  {
    float currRatio = (float)Screen.width / Screen.height;

    float refRatio = refWidth / refHeight;

    if (currRatio > refRatio)
    {
      mainCamera.orthographicSize = refOrthoSize;
    }
    else
    {
      float orthographicWidth = refOrthoSize * currRatio / refRatio;
      mainCamera.orthographicSize = orthographicWidth;
    }
  }
}