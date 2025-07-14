using UnityEngine;

public enum ItemType
{
  PowerCore,  // ElecBoy's item
  CoolCore // IceBoy's item
}

public class Item : MonoBehaviour
{
  public ItemType itemType;

  //void OnTriggerEnter2D(Collider2D other)
  //{

  //  Player player = other.GetComponent<Player>();
  //  if (player != null)
  //  {
  //    if(player.myAssignedItemType == itemType)
  //    {
  //      player.CollectItem(itemType);
  //      gameObject.SetActive(false); // item deactivated in map
  //      Debug.Log($"{player.name} got {itemType}!");
  //    }
  //    else
  //    {
  //      Debug.Log("The player and item types are different. Can't obtain the item.");
  //    }
      
  //  }
  //}
}