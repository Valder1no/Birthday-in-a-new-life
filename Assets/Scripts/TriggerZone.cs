using System;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("The player walked into me");
            onTriggerEnter.Invoke();
            gameObject.SetActive(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }
}