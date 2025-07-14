using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandleToggle : MonoBehaviour
{
  public Transform levHandle;
  public float activeAngle = -45f;
  public float deactiveAngle = 45f;
  [Tooltip("Assign the object to control")]
  public GameObject[] targets;

  private bool isOn = false;

  public void TryToggle()
  {
    float angle = levHandle.localEulerAngles.z;
    angle = (angle > 180f) ? angle - 360f : angle;

    bool crrOn = Mathf.Abs(angle - activeAngle) < 5f;
    isOn = !crrOn;

    levHandle.rotation = Quaternion.Euler(0, 0, isOn ? activeAngle : deactiveAngle);

    foreach (var target in targets)
    {
      if (target.TryGetComponent(out PlatformBlockToggle platform))
      {
        platform.SetState(isOn);
      }
      else if (target.TryGetComponent(out IInteractables interactable))
      {
        if (interactable != null)
        {
          if (isOn) { interactable.TriggerAction(); }
          else { interactable.ResetAction(); }
        }
      }
    }
  }
}
