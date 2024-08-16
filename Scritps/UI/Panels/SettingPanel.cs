using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    public void EnterSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void ExitSettingPanel() 
    { 
        settingPanel.SetActive(false);
    }
}
