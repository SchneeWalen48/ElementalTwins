using UnityEngine;

public class DebuffProjectile : MonoBehaviour
{
  public float moveSpeedDebuff = 1f; // debuff player speed amount
  public float knockbackForce = 0.3f;

  public float damage = 10f; // Attacker bullet damage

  public float lifetime = 3f; // bullet lifetime

  void Start()
  {
    Destroy(gameObject, lifetime);
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      Player player = collision.gameObject.GetComponent<Player>();
      if (player != null)
      {
        player.ApplyDebuff(moveSpeedDebuff);

        Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
        player.ApplyKnockback(knockbackDirection, knockbackForce);
      }
      Destroy(gameObject);
    }
    else if (collision.gameObject.CompareTag("Props") || collision.gameObject.CompareTag("Wall"))
    {
      Destroy(gameObject);
    }
  }
}