using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
  public GameObject pause;
  public Image pauseImg;
  public Image playImg;

  private bool isPaused = false;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (!isPaused)
        PauseGame();
      else
        ResumeGame();
    }
  }

  public void PauseGame()
  {
    isPaused = true;
    Time.timeScale = 0f;
    pause.SetActive(true);
    FindObjectOfType<GameTimerUI>()?.StopTimer();
    pauseImg.gameObject.SetActive(false);
    playImg.gameObject.SetActive(true);
  }
  public void ResumeGame()
  {
    isPaused = false;
    Time.timeScale = 1f;
    FindObjectOfType<GameTimerUI>()?.ResumeTimer();
    pauseImg.gameObject.SetActive(true);
    playImg.gameObject.SetActive(false);
  }
}
