using UnityEngine;
using UnityEngine.UI;

public class ActivePlayerUI : MonoBehaviour
{
  public Image elecIcon;
  public Image iceIcon;

  public Color activeColour = Color.white;
  public Color inactiveColour = new Color(0.5f, 0.5f, 0.5f, 0.8f);

  public void UpdateIcon(Player activePlayer)
  {
    if (activePlayer.GetComponent<PlayerType>().type == PlayerElementType.ElecBoy)
    {
      elecIcon.color = activeColour;
      iceIcon.color = inactiveColour;
    }
    else
    {
      elecIcon.color = inactiveColour;
      iceIcon.color = activeColour;
    }
  }
}
