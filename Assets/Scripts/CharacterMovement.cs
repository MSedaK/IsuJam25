using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private float currentSpeed;

    private Vector3 moveDirection;
    private CharacterController controller;
    private Animator animator;

    public bool isComboActive = false; 
    public float rotationSpeed = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed; 

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }

    void Update()
    {
        if (!isComboActive) 
        {
            float moveX = Input.GetAxisRaw("Horizontal"); 
            float moveZ = Input.GetAxisRaw("Vertical");   

            moveDirection = new Vector3(moveX, 0, moveZ).normalized;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
                animator.SetBool("isRunning", true);
            }
            else
            {
                currentSpeed = walkSpeed;
                animator.SetBool("isRunning", false);
            }

            if (moveDirection.magnitude >= 0.1f)
            {
                controller.Move(moveDirection * currentSpeed * Time.deltaTime);
                animator.SetBool("isWalking", true);

                RotateCharacter(moveDirection);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
    void RotateCharacter(Vector3 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    public void StartCombo()
    {
        Debug.Log("Combo Started: Movement Stopped");
        isComboActive = true;
    }

    public void EndCombo()
    {
        Debug.Log("Combo Ended: Movement Resumed");
        isComboActive = false;
    }
}
