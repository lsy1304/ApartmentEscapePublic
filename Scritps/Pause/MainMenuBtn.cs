using UnityEngine;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour
{
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => SceneLoadManager.Instance.LoadMainMenu(false));
    }
}
