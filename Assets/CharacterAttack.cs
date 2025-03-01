using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public GameObject stonePrefab;
    public Transform firePoint;
    public float stoneSpeed = 10f;

    void Update()
    {
        if (gameObject.CompareTag("CharacterA"))
        {
            if (Input.GetKeyDown(KeyCode.E)) Attack(1);
            if (Input.GetKeyDown(KeyCode.Q)) Attack(3);
            if (Input.GetKeyDown(KeyCode.X)) Attack(5);
        }
        else if (gameObject.CompareTag("CharacterB"))
        {
            if (Input.GetKeyDown(KeyCode.K)) Attack(1);
            if (Input.GetKeyDown(KeyCode.L)) Attack(3);
            if (Input.GetKeyDown(KeyCode.M)) Attack(5);
        }
    }

    void Attack(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject stone = Instantiate(stonePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * stoneSpeed;
        }
    }
}
