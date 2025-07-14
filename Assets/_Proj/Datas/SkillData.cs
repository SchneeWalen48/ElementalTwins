using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
  public string skillName;
  public Sprite skillIcon;
  public float cooldown;
  public GameObject skillPrefab;
}