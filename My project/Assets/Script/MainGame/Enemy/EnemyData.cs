using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int hp;
    public float MoveSpeed;
    public int attackPower;
    public Sprite sprite;
}
