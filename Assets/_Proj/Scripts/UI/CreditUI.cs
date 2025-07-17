using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
  public GameObject creditPanel;
  public ScrollRect creditContent;

  public void OpenCredit()
  {
    if(creditContent != null)
    {
      creditContent.verticalNormalizedPosition = 1f;
    }
    creditPanel.SetActive(true);
  }

  public void Closecredit()
  {

    if (creditContent != null)
    {
      creditContent.verticalNormalizedPosition = 1f;
    }
    creditPanel.SetActive(false);
  }
}
