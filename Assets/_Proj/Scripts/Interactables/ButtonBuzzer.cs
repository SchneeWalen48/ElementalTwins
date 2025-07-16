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

  private int pressingObjCnt = 0;

  void Awake()
  {
    originPos = transform.localPosition;
    originScale = transform.localScale;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Props"))
    {
      pressingObjCnt++;
      Debug.Log($"[Buzzer] OnTriggerEnter2D: {other.gameObject.name} 진입. 현재 pressingObjectCount: {pressingObjCnt}, isPressed: {isPressed}");
      if (pressingObjCnt == 1)
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
  //private void OnTriggerStay2D(Collider2D other) => TryPress(other);
  
  void OnTriggerExit2D(Collider2D other)
  {
    if (!gameObject.activeInHierarchy) return;
    if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Props"))
    {
      pressingObjCnt--;
      if (pressingObjCnt < 0) pressingObjCnt = 0;
      Debug.Log($"[Buzzer] OnTriggerExit2D: {other.gameObject.name} 이탈. 현재 pressingObjectCount: {pressingObjCnt}, isPressed: {isPressed}");
      if (pressingObjCnt == 0)
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
    Debug.Log($"[Buzzer] DelayedPlatformReset 실행 완료 후 확인. isPressed: {isPressed}. 플랫폼 리셋 시도 중.");
    if (isPressed)
    {
      Debug.Log("[Buzzer] DelayedPlatformReset: 버튼이 눌려있으므로 플랫폼 리셋을 막습니다."); yield break;
    }
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