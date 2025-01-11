using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFeetTracker : MonoBehaviour
{
    public Transform rootPosition;

    public Transform thighTarget;
    public Transform shinTarget;
    public Transform footTarget;

    public Vector3 shinRotationOffset = new Vector3(-85, 328, -329);
    public bool shinRotationSign = false;

    public bool isAttached = false;

    public bool positionReached = false;

    float thighLength;
    float shinLength;

    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        thighLength = Vector3.Distance(thighTarget.position, shinTarget.position);
        shinLength = Vector3.Distance(shinTarget.position, footTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttached) {
            float distance = Vector3.Distance(transform.position, rootPosition.position);

            if (distance > 0.1f) {
                Detach();
                ResetPosition();
            }
        } else {
            if (positionReached) {
                if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, 1.0f, ~LayerMask.GetMask("Player"))) {
                    Attach(hit.transform, hit.point);
                }
            }
        }

        UpdateLinkage();
    }

    void UpdateLinkage() {
        // Simpified version

        float targetDistance = Vector3.Distance(thighTarget.position, transform.position);

        float jointAngle = GetJointAngle(thighLength, shinLength, targetDistance);

        Debug.Log(jointAngle);
        shinTarget.localRotation = Quaternion.Euler(shinRotationOffset + new Vector3(jointAngle * (shinRotationSign ? -1:1), 0, 0));

        targetRotation = Quaternion.FromToRotation(footTarget.position - thighTarget.position, transform.position - thighTarget.position) * thighTarget.rotation;
        
        if (!isAttached) {
            if (Quaternion.Angle(thighTarget.rotation, targetRotation) < 1.0f) positionReached = true;
            else positionReached = false;

            thighTarget.rotation = Quaternion.Slerp(thighTarget.rotation, targetRotation, 0.2f);
        } else {
            thighTarget.rotation = targetRotation;
        }
        
        //footTarget.position = transform.position;
        //footTarget.rotation = transform.rotation;
    }

    float GetJointAngle(float a, float b, float c) {
        Debug.Log("a: " + a + " b: " + b + " c: " + c);
        c = Mathf.Clamp(c, 0, a + b - 0.001f);
        return 180 - Mathf.Rad2Deg * Mathf.Acos((a*a + b*b - c*c) / (2*a*b));
    }

    void Attach(Transform t, Vector3 location) {
        transform.SetParent(t, true);
        transform.position = location;
        isAttached = true;
    }

    void Detach() {
        transform.SetParent(rootPosition.transform.parent, true);
        isAttached = false;
    }

    void ResetPosition() {
        transform.position = rootPosition.position + (rootPosition.position - transform.position) * 0.5f;
        transform.rotation = rootPosition.rotation;
    }
}
