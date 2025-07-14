using UnityEngine;
using Cinemachine;

public class VCamX : MonoBehaviour
{
  public float fixedX = 0f; // fixed x position

  private CinemachineVirtualCamera vCam;
  private Transform camTransform;

  void Start()
  {
    vCam = GetComponent<CinemachineVirtualCamera>();
    camTransform = vCam.VirtualCameraGameObject.transform;
  }

  void LateUpdate()
  {
    if (vCam.Follow != null)
    {
      Vector3 pos = camTransform.position;
      pos.x = fixedX; // fix x position
      camTransform.position = pos;
    }
  }
}
