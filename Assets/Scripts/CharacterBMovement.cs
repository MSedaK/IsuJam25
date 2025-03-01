using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private float currentSpeed;

    private Vector3 moveDirection;
    private CharacterController controller;
    private Animator animator;

    public float rotationSpeed = 10f;
    private bool isComboActive = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (!isComboActive) 
        {
            float moveX = 0f;
            float moveZ = 0f;

            if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;

            moveDirection = new Vector3(moveX, 0, moveZ).normalized;

            if (Input.GetKey(KeyCode.RightShift))
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
                RotateCharacter(moveDirection);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    void RotateCharacter(Vector3 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    public void StartCombo()
    {
        isComboActive = true;
        animator.SetBool("isWalking", false);
    }

    public void EndCombo()
    {
        isComboActive = false;
    }
}