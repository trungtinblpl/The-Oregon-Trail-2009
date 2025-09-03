using UnityEngine;

public class InfoController : MonoBehaviour
{
    [SerializeField] private string[] subNames;   // Tên (Banker, Farmer, Carpenter...)
    [SerializeField] private GameObject[] subPanels; // Panel tương ứng

    public void ShowInfo(string subName)
    {
        // Debug.Log($"[InfoController] Nhận subName: {subName}");

        for (int i = 0; i < subNames.Length; i++)
        {
            bool active = (subNames[i] == subName);
            if (subPanels[i] != null)
            {
                subPanels[i].SetActive(active);
                // Debug.Log($"[InfoController] SubPanel {subNames[i]} -> {active}");
            }
        }
    }

}
