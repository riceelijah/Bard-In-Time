using UnityEngine;
using UnityEngine.SceneManagement;

// Lets UI buttons load scenes; LoadScene is static so a button can't call it directly.
public class MainMenu : MonoBehaviour
{
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
}
