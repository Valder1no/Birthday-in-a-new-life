using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewindEffectUI : MonoBehaviour
{
    [Header("Rewind Effect Settings")]
    public Canvas rewindCanvas;
    public RawImage noiseImage;
    [Range(0f, 1f)]
    public float noiseOpacity = 0.7f;
    public float noiseAnimationSpeed = 10f;

    private Texture2D noiseTexture;
    private bool isEffectActive = false;

    void Start()
    {
        // Create the noise texture
        CreateNoiseTexture();

        // Setup canvas if not assigned
        if (rewindCanvas == null)
            SetupRewindCanvas();

        // Make sure effect starts disabled
        SetEffectActive(false);
    }

    void CreateNoiseTexture()
    {
        int width = 512;
        int height = 512;
        noiseTexture = new Texture2D(width, height);

        GenerateNoiseTexture();

        if (noiseImage != null)
            noiseImage.texture = noiseTexture;
    }

    void GenerateNoiseTexture()
    {
        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float noise = Random.Range(0f, 1f);
                Color noiseColor = new Color(noise, noise, noise, noiseOpacity);
                noiseTexture.SetPixel(x, y, noiseColor);
            }
        }
        noiseTexture.Apply();
    }

    void SetupRewindCanvas()
    {
        // Create canvas
        GameObject canvasGO = new GameObject("RewindEffectCanvas");
        rewindCanvas = canvasGO.AddComponent<Canvas>();
        rewindCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        rewindCanvas.sortingOrder = 1; // Make sure it renders on top

        // Add Canvas Scaler
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Add GraphicRaycaster
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create noise image
        GameObject imageGO = new GameObject("NoiseImage");
        imageGO.transform.SetParent(rewindCanvas.transform);

        noiseImage = imageGO.AddComponent<RawImage>();
        RectTransform rectTransform = imageGO.GetComponent<RectTransform>();

        // Make it fullscreen
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    public void StartRewindEffect()
    {
        if (!isEffectActive)
        {
            isEffectActive = true;
            SetEffectActive(true);
            StartCoroutine(AnimateNoise());
        }
    }

    public void StopRewindEffect()
    {
        isEffectActive = false;
        SetEffectActive(false);
    }

    void SetEffectActive(bool active)
    {
        if (rewindCanvas != null)
            rewindCanvas.gameObject.SetActive(active);
    }

    IEnumerator AnimateNoise()
    {
        while (isEffectActive)
        {
            GenerateNoiseTexture();
            yield return new WaitForSeconds(1f / noiseAnimationSpeed);
        }
    }

    void OnDestroy()
    {
        if (noiseTexture != null)
            Destroy(noiseTexture);
    }
}