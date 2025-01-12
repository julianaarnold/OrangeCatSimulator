using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum AnimationState {
    Walking,
    Bonking,
    Jumping
}

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


    Quaternion initialThighRotation;

    public AnimationState animationState = AnimationState.Walking;


    Transform playerRoot;

    // Start is called before the first frame update
    void Start()
    {
        thighLength = Vector3.Distance(thighTarget.position, shinTarget.position);
        shinLength = Vector3.Distance(shinTarget.position, footTarget.position);

        initialThighRotation = thighTarget.localRotation;

        playerRoot = thighTarget.GetComponentInParent<CatAnimator>().transform;
    }

    Quaternion getXZRotationBetween(Vector3 a, Vector3 b, Vector3 x, Vector3 z) {

        float alpha = Vector3.SignedAngle(a, b, x);

        Quaternion rotX = Quaternion.AngleAxis(alpha, x);

        Vector3 aRotated = rotX * b;
        Vector3 zRotated = rotX * z;

        float beta = Vector3.SignedAngle(aRotated, b, zRotated);

        Quaternion rotZ = Quaternion.AngleAxis(beta, z);

        return rotZ * rotX;
    }



    // Update is called once per frame
    void Update()
    {
        if (animationState == AnimationState.Walking) {
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
        }

        if (animationState == AnimationState.Jumping) {
            if (isAttached) {
                Detach();
                ResetPosition();

                /*transform.position = rootPosition.position;
                transform.rotation = rootPosition.rotation;*/
            }
        }
        
        UpdateLinkage();
    }

    void UpdateLinkage() {
        // Simpified version

        float targetDistance = Vector3.Distance(thighTarget.position, transform.position);

        float jointAngle = GetJointAngle(thighLength, shinLength, targetDistance);

        shinTarget.localRotation = Quaternion.Euler(shinRotationOffset + new Vector3(jointAngle * (shinRotationSign ? -1:1), 0, 0));

        targetRotation = calculateNewTargetRotation(); // Quaternion.FromToRotation(footTarget.position - thighTarget.position, transform.position - thighTarget.position) * thighTarget.rotation;
        
        if (!isAttached) {
            if (Quaternion.Angle(thighTarget.rotation, targetRotation) < 1.0f) {
                positionReached = true;
                //CancelRotationBuildup();
            }
            else positionReached = false;

            thighTarget.rotation = Quaternion.Slerp(thighTarget.rotation, targetRotation, 0.2f);
        } else {
            thighTarget.rotation = targetRotation;
        }

        //footTarget.position = transform.position;
        //footTarget.rotation = transform.rotation;
    }

    Quaternion calculateNewTargetRotation() {
        Quaternion oldRotation = thighTarget.localRotation;

        thighTarget.localRotation = initialThighRotation;
        Quaternion rot = Quaternion.FromToRotation(footTarget.position - thighTarget.position, transform.position - thighTarget.position) * thighTarget.rotation;
        thighTarget.localRotation = oldRotation;

        return rot;
    }
    


    float GetJointAngle(float a, float b, float c) {
        c = Mathf.Clamp(c, 0, a + b - 0.001f);

        double aD = a;
        double bD = b;
        double cD = c;

        double cos = (aD*aD + bD*bD - cD*cD) / (2*aD*bD);
        float angle = 180 - Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp01((float)cos));
        if (float.IsNaN(angle)) Debug.LogError("a: " + a + " b: " + b + " c: " + c + " (a*a + b*b - c*c) / (2*a*b): " + (aD*aD + bD*bD - cD*cD) / (2*aD*bD));
        return 0; //angle;
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
        transform.localPosition = rootPosition.localPosition + (rootPosition.localPosition.z - transform.localPosition.z) * Vector3.forward * 0.5f;
        //transform.localPosition = new Vector3(rootPosition.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        transform.rotation = rootPosition.rotation;
    }

    public void StartBonk(Bonker bonker) {
        Attach(bonker.transform, bonker.transform.position);
        animationState = AnimationState.Bonking;

        bonker.bonkFinished.AddListener(StopBonk);
    }

    public void StopBonk() {
        Detach();
        animationState = AnimationState.Walking;
    }

    private void CancelRotationBuildup() {
        thighTarget.rotation = initialThighRotation;
        thighTarget.rotation = Quaternion.FromToRotation(footTarget.position - thighTarget.position, transform.position - thighTarget.position) * thighTarget.rotation;
    }

    public void SetJumping(bool isJumping) {
        if (animationState == AnimationState.Bonking) return;

        if (isJumping) {
            animationState = AnimationState.Jumping;
        } else {
            animationState = AnimationState.Walking;
        }
    }
}
