using UnityEngine;
using Cinemachine;

public class RearrangeTrigger : MonoBehaviour
{
  public CinemachineVirtualCamera virtualCamera;

  // 플레이어가 카메라의 Y축을 따라가야 하는지 여부
  //private bool forceYFollow = false;

  // Cinemachine Framing Transposer의 Y Damping 원래 값 저장
  private float originalYDamping;
  // Cinemachine Framing Transposer의 Y Dead Zone Height 원래 값 저장
  private float originalYDeadZoneHeight;
  private float originYSoftZoneHeight;

  void Start()
  {
    if (virtualCamera == null)
    {
      Debug.LogError("Virtual Camera 할당 안 됨.");
      return;
    }

    CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

    if (framingTransposer != null)
    {
      originalYDamping = framingTransposer.m_YDamping;
      originalYDeadZoneHeight = framingTransposer.m_DeadZoneHeight;
      originYSoftZoneHeight = framingTransposer.m_SoftZoneHeight;
    }
    else
    {
      Debug.LogError("카메라에 Framing Transposer Body없어 보임.");
    }
  }
  
  void OnTriggerEnter2D(Collider2D other)
  {
    // "Player" 태그를 가진 오브젝트만 반응
    if (other.CompareTag("Player"))
    {
      Debug.Log("플레이어 트리거 진입");
      //forceYFollow = true;
      AdjustCameraForcedFollow(true);
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      Debug.Log("플레이어 트리거 벗어남");
      //forceYFollow = false;
      AdjustCameraForcedFollow(false);
    }
  }

  // 카메라 설정을 조절하는 함수
  void AdjustCameraForcedFollow(bool enableForce)
  {
    CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

    if (framingTransposer != null)
    {
      if (enableForce)
      {
        // 강제 추적 시, Y Damping과 Dead Zone을 0으로 설정하여 즉시 따라가게 함
        framingTransposer.m_YDamping = 0.3f;
        framingTransposer.m_DeadZoneHeight = 0f; 
        framingTransposer.m_SoftZoneHeight = 0f;
      }
      else
      {
        // 강제 추적 해제 시, 원래 값으로 복원
        framingTransposer.m_YDamping = originalYDamping;
        framingTransposer.m_DeadZoneHeight = originalYDeadZoneHeight;
        framingTransposer.m_SoftZoneHeight = originYSoftZoneHeight;
      }
    }
  }
}