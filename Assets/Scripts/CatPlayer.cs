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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        selfRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        movement += transform.right * Input.GetAxis("Horizontal");
        movement += transform.forward * Input.GetAxis("Vertical");

        if (Input.GetKeyUp(KeyCode.Space)) {
            selfRigidbody.AddForce(0, forceConst, 0, ForceMode.Impulse);
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
}
