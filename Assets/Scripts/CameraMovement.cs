using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float targetDistance = 0.6f;
    public float maxDistance = 1.0f;

    public float minDistance = 0.2f;

    public float lagSpeed = 2.0f;

    public Transform gimbal;


    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (gimbal == null) {
            gimbal = transform.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // calculate laggy position
        Vector3 targetPosition = gimbal.position - gimbal.forward * targetDistance;
        transform.position = Vector3.Lerp(lastPosition, targetPosition, 1.0f - Mathf.Pow(lagSpeed, -Time.deltaTime));


        float currMaxDistance = maxDistance;

        if (Physics.Raycast(gimbal.position, transform.position - gimbal.position, out RaycastHit hit, currMaxDistance, ~LayerMask.GetMask("Player"))) {
            if (hit.distance < maxDistance) {
                currMaxDistance = hit.distance;
            }
        }

        currMaxDistance = Mathf.Max(currMaxDistance, minDistance);

        float currDistance = (transform.position - gimbal.position).magnitude;

        if (currDistance > currMaxDistance) {
            transform.position = gimbal.position + (transform.position - gimbal.position).normalized * currMaxDistance;
        }

        if (currDistance < minDistance) {
            transform.position = gimbal.position + (transform.position - gimbal.position).normalized * minDistance;
        }


        lastPosition = transform.position;
    }
}
