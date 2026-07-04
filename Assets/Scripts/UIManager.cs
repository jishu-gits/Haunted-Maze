using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject gameOver;

    public TextMeshProUGUI winLoseText;
    public static UIManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameOver.SetActive(false);
    }

    public void ShowGameOver(bool isWin)
    {
        gameOver.SetActive(true);
        winLoseText.text = isWin ? "You Win" : "You Lose";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void PlayAgain()
    {
        // Reset time scale just in case the game was paused during game over
        Time.timeScale = 1f; 
        
        // Reloads the CURRENT scene, whatever index it happens to be
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
