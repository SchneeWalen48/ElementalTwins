using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeProj : Projectiles
{
  public float duration = 1f;

  protected override void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log($"[FreezeProj] 충돌한 대상: {other.name}, 태그: {other.tag}");
    if (!enabled || !gameObject.activeInHierarchy) return;
    StartCoroutine(HandleFreeze(other));

    base.OnTriggerEnter2D(other);
  }

  private IEnumerator HandleFreeze(Collider2D other)
  {
    var enemy = other.GetComponent<Enemy>();
    if (enemy == null) yield break;

    Debug.Log(enemy.name);

    if(enemy is EnemyAttacker ea)
    {
      ea.TakeDmg(dmg);
    }

    yield return enemy.StartCoroutine(enemy.Freeze(duration));

    Destroy(gameObject);
   
  }

  public void SetDir(Vector2 dir)
  {
    SetDirection(dir); 
  }
}

