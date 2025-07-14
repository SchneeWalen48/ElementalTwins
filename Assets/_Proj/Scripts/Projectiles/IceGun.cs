using UnityEngine;

public class IceGun : Projectiles
{
  public SpriteRenderer srProjI;

  protected override void Awake()
  {
    base.Awake();
    srProjI = GetComponent<SpriteRenderer>();
  }
  
}
