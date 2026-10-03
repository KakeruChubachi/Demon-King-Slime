using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public int copiedHp;
    public float copiedMoveSpeed;
    public Sprite copiedSprite;
    public Sprite icon;

    public Effect[] effects; // ’Ç‰ÁF‚±‚ÌƒXƒLƒ‹‚ª‚ÂŒø‰Êˆê——
}