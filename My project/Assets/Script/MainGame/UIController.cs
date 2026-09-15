using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text slimeLabelText;
    public Text lifeText;
    public Text timeText;
    public Text stageText;
    public Slider hpSlider;
    public Slider expSlider;
    public Slider timeSlider;

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
        hpSlider.value = life;
    }

    public void SetTime(float time,float maxTime)
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;
        timeText.text =  min.ToString("00") + ":" + sec.ToString("00");
        timeSlider.maxValue = maxTime;
        timeSlider.value = time;
    }

    public void SetExp(int currentExp, int levelUpExp)
    {
        expSlider.maxValue = levelUpExp;
        expSlider.value = currentExp;
    }
}
