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
        Debug.Log("ÉIÅ[Éuè¡îÔ: " + gameObject.name);
        Destroy(gameObject);
        return skillData;
    }
}
