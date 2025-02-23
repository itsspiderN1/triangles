using UnityEngine;

public class Shooting : MonoBehaviour
{
   public int fireRate;
   public float reloadTime;
   public int bullets;

   public Transform gunFirePoint;

   public GameObject bulletPrefab;
    void Start()
    {
        
    }

   
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && bullets>0 && reloadTime<=0)
        {
            Shoot();
            bullets--;
            reloadTime = 1;
        }
        reloadTime -= Time.deltaTime;
    }
   void Shoot()
{
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 shootDirection = (mousePos - (Vector2)gunFirePoint.position).normalized;
    float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
    var bullet = Instantiate(bulletPrefab, gunFirePoint.position, Quaternion.Euler(0, 0, angle - 90));
    var bulletStats = bullet.GetComponent<Bullet>();
    bullet.GetComponent<Rigidbody2D>().linearVelocity = shootDirection * bulletStats.bulletSpeed;
}

}
