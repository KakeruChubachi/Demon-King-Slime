using UnityEngine;

public class SkillAbsorptionRange : MonoBehaviour
{
    public Player player;

    void OnTriggerStay2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            if (!player.nearSkillOrbs.Contains(nearSkillOrb))
            {
                player.nearSkillOrbs.Add(nearSkillOrb);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            player.nearSkillOrbs.Remove(nearSkillOrb);
        }
    }
}
