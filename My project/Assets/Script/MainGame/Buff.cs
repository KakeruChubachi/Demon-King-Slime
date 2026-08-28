using UnityEngine;

public class Buff : Effect
{
    public int buffAmount = 5;
    public int buffType = 0;
    public override void ApplyEffect(Player player)
    {
        switch (buffType)
        {
            case 0:
                //player.physics += buffAmount;
                Debug.Log("物理攻撃力が" + buffAmount + "増加しました。");
                break;
            case 1:
                //player.longdistance += buffAmount;
                Debug.Log("遠距離攻撃力が" + buffAmount + "増加しました。");
                break;
            case 2:
                //player.magicAttack += buffAmount;
                Debug.Log("魔法攻撃力が" + buffAmount + "増加しました。");
                break;
            case 3:
                //player.hp += buffAmount;
                Debug.Log("HPが" + buffAmount + "増加しました。");
                break;
            default:
                Debug.Log("無効なバフタイプです。");
                break;
        }
    }
}
