using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{

    public PlayerMovement1 pm;
    public Transform shootingPoint;

    public GameObject bulletPrefab;
    public GameObject playerBody;
    public Rigidbody2D body;

    public int maxBullets = 10;
    public int bulletCount;

    public TextMeshProUGUI bulletText;

    public bool isReloading;
    public bool canShoot;

    public float timeBtwShots = .1f;  //Time between shots
    private float timeOfLastShot;
   

    void Start()
    {
        

        bulletCount = maxBullets;
        if (bulletCount > 0)
        {
            canShoot = true;
        }
    }

    void Update()
    {
   
        if (Input.GetButtonDown("Reload") && bulletCount < 10 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        bulletText.text = bulletCount.ToString();


        if (bulletCount == 0)
        {
            canShoot = false;

            if (Input.GetButtonDown("Reload") && !isReloading)
                {
                StartCoroutine(Reload());
            }
           
        }


        if (Input.GetMouseButtonDown(0) && canShoot == true && pm.isRolling == false)
        {
            if (Time.time - timeOfLastShot >= timeBtwShots) //If the time elapsed is more than the fire rate, allow a shot
            {
                Shoot();
             
                timeOfLastShot = Time.time;   //set new time of last shot

            }

        }

        if (Input.GetMouseButtonDown(0) && bulletCount == 0 && !isReloading) 
        {
            StartCoroutine(Reload());
        }

        Physics2D.IgnoreCollision(bulletPrefab.GetComponent<BoxCollider2D>(), gameObject.GetComponent<CircleCollider2D>());

    }

    void Shoot()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Set bullet direction based on player's facing direction
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        float bulletSpeed = 10f; // Set bullet speed

        // Check facing direction and apply velocity accordingly
        if (pm.isFacingRight)
        {
            bulletRb.velocity = new Vector2(bulletSpeed, 0); // Fire to the right
        }
        else
        {
            bulletRb.velocity = new Vector2(-bulletSpeed, 0); // Fire to the left
        }

        bulletCount--;
    }
        
        

    public IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(3.0f);
        bulletCount = maxBullets;
        canShoot = true;
        isReloading = false;
    }

}
