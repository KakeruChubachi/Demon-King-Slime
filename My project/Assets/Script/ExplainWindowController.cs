using UnityEngine;

public class ExplainWindowController : MonoBehaviour
{
    [SerializeField]
    private GameObject explainWindow;

    // à–¾‚ğŠJ‚­
    public void OpenWindow()
    {
        explainWindow.SetActive(true);
    }

    // à–¾‚ğ•Â‚¶‚é
    public void CloseWindow()
    {
        explainWindow.SetActive(false);
    }
}