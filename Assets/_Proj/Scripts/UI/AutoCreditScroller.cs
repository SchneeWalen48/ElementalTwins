using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoCreditScroller : MonoBehaviour
{
  public RectTransform content;
  public float scrollSpeed = 50f;
  public float endDelay = 2f;
  public string nextSceneName = "Title";
  public CanvasGroup fadeGroup;

  private float targetY;
  private bool finished = false;

  void Start()
  {
    targetY = content.anchoredPosition.y + content.rect.height;
  }

  void Update()
  {
    if (finished) return;
    content.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

    if(content.anchoredPosition.y >= targetY)
    {
      finished = true;
      StartCoroutine(EndCredit());
    }
  }

  private IEnumerator EndCredit()
  {
    if(fadeGroup != null)
      yield return StartCoroutine(FadeOut(fadeGroup));
    else
      yield return new WaitForSeconds(endDelay);
    SceneManager.LoadScene(nextSceneName);
  }

  IEnumerator FadeOut(CanvasGroup cg)
  {
    float t = 0f;
    while(t < 1f)
    {
      t += Time.deltaTime / 1f;
      cg.alpha = t;
      yield return null;
    }
  }
}
