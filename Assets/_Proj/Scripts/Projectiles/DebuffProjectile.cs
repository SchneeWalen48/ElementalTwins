using UnityEngine;

public class DebuffProjectile : MonoBehaviour
{
  public float moveSpeedDebuff = 1f; // debuff player speed amount
  public float knockbackForce = 0.3f;

  public float damage = 10f; // Attacker bullet damage

  public float lifetime = 3f; // bullet lifetime

  private bool hasHit = false;
  void Start()
  {
    Destroy(gameObject, lifetime);
  }


  void OnTriggerEnter2D(Collider2D other)
  {
    if(hasHit) return;

    if (other.CompareTag("Player"))
    {
      hasHit = true;
      if(other.TryGetComponent<Player>(out var player))
      {
        Vector2 knockDir = (player.transform.position - transform.position).normalized;
        player.ApplyDebuff(moveSpeedDebuff);
        player.ApplyKnockback(knockDir, knockbackForce);
      }
      Destroy(gameObject);
    }
    else if(other.CompareTag("Ground") || other.CompareTag("Wall") || other.CompareTag("Enemy") || other.CompareTag("Props"))
    {
      Destroy(gameObject) ;
    }
  }
}