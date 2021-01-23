using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float moveSpeed, damage, damageDisableTime = 0.75f;
    private Rigidbody2D rig;
    public Collider2D damageCollider;

    public float health = 100;

    bool dying;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Spawner.enemiesAlive++;

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        if (health <= 0)
            OnDie();
    }

    public void FixedUpdate()
    {
        if (dying)
            return;

        LookAtTarget();
        MoveTowardTarget();
    }

    public void MoveTowardTarget()
    {
        
        rig.MovePosition(rig.position + new Vector2(transform.right.x, transform.right.y) * moveSpeed * Time.fixedDeltaTime);

       
    }

    public void LookAtTarget()
    {
        Vector2 lookDir = new Vector2(target.position.x, target.position.y) - rig.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rig.rotation = angle;
    }

    public void OnDie()
    {
        dying = true;
        //Explode

        Spawner.enemiesAlive--;
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D o)
    {
        if(o.gameObject.GetComponent<Bullet>())
        {
            Bullet b = o.gameObject.GetComponent<Bullet>();
            health -= b.damage;

            if(!b.piercing)
                Destroy(o.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D o)
    {
        if(o.tag == "Player")
        {
            o.GetComponent<Player>().TakeDamage(damage);
            StartCoroutine(DisableCollider());
        }
    }

    private IEnumerator DisableCollider()
    {
        damageCollider.enabled = false;
        yield return new WaitForSeconds(damageDisableTime);
        damageCollider.enabled = true;

        yield return null;
    }
}
