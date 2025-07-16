using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUI : MonoBehaviour
{
  public TMP_Text text;
  private float elapsed = 0f;
  private bool isRunning = true;

  void Update()
  {
    if (!isRunning) return;

    elapsed += Time.deltaTime;

    int m = Mathf.FloorToInt(elapsed / 60f);
    int s = Mathf.FloorToInt(elapsed % 60);

    text.text = $"{m:00}:{s:00}";
  }

  public float GetTime() => elapsed;
  public void StopTimer() => isRunning = false;
  public void ResumeTimer() => isRunning = true;
}
