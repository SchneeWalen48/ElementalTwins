using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
  public static ItemSlotUI Instance;

  public Image powercoreIcon;
  public Sprite pcActiveSprite;
  public Sprite pcUsedSprite;

  public Image coolcoreIcon;
  public Sprite ccActiveSprite;
  public Sprite ccUsedSprite;

  void Awake()
  {
    if(Instance == null) Instance = this;
  }

  public void UpdateUI(Player p)
  {
    if (p.HasItem(ItemType.PowerCore))
    {
      powercoreIcon.sprite = pcActiveSprite;
      powercoreIcon.color = Color.white;
    }
    else
    {
      powercoreIcon.sprite = null;
    }

    if (p.HasItem(ItemType.CoolCore))
    {
      coolcoreIcon.sprite = ccActiveSprite;
      coolcoreIcon.color = Color.white;
    }
    else
    {
      coolcoreIcon.sprite= null;
    }
  }

  public void MarkItemUsed(ItemType type)
  {
    if (type == ItemType.PowerCore)
    {
      powercoreIcon.sprite = pcUsedSprite;
    }
    else if (type == ItemType.CoolCore)
    {
      coolcoreIcon.sprite = ccUsedSprite;
    }
  }
}
