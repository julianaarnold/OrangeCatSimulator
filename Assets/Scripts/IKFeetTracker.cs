using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFeetTracker : MonoBehaviour
{
    public Transform rootPosition;

    public Transform thighTarget;
    public Transform shinTarget;
    public Transform footTarget;

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
                if (Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, out RaycastHit hit, 1.0f, ~LayerMask.GetMask("Player"))) {
                    Attach(hit.transform);
                }
            }
        }

        UpdateLinkage();
    }

    void UpdateLinkage() {
        // Simpified version

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

    void Attach(Transform t) {
        transform.SetParent(t, true);
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
