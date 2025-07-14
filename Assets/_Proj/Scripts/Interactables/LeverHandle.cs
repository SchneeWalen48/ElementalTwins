using UnityEngine;

public class LeverHandle : MonoBehaviour
{
  public Transform leverHandle;
  public float rotateAngle = -45f; // 누르면 몇 도로 기울일지
  public float resetDelay = 2f;
  [Tooltip("Assign the object to control")]
  public GameObject[] targets;

  private Quaternion originalRot;
  private bool triggered = false;

  void Start()
  {
    originalRot = leverHandle.rotation;
  }

  void ResetLever()
  {
    leverHandle.rotation = originalRot;
    triggered = false;
  }
  public void TryActivate()
  {
    if(triggered) return;

    triggered = true;
    leverHandle.rotation = Quaternion.Euler(0, 0, rotateAngle);

    foreach(var obj in targets)
    {
      var interactable = obj.GetComponent<IInteractables>();
      interactable?.TriggerAction();
    }
    Invoke(nameof(ResetLever), resetDelay);
  }
}
