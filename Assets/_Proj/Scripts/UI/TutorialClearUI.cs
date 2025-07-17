using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialClearUI : MonoBehaviour
{
  public static TutorialClearUI Instance;

  public GameObject panel;
  public Button startBtn;

  private void Awake()
  {
    if (Instance == null) Instance = this;
    panel.SetActive(false);

    startBtn.onClick.AddListener(() =>
    {
      Time.timeScale = 1f;
      SceneManager.LoadScene(2);
    });
  }

  public void ShowTutClearUI(float clearTime)
  {
    Time.timeScale = 0f;
    panel.SetActive(true);
  }
}
