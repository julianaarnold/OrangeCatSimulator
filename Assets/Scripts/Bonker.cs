using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bonker : MonoBehaviour
{
    public Transform leftBonkStart;
    public Transform rightBonkStart;

    public Transform leftBonkEnd;
    public Transform rightBonkEnd;

    public float bonkTime = 1.0f;

    private float bonkStart;

    private Vector3 currentBonkStart;
    private Vector3 currentBonkEnd;

    private	bool bonking = false;

    private Rigidbody rb;
    private Collider collision;

    public UnityEvent bonkFinished;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collision = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bonking) {
            float t = (Time.time - bonkStart) / bonkTime;

            if(t > 1.0f) {
                t = 1.0f;
                StopBonk();
            }

            Vector3 newPos = Vector3.Lerp(currentBonkStart, currentBonkEnd, t);

            transform.localPosition = newPos;
        }
    }
    
    [ContextMenu("Bonk Left")]
    public void BonkLeft(Transform bonkable) {
        if (bonkable == null) {
            currentBonkStart = leftBonkStart.localPosition;
            currentBonkEnd = leftBonkEnd.localPosition;
        } else {
            currentBonkStart = transform.parent.InverseTransformPoint(bonkable.position) + Vector3.left * 0.1f;
            currentBonkEnd = transform.parent.InverseTransformPoint(bonkable.position) + Vector3.right * 0.1f;
        }

        StartBonk();
    }

    [ContextMenu("Bonk Right")]
    public void BonkRight(Transform bonkable) {
        if (bonkable == null) {
            currentBonkStart = rightBonkStart.localPosition;
            currentBonkEnd = rightBonkEnd.localPosition;
        } else {
            currentBonkStart = transform.parent.InverseTransformPoint(bonkable.position) + Vector3.right * 0.1f;
            currentBonkEnd = transform.parent.InverseTransformPoint(bonkable.position) + Vector3.left * 0.1f;
        }
        

        StartBonk();
    }

    private void StartBonk() {
        bonkStart = Time.time;
        bonking = true;

        collision.enabled = true;
    }

    public void StopBonk() {
        bonking = false;
        collision.enabled = false;

        bonkFinished.Invoke();
        bonkFinished.RemoveAllListeners();
    }
}
