using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttack : MonoBehaviour
{
    public GameObject sphereA_Prefab;
    public GameObject sphereB_Prefab;
    public GroundSlash groundSlashPrefab; 
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

    public Transform cameraTransform;
    private Vector3 originalCameraPosition;
    public float cameraShakeAmount = 0.2f;
    public float shakeDuration = 0.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        originalCameraPosition = cameraTransform.position;
        UpdateSkillUI();
    }

    void Update()
    {
        if (gameObject.CompareTag("CharacterA"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FireGroundSlash(); 
            }

            if (Input.GetKeyDown(KeyCode.Q) && isStrongUnlocked && !isStrongUsed)
            {
                StartAttack(3, "AttackStrong", sphereA_Prefab, "CharacterB", strongAttackImage, true);
            }
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartAttack(1, "AttackNormal", sphereB_Prefab, "CharacterA", normalAttackImage);
            }
            if (Input.GetKeyDown(KeyCode.L) && isStrongUnlocked && !isStrongUsed)
            {
                StartAttack(3, "AttackStrong", sphereB_Prefab, "CharacterA", strongAttackImage, true);
            }
        }
    }

    void StartAttack(int stoneCount, string animationName, GameObject stonePrefab, string target, Image skillImage, bool isStrong = false)
    {
        queuedStones = stoneCount;
        currentStonePrefab = stonePrefab;
        targetTag = target;
        animator.SetTrigger(animationName);

        if (skillImage != null)
        {
            StartCoroutine(FadeSkillIcon(skillImage));
        }

        if (isStrong)
        {
            isStrongUsed = true;
            StartCoroutine(CameraShakeEffect());
            UpdateSkillUI();
        }
    }

    public void FireStones()
    {
        for (int i = 0; i < queuedStones; i++)
        {
            GameObject stone = Instantiate(currentStonePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = stone.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = transform.forward * stoneSpeed;
            }

            Stone stoneScript = stone.GetComponent<Stone>();
            if (stoneScript != null)
            {
                stoneScript.targetTag = targetTag;
            }
        }
        queuedStones = 0;
    }

    void FireGroundSlash()
    {
        if (groundSlashPrefab != null && firePoint != null)
        {
            animator.SetTrigger("AttackNormal"); 

            GroundSlash newSlash = Instantiate(groundSlashPrefab, firePoint.position, Quaternion.identity);
            newSlash.Initialize(firePoint); 

            Debug.Log(gameObject.name + " used GroundSlash!"); 
        }
    }

    IEnumerator CameraShakeEffect()
    {
        Debug.Log("Strong Attack - Camera Shake Started!");

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float x = Random.Range(-cameraShakeAmount, cameraShakeAmount);
            float y = Random.Range(-cameraShakeAmount, cameraShakeAmount);

            cameraTransform.position += new Vector3(x, y, 0);
            yield return null;
        }

        cameraTransform.position = originalCameraPosition;
        Debug.Log("Camera Reset to Default.");
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
        isStrongUnlocked = false;
        isStrongUsed = false;
        UpdateSkillUI();
        Debug.Log(gameObject.name + " lost Strong Attack due to new combo phase!");
    }

    void UpdateSkillUI()
    {
        if (strongAttackImage != null)
        {
            Color color = strongAttackImage.color;
            color.a = (isStrongUnlocked && !isStrongUsed) ? 1f : 0.3f;
            strongAttackImage.color = color;
        }
    }
}
