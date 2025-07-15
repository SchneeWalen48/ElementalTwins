using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
  [Header("Required Items for Activation")]
  public ItemType elecItem = ItemType.PowerCore; // ElecBoy's
  public ItemType iceItem = ItemType.CoolCore; // IceBoy's

  public GameObject portalPrefab;
  public Transform portalPos;
  public Transform itemDisplay_E;
  public Transform itemDisplay_I;
  public GameObject elecVisualPrefab;
  public GameObject iceVisualPrefab;
  public Transform leverTrans;


  [Header("Settings")]
  public float interactionRadius = 2f; // The radius within which the player must press the key near a generator.
  public Text msgText;

  private bool isActivated = false;
  private bool elecItemRegistered = false;
  private bool iceItemRegistered = false;

  private GameObject visualE, visualI;

  public void TryActivate(Player p)
  {
    if (isActivated)
    {
      Debug.Log("Already running!");
      return;
    }

    if (Vector2.Distance(p.transform.position, transform.position) > interactionRadius)
    {
      Debug.Log($"{p.name} is too far from the generator!");
      return;
    }

    ItemType type = p.myAssignedItemType;

    if (!p.HasItem(type))
    {
      StartCoroutine(ShowMsg("발전기를 가동하기에 아이템이 충분하지 않습니다"));
      return;
    }

    if (type == iceItem && !iceItemRegistered)
    {
      iceItemRegistered = true;
      p.RemoveItem(type);
      Debug.Log($"{type} assigned");
      ItemSlotUI.Instance?.MarkItemUsed(type);
    }
    else if(type == elecItem && !elecItemRegistered)
    {
      if (!iceItemRegistered)
      {
        StartCoroutine(ShowMsg("발전기를 먼저 식히세요."));
        return;
      }
      elecItemRegistered = true;
      p.RemoveItem(type);
      Debug.Log($"{type} assigned");
      ItemSlotUI.Instance?.MarkItemUsed(type);
    }
    else
    {
      Debug.Log("Already Assigned or Wrong order"); return;
    }

    if(elecItemRegistered && iceItemRegistered)
    {
      ActivateGenerator();
    }

    Debug.Log($"현재 등록 상태 → Elec: {elecItemRegistered}, Ice: {iceItemRegistered}");
  }

  private GameObject ShowVisual(Transform target, GameObject prefab)
  {
    if (target == null || prefab == null) return null;
    return Instantiate(prefab, target.position, Quaternion.identity, target);
  }

  private void ActivateGenerator()
  {
    isActivated = true;
    Debug.Log("The Generator starts running");
    StartCoroutine(AnimateLeverAndPortal());
  }

  IEnumerator AnimateLeverAndPortal()
  {
    if (leverTrans != null)
    {
      Quaternion from = Quaternion.Euler(0, 0, -45f);
      Quaternion to = Quaternion.Euler(0, 0, -90f);
      float t = 0f;
      while (t < 0.4f)
      {
        leverTrans.localRotation = Quaternion.Slerp(from, to, t / 0.4f);
        t += Time.deltaTime;
        yield return null;
      }
      leverTrans.localRotation = to;
    }
    yield return new WaitForSeconds(0.4f);

    if (portalPrefab && portalPos)
    {
      Instantiate(portalPrefab, portalPos.position, Quaternion.identity);
    }
  }
  
  IEnumerator ShowMsg(string msg)
  {
    msgText.text = msg;
    yield return new WaitForSeconds(1f);
    msgText.text = "";
  }
}