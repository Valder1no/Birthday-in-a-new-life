using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class TimeCheckpointManager : MonoBehaviour
{
    public static TimeCheckpointManager Instance;

    public GameObject checkpointMarkerPrefab;
    public Transform playerTransform;
    public float checkpointCooldown = 5f;
    public float rewindDelay = 5f;
    public float recordInterval = 0.01f;

    private FirstPersonController playerController;
    private EnemyAiTutorial enemyAI;

    public RewindEffectUI rewindEffectUI;

    private float cooldownTimer = 0f;
    private float rewindTimer = 0f;
    private float recordTimer = 0f;

    private bool isRewinding = false;
    private bool checkpointActive = false;

    private GameObject currentMarker;
    private List<ITimeRecordable> recordables = new List<ITimeRecordable>();

    void Awake() 
    {
        Instance = this;

        if (playerTransform != null)
            playerController = playerTransform.GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0f)
            ActivateCheckpoint();

        if (!checkpointActive) return;

        // Timer countdown
        rewindTimer -= Time.deltaTime;
        recordTimer += Time.deltaTime;

        if (recordTimer >= recordInterval)
        {
            foreach (var obj in recordables)
            {
//                Debug.Log($"Recording snapshot for: {((MonoBehaviour)obj).gameObject.name}");
                obj.RecordSnapshot();
            }


            recordTimer = 0f;
        }

        if (rewindTimer <= 0f && !isRewinding)
        {
            StartCoroutine(PerformRewind());
        }

        cooldownTimer -= Time.deltaTime;
    }

    void ActivateCheckpoint()
    {
        checkpointActive = true;
        isRewinding = false;

        cooldownTimer = checkpointCooldown;
        rewindTimer = rewindDelay;
        recordTimer = 0f;

        if (currentMarker)
            Destroy(currentMarker);

        currentMarker = Instantiate(checkpointMarkerPrefab, playerTransform.position + Vector3.up * 5, Quaternion.identity);

        foreach (var obj in recordables)
        {
            obj.ClearSnapshots();

            // NEW: Record the current state as a backup
            var mono = (MonoBehaviour)obj;
            Debug.Log(obj);
            Rigidbody rb = mono.GetComponent<Rigidbody>();
            var snapshot = new TimeSnapshot(rb);

            // You'll need a method to store it
            if (obj is TimeRecordableObject tr)
                tr.SetBackupSnapshot(snapshot);
        }
    }

    public void Register(ITimeRecordable recordable)
    {
        if (!recordables.Contains(recordable))
        {
            recordables.Add(recordable);
            Debug.Log($"✔️ Registered: {((MonoBehaviour)recordable).gameObject.name}");
        }
        else
        {
            Debug.Log($"⚠️ Already registered: {((MonoBehaviour)recordable).gameObject.name}");
        }

        if (recordables.Count == 0)
        {
            Debug.LogWarning("❌ No recordable objects registered!");
        }
        else
        {
            Debug.Log($"📸 About to record {recordables.Count} objects.");
        }
    }



    System.Collections.IEnumerator PerformRewind()
    {
        isRewinding = true;
        checkpointActive = false;


        if (rewindEffectUI != null)

            rewindEffectUI.StartRewindEffect();

        if (playerController != null)
            playerController.enabled = false;

        if (currentMarker)
            Destroy(currentMarker);

        // Disable physics
        foreach (var obj in recordables)
            ((MonoBehaviour)obj).GetComponent<Rigidbody>().isKinematic = true;

        foreach (var obj in recordables)

        {

            var mono = (MonoBehaviour)obj;

            mono.GetComponent<Rigidbody>().isKinematic = true;

            if (mono.gameObject.CompareTag("Enemy"))

            {

                mono.GetComponent<EnemyAiTutorial>().enabled = false;

                mono.GetComponent<NavMeshAgent>().enabled = false;

            }

        }

        // Rewind in reverse snapshot order
        int frameCount = recordables[0].GetSnapshots().Count;

        for (int i = frameCount - 1; i >= 0; i--)
        {
            foreach (var obj in recordables)
            {
                var snapshots = obj.GetSnapshots();
                if (i < snapshots.Count)
                {
                    var snapshot = snapshots[i];
                    GameObject go = ((MonoBehaviour)obj).gameObject;

                    if (go.CompareTag("Player") && go.TryGetComponent<FirstPersonController>(out var fp))
                    {
                        fp.SnapToRewindState(snapshot.position, snapshot.rotation, snapshot.velocity);
                    }
                    else
                    {
                        obj.ApplySnapshot(snapshot);
                    }
                }
            }

            yield return new WaitForSeconds(0.15f);
        }
        Debug.Log($"Final rewind position should be: {transform.position}");

        // Restore physics
        //foreach (var obj in recordables)
        //    ((MonoBehaviour)obj).GetComponent<Rigidbody>().isKinematic = false;


        foreach (var obj in recordables)
        {
            var mono = (MonoBehaviour)obj;

            mono.GetComponent<Rigidbody>().isKinematic = false;

            if (mono.CompareTag("Door"))
            {
                mono.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                mono.GetComponent<Rigidbody>().isKinematic = false;
            }

            if (mono.gameObject.CompareTag("Enemy"))

            {

                mono.GetComponent<EnemyAiTutorial>().enabled = true;

                mono.GetComponent<NavMeshAgent>().enabled = true;

            }

        }

        yield return new WaitForSeconds(0);

        if (playerController != null)
            playerController.enabled = true;


        if (rewindEffectUI != null)
            rewindEffectUI.StopRewindEffect();
    }


    void OnDrawGizmos()
    {
        foreach (var obj in recordables)
        {
            var snapshots = obj.GetSnapshots();
            if (snapshots.Count > 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(snapshots[0].position, 0.2f);
            }
        }
    }
}
