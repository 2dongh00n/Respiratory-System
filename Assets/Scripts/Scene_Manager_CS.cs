using UnityEngine.SceneManagement;
using UnityEngine;

public class Scene_Manager_CS : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }
    public void GOMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void Quizscene()
    {
        SceneManager.LoadScene("Quiz");
    }
}
