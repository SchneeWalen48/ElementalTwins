using UnityEngine;

public class HowToPortal : MonoBehaviour
{
  private bool triggered = false;

  void Start()
  {
    if (TutorialClearUI.Instance == null) { Debug.Log("null"); }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (triggered) return;

    if (collision.CompareTag("Player"))
    {
      if (TutorialClearUI.Instance != null)
      {
        TutorialClearUI.Instance.ShowTutClearUI(0f);
        triggered = true;
        
      }
    }
  }
}