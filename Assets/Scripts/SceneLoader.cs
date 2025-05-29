using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("IntroCinematic");
    }
    public void OnClickSettings()
    {
        Debug.Log("환경설정 UI 열기"); 
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
