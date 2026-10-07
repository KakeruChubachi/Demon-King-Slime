using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultSkillDisplay : MonoBehaviour
{
    [Header("スキルアイコン")]
    [SerializeField] private Image[] skillIcons = new Image[6];

    [Header("スライムレベル")]
    [SerializeField] private TextMeshProUGUI slimeLevelText;

    [Header("スキル名")]
    [SerializeField] private TextMeshProUGUI[] skillNames = new TextMeshProUGUI[6];

    private void Start()
    {
        ShowSkills();
    }

    private void ShowSkills()
    {
        // ResultSkillStorageが存在しない場合
        if (ResultSkillStorage.Instance == null)
        {
            Debug.LogWarning("ResultSkillStorageがありません");
            return;
        }

        SkillData[] skills = ResultSkillStorage.Instance.acquiredSkills;
        slimeLevelText.text = "Lv. " + ResultSkillStorage.Instance.resultSlimeLevel;

        // 4つまで表示
        for (int i = 0; i < 6; i++)
        {
            if (skills[i] != null)
            {
                // アイコンを表示
                skillIcons[i].sprite = skills[i].icon;
                skillIcons[i].enabled = (skills[i].icon != null);

                // スキル名を表示
                skillNames[i].text = skills[i].skillName;
            }
            else
            {
                // スキルがない場所は消す
                skillIcons[i].sprite = null;
                skillIcons[i].enabled = false;

                skillNames[i].text = "";
            }
        }
    }
}