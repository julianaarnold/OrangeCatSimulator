using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayer : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float lookSpeed = 50.0f;
    public float forceConst = 2.0f;

    private float pitch = 30.0f;

    public Transform cameraOrigin;
    private Rigidbody selfRigidbody;
    private CatAnimator kittyAnimator;
    private bool canJump;
    public float jumpRay = 0.25f;
    public Bonker bonker;

    public BoxCollider bonkVolume;
    public BoxCollider scratchBonkVolume;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        selfRigidbody = GetComponent<Rigidbody>();
        kittyAnimator = GetComponent<CatAnimator>();
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        canJump = Physics.OverlapBox(transform.position + Vector3.up * 0.1f, new Vector3(0.2f, 0.2f, 0.2f), transform.rotation, LayerMask.GetMask("Ground") | LayerMask.GetMask("Counter")).Length > 0;
        //Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, jumpRay, LayerMask.GetMask("Ground") | LayerMask.GetMask("Counter"));

        movement += transform.right * Input.GetAxis("Horizontal");
        movement += transform.forward * Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canJump) {
            kittyAnimator.PrepareJump();
        }

        if (Input.GetKeyUp(KeyCode.Space) && canJump) {
            kittyAnimator.Jump();
            selfRigidbody.AddForce(0, forceConst, 0, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            bonker.BonkLeft(getCurrentBonkable());
            kittyAnimator.BonkLeft();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            bonker.BonkRight(getCurrentBonkable());
            kittyAnimator.BonkRight();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            ScoreBehaviour_bite biteable = getCurrentBiteable();
            Debug.Log("Biting " + biteable.name, biteable.gameObject);
            if (biteable != null) {
                biteable.biteCounter();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ScoreBehaviour_scratch scratchable = getCurrentScratchable();
            Debug.Log("Scratching " + scratchable.name, scratchable.gameObject);
            if (scratchable != null) {
                scratchable.scratchCounter();
            }
        }

        transform.position += movement * moveSpeed * Time.deltaTime;

        // mouse look
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * lookSpeed * Time.deltaTime);
        
        pitch -= mouseY * lookSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -5.0f, 80.0f);

        cameraOrigin.localRotation = Quaternion.Euler(pitch, 0, 0);

    }

    private Transform getCurrentBonkable() {
        Collider[] colliders = Physics.OverlapBox(bonkVolume.bounds.center, bonkVolume.size, bonkVolume.transform.rotation);

        float minDistance = float.MaxValue;
        Transform closest = null;

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent(out ScoreBehaviour bonkable)) {
                float distance = Vector3.Distance(bonkVolume.bounds.center, colliders[i].transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    closest = colliders[i].transform;
                }
            }
        }

        return closest;
    }

    private ScoreBehaviour_scratch getCurrentScratchable() {
        Collider[] colliders = Physics.OverlapBox(scratchBonkVolume.bounds.center, scratchBonkVolume.size, scratchBonkVolume.transform.rotation);

        float minDistance = float.MaxValue;
        ScoreBehaviour_scratch closest = null;

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent(out ScoreBehaviour_scratch scratchable)) {
                float distance = Vector3.Distance(scratchBonkVolume.bounds.center, colliders[i].transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    closest = scratchable;
                }
            }
        }

        return closest;
    }

    private ScoreBehaviour_bite getCurrentBiteable() {
        Collider[] colliders = Physics.OverlapBox(scratchBonkVolume.bounds.center, scratchBonkVolume.size, scratchBonkVolume.transform.rotation);

        float minDistance = float.MaxValue;
        ScoreBehaviour_bite closest = null;

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent(out ScoreBehaviour_bite biteable)) {
                float distance = Vector3.Distance(scratchBonkVolume.bounds.center, colliders[i].transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    closest = biteable;
                }
            }
        }

        return closest;
    }
}
