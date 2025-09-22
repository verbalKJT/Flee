using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ReStartGame()
    {
        SceneManager.LoadScene("1stFloor");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
