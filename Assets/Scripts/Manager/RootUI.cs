using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RootUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject settingCanvas;
    [SerializeField]private TMP_Dropdown windowDropdown;
    [SerializeField]private TMP_Dropdown resolutionDropdown;
    [SerializeField]private Scrollbar volumeScrollbar;
    void Start()
    {
        UiManager.instance.Register(pauseCanvas, settingCanvas,windowDropdown,resolutionDropdown,volumeScrollbar);
    }
}
