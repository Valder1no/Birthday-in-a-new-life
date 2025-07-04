using UnityEngine;
using UnityEngine.SceneManagement;

public class KoferTouched : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WON");
            SceneManager.LoadScene("Win");
        }
    }
}
