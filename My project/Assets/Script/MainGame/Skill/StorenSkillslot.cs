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

    public Image[] slotIcons = new Image[4]; // 各スロットのアイコン
    public Image newIcon;                    // 確認パネル右側のアイコン

    // ★追加：交換の絵表示用
    public Image[] confirmSlotIcons = new Image[4]; // 確認パネル内の各スロットの絵
    public Image swapOutIcon;                       // 交換される側の絵（選択中スロット）

    void Start()
    {
        UpdateSlotColor();
        RefreshSlotIcons();
    }

    public void ReceiveSkills(SkillData skillData)
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] == null)
            {
                skillSlots[i] = skillData;
                Debug.Log("スキルをスロットに保存しました: " + skillData.skillName);
                RefreshSlotIcons();
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
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectAndUse(3);
        else if (Input.GetKeyDown(KeyCode.Alpha8)) SelectAndUse(2);
        else if (Input.GetKeyDown(KeyCode.Alpha9)) SelectAndUse(1);
        else if (Input.GetKeyDown(KeyCode.Alpha0)) SelectAndUse(0);
    }

    void SelectAndUse(int index)
    {
        Debug.Log("★★★ SelectAndUseに入りました index = " + index);

        selectedIndex = index;
        UpdateSlotColor();

        // ★追加：確認パネル中は選択だけ（スキルは発動しない）
        if (confirmPanel.activeSelf) return;

        if (skillSlots[index] == null)
        {
            Debug.Log("★★★ このスロットは空です");
            return;
        }

        Debug.Log("★★★ 発動するスキル：" + skillSlots[index].skillName);

        player.ActivateCopy();
    }

    // スロットのアイコンを現在のスキルに合わせて更新
    void RefreshSlotIcons()
    {
        for (int i = 0; i < slotIcons.Length; i++)
        {
            SkillData s = skillSlots[i];
            slotIcons[i].sprite = (s != null) ? s.icon : null;
            slotIcons[i].enabled = (s != null && s.icon != null);
        }
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

    public void ConsumeSelectedSkill()
    {
        skillSlots[selectedIndex] = null;
        RefreshSlotIcons();
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
        RefreshSlotIcons();
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

        // 拾ったスキルのアイコン
        newIcon.sprite = pendingSkill.icon;
        newIcon.enabled = (pendingSkill.icon != null);

        for (int i = 0; i < slotInfoTexts.Length; i++)
        {
            slotInfoTexts[i].color = (i == selectedIndex) ? highlightColor : normalColor;
        }

        // ★追加：確認パネル内の各スロットの絵（選択中だけ少し大きくする）
        for (int i = 0; i < confirmSlotIcons.Length; i++)
        {
            SkillData s = skillSlots[i];
            confirmSlotIcons[i].sprite = (s != null) ? s.icon : null;
            confirmSlotIcons[i].enabled = (s != null && s.icon != null);
            confirmSlotIcons[i].transform.localScale = (i == selectedIndex) ? Vector3.one * 1.2f : Vector3.one;
        }

        // ★追加：「交換される絵 → 新しい絵」
        SkillData outSkill = skillSlots[selectedIndex];
        swapOutIcon.sprite = (outSkill != null) ? outSkill.icon : null;
        swapOutIcon.enabled = (outSkill != null && outSkill.icon != null);
    }
}