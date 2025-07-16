using UnityEngine;
public class Portal : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      float time = FindObjectOfType<GameTimerUI>().GetTime();
      StageClearUI.Instance.ShowClearUI(time);
      FindObjectOfType<GameTimerUI>().StopTimer();
    }
  }
}
