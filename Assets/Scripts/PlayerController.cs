using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup;
    public bool hasShootingPowerup;
    public bool hasJumpPowerup = false;
    public float smashForce = 30;
    public float smashRadius = 5;
    public float smashJumpHeight = 10;
    public float powerupStrength = 15;
    public GameObject powerupIndicator;
    public GameObject shootingPowerupIndicator;
    public GameObject jumpPowerupIndicator;
    public GameObject rocketPrefabUp;
    public GameObject rocketPrefabLeft;
    public GameObject rocketPrefabDown;
    public GameObject rocketPrefabRight;
    private GameObject player;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        player = GameObject.Find("Player");
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0,0,0);
            spawnManager.deathCount += 1;
        }


        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        shootingPowerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        jumpPowerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (Input.GetKeyDown(KeyCode.LeftShift) && hasShootingPowerup == true)
        {
            Instantiate(rocketPrefabUp, player.transform.position, player.transform.rotation);
            Instantiate(rocketPrefabLeft, player.transform.position, player.transform.rotation);
            Instantiate(rocketPrefabDown, player.transform.position, player.transform.rotation);
            Instantiate(rocketPrefabRight, player.transform.position, player.transform.rotation);

            
        }

        if (hasJumpPowerup == true)
        {
            jumpPowerupIndicator.gameObject.SetActive(true);
        }

        else if (hasJumpPowerup == false)
        {
            jumpPowerupIndicator.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }

        if (other.CompareTag("Shooting Powerup"))
        {
            hasShootingPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            shootingPowerupIndicator.gameObject.SetActive(true);
        }

        if (other.CompareTag("Jump Powerup"))
        {
            Destroy(other.gameObject);
            StartCoroutine(GroundSmashRoutine());
        }
    }
   //IEnumerator used as return type for coroutine
   //coroutine can suspend executioon (yield) until YieldInstruction finishes
    IEnumerator PowerupCountdownRoutine()
    //          Coroutine data type
    {
        yield return new WaitForSeconds(7);
        //                  YieldInstruction
        hasPowerup = false;
        hasShootingPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
        shootingPowerupIndicator.gameObject.SetActive(false);
    }
//access modifier (optional)
            //return type (like void)
                    //method name (no spaces, must start with letter)
                    //should be concise and meaningful
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            Debug.Log("Collided with " + collision.gameObject.name + "with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpPowerupIndicator.SetActive(false);
        }
    }

    IEnumerator GroundSmashRoutine()
    {
        hasJumpPowerup = true;
        playerRb.velocity = new Vector3 (0, smashJumpHeight, 0);
        yield return new WaitForSeconds(0.5f);
        playerRb.velocity = new Vector3 (0, -smashJumpHeight * 2, 0);
        yield return new WaitUntil(() => playerRb.velocity.y == 0);
        SmashInpact();
        hasJumpPowerup = false;
    }

    void SmashInpact()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, smashRadius);

        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                Vector3 direction = enemy.transform.position - transform.position;
                float distance = direction.magnitude;

                float forceMultiplier = 1 - (distance / smashRadius); 
                enemyRb.AddForce(direction.normalized * smashForce * forceMultiplier, ForceMode.Impulse);
            }
        }
    }
}
