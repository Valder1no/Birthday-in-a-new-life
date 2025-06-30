using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCheckpointManager : MonoBehaviour
{
    public static TimeCheckpointManager Instance;

    [Header("References")]
    public GameObject checkpointMarkerPrefab;
    public Transform playerTransform;
    public TextMeshProUGUI debugTimerText; // Assign in Canvas (for testing only)

    [Header("Cooldown Settings")]
    public float minCooldown = 5f;
    public float maxCooldown = 10f;

    [Header("Rewind Settings")]
    public float minRewindTime = 5f;
    public float maxRewindTime = 20f;

    private List<ITimeRecordable> timeObjects = new List<ITimeRecordable>();
    private GameObject activeCheckpointMarker;

    private float cooldownTimer = 0f;
    private float rewindTimer = 0f;
    private bool isCheckpointActive = false;

    void Awake() => Instance = this;

    void Start()
    {
        SetInitialCheckpoint();
    }

    void Update()
    {
        HandleDebugUI();

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isCheckpointActive)
        {
            rewindTimer -= Time.deltaTime;

            if (rewindTimer <= 0f)
            {
                Debug.Log(">> Rewinding!");
                Rewind();
            }
        }

        // Player manually places a checkpoint (E key), if off cooldown
        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0f)
        {
            SetCheckpoint();
        }
    }

    public void Register(ITimeRecordable obj)
    {
        if (!timeObjects.Contains(obj))
            timeObjects.Add(obj);
    }

    void SetInitialCheckpoint()
    {
        Debug.Log(">> First checkpoint at game start.");
        SetCheckpoint();
    }

    void SetCheckpoint()
    {
        Debug.Log(">> New checkpoint set!");

        // Save states of all recordable objects
        foreach (var obj in timeObjects)
            obj.SaveCheckpoint();

        // Destroy old marker if it exists
        if (activeCheckpointMarker != null)
            Destroy(activeCheckpointMarker);

        // Create a new marker above the player
        Vector3 markerPos = playerTransform.position + Vector3.up * 6f;
        activeCheckpointMarker = Instantiate(checkpointMarkerPrefab, markerPos, Quaternion.identity);

        // Reset cooldown + set new random rewind timer
        cooldownTimer = Random.Range(minCooldown, maxCooldown);
        rewindTimer = Random.Range(minRewindTime, maxRewindTime);
        isCheckpointActive = true;

        Debug.Log($"Cooldown set to {cooldownTimer:F1}s | Rewind in {rewindTimer:F1}s");
    }

    void Rewind()
    {
        // Destroy the visual marker
        if (activeCheckpointMarker != null)
            Destroy(activeCheckpointMarker);

        foreach (var obj in timeObjects)
            obj.LoadCheckpoint();

        isCheckpointActive = false;
        Debug.Log(">> Rewind complete!");
    }

    void HandleDebugUI()
    {
        if (debugTimerText != null)
        {
            if (isCheckpointActive)
                debugTimerText.text = $"⏪ Rewind In: {rewindTimer:F1}s";
            else
                debugTimerText.text = $"⏱️ Cooldown: {cooldownTimer:F1}s";
        }
    }
}
