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
    currState?.Execute();
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
    currState?.Exit();
    currState = newState;
    currState.Enter();
  }
}