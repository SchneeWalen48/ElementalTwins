using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
  public static SkillSlotUI Instance;
  public Image elecKIcon;
  public Image iceKIcon;

  float elecCool = 0f;
  float iceCool = 0f;
  float elecDuration = 0f;
  float iceDuration = 0f;

  void Awake()
  {
    if (Instance == null) Instance = this;
  }

  void Update()
  {
    if (elecCool > 0f)
    {
      elecCool -= Time.deltaTime;
      elecKIcon.fillAmount = elecCool / elecDuration;
      if (elecCool <= 0f) { elecKIcon.fillAmount = 0f; }
    }
    if (iceCool > 0f)
    {
      iceCool -= Time.deltaTime;
      iceKIcon.fillAmount = iceCool / iceDuration;
      if (iceCool <= 0f) { iceKIcon.fillAmount = 0f; }
    }
  }

  public void ElecCoolDown(float duration)
  {
    elecDuration = duration;
    elecCool = duration;
    elecKIcon.fillAmount = 1;
  }

  public void IceCoolDown(float duraton)
  {
    iceDuration = duraton;
    iceCool = duraton;
    iceKIcon.fillAmount = 1;
  }

}
