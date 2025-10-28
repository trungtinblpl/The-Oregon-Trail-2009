using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour
{
   public PlayableDirector director; // Timeline
    public CinemachineVirtualCamera vcamGameplay; // Camera gameplay chính

    // Hàm này sẽ được gọi bởi Signal Emitter
    public void EndCutscene()
    {
        Debug.Log("Cutscene finished -> Switch back to gameplay camera");

        // Tắt timeline
        director.gameObject.SetActive(false);

        // Kích hoạt lại camera gameplay
        if (vcamGameplay != null)
        {
            vcamGameplay.Priority = 20; // Ưu tiên cao hơn cutscene cam
        }
    }
}
