using UnityEngine;

public class Generator : MonoBehaviour
{
  [Header("Required Items for Activation")]
  public ItemType player1RequiredItemType; // ElecBoy's
  public ItemType player2RequiredItemType; // IceBoy's

  [Header("Activation Settings")]
  public float interactionRadius = 2f; // The radius within which the player must press the key near a generator.
  public GameObject objectToActivate; // generate portal

  private bool isActivated = false;
  
  // Track player with generator running when key is pressed
  private Player player1InVicinity = null;
  private Player player2InVicinity = null;

  void Update()
  {
    if (isActivated) return;
  }

  public void TryActivate(Player interactingPlayer)
  {
    if (isActivated) return;

    if (Vector2.Distance(interactingPlayer.transform.position, transform.position) > interactionRadius)
    {
      Debug.Log($"{interactingPlayer.name}is too far from the generator!");
      return;
    }

    if (interactingPlayer.CompareTag("PlayerActive"))
    {
      player1InVicinity = interactingPlayer;
    }
    else if (interactingPlayer.CompareTag("PlayerActive"))
    {
      player2InVicinity = interactingPlayer;
    }

    CheckAllConditionsAndActivate();
  }


  private void CheckAllConditionsAndActivate()
  {
    if (player1InVicinity == null || player2InVicinity == null)
    {
      return; 
    }

    bool player1HasItem = player1InVicinity.HasItem(player1RequiredItemType);
    bool player2HasItem = player2InVicinity.HasItem(player2RequiredItemType);

    Debug.Log($"Player1 ({player1InVicinity.name}) item ({player1RequiredItemType}): {player1HasItem}");
    Debug.Log($"Player2 ({player2InVicinity.name}) item ({player2RequiredItemType}): {player2HasItem}");

    if (player1HasItem && player2HasItem)
    {
      ActivateGenerator();
    }
    else
    {
      Debug.Log("Either not all items have been collected, or neither players has attempted to interact near the generator.");
      // 아이템이 부족하다는 시각/청각 피드백 제공 가능
    }
  }

  private void ActivateGenerator()
  {
    isActivated = true;
    Debug.Log("Generator activatoin successful!");

    if (objectToActivate != null)
    {
      objectToActivate.SetActive(true); // generate portal
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, interactionRadius);
  }
}