using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClearUI : MonoBehaviour
{
  public static StageClearUI Instance;

  public GameObject panel;
  public Text timeTxt;
  public GameObject[] stars;
  public Button nextBtn;

  private void Awake()
  {
    if (Instance == null) Instance = this;
    panel.SetActive(false);

    nextBtn.onClick.AddListener(() =>
    {
      Time.timeScale = 1f;
      SceneManager.LoadScene("Ending");
    });
  }

  public void ShowClearUI(float clearTime)
  {
    Time.timeScale = 0f;
    panel.SetActive(true);

    int min = (int)(clearTime / 60);
    int sec = (int)(clearTime % 60);
    timeTxt.text = $"{min:00}:{sec:00}";

    int starCnt = clearTime < 420f ? 3 : (clearTime < 510f ? 2 : 1);
    for (int i = 0; i < stars.Length; i++)
    {
      stars[i].SetActive(i < starCnt);
    }
  }
}