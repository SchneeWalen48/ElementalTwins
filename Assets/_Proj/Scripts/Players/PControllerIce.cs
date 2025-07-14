using UnityEngine;

public class PControllerIce : Player
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

    IceGun ig = proj.GetComponent<IceGun>();
    ig.speed = shotSpeed;
    ig.lifeTime = shotLife;

    Vector2 dir = lastDir > 0 ? Vector2.right : Vector2.left;
    if(dir == Vector2.left && ig.srProjI != null)
    {
      ig.srProjI.flipX = true;
    }
    else ig.srProjI.flipX= false;
    ig.SetDirection(dir);
  }

}
