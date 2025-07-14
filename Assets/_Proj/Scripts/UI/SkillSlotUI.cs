using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
  public Image iconImage;
  public Image cooldownOverlay; // fillAmount로 쿨타임 표시
  public Text cooldownText;

  private float cooldownTime;
  private float cooldownRemaining;

  private bool isOnCooldown = false;

  public void Initialize(Sprite icon, float cooldown)
  {
    iconImage.sprite = icon;
    cooldownTime = cooldown;
    cooldownRemaining = 0;
    cooldownOverlay.fillAmount = 0;
    cooldownText.text = "";
  }

  public void StartCooldown()
  {
    cooldownRemaining = cooldownTime;
    isOnCooldown = true;
  }

  void Update()
  {
    if (isOnCooldown)
    {
      cooldownRemaining -= Time.deltaTime;
      if (cooldownRemaining <= 0f)
      {
        cooldownRemaining = 0f;
        isOnCooldown = false;
      }

      cooldownOverlay.fillAmount = cooldownRemaining / cooldownTime;
      cooldownText.text = Mathf.CeilToInt(cooldownRemaining).ToString();
    }
    else
    {
      cooldownOverlay.fillAmount = 0f;
      cooldownText.text = "";
    }
  }

  public bool IsUsable()
  {
    return !isOnCooldown;
  }
}
