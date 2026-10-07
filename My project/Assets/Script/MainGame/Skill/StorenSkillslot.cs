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

    public Image[] slotIcons = new Image[4]; // ★追加：各スロットのアイコン
    public Image newIcon;                    // ★追加：確認パネル右側のアイコン

    void Start()
    {
        UpdateSlotColor();
        RefreshSlotIcons(); // ★追加
    }

    public void ReceiveSkills(SkillData skillData)
    {

        // ★一度でも取得したスキルとして保存
        if (ResultSkillStorage.Instance != null)
        {
            ResultSkillStorage.Instance.AddAcquiredSkill(skillData);
        }
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] == null)
            {
                skillSlots[i] = skillData;
               Debug.Log("スキルをスロットに保存しました: " + skillData.skillName);
                RefreshSlotIcons(); // ★追加（return の前）
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
        //if (Input.GetKeyDown(KeyCode.Alpha7)) SelectAndUse(0);
        //else if (Input.GetKeyDown(KeyCode.Alpha8)) SelectAndUse(1);
        //else if (Input.GetKeyDown(KeyCode.Alpha9)) SelectAndUse(2);
        //else if (Input.GetKeyDown(KeyCode.Alpha0)) SelectAndUse(3);
    

    void SelectAndUse(int index)
    {
        Debug.Log("★★★ SelectAndUseに入りました index = " + index);

        selectedIndex = index;
        UpdateSlotColor();

        if (skillSlots[index] == null)
        {
            Debug.Log("★★★ このスロットは空です");
            return;
        }

        Debug.Log("★★★ 発動するスキル：" + skillSlots[index].skillName);

        player.ActivateCopy();
    }
    //void SelectAndUse(int index)
    //{
    //    selectedIndex = index;      // 先に選択を更新する
    //    UpdateSlotColor();

    //    if (skillSlots[index] == null) return; // 空きなら選択だけ
    //    player.ActivateCopy();
    //}

    // ★追加：スロットのアイコンを現在のスキルに合わせて更新
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
        RefreshSlotIcons(); // ★追加
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
        if (ResultSkillStorage.Instance != null)
        {
            ResultSkillStorage.Instance.AddAcquiredSkill(skillSlots[selectedIndex]);
            ResultSkillStorage.Instance.AddAcquiredSkill(pendingSkill);
        }


        Debug.Log(skillSlots[selectedIndex].skillName + " を " + pendingSkill.skillName + " に入れ替えました");


        skillSlots[selectedIndex] = pendingSkill;
        RefreshSlotIcons(); // ★追加
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

        // ★追加：拾ったスキルのアイコン
        newIcon.sprite = pendingSkill.icon;
        newIcon.enabled = (pendingSkill.icon != null);

        for (int i = 0; i < slotInfoTexts.Length; i++)
        {
            slotInfoTexts[i].color = (i == selectedIndex) ? highlightColor : normalColor;
        }
    }

    public void SaveSkillsForResult()
    {
        if (ResultSkillStorage.Instance == null)
        {
            Debug.LogWarning("ResultSkillStorageがありません");
            return;
        }

        // ★確認用：現在の4スロットの中身を見る
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] != null)
            {
                Debug.Log(
                    "保存前 スロット" + i +
                    " / スキル名：" + skillSlots[i].skillName +
                    " / アイコン：" + skillSlots[i].icon
                );
            }
            else
            {
                Debug.Log("保存前 スロット" + i + " / 空です");
            }
        }

        ResultSkillStorage.Instance.SaveSkills(skillSlots);

        Debug.Log("現在の4スキルをリザルト用に保存しました");
    }
}