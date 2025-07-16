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
  private void OnDestroy()
  {
    string msg = $"[VCamX] !!!!! {gameObject.name} (Virtual Camera) 오브젝트가 파괴되었습니다 !!!!!";
    // 이 한 줄을 추가하여 누가 Destroy를 호출했는지 스택 트레이스를 얻습니다.
    Debug.LogException(new System.Exception(msg), this);
  }
}
