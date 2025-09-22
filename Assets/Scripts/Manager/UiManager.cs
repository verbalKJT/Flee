using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    void Awake()
    {
        if (FindObjectsOfType<UiManager>().Length > 1) // 싱글톤 모든 UI 관련 -> 이 오브젝트에서 관리
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
    public void OnSetting()
    {
        settingPanel.SetActive(true);
    }

    void FixedUpdate()
    {
        if (settingPanel.activeSelf && Input.GetKey(KeyCode.Escape))
        {
            settingPanel.SetActive(false);
        }
    }
    
}
