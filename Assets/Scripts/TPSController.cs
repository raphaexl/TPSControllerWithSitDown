using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 6f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float distanceFromTarget = 2f;

    [SerializeField]
    private Vector2 pitchMax = new Vector2(-40, 85);

    [SerializeField]
    private float rotationSmoothTime = .12f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    private float yaw;
    private float pitch;

    public bool lockCursor;

    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMax.x, pitchMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * distanceFromTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
