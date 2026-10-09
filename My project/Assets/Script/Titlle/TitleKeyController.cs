
using UnityEngine;
using UnityEngine.UI;

public class TitleKeyController : MonoBehaviour
{
    [Header("左から順番にボタンを登録")]
    [SerializeField] private Button[] menuButtons;

    [Header("選択中の枠")]
    [SerializeField] private Outline[] outlines;

    private int currentIndex = 0;

    private void Start()
    {
        currentIndex = 0;
        SelectCurrentButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Decide();
        }
    }

    private void MoveLeft()
    {
        if (menuButtons.Length == 0) return;

        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = menuButtons.Length - 1;
        }

        SelectCurrentButton();
    }

    private void MoveRight()
    {
        if (menuButtons.Length == 0) return;

        currentIndex++;

        if (currentIndex >= menuButtons.Length)
        {
            currentIndex = 0;
        }

        SelectCurrentButton();
    }

    private void SelectCurrentButton()
    {
        // 全部の枠を消す
        for (int i = 0; i < outlines.Length; i++)
        {
            if (outlines[i] != null)
            {
                outlines[i].enabled = false;
            }
        }

        // 選択中の枠を表示
        if (currentIndex < outlines.Length &&
            outlines[currentIndex] != null)
        {
            outlines[currentIndex].enabled = true;
        }

        // 選択中のボタンを変更
        if (currentIndex < menuButtons.Length &&
            menuButtons[currentIndex] != null)
        {
            menuButtons[currentIndex].Select();
        }
    }

    private void Decide()
    {
        if (currentIndex < menuButtons.Length &&
            menuButtons[currentIndex] != null)
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }
}
