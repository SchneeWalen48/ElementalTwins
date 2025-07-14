using UnityEngine;

public class ElecProjectile : Projectiles
{
  public SpriteRenderer srProjE;

  protected override void Awake()
  {
    base.Awake();
    srProjE = GetComponent<SpriteRenderer>();
  }
}
