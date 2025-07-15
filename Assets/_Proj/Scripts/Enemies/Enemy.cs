using System.Collections;
using UnityEngine;

public abstract class EnemyState
{
  protected Enemy enemy;
  public EnemyState(Enemy enemy)
  {
    this.enemy = enemy;
  }

  public abstract void Enter();
  public abstract void Execute();
  public abstract void Exit();
}
public class Enemy : MonoBehaviour
{
  public float detectRangeX = 5f;
  public float detectRangeY = 1f;
  public Transform playerTarget;

  protected EnemyState currState;
  public Animator anim;

  [HideInInspector]public bool isFrozen = false;

  protected float freezeTimer = 0f;
  protected virtual void Start()
  {

    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null){
      playerTarget = playerObj.transform;
    }
    else
    {
      print("플레이어 타깃 없음"); 
    }
  }

  protected virtual void Update()
  {
    UpdateTarget();
    if (isFrozen) return;
    currState?.Execute();

  }

  protected virtual void FixedUpdate()
  {
    if (isFrozen) return;
  }

  private void UpdateTarget()
  {
    if(PlayerManager.ActivatePlayerTrans != null && playerTarget != PlayerManager.ActivatePlayerTrans)
    {
      playerTarget = PlayerManager.ActivatePlayerTrans;
      print("타깃 변경됨" + playerTarget.name);
    }
  }
  

  public void ChangeState(EnemyState newState)
  {
    if (isFrozen) return;
    currState?.Exit();
    currState = newState;
    currState.Enter();
  }

  public void StartFreeze(float duration)
  {
    if (freezeTimer > 0f) return;
    freezeTimer = duration;
  }
  public IEnumerator Freeze(float duration) {
    Debug.Log($"[Freeze] 코루틴 시작됨: {gameObject.name}");
    if (isFrozen)
    {
      Debug.LogWarning("[Freeze] 이미 얼어있음 → 종료"); yield break;
    }
    isFrozen = true;
    print("freeze start");
    if (anim != null) 
    { 
      Debug.Log("[Freeze] 애니메이터 존재함 → freeze 상태 적용 중");
      anim.SetBool("isFreeze", true);
      //anim.speed = 0.05f;
    }

    yield return new WaitForSeconds(duration);

    print("freeze end");

    if (anim != null)
    {
      try
      {
        //anim.speed = 1f;
        anim.SetBool("isFreeze", false);

      }
      catch (System.Exception e) { yield break; }
    }
    isFrozen = false;
    //currState?.Enter();
  }
}