using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text slimeLabelText;
    public Text lifeText;
    public Text timeText;
    public Text stageText;

    void Start()
    {
       // SetSllimeLevel(1);
    }

    public void SetSllimeLevel(int level)
    {
        slimeLabelText.text = "Lv:" + level;
    }

    public void SetLife(int life)
    {
        lifeText.text = "Life:" + life;
    }

    public void SetTime(float time)
    {
        timeText.text = "Time:" + time.ToString("F2");
    }

    public void SetStage(int stage)
    {
        stageText.text = "Stage:" + stage;
    }
}
