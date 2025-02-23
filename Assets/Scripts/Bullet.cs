using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int bulletSpeed;

    public int bulletDamage;
    
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {

            Enemy enemy = col.GetComponent<Enemy>();
            enemy.health-= bulletDamage;
            enemy.StartCoroutine(enemy.FlashCo());
            Destroy(gameObject);
            
        }
    }
}
