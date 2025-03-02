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
    public Image strongAttackExclamation;

    private bool isStrongUnlocked = false;
    private bool isStrongUsed = false;
    private Coroutine exclamationCoroutine;

    public Transform cameraTransform;
    private Vector3 originalCameraPosition;
    public float cameraShakeAmount = 0.2f;
    public float shakeDuration = 0.5f;
    public float cameraZoomSpeed = 2f; 
    public float zoomDistance = 2f;

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
            if (Input.GetKeyDown(KeyCode.E)) StartAttack(1, "AttackNormal", sphereA_Prefab, "CharacterB", normalAttackImage);
            if (Input.GetKeyDown(KeyCode.Q) && isStrongUnlocked && !isStrongUsed)
                StartAttack(3, "AttackStrong", sphereA_Prefab, "CharacterB", strongAttackImage, true);
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            if (Input.GetKeyDown(KeyCode.K)) StartAttack(1, "AttackNormal", sphereB_Prefab, "CharacterA", normalAttackImage);
            if (Input.GetKeyDown(KeyCode.L) && isStrongUnlocked && !isStrongUsed)
                StartAttack(3, "AttackStrong", sphereB_Prefab, "CharacterA", strongAttackImage, true);
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

            if (strongAttackExclamation != null)
            {
                strongAttackExclamation.gameObject.SetActive(false);
                if (exclamationCoroutine != null)
                {
                    StopCoroutine(exclamationCoroutine);
                    exclamationCoroutine = null;
                }
            }

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
        float fastFadeDuration = 0.3f;  
        Color originalColor = skillImage.color;
        Color fadedColor = originalColor;
        fadedColor.a = 0.3f; 

        float elapsedTime = 0f;
        while (elapsedTime < fastFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            skillImage.color = Color.Lerp(originalColor, fadedColor, elapsedTime / fastFadeDuration);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < fastFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            skillImage.color = Color.Lerp(fadedColor, originalColor, elapsedTime / fastFadeDuration);
            yield return null;
        }

        skillImage.color = originalColor; 
    }


    public void UnlockStrongAttack()
    {
        isStrongUnlocked = true;
        isStrongUsed = false;
        UpdateSkillUI();

        if (gameObject.CompareTag("CharacterA") || gameObject.CompareTag("CharacterB"))
        {
            animator.SetTrigger("Victory");  
            Debug.Log(gameObject.name + " unlocked Strong Attack and played Victory animation!");
            
            StartCoroutine(ZoomToCharacter());
        }

        if (strongAttackExclamation != null)
        {
            strongAttackExclamation.gameObject.SetActive(true);
            if (exclamationCoroutine == null)
            {
                exclamationCoroutine = StartCoroutine(ExclamationShake());
            }
        }
    }

    IEnumerator ZoomToCharacter()
    {
        yield return new WaitForEndOfFrame();

        Vector3 characterPosition = transform.position + Vector3.up * 1.5f; 
        Vector3 targetPosition = cameraTransform.position + (characterPosition - cameraTransform.position).normalized * zoomDistance; 
        Vector3 startPosition = cameraTransform.position;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * cameraZoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        StartCoroutine(ResetCameraPosition());
    }

    IEnumerator ResetCameraPosition()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = cameraTransform.position;
        while (elapsedTime < 1f)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, originalCameraPosition, elapsedTime * cameraZoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ExclamationShake()
    {
        Vector3 originalPos = strongAttackExclamation.rectTransform.anchoredPosition;

        while (true)
        {
            float shakeAmount = 5f;
            strongAttackExclamation.rectTransform.anchoredPosition = originalPos + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            yield return new WaitForSeconds(0.05f);
            strongAttackExclamation.rectTransform.anchoredPosition = originalPos;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void ResetStrongAttack()
    {
        isStrongUnlocked = false;
        isStrongUsed = false;
        UpdateSkillUI();
        Debug.Log(gameObject.name + " lost Strong Attack due to new combo phase!");

        if (strongAttackExclamation != null)
        {
            strongAttackExclamation.gameObject.SetActive(false);
            if (exclamationCoroutine != null)
            {
                StopCoroutine(exclamationCoroutine);
                exclamationCoroutine = null;
            }
        }
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
