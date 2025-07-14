using System.Collections;
using UnityEngine;

public class WallTrigger : MonoBehaviour, IInteractables
{
  public float scaleReduce = 0.01f;
  public float shrinkDuration = 1f;
  public float actionDelay = .4f;

  private Vector3 originScale;

  void Awake()
  {
    originScale = transform.localScale;
  }

  public void TriggerAction()
  {
    StartCoroutine(ShrinkWallAfterDelay(actionDelay));
  }

  public void ResetAction() { }

  IEnumerator ScaleWall(Vector3 scale, float duration)
  {
    float timer = 0f;
    Vector3 startScale = transform.localScale;

    while(timer < duration)
    {
      transform.localScale = Vector3.Lerp(startScale, scale, timer / duration);
      timer += Time.deltaTime;
      yield return null;
    }
    transform.localScale = scale;
  }
  IEnumerator ShrinkWallAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);

    Vector3 scale = originScale;
    scale.y = originScale.y * scaleReduce;

    yield return StartCoroutine(ScaleWall(scale, shrinkDuration));
  }
}
