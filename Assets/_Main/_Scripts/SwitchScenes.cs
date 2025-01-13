using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void StartGameButton()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
