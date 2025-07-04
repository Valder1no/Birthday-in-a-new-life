using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBrick : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillBrick"))
        {
            SceneManager.LoadScene("Raigo");
        }
    }
}
