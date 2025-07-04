using UnityEngine;
using UnityEngine.UI;

public class VerticalProgressBar : MonoBehaviour
{
    public Transform player;
    public float goalY;
    
    private float startY;

    public Image fillImage;  // Assign the FillImage in the Inspector

    void Start()
    {
        if (player != null)
            startY = player.position.y;
    }

    void Update()
    {
        if (player != null && fillImage != null)
        {
            float progress = Mathf.InverseLerp(startY, goalY, player.position.y);
            fillImage.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
