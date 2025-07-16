using System.Collections;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
  [Header("Linked Teleportal")]
  public Transform linkedPortal;

  private bool isTeleporting = false;

  void OnTriggerEnter2D(Collider2D coll)
  {
    if((coll.CompareTag("Player") || (coll.CompareTag("Props"))) && linkedPortal != null && !isTeleporting)
    {
      StartCoroutine(Teleport(coll.transform));
    }
  }
  private IEnumerator Teleport(Transform p)
  {
    isTeleporting = true;

    TeleportPortal linkedScript = linkedPortal.GetComponent<TeleportPortal>();
    if (linkedScript != null)
    {
      linkedScript.isTeleporting = true;
    }
    p.transform.position = linkedPortal.position;
    if(CharacterSwitchController.Instance != null)
      CharacterSwitchController.Instance.NotifyPlayerTeleported();
    yield return new WaitForSeconds(0.5f);
    isTeleporting = false;
    if(linkedScript != null)
    {
      linkedScript.isTeleporting = false;
    }
  }
}
