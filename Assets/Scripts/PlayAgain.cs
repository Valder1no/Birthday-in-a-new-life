using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    void Start()
    {
        //GameObject.Find("Play again").GetComponentInChildren<Text>().text = "Play once more";
    }

    public void PlayOnceMore()
    {
        Debug.Log("Gettin clicked");
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
