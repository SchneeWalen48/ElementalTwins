using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlockToggle : MonoBehaviour, IInteractables
{
  public Vector2 targetPosOffset = new Vector2(0, 5f);
  public float moveDuration = 2.0f;

  public MoveDirection moveDir = MoveDirection.Up;

  private Vector3 originPos;
  private Vector3 targetPos;
  private Coroutine currentMoveCoroutine;
  private bool isOn = false;
  
  public enum MoveDirection { Up, Down, Left, Right }

  void Awake()
  {
    originPos = transform.localPosition;

    switch (moveDir)
    {
      case PlatformBlockToggle.MoveDirection.Up:
        targetPos = originPos + new Vector3(0, targetPosOffset.y, 0);
        break;
      case PlatformBlockToggle.MoveDirection.Down:
        targetPos = originPos + new Vector3(0, -targetPosOffset.y, 0);
        break;
      case PlatformBlockToggle.MoveDirection.Left:
        targetPos = originPos + new Vector3(-targetPosOffset.x, 0, 0);
        break;
      case PlatformBlockToggle.MoveDirection.Right:
        targetPos = originPos + new Vector3(targetPosOffset.x, 0, 0);
        break;
    }

  }
  
  public void SetState(bool turnOn){
	  if(isOn == turnOn) return;
	  
	  isOn = turnOn;
	  if(currentMoveCoroutine != null) StopCoroutine(currentMoveCoroutine);
	  currentMoveCoroutine = StartCoroutine(MovePlatform(isOn? targetPos : originPos));
  }

  public void TriggerAction(){}

  public void ResetAction() { }

  IEnumerator MovePlatform(Vector3 target)
  {
    float timer = 0f;
    Vector3 start = transform.localPosition;

    while(timer < moveDuration)
    {
      transform.localPosition = Vector3.Lerp(start, target, timer / moveDuration);
      timer+= Time.deltaTime;
      yield return null;
    }
    transform.localPosition = target;
  }

}
