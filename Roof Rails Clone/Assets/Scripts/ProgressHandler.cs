using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressHandler : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform finishPoint;
    private float maxDistance;
    private float distance;
    public float Distance => distance;
    private void Start()
    {
        maxDistance = finishPoint.position.z - startPoint.position.z;
    }
    void Update()
    {
        if (GameManager.isGameStarted && !GameManager.isGameFinished)
        {
            var remaining = finishPoint.position.z - target.position.z;
            distance = (maxDistance - remaining) / maxDistance * 100;
        }
    }
}
