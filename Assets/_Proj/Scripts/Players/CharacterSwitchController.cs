using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public static Transform ActivatePlayerTrans;
  public static PlayerManager Instance { get; private set; }

  [HideInInspector] public Transform activePlayerTrans;

  void Awake()
  {
    if(Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }
}
public class CharacterSwitchController : MonoBehaviour
{
  public Player ElecBoy;
  public Player IceBoy;
  public CinemachineVirtualCamera vCam;

  private GameObject currActivePlayer;
  private CinemachineFramingTransposer transposer;

  private bool isPlayer1Active = true;

  private float originYDamping;
  private float originYDeadZoneHeight;
  private float originScreenY;

  void Start()
  {
    ElecBoy.isControlled = true;
    IceBoy.isControlled = false;

    vCam.Follow = ElecBoy.transform;
    transposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();

    originYDamping =transposer.m_YDamping;
    originYDeadZoneHeight = transposer.m_DeadZoneHeight;
    originScreenY = transposer.m_ScreenY;

    ElecBoy.gameObject.layer = LayerMask.NameToLayer("PlayerActive");
    IceBoy.gameObject.layer = LayerMask.NameToLayer("PlayerInactive");
    currActivePlayer = ElecBoy.gameObject;

    PlayerManager.ActivatePlayerTrans = ElecBoy.transform;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
    {
      SwitchPlayer();
    }
  }

  private void SwitchPlayer()
  {
    Player currPlayer = isPlayer1Active ? ElecBoy : IceBoy;
    Player nextPlayer = isPlayer1Active ? IceBoy : ElecBoy;

    // Deactivate Current Player
    currPlayer.isControlled = false;
    currPlayer.anim.SetBool("isJump", false);
    currPlayer.isGrounded = true;
    currPlayer.gameObject.layer = LayerMask.NameToLayer("PlayerInactive");

    // Activate Next Player
    nextPlayer.isControlled = true;
    nextPlayer.anim.SetBool("isJump", false);
    nextPlayer.gameObject.layer = LayerMask.NameToLayer("PlayerActive");

    // Transfer Camera Target
    vCam.Follow = nextPlayer.transform;

    currPlayer = nextPlayer;

    nextPlayer.isGrounded = true;

    // Transfer character status flag
    isPlayer1Active = !isPlayer1Active;

    ForceCamToNewPlayerPos(nextPlayer.transform);

    PlayerManager.ActivatePlayerTrans = nextPlayer.transform;
  }

  void ForceCamToNewPlayerPos(Transform targetTransform)
  {
    Player currPlayer = isPlayer1Active ? ElecBoy : IceBoy;
    Player nextPlayer = isPlayer1Active ? IceBoy : ElecBoy;

    if (transposer != null)
    {
      transposer.m_YDamping = 0.3f;
      transposer.m_DeadZoneHeight = 0.2f;
      //transposer.m_ScreenY = 0.97f;
      float yDiff = currPlayer.transform.position.y - nextPlayer.transform.position.y;

      float thresholdMin = originScreenY - 0.5f;
      float thresholdMax = originScreenY + 0.5f;
      if(yDiff > thresholdMin)
      {
        float t = Mathf.InverseLerp(thresholdMin, thresholdMax, yDiff);
        float boostedY = Mathf.Lerp(originScreenY, 1f - originYDeadZoneHeight, t);
        transposer.m_ScreenY = boostedY;
        Invoke("RestoreCamYSetting", 0f);
      }
      Invoke("RestoreCamYSetting", 1f);
    }
  }

  void RestoreCamYSetting()
  {
    if(transposer != null)
    {
      transposer.m_YDamping = originYDamping;
      transposer.m_DeadZoneHeight = originYDeadZoneHeight;
      transposer.m_ScreenY = originScreenY;
    }
  }
}
