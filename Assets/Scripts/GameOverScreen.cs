using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    Button restartButton;
    public void Setup(){
        gameObject.SetActive(true);
        restartButton = GetComponentInChildren<Button>();
        restartButton.Select();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("TerrainDemoScene");
    }
}
