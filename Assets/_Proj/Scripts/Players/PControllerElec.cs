using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PControllerElec : Player
{
  protected override void Update()
  {
    base.Update();
  }
  protected override void Move()
  {
    base.Move();
  }

  protected override void KeyInputControl()
  {
    base.KeyInputControl();
  }

  protected override void OnCollisionEnter2D(Collision2D collision)
  {
    base.OnCollisionEnter2D(collision);
    
  }
  protected override void BasicAtck()
  {
    base.BasicAtck();
    GameObject proj = Instantiate(basicProjPrefab, shotPoint.position, Quaternion.identity);

    ElecProjectile ep = proj.GetComponent<ElecProjectile>();
    
    ep.speed = shotSpeed;
    ep.lifeTime = shotLife;

    Vector2 dir = lastDir > 0 ? Vector2.right : Vector2.left;
    if (dir == Vector2.left && ep.srProjE != null)
    {
      ep.srProjE.flipX = true;
    }
    else ep.srProjE.flipX = false;
    ep.SetDirection(dir);
  }
}
