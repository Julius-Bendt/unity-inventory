using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public int damage;
    public bool piercing = false;
    Rigidbody2D rig;

    const float ALIVETIME = 7.5f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        StartCoroutine(SmoothDestroy());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.MovePosition(rig.position + new Vector2(transform.right.x, transform.right.y) * speed * Time.fixedDeltaTime);
    }

    public IEnumerator SmoothDestroy()
    {
        float fadeAfter = ALIVETIME * 0.5f;
        float elapsedTime = 0;

        yield return new WaitForSeconds(fadeAfter);

        while(elapsedTime < fadeAfter)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, elapsedTime / fadeAfter);

            yield return null;
        }

        Destroy(gameObject);

        yield return null;
    }

}
