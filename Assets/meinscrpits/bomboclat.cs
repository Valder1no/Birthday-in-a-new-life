using UnityEngine;
using System.Collections.Generic;

public class PhysicsGun : MonoBehaviour
{
    public float grabDistance = 5f;
    public float holdDistance = 3f;
    public float throwForce = 500f;
    public LayerMask grabLayerMask;

    private Camera playerCamera;
    private List<Rigidbody> grabbedRigidbodies = new List<Rigidbody>();
    private Dictionary<Rigidbody, ConfigurableJoint> frozenObjects = new Dictionary<Rigidbody, ConfigurableJoint>();
    private bool multiGrabMode = false;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            Debug.LogError("PhysicsGun: No MainCamera found. Tag your camera as 'MainCamera'.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            multiGrabMode = !multiGrabMode;
            Debug.Log("Mode: " + (multiGrabMode ? "Multi" : "Single"));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabLayerMask))
            {
                Rigidbody rb = hit.rigidbody;
                if (rb != null)
                {
                    if (grabbedRigidbodies.Contains(rb))
                    {
                        ReleaseSingleObject(rb);
                    }
                    else
                    {
                        if (!multiGrabMode)
                            ReleaseAllObjects();

                        if (grabbedRigidbodies.Count < 5 && !frozenObjects.ContainsKey(rb))
                        {
                            GrabObject(rb);
                        }
                    }
                }
            }
        }

        if (grabbedRigidbodies.Count > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                //float scaleScroll = Input.GetAxis("Mouse ScrollWheel");
                //if (scaleScroll != 0f)
                //{
                //    foreach (var rb in grabbedRigidbodies)
                //    {
                //        Vector3 newScale = rb.transform.localScale + Vector3.one * scaleScroll;
                //        newScale = Vector3.Max(newScale, Vector3.one * 0.1f);
                //        newScale = Vector3.Min(newScale, Vector3.one * 3f);
                //        rb.transform.localScale = newScale;
                //    }
                //}
            }
            else
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f)
                    holdDistance = Mathf.Clamp(holdDistance + scroll * 2f, 1f, grabDistance);
            }

            if (Input.GetMouseButtonDown(1))
                ThrowAllObjects();

            //if (Input.GetKeyDown(KeyCode.E))
               // TryFreezeObject();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
                TryUnfreezeObject();
        }
    }

    void FixedUpdate()
    {
        foreach (var rb in grabbedRigidbodies)
        {
            Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
            float smoothSpeed = grabbedRigidbodies.Count == 1 ? 20f : 5f;
            rb.linearVelocity = (targetPos - rb.position) * smoothSpeed;
        }
    }

    void GrabObject(Rigidbody rb)
    {
        rb.useGravity = false;
        rb.linearDamping = 10f;
        grabbedRigidbodies.Add(rb);
    }

    void ReleaseSingleObject(Rigidbody rb)
    {
        rb.useGravity = true;
        rb.linearDamping = 0f;
        grabbedRigidbodies.Remove(rb);
    }

    void ReleaseAllObjects()
    {
        foreach (var rb in grabbedRigidbodies)
        {
            rb.useGravity = true;
            rb.linearDamping = 0f;
        }
        grabbedRigidbodies.Clear();
    }

    void ThrowAllObjects()
    {
        foreach (var rb in grabbedRigidbodies)
        {
            rb.useGravity = true;
            rb.linearDamping = 0f;
            rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
        }
        grabbedRigidbodies.Clear();
    }

    void TryFreezeObject()
    {
        foreach (var rb in new List<Rigidbody>(grabbedRigidbodies))
        {
            Collider[] colliders = Physics.OverlapSphere(rb.position, 1f);
            foreach (var col in colliders)
            {
                Rigidbody targetRb = col.attachedRigidbody;

                if ((targetRb == rb) || frozenObjects.ContainsKey(rb))
                    continue;

                ConfigurableJoint joint = rb.gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = targetRb;
                LockJoint(joint);

                frozenObjects.Add(rb, joint);
                ReleaseSingleObject(rb);
                Debug.Log("Object welded.");
                return;
            }

            if (!frozenObjects.ContainsKey(rb))
            {
                ConfigurableJoint joint = rb.gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = null;
                LockJoint(joint);

                frozenObjects.Add(rb, joint);
                ReleaseSingleObject(rb);
                Debug.Log("Object welded to world.");
            }
        }
    }

    void TryUnfreezeObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            Rigidbody rb = hit.rigidbody;
            if (rb != null && frozenObjects.TryGetValue(rb, out ConfigurableJoint joint))
            {
                Destroy(joint);
                frozenObjects.Remove(rb);
                rb.useGravity = true;
                Debug.Log("Object unwelded.");
            }
        }
    }

    void LockJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;
    }
}