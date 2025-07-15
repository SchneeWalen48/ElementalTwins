using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBuzzer : MonoBehaviour
{
  public float pressedOffsetY = -0.1f;
  public float pressDuration = 0.1f;
  public float resetDuration = 0.5f;
  public float pressedScaleY = 0.8f;

  [Tooltip("Assign the object to control")]
  public List<GameObject> targets = new List<GameObject>();

  public float platformResetDelay = 3.0f;

  private Vector3 originPos;
  private Vector3 originScale;
  private bool isPressed = false;
  private Coroutine currPressCoroutine;

  void Awake()
  {
    originPos = transform.localPosition;
    originScale = transform.localScale;
  }

  void OnTriggerEnter2D(Collider2D other)=>TryPress(other);
  private void OnTriggerStay2D(Collider2D other) => TryPress(other);
  
  void OnTriggerExit2D(Collider2D other)
  {
    if (!gameObject.activeInHierarchy) return;
    if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Props"))
    {
      if (isPressed)
      {
        isPressed = false;
        ReleaseBtn();

        StartCoroutine(DelayedPlatformReset());
      }
    }
  }
  void TryPress(Collider2D other)
  {
    if (!isPressed)
    {
      if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Props"))
      {
        isPressed = true;
        PressBtn();
        foreach (GameObject target in targets)
        {
          if (target.TryGetComponent(out IInteractables interact))
          {
            interact.TriggerAction();
          }
        }
      }
    }
  }
  private void PressBtn()
  {
    if (currPressCoroutine != null)
    {
      StopCoroutine(currPressCoroutine);
    }
    Vector3 pressPos = new Vector3(0, pressedOffsetY, 0);
    Vector3 pressScale = new Vector3(1, pressedScaleY, 1);
    currPressCoroutine = StartCoroutine(MoveBuzzer((originPos + pressPos), pressScale, pressDuration));

  }

  private void ReleaseBtn()
  {
    if (!gameObject.activeInHierarchy) return;
    if (currPressCoroutine != null)
    {
      StopCoroutine(currPressCoroutine);
    }
    currPressCoroutine = StartCoroutine(MoveBuzzer(originPos, originScale, resetDuration));
  }
  IEnumerator DelayedPlatformReset()
  {
    yield return new WaitForSeconds(platformResetDelay);
    foreach (GameObject target in targets)
    {
      if (target.TryGetComponent(out IInteractables interact))
      {
        interact.ResetAction();
      }
    }
  }

  public bool IsCurrPressed()
  {
    return isPressed;
  }

  IEnumerator MoveBuzzer(Vector3 pos, Vector3 scale, float duration)
  {
    Vector3 startPos = transform.localPosition;
    Vector3 startScale = transform.localScale;
    float timer = 0f;
    while (timer < duration)
    {
      transform.localPosition = Vector3.Lerp(startPos, pos, timer / duration);
      transform.localScale = Vector2.Lerp(startScale, scale, timer / duration);
      timer += Time.deltaTime;
      yield return null;
    }
    transform.localPosition = pos;
    transform.localScale = scale;
  }
}