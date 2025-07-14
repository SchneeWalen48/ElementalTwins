using UnityEngine;
using System.Collections;

public class PlatformBlock : MonoBehaviour, IInteractables
{
  public Vector2 targetPosOffset = new Vector2(0, 5f);
  public float moveDuration = 2.0f;
  public float resetDelay = 3.0f;
  public float delayTimer = 0.4f;

  public MoveDirection moveDir = MoveDirection.Up;

  public enum MoveDirection { Up, Down, Left, Right }

  private Vector3 originalPos;
  private Coroutine currMoveCoroutine;

  void Awake()
  {
    originalPos = transform.localPosition;
  }

  public void TriggerAction()
  {
    Debug.Log("Platform Triggered!");
    if (currMoveCoroutine != null)
    {
      StopCoroutine(currMoveCoroutine);
    }
    currMoveCoroutine = StartCoroutine(MovePlatform(true));
  }

  public void ResetAction()
  {
    Debug.Log("Platform Resetting...");
    if (currMoveCoroutine != null)
    {
      StopCoroutine(currMoveCoroutine);
    }
    currMoveCoroutine = StartCoroutine(MovePlatform(false));
  }

  IEnumerator MovePlatform(bool isMovingToTarget)
  {
    yield return new WaitForSeconds(delayTimer);
    Vector3 startPos = transform.localPosition;
    Vector3 endPos;

    if (isMovingToTarget)
    {
      switch (moveDir)
      {
        case MoveDirection.Up: endPos = originalPos + new Vector3(0, targetPosOffset.y, 0); break;
        case MoveDirection.Down: endPos = originalPos + new Vector3(0, -targetPosOffset.y, 0); break;
        case MoveDirection.Left: endPos = originalPos + new Vector3(-targetPosOffset.x, 0, 0); break;
        case MoveDirection.Right: endPos = originalPos + new Vector3(targetPosOffset.x, 0, 0); break;
        default: endPos = originalPos; break;
      }
    }
    else
    {
      endPos = originalPos;
    }

    float timer = 0f;
    while (timer < moveDuration)
    {
      transform.localPosition = Vector3.Lerp(startPos, endPos, timer / moveDuration);
      timer += Time.deltaTime;
      yield return null;
    }
    transform.localPosition = endPos;

    if (isMovingToTarget)
    {
      yield return new WaitForSeconds(resetDelay);
      ResetAction();
    }
  }
}