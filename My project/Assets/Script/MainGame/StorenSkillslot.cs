using UnityEngine;
using UnityEngine.UI;

public class StorenSkillslot : MonoBehaviour
{
    public SkillData[] skillSlots = new SkillData[4];
    int selectedIndex = 0;
    public Image[] slotImages = new Image[4];
    public Color highlightColor = Color.yellow;
    public Color normalColor = Color.white;

    void Start()
    {
        UpdateSlotColor();
    }

    public void ReceiveSkills(SkillData skillData)
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] == null)
            {
                skillSlots[i] = skillData;
                Debug.Log("スキルをスロットに保存しました: " + skillData.skillName);
                return;
            }
        }
        Debug.Log("スキルスロットがいっぱいです。");
    }

    public void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                selectedIndex += 1;
                Debug.Log("+1");
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                selectedIndex -= 1;
                Debug.Log("-1");
            }
            selectedIndex = (selectedIndex + 4) % 4;
            UpdateSlotColor();
        }
    }

    void UpdateSlotColor()
    {
        for(int i = 0; i < slotImages.Length; i++)
        {
            if(i == selectedIndex)
            {
                slotImages[i].color = highlightColor;
            }
            else
            {
                slotImages[i].color = normalColor;
            }
        }
    }

    public SkillData GetSelectedSkill()
    {
        return skillSlots[selectedIndex];
    }
}
