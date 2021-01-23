using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    public Weapon weapon;

    public float moveSpeed;

    private Camera cam;
    private Rigidbody2D rig;

    Vector2 input, mousePos;

    public int health = 100;
    public Transform shootPos;

    public Stat stats;
    private PlayerUIManager playerUI;

    void Start()
    {
        playerUI = GetComponent<PlayerUIManager>();
        playerUI.player = this;

        cam = Camera.main;
        rig = GetComponent<Rigidbody2D>();

        stats = new Stat(moveSpeed, cam.orthographicSize, 1);

        StartCoroutine(Shoot());

    }

    public void test (bool success)
    {
        Debug.Log("event success:" + success);
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        cam.orthographicSize = stats.Sight;

        stats.UpdateStats(false);
        playerUI.UpdateUI();
        
    }

    public void FixedUpdate()
    {
        if(!App.Instance.inventory.InventoryOpen)
        {
            rig.MovePosition(rig.position + input.normalized * stats.MoveSpeed * Time.fixedDeltaTime);

            Vector2 lookDir = mousePos - rig.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rig.rotation = angle;
        }

    }

    public void TakeDamage(float amount)
    {
        App.Instance.shake.TriggerShake(0.25f);
        stats.health -= amount;
        playerUI.UpdateUI();
    }

    public IEnumerator Shoot()
    {

        while(health > 0)
        {
            
            //check if pressed
            bool fire = false;

            if (weapon.autoFire)
                fire = Input.GetButton("Fire");
            else
                fire = Input.GetButtonDown("Fire");


            if(fire && !App.Instance.inventory.InventoryOpen)
            {
                //create bullet
                Instantiate(weapon.bullet, shootPos.position, shootPos.rotation);
               // Instantiate(weapon.muzzle, shootPos.position, shootPos.rotation);
                yield return new WaitForSeconds(weapon.fireRate);
            }

            yield return null;
        }

        yield return null;

    }
}
