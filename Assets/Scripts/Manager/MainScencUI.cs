using UnityEngine;
using UnityEngine.UI;

public class MainScencUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        continueButton.onClick.AddListener(() => GameManager.instance.continueGame());
        settingButton.onClick.AddListener(() => UiManager.instance.OnSetting());
        quitButton.onClick.AddListener(() => GameManager.instance.GoToMainMenu());
    }
}
