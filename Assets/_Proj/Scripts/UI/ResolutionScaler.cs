using UnityEngine;

public class ResolutionScaler : MonoBehaviour
{
  public int referenceWidth = 1920;
  public int referenceHeight = 1080;
  private Camera cam;

  void Start()
  {
    cam = Camera.main;
    AdjustCamera();
  }

  void AdjustCamera()
  {
    float targetAspect = (float)referenceWidth / referenceHeight;
    float currentAspect = (float)Screen.width / Screen.height;

    if (currentAspect >= targetAspect)
    {
      cam.orthographicSize = referenceHeight / 200f / 2f;
    }
    else
    {
      float difference = targetAspect / currentAspect;
      cam.orthographicSize = (referenceHeight / 200f / 2f) * difference;
    }
  }

  void Update()
  {
    if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
    {
      AdjustCamera();
    }
  }
}
