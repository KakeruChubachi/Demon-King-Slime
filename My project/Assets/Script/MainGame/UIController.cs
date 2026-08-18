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
        lifeText.text = "ƒ‰ƒCƒt:" + life;
    }

    public void SetTime(float time)
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;
        timeText.text =  min.ToString("00") + ":" + sec.ToString("00");

    }
}
