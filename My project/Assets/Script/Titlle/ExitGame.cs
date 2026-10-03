using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
#if UNITY_EDITOR
        // Unityで再生中なら停止
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドしたゲームなら終了
        Application.Quit();
#endif
    }
}