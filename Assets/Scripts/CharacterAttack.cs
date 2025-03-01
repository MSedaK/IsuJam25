using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public GameObject sphereA_Prefab; 
    public GameObject sphereB_Prefab; 
    public Transform firePoint;
    public float stoneSpeed = 10f;
    private Animator animator;
    private int queuedStones = 0;
    private GameObject currentStonePrefab;
    private string targetTag; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameObject.CompareTag("CharacterA"))
        {
            if (Input.GetKeyDown(KeyCode.E)) StartAttack(1, "AttackNormal", sphereA_Prefab, "CharacterB");
            if (Input.GetKeyDown(KeyCode.Q)) StartAttack(3, "AttackStrong", sphereA_Prefab, "CharacterB");
            if (Input.GetKeyDown(KeyCode.X)) StartAttack(5, "AttackRare", sphereA_Prefab, "CharacterB");
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            if (Input.GetKeyDown(KeyCode.K)) StartAttack(1, "AttackNormal", sphereB_Prefab, "CharacterA");
            if (Input.GetKeyDown(KeyCode.L)) StartAttack(3, "AttackStrong", sphereB_Prefab, "CharacterA");
            if (Input.GetKeyDown(KeyCode.M)) StartAttack(5, "AttackRare", sphereB_Prefab, "CharacterA");
        }
    }

    void StartAttack(int stoneCount, string animationName, GameObject stonePrefab, string target)
    {
        queuedStones = stoneCount;
        currentStonePrefab = stonePrefab;
        targetTag = target;
        animator.SetTrigger(animationName);
    }
    public void FireStones()
    {
        for (int i = 0; i < queuedStones; i++)
        {
            GameObject stone = Instantiate(currentStonePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * stoneSpeed;

            Stone stoneScript = stone.GetComponent<Stone>();
            if (stoneScript != null)
            {
                stoneScript.targetTag = targetTag; 
            }
        }
        queuedStones = 0;
    }
}
