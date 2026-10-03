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

        // 最初は全部の枠を消す
        for (int i = 0; i < outlines.Length; i++)
        {
            outlines[i].enabled = false;
        }

        // 説明ボタンを選択
        SelectCurrentButton();
    }

    private void Update()
    {
        // Aキー：左
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }

        // Dキー：右
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

        // スペース：決定
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Decide();
        }
    }

    private void MoveLeft()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = menuButtons.Length - 1;
        }

        SelectCurrentButton();
    }

    private void MoveRight()
    {
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
            outlines[i].enabled = false;
        }

        // 現在選択しているボタンの枠だけ表示
        outlines[currentIndex].enabled = true;

        // Unityの選択状態も変更
        menuButtons[currentIndex].Select();
    }

    private void Decide()
    {
        // 現在選択しているボタンを押す
        menuButtons[currentIndex].onClick.Invoke();
    }
}