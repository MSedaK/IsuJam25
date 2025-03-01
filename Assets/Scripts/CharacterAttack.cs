using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image normalAttackImage;
    public float fadeDuration = 1.5f; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameObject.CompareTag("CharacterA"))
        {
            if (Input.GetKeyDown(KeyCode.E)) StartAttack(1, "AttackNormal", sphereA_Prefab, "CharacterB", normalAttackImage);
            if (Input.GetKeyDown(KeyCode.Q)) StartAttack(3, "AttackStrong", sphereA_Prefab, "CharacterB", null);
            if (Input.GetKeyDown(KeyCode.X)) StartAttack(5, "AttackRare", sphereA_Prefab, "CharacterB", null);
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            if (Input.GetKeyDown(KeyCode.K)) StartAttack(1, "AttackNormal", sphereB_Prefab, "CharacterA", normalAttackImage);
            if (Input.GetKeyDown(KeyCode.L)) StartAttack(3, "AttackStrong", sphereB_Prefab, "CharacterA", null);
            if (Input.GetKeyDown(KeyCode.M)) StartAttack(5, "AttackRare", sphereB_Prefab, "CharacterA", null);
        }
    }

    void StartAttack(int stoneCount, string animationName, GameObject stonePrefab, string target, Image skillImage)
    {
        queuedStones = stoneCount;
        currentStonePrefab = stonePrefab;
        targetTag = target;
        animator.SetTrigger(animationName);

        if (skillImage != null)
        {
            StartCoroutine(FadeSkillIcon(skillImage));
        }
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

    IEnumerator FadeSkillIcon(Image skillImage)
    {
        Color originalColor = skillImage.color;
        Color fadedColor = originalColor;
        fadedColor.a = 0.3f; 

        skillImage.color = fadedColor;
        yield return new WaitForSeconds(fadeDuration); 
        skillImage.color = originalColor; 
    }
}
