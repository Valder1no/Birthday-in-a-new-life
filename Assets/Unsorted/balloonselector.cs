using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public GameObject[] selectableObjects;
    public float growSpeed = 0.5f;
    public float maxSize = 3f;

    private int currentIndex = 0;
    private Vector3[] originalScales;

    void Start()
    {
        // Store original scales for reset
        originalScales = new Vector3[selectableObjects.Length];
        for (int i = 0; i < selectableObjects.Length; i++)
        {
            originalScales[i] = selectableObjects[i].transform.localScale;
        }

        UpdateSelectionVisuals();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % selectableObjects.Length;
            UpdateSelectionVisuals();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + selectableObjects.Length) % selectableObjects.Length;
            UpdateSelectionVisuals();
        }

        // Tap Up Arrow → Reset all balloons
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ResetAllBalloonSizes();
        }

        // Hold Up Arrow → Grow selected balloon
        if (Input.GetKey(KeyCode.UpArrow))
        {
            BalloonBehavior selected = selectableObjects[currentIndex].GetComponent<BalloonBehavior>();
            if (selected != null)
            {
                selected.GrowBalloonWithMaxSize(growSpeed, maxSize);
            }
        }
    }

    void ResetAllBalloonSizes()
    {
        for (int i = 0; i < selectableObjects.Length; i++)
        {
            selectableObjects[i].transform.localScale = originalScales[i];
        }
    }

    void UpdateSelectionVisuals()
    {
        for (int i = 0; i < selectableObjects.Length; i++)
        {
            GameObject obj = selectableObjects[i];
            Transform outline = obj.transform.Find("Outline");
            Renderer rend = obj.GetComponent<Renderer>();

            bool isSelected = (i == currentIndex);

            if (outline != null)
                outline.gameObject.SetActive(isSelected);

            if (rend != null)
                rend.material.color = isSelected ? Color.yellow : Color.red;
        }
    }
}