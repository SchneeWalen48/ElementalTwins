using UnityEngine;

public class HowToPortal : MonoBehaviour
{
  private GameObject clearPanel;
  private bool triggered = false;

  void Start()
  {
    clearPanel = GameObject.Find("StageClearPanel");
    if (clearPanel != null)
      clearPanel.SetActive(false);
    else
      Debug.LogWarning("[HowToPortal] StageClearPanel not found in scene.");
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (triggered) return;

    if (collision.CompareTag("Player"))
    {
      if (clearPanel != null)
      {
        clearPanel.SetActive(true);
        triggered = true;
      }
    }
  }
}
