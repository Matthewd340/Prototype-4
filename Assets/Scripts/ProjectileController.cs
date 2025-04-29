using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody projectileRb;
    private GameObject[] enemies;
    private int enemyIndex;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        enemyIndex = Random.Range(0, enemies.Length);
        Vector3 lookDirection = (enemies[enemyIndex].transform.position - transform.position).normalized;
        projectileRb.AddForce(lookDirection * speed);
        StartCoroutine(AutoDestroyRoutine());
    }

    IEnumerator AutoDestroyRoutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
