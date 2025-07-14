using UnityEngine;

public class Rock : MonoBehaviour
{
  private Vector2 originalPos;
  public float resetThresholdY = 5f;

  private Rigidbody2D rb;

  void Start()
  {
    originalPos = transform.position;
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    float yDist = Mathf.Abs(transform.position.y - originalPos.y);

    if (yDist > resetThresholdY)
    {
      InstantReset();
    }
  }

  void InstantReset()
  {
    // eliminate physics effect
    rb.velocity = Vector2.zero;
    rb.angularVelocity = 0f;

    transform.position = originalPos;

    Debug.Log("Rock Reset!");
  }
}
