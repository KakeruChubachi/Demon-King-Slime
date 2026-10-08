using UnityEngine;

public class ResultSkillStorage : MonoBehaviour
{
    public static ResultSkillStorage Instance;

    // リザルトに表示する4スキル
    public SkillData[] resultSkills = new SkillData[4];

    // 一度でも取得したスキルを保存
    public SkillData[] acquiredSkills = new SkillData[6];

    // リザルト用のスライムレベル
    public int resultSlimeLevel = 1;

    private void Awake()
    {
        // すでに存在していたら新しい方を削除
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーンが変わっても残す
        DontDestroyOnLoad(gameObject);
    }

    public void AddAcquiredSkill(SkillData skill)
    {
        if (skill == null) return;

        // 同じスキル名なら、すでに取得済み
        for (int i = 0; i < acquiredSkills.Length; i++)
        {
            if (acquiredSkills[i] != null &&
                acquiredSkills[i].skillName == skill.skillName)
            {
                return;
            }
        }

        // 空いている場所に保存
        for (int i = 0; i < acquiredSkills.Length; i++)
        {
            if (acquiredSkills[i] == null)
            {
                acquiredSkills[i] = skill;
                Debug.Log("取得履歴に追加：" + skill.skillName);
                return;
            }
        }
    }

    // 現在の4スロットを保存
    public void SaveSkills(SkillData[] skills)
    {
        for (int i = 0; i < resultSkills.Length; i++)
        {
            if (i < skills.Length)
            {
                resultSkills[i] = skills[i];
            }
            else
            {
                resultSkills[i] = null;
            }
        }

        Debug.Log("リザルト用スキルを保存しました");
    }
}