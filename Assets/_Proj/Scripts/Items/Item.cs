using UnityEngine;

public enum ItemType
{
  PowerCore,  // ElecBoy's item
  HeatCore // IceBoy's item
}

public class Item : MonoBehaviour
{
  public ItemType itemType;
  public string requiredPlayerTag;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag(requiredPlayerTag))
    {
      Player player = other.GetComponent<Player>();
      if (player != null)
      {
        player.CollectItem(itemType);
        gameObject.SetActive(false); // item deactivated in map
        Debug.Log($"{requiredPlayerTag} got {itemType}!");
      }
    }
  }
}