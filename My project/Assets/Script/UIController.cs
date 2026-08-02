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
}
