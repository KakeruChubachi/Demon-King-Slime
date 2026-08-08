using UnityEngine;

public class SkillOrb : MonoBehaviour
{
    public Transform target;
    public SkillData skillData;

    private void Update()
    {
        if (target == null)
        {
            return;
        }
    }

    public SkillData GetSkillOrb()
    {
        Destroy(gameObject);
        return skillData;
    }
}
