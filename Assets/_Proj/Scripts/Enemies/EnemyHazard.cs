using System.Threading.Tasks;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHazard : Enemy
{
  public float patrolSpeed;
  public float chaseSpeed;
  public Rigidbody2D rb;
  public SpriteRenderer sr;

  private Vector2 originPos;
  private Vector2 obstaclePos;

  private bool isBlocked = false;

  protected override void Start()
  {
    base.Start();
    originPos = transform.position;
    ChangeState(new HazardPatrol(this));
  }

  void FixedUpdate()
  {
    if (PlayerManager.Instance != null && PlayerManager.Instance.activePlayerTrans != null)
    {
      this.playerTarget = PlayerManager.Instance.activePlayerTrans;
    }
    else
    {
      this.playerTarget = null;
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      Player p = collision.gameObject.GetComponent<Player>();
      if (p != null)
      {
        p.Respawn();
      }
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("EWall") || collision.gameObject.CompareTag("Props"))
    {
      isBlocked = true;
      obstaclePos = collision.transform.position;
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if(collision.gameObject.CompareTag("EWall") || collision.gameObject.CompareTag("Props"))
      isBlocked = false;
  }

  // ==================================================
  // Define a nested FSM state class insdie
  // ==================================================


  // ==================================================
  // PATROL
  // ==================================================
  private class HazardPatrol : EnemyState
  {
    float patrolLX;
    float patrolRX;
    int dir = 1;
    public HazardPatrol(Enemy enemy) : base(enemy) { 
      EnemyHazard h = (EnemyHazard)enemy;
      float patrolRange = 2f;
      patrolLX = h.originPos.x - patrolRange;
      patrolRX = h.originPos.x + patrolRange;
      h.sr.flipX = (dir == 1);
    }
    public override void Enter() => print("patrol start");
    public override void Execute()
    {
      EnemyHazard h = (EnemyHazard)enemy;
      if (enemy.playerTarget != null)
      {
        float distX = Mathf.Abs(enemy.playerTarget.position.x - enemy.transform.position.x);
        float distY = Mathf.Abs(enemy.playerTarget.position.y - enemy.transform.position.y);
        if (distX < enemy.detectRangeX && distY < enemy.detectRangeY)
        {
          enemy.ChangeState(new HazardChase(enemy));
          return;
        }

        if (h.isBlocked)
        {
          Debug.Log("Blocked by a prop or a wall.");
          return;
        }

      }
      Vector2 currPos = enemy.transform.position;
      Vector2 taragetPos = currPos + Vector2.right * dir * h.patrolSpeed * Time.deltaTime;

      h.rb.MovePosition(taragetPos);

      if (currPos.x >= patrolRX && dir == 1) { dir = -1; h.sr.flipX = false; }
      else if (currPos.x <= patrolLX && dir == -1) {dir = 1; h.sr.flipX = true;}
    }

    public override void Exit() => print("end patrol");
  }



  // ==================================================
  // CHASE
  // ==================================================
  private class HazardChase : EnemyState
  {
    public HazardChase(Enemy enemy) : base(enemy) { }
    public override void Enter() { }

      //TODO: 추적 애니메이션 시작. 회전 시 flipX. 활성/비활성화 캐릭터 에게 다가가서 몸통 박치기로 공격
    public override void Execute()
    {
      EnemyHazard h = (EnemyHazard)enemy;
      if (enemy.playerTarget == null)
      {
        enemy.ChangeState(new HazardReturn(enemy));
        return;
      }

      Vector2 currPos = h.rb.position;
      Vector2 targetPos = enemy.playerTarget.position;

      float distX = Mathf.Abs(currPos.x - targetPos.x);
      float distY = Mathf.Abs(currPos.y - targetPos.y);

      if (distX > enemy.detectRangeX + 1f || distY > enemy.detectRangeY + 0.1f) 
      {
        enemy.ChangeState(new HazardReturn(enemy));
      }
      
      if (h.isBlocked)
      {
        Debug.Log("Chasing...Blocked by a prop!");
        enemy.ChangeState(new RestrictPatrol(enemy,h.obstaclePos.x));
        return;
      }

      float dir = Mathf.Sign(targetPos.x - currPos.x);
      if (dir < 0) h.sr.flipX = false; else if(dir > 0) h.sr.flipX = true;
        Vector2 nextPos = currPos + Vector2.right * dir * h.chaseSpeed * Time.deltaTime;
      
      h.rb.MovePosition(nextPos);
    }

    public override void Exit()
    {
      print("추적 종료");
      // 추적 애니메이션 중지
    }
  }



  // ==================================================
  // RESTRICED PATROL
  // ==================================================
  private class RestrictPatrol : EnemyState
  {
    float patrolLX;
    float patrolRX;
    int dir = 1;
    private float obstacleXRef;
    private float exitObsMargin = 0.2f;

    public RestrictPatrol(Enemy enemy, float obsX) : base(enemy)
    {
      EnemyHazard h = (EnemyHazard)enemy;
      float patrolRange = 1f;
      obstacleXRef = obsX;

      if (obsX < h.transform.position.x)
      {
        patrolLX = obsX;
        patrolRX = obsX + patrolRange;
        dir = 1;
        h.sr.flipX = true;
      }
      else
      {
        patrolLX = obsX - patrolRange;
        patrolRX = obsX;
        dir = -1;
        h.sr.flipX = false;
      }
    }
    public override void Enter() => Debug.Log("Restrict Patrol Start...");

    public override void Execute()
    {
      EnemyHazard h = (EnemyHazard)enemy;
      if (enemy.playerTarget == null || Mathf.Abs(enemy.playerTarget.position.x - enemy.transform.position.x) > enemy.detectRangeX)
      {
        enemy.ChangeState(new HazardReturn(enemy));
        return;
      }

      if (!h.isBlocked && enemy.playerTarget != null)
      {
        float distX = Mathf.Abs(enemy.playerTarget.position.x - enemy.transform.position.x);
        float distY = Mathf.Abs(enemy.playerTarget.position.y - enemy.transform.position.y);

        bool awayFromObstacl = false;
        if (dir == 1)
        {
          awayFromObstacl = (h.transform.position.x > obstacleXRef + exitObsMargin);
        }
        if (distX < enemy.detectRangeX && distY < enemy.detectRangeY && awayFromObstacl)
        {
          enemy.ChangeState(new HazardChase(enemy));
          return;
        }
      }
      Vector2 currPos = h.rb.position;
      Vector2 nextPos = currPos + Vector2.right * dir * h.patrolSpeed * Time.deltaTime;
      h.rb.MovePosition(nextPos);

      if (currPos.x >= patrolRX - 0.05f && dir == 1) { dir = -1; h.sr.flipX = false; }
      else if (currPos.x <= patrolLX + 0.05f && dir == -1) {dir = 1; h.sr.flipX = true; }
    }

    public override void Exit() { }
  }


  // ==================================================
  // RETURN
  // ==================================================
  private class HazardReturn : EnemyState
  {
    public float returnSpeed;
    public HazardReturn(Enemy enemy) : base(enemy) {
      EnemyHazard h = (EnemyHazard)enemy;
      returnSpeed = h.patrolSpeed;
    }
    public override void Enter() { }
    public override void Execute()
    {
      EnemyHazard h = (EnemyHazard)enemy;
      if (h.isBlocked)
      {
        Debug.Log("Returning...Blocked by a prop!! I want to go home!!");
        enemy.ChangeState(new RestrictPatrol(enemy, h.obstaclePos.x));
        return;
      }

      Vector2 currPos = h.rb.position;
      float dir = Mathf.Sign(h.originPos.x - currPos.x);
      if (dir < 0) h.sr.flipX = false; else if (dir > 0) h.sr.flipX = true;
        Vector2 nextPos = currPos + Vector2.right * dir * returnSpeed * Time.deltaTime;
      
      h.rb.MovePosition(nextPos);

      if(Mathf.Abs(h.rb.position.x - h.originPos.x) < 0.05f)
      {
        enemy.ChangeState(new HazardPatrol(enemy));
      }
    }

    public override void Exit() { }
  }
}
