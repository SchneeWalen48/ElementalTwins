using UnityEngine;
public class Portal : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      float time = Time.timeSinceLevelLoad;
      StageClearUI.Instance.ShowClearUI(time);
    }
  }
}
