using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void BonkLeft() {
        currentBonkStart = leftBonkStart.localPosition;
        currentBonkEnd = leftBonkEnd.localPosition;

        StartBonk();
    }

    [ContextMenu("Bonk Right")]
    public void BonkRight() {
        currentBonkStart = rightBonkStart.localPosition;
        currentBonkEnd = rightBonkEnd.localPosition;

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
    }
}
