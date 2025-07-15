using UnityEngine;

public class PControllerIce : Player
{
  public SkillData freezeData;
  private float freezeCoolTimer = 0f;
  protected override void Update()
  {
    base.Update();
    if(freezeCoolTimer > 0f) freezeCoolTimer -= Time.deltaTime;
    Debug.Log("Update from Ice");
  }
  protected override void Move()
  {
    base.Move();
  }

  protected override void KeyInputControl()
  {
    base.KeyInputControl();
    if (Input.GetKeyDown(KeyCode.K) && freezeCoolTimer <= 0f)
    {
      Debug.Log("KKK");
      Skill1();
      freezeCoolTimer = freezeData.cooldown;
    }
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

  protected override void Skill1()
  {
    if (freezeCoolTimer > 0f) return;
    if (freezeData.projPrefab == null || freezeData == null || shotPoint == null) { Debug.Log("데이어 빔"); return; }

    GameObject proj = Instantiate(freezeData.projPrefab, shotPoint.position, Quaternion.identity);
    FreezeProj fp = proj.GetComponent<FreezeProj>();
    if( fp == null)
    {
      print("컴포넌트 누락");
      return;
    }
    Vector2 dir = lastDir > 0? Vector2.right : Vector2.left;
    fp.SetDir(dir);
    fp.duration = freezeData.duration;

    freezeCoolTimer = freezeData.cooldown;
  }
}
