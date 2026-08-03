using UnityEngine;

public class SkillAbsorptionRange : MonoBehaviour
{
    public Player player;

    void OnTriggerStay2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            player.nearskillOrb = nearSkillOrb;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null && nearSkillOrb == player.nearskillOrb)
        {
            player.nearskillOrb = null;
        }
    }
}
