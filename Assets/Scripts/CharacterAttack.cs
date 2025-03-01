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
    public Image strongAttackImage;
    public float fadeDuration = 1.5f;

    private bool isStrongUnlocked = false; 
    private bool isStrongUsed = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateSkillUI(); 
    }

    void Update()
    {
        if (gameObject.CompareTag("CharacterA"))
        {
            if (Input.GetKeyDown(KeyCode.E)) StartAttack(1, "AttackNormal", sphereA_Prefab, "CharacterB", normalAttackImage);
            if (Input.GetKeyDown(KeyCode.Q) && isStrongUnlocked) StartAttack(3, "AttackStrong", sphereA_Prefab, "CharacterB", strongAttackImage);
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            if (Input.GetKeyDown(KeyCode.K)) StartAttack(1, "AttackNormal", sphereB_Prefab, "CharacterA", normalAttackImage);
            if (Input.GetKeyDown(KeyCode.L) && isStrongUnlocked) StartAttack(3, "AttackStrong", sphereB_Prefab, "CharacterA", strongAttackImage);
        }
    }

    void StartAttack(int stoneCount, string animationName, GameObject stonePrefab, string target, Image skillImage)
    {
        queuedStones = stoneCount;
        currentStonePrefab = stonePrefab;
        targetTag = target;

        Debug.Log(gameObject.name + " Attack Triggered: " + animationName);

        animator.SetTrigger(animationName);

        if (skillImage != null)
        {
            StartCoroutine(FadeSkillIcon(skillImage));
        }

        if (animationName == "AttackStrong")
        {
            isStrongUsed = true;
        }
    }

    public void FireStones()
    {
        Debug.Log(gameObject.name + " FireStones Called!");

        if (queuedStones <= 0 || currentStonePrefab == null)
        {
            Debug.LogWarning("No stones to fire or prefab is missing!");
            return;
        }

        for (int i = 0; i < queuedStones; i++)
        {
            GameObject stone = Instantiate(currentStonePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = stone.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = transform.forward * stoneSpeed;
                Debug.Log(gameObject.name + " Fired Stone!");
            }
            else
            {
                Debug.LogError("Rigidbody missing on stone prefab!");
            }

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

    public void UnlockStrongAttack()
    {
        isStrongUnlocked = true;
        isStrongUsed = false; 
        UpdateSkillUI();
        Debug.Log(gameObject.name + " unlocked Strong Attack!");
    }

    public void ResetStrongAttack()
    {
        if (!isStrongUsed) 
        {
            isStrongUnlocked = false;
            UpdateSkillUI();
            Debug.Log(gameObject.name + " lost Strong Attack due to inactivity!");
        }
    }

    void UpdateSkillUI()
    {
        if (strongAttackImage != null)
        {
            Color color = strongAttackImage.color;
            color.a = isStrongUnlocked ? 1f : 0.3f;
            strongAttackImage.color = color;
        }
    }
}