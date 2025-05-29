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
        Debug.Log("ȯ�漳�� UI ����"); 
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
