using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene("1stFloor");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
