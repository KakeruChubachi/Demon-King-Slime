using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Barrier,
    Copy,
    Avoidance,
    None
}

public class SkillSlot : MonoBehaviour
{
    public Image SkillImage;
    public KeyCode SkillKey;
    public float CooldownTime = 1.0f;
    public float nowTime = 0.0f;
    public Player player;
    public SkillType skillType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nowTime > 0)
        {
            nowTime -= Time.deltaTime;
            SkillImage.color = Color.Lerp(Color.white, Color.black, nowTime/CooldownTime);
            if (nowTime < 0)
            {
                nowTime = 0;
            }
        }
        else
        {
            if(Input.GetKeyDown(SkillKey))
            {
                nowTime = CooldownTime;
                switch(skillType)
                {
                    case SkillType.Barrier:
                        player.ActivateBarrier();
                        break;
                    case SkillType.Copy:
                        player.ActivateCopy();
                        break;
                    case SkillType.Avoidance:
                        player.ActivateAvoidance();
                        break;
                }
            }
        }
    }
}
