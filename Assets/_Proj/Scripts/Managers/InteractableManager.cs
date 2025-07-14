using UnityEngine;

public class InteractableManager : MonoBehaviour
{
  public ButtonBuzzer btnHandler;
  public LeverHandle leverHandler;
  public WallTrigger wallTriggerHandler;
  public PlatformBlock pBlockHandler;

  void Awake()
  {
    // ButtonBuzzer와 LeverHandle은 경우에 따라 WallTrigger를 제어할 수도, PlatformBlock을 제어할 수도 잇음.
    if (btnHandler != null && wallTriggerHandler != null) { }

    if (leverHandler != null && pBlockHandler != null) { }
  }
}