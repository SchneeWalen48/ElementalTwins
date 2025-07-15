using UnityEngine;

public enum SkillType { Dash, Freeze }

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
  public SkillType skillType;

  public float cooldown;
  public float duration;
  public float range;
  public int dmgMultiplier = 2;

  public GameObject projPrefab;

  public string skillName;
  public Sprite skillIcon;
}