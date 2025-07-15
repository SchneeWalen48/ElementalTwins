using UnityEngine;

public enum TrapType { Lava, Earth }
public class TrapZone : MonoBehaviour
{
  public TrapType trapType;

  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (!coll.CompareTag("Player")) return;
    
    PlayerType pType = coll.GetComponent<PlayerType>();
    if(pType == null) return;

    bool shouldRespawn = (trapType == TrapType.Lava && pType.type == PlayerElementType.IceBoy) || (trapType == TrapType.Earth && pType.type == PlayerElementType.ElecBoy);

    if(shouldRespawn)
    {
      Player player = coll.GetComponent<Player>();
      if (player != null)
      {
        player.Respawn();
      }
    }
  }
}
