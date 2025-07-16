using Cinemachine;
using System.Xml;
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

  public Transform camFollowTarget1; // zone1 position target
  public Transform camFollowTarget2; // zone2 position target

  public float cameraZoneH = 9*2f; // orthographic size = 9

  private GameObject currActivePlayer;
  private CinemachineFramingTransposer transposer;

  private bool isPlayer1Active = true;

  public static CharacterSwitchController Instance { get; private set; }

  void Awake()
  {
    if(Instance == null)
      Instance = this;
    else
      Destroy(gameObject);
  }

  void Start()
  {
    ElecBoy.isControlled = true;
    IceBoy.isControlled = false;

    transposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();

    transposer.m_XDamping = 1000f;
    transposer.m_DeadZoneWidth = 1000f;
    transposer.m_ScreenX = 0.5f;

    transposer.m_YDamping = 0f;

    ElecBoy.gameObject.layer = LayerMask.NameToLayer("PlayerActive");
    IceBoy.gameObject.layer = LayerMask.NameToLayer("PlayerInactive");
    currActivePlayer = ElecBoy.gameObject;

    PlayerManager.ActivatePlayerTrans = ElecBoy.transform;

    SetCamFollowTarget(ElecBoy.transform);


    Player currPlayer = isPlayer1Active ? ElecBoy : IceBoy;
    FindObjectOfType<ActivePlayerUI>()?.UpdateIcon(currPlayer);
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

    SetCamFollowTarget(nextPlayer.transform);
    FindObjectOfType<ActivePlayerUI>()?.UpdateIcon(nextPlayer);
    PlayerManager.ActivatePlayerTrans = nextPlayer.transform;
  }

  private void SetCamFollowTarget(Transform playerTrans)
  {
    // where is player(zone)
    int zoneIndex = Mathf.FloorToInt(playerTrans.position.y);
    // middle of the zone
    float targetCamY = zoneIndex * cameraZoneH + (cameraZoneH / 2f);

    float midPointY = cameraZoneH / 2f; // 1920*1080 y unit = 18 18/2=9
    if (playerTrans.position.y < midPointY)
    {
      if (camFollowTarget1 != null)
      {
        vCam.Follow = camFollowTarget1;
      }
    }
    else
    {
      if (camFollowTarget2 != null)
      {
        vCam.Follow = camFollowTarget2;
      }
    }
  }

  public void NotifyPlayerTeleported()
  {
    if(PlayerManager.ActivatePlayerTrans != null)
      SetCamFollowTarget(PlayerManager.ActivatePlayerTrans);
  }
}
