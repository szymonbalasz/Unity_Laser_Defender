using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delay = 3f;

    public void LoadGameOver()
    {
        StartCoroutine(GameOverDelay());        
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Game Over");        
    }

    public void LoadGame()
    {
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene("Game");
    }

    public void LoadStartMenu() { SceneManager.LoadScene("Start Menu"); }

    public void QuitGame() { Application.Quit(); }
}
