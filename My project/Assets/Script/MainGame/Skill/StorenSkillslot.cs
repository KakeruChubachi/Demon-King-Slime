using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorenSkillslot : MonoBehaviour
{
    public SkillData[] skillSlots = new SkillData[4];
    int selectedIndex = 0;
    public Image[] slotImages = new Image[4];
    public Color highlightColor = Color.yellow;
    public Color normalColor = Color.white;
    public GameObject confirmPanel;
    private SkillData pendingSkill;
    public TextMeshProUGUI[] slotInfoTexts = new TextMeshProUGUI[4]; // 4スロット分の内容表示
    public TextMeshProUGUI newInfoText; // 右側：拾ったスキルの内容
    public Player player; // Inspectorで設定

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

        // 満タンなら確認パネルを表示する
        pendingSkill = skillData;
        confirmPanel.SetActive(true);
        Time.timeScale = 0f;
        UpdateConfirmInfo();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectAndUse(0);
        else if (Input.GetKeyDown(KeyCode.Alpha8)) SelectAndUse(1);
        else if (Input.GetKeyDown(KeyCode.Alpha9)) SelectAndUse(2);
        else if (Input.GetKeyDown(KeyCode.Alpha0)) SelectAndUse(3);
    }

    void SelectAndUse(int index)
    {
        if (skillSlots[index] == null) return; // 空きスロットなら何もしない

        selectedIndex = index;
        UpdateSlotColor();
        player.ActivateCopy();
    }

    void UpdateSlotColor()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (i == selectedIndex)
            {
                slotImages[i].color = highlightColor;
            }
            else
            {
                slotImages[i].color = normalColor;
            }
        }

        if (confirmPanel.activeSelf)
        {
            UpdateConfirmInfo();
        }
    }

    public SkillData GetSelectedSkill()
    {
        return skillSlots[selectedIndex];
    }

    public int CountDuplicates(SkillData skill)
    {
        int count = 0;
        foreach (SkillData s in skillSlots)
        {
            if (s == skill)
            {
                count++;
            }
        }
        return count;
    }

    public void ConfirmSwap()
    {
        Debug.Log(skillSlots[selectedIndex].skillName + " を " + pendingSkill.skillName + " に入れ替えました");
        skillSlots[selectedIndex] = pendingSkill;
        pendingSkill = null;
        confirmPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CancelSwap()
    {
        Debug.Log(pendingSkill.skillName + " の入手をキャンセルしました");
        pendingSkill = null;
        confirmPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void UpdateConfirmInfo()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            string info = "";
            if (skillSlots[i] != null)
            {
                foreach (Effect effect in skillSlots[i].effects)
                {
                    info += effect.effectName + "\n" + effect.description + "\n\n";
                }
            }
            else
            {
                info = "（空き）";
            }
            slotInfoTexts[i].text = info;
        }

        string newInfo = "";
        foreach (Effect effect in pendingSkill.effects)
        {
            newInfo += effect.effectName + "\n" + effect.description + "\n\n";
        }
        newInfoText.text = newInfo;

        for (int i = 0; i < slotInfoTexts.Length; i++)
        {
            slotInfoTexts[i].color = (i == selectedIndex) ? highlightColor : normalColor;
        }
    }
}
