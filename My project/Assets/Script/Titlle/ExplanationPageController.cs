using UnityEngine;
using UnityEngine.UI;

public class ExplanationPageController : MonoBehaviour
{
    [SerializeField] private Image explanationImage;
    [SerializeField] private Sprite[] pages;

    private int currentPage = 0;

    private void Start()
    {
        currentPage = 0;
        ShowPage();
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    private void ShowPage()
    {
        if (explanationImage != null && pages.Length > 0)
        {
            explanationImage.sprite = pages[currentPage];
        }
    }
}