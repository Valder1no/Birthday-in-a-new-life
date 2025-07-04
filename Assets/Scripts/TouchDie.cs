using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchDie : MonoBehaviour
{
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane") || other.CompareTag("Helicopter"))
        {
            Debug.Log("Player died.");
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
