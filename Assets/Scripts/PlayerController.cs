using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed = 1.5f;
    [SerializeField]
    private float runSpeed = 6f;
    [SerializeField]
    private float gravity = -12f;
    [SerializeField]
    private float jumpHeight = 2f;
    [Range(0, 1f), SerializeField]
    private float airControlPercent = 0f;
    [SerializeField]
    private float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    [SerializeField]
    private float speedSmoothTime = 0.001f;
    private float speedSmoothVelocity;
    private float currentSpeed;

    private float velocityY;
    private float jumpVelocity = 0f;
    private Transform cameraTransform;


    private Animator animator;
    private CharacterController characterController;

    [HideInInspector]
    private bool isWalkingTorwards = false;
    [HideInInspector]
    private bool    sittingOn = false;
    [HideInInspector]
    public Transform targetLocation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update: isWalkingTorwards ? "+ isWalkingTorwards + " sittingOn ? " + sittingOn);
        if (!isWalkingTorwards && !sittingOn)
        {
            //Debug.Log("Input valid no ?");
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            bool running = Input.GetKey(KeyCode.LeftShift);

            Move(inputDir, running);


            float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
            animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        }
        else if (isWalkingTorwards)
        {
            MoveToLocation();
        }
    }

    void Jump()
    {
        if (characterController.isGrounded)
        {
            jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Deg2Rad + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector2.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        characterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        if (characterController.isGrounded)
        {
            velocityY = 0f;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (characterController.isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }


    public void SitDown(Transform _targetLocation)
    {
        if (!sittingOn)
        {
            //animator.SetTrigger("isWalking");
            //character.GetComponent<Animator>().GetComponent<CharacterController>().enabled = false;
            animator.SetFloat("speedPercent", .5f); //Walking Speed
            isWalkingTorwards = true;
        }

        this.targetLocation = _targetLocation;
        MoveToLocation();
    }

    void MoveToLocation()
    {
        Vector3 targetDir;

        targetDir = new Vector3(targetLocation.position.x - this.transform.position.x, 0f, targetLocation.position.z - this.transform.position.z);
        Quaternion rot = Quaternion.LookRotation(targetDir);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, 0.05f);
        this.transform.Translate(Vector3.forward * .0025f);
       /* float targetSpeed =5f;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        characterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        if (characterController.isGrounded)
        {
            velocityY = 0f;
        }*/
       // if (Vector3.Distance(this.transform.position, targetLocation.position) < 0.6f)
          if (Vector3.Distance(this.characterController.transform.position, targetLocation.position) < 0.6f)
            {
            animator.SetTrigger("isSitting");
            this.transform.rotation = targetLocation.rotation; // Or Slerp aiguain
            isWalkingTorwards = false;
            sittingOn = true;
            Debug.Log("I reached the destination and should be sitting down");
        }
    }
}

