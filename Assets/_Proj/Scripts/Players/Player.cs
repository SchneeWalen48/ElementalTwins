using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
  public Animator anim;
  private Rigidbody2D rb;
  private SpriteRenderer sr;

  public float speed;
  private float currSpeed;
  public float debuffAmount;
  public float shotInterval;
  public float skillCool = 2f;
  private float skillTimer = 0f;
  public float jumpForce;
  [Tooltip("0 < reduce speed < 1")]
  public float reduceSpeed;

  [HideInInspector]public bool isGrounded = true;
  [HideInInspector] public bool isControlled = false;

  protected int lastDir = 1;

  public GameObject basicProjPrefab;
  public GameObject skillPrefab;
  public float shotSpeed;
  public float shotLife;

  public Transform shotPoint;

  private bool leverActivate = false;
  public float levInteractCool = 1f;
  public float levInteractTimer = 0f;

  public LayerMask levLayer;
  public float levInterRange = 1f;

  public Transform respawnPoint;

  private Dictionary<ItemType, bool> collectedItem = new Dictionary<ItemType, bool>();

  public ItemType myAssignedItemType;
  void Awake()
  {
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    rb.freezeRotation = true;
    sr = GetComponentInChildren<SpriteRenderer>();
    currSpeed = speed;

    foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
    {
        collectedItem[type] = false;
    }
  }


  protected virtual void Update()
  {
    if (!isControlled) return;
    Move();
    KeyInputControl();
    CoolTimerUpdate();

    if(levInteractTimer > 0)
    {
      levInteractTimer -= Time.deltaTime;
    }
  }

  protected virtual void Move() 
  {
    float x = Input.GetAxisRaw("Horizontal");
    float currSpeed = rb.velocity.x;
    if (x != 0)
    {
      lastDir = (int)Mathf.Sign(x);
      sr.flipX = (lastDir == -1);
    }
   
    float targetSpeed = x * speed;

    if (!isGrounded)
    {
      if (Mathf.Sign(currSpeed) == Mathf.Sign(x) || x == 0)
      {
        rb.velocity = new Vector2(currSpeed, rb.velocity.y);
      }
      else if(Mathf.Sign(currSpeed) != Mathf.Sign(x) && x != 0)
      {
        float smoothX = Mathf.Lerp(currSpeed, targetSpeed, reduceSpeed);
        rb.velocity = new Vector2(smoothX, rb.velocity.y);
      }
      else if(x == 0)
      {
        float smoothX = Mathf.MoveTowards(currSpeed, targetSpeed, reduceSpeed);
        rb.velocity = new Vector2(smoothX, rb.velocity.y);
      }
    }
    else
      rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

    anim.SetBool("isWalk", x != 0 && isGrounded);

    if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      rb.velocity = new Vector2(rb.velocity.x, 0f);
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      isGrounded = false;
      anim.SetBool("isJump", !isGrounded);
    }
  }

  protected virtual void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      isGrounded = true;
      anim.SetBool("isJump", false);
    }
    if (CompareTag("EnemyAtck")) { //TODO : Player가 Enemy나 Enemy가 발사한 총에 3번 맞으면 리스폰
    }
  }

  public void SetControlled(bool controlled)
  {
    isControlled = controlled;

    gameObject.layer = LayerMask.NameToLayer(controlled ? "PlayerActive" : "PlayerInActive");
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
  }

  // J : Basick Attack. K : Skill1. L : Skill2. O : Interact lever
  protected virtual void KeyInputControl()
  {
    if (Input.GetKeyDown(KeyCode.J))
    {
      BasicAtck();
    }
    if (Input.GetKeyDown(KeyCode.K) && skillTimer <= 0f)
    {
      Skill();
      skillTimer = skillCool;
    }
    if (Input.GetKeyDown(KeyCode.O)) 
    {
      if (levInteractTimer <= 0f)
      {
        bool interacted = TryInteractLever();
        if (interacted)
        {
          levInteractTimer = levInteractCool;
        }
        else
        {
          Debug.Log("Failed manipulate the lever!");
        }
      }
      else Debug.Log($"Lever Cooldown. Remain Time : {levInteractTimer}");
    }
  }

  private bool TryInteractLever()
  {
    Vector2 centre = transform.position + Vector3.up * 0.5f;
    Collider2D[] hits = Physics2D.OverlapCircleAll(centre, levInterRange, levLayer);
    foreach(var hit in hits)
    {
      if (hit.TryGetComponent(out LeverHandle lev))
      {
        lev.TryActivate();
        leverActivate = true;

      }
      if(hit.TryGetComponent(out LeverHandleToggle toggleLev))
      {
        toggleLev.TryToggle();
        leverActivate = true;
        
      }

      if(hit.TryGetComponent(out Generator generator))
      {
        generator.TryActivate(this);
        leverActivate = true;
        return true;
      }
    }
    return leverActivate;
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Vector3 centre = transform.position + Vector3.up * 0.5f;
    Gizmos.DrawWireSphere(centre, levInterRange);
  }

  public void Respawn()
  {
    Debug.Log("Respawn!");
    transform.position = respawnPoint.position;
    rb.velocity = Vector2.zero;

    if(anim != null)
    {
      anim.enabled = true;
      anim.Rebind();
      anim.Update(0f);
      SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
      if (sr != null) sr.enabled = true;

      anim.SetTrigger("respawn");
    }

    anim.SetBool("isJump", false);
    anim.SetBool("isWalk", false );
  }

  public void ApplyDebuff(float amount)
  {
    currSpeed -= amount;
    if (currSpeed < 0f) { currSpeed = 0.5f; }
    StartCoroutine(RemoveDebuff(amount, 1.5f));
  }
  void CoolTimerUpdate()
  {
    if (skillTimer > 0) skillTimer -= Time.deltaTime;
  }
  protected virtual void BasicAtck()
  {
    anim.SetTrigger("shot");
  }

  private void Skill()
  {
    Instantiate(skillPrefab, shotPoint.position, Quaternion.identity);
  }

  public void ApplyKnockback(Vector2 dir, float force)
  {
    if(rb != null)
    {
      rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
  }

  IEnumerator RemoveDebuff(float amount, float delay)
  {
    yield return new WaitForSeconds(delay);
    currSpeed += amount;
  }
  public void CollectItem(ItemType type)
  {
    if (collectedItem.ContainsKey(type))
    {
      collectedItem[type] = true;
    }
  }

  public bool HasItem(ItemType type)
  {
    return collectedItem.ContainsKey(type) && collectedItem[type];
  }

  public bool HasMyAssignedItem()
  {
    return HasItem(myAssignedItemType);
  }
}
