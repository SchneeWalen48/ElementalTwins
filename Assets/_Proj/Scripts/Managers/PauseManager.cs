using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
  public GameObject pause;
  public Image pauseImg;
  public Image playImg;
  public Button restartBtn;

  private bool isPaused = false;

  void Awake()
  {
    if(restartBtn != null)
    {
      restartBtn.onClick.AddListener(RestartGame);
    }
  }
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
    restartBtn.gameObject.SetActive(true);
  }
  public void ResumeGame()
  {
    isPaused = false;
    Time.timeScale = 1f;
    FindObjectOfType<GameTimerUI>()?.ResumeTimer();

    pauseImg.gameObject.SetActive(true);
    playImg.gameObject.SetActive(false);
    restartBtn.gameObject.SetActive(false);
  }

  public void RestartGame()
  {
    isPaused = false;
    Time.timeScale = 1f;

    string currSceneName = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(currSceneName);
  }
}
