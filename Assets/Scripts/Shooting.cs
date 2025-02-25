using UnityEngine;

public class Shooting : MonoBehaviour
{
   public int fireRate;
   public float reloadTime;
   public int bulletSpeedAmp;
   public int bulletDmgAmp;

   private Stats stats;

   public Transform gunFirePoint;

   public Transform gunFirePoint2;

   public GameObject bulletPrefab;
    void Start()
    {
     stats=GetComponent<Stats>();   
    }

   
    void Update()
    {
        bulletSpeedAmp = stats.bulletSpeedAmplifier;
        bulletDmgAmp = stats.bulletDamageAmplifier;
        if(Input.GetMouseButtonDown(0) && reloadTime<=0)
        {
            Shoot();
           
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
    if(gunFirePoint2!=null)
    {
        var bullet1 = Instantiate(bulletPrefab, gunFirePoint2.position, Quaternion.Euler(0, 0, angle - 90));
    var bulletStats1 = bullet.GetComponent<Bullet>();
    bulletStats1.bulletDamage+=bulletDmgAmp;
    bullet1.GetComponent<Rigidbody2D>().linearVelocity = shootDirection * (bulletStats1.bulletSpeed+bulletSpeedAmp);
    }
    bulletStats.bulletDamage+=bulletDmgAmp;
    bullet.GetComponent<Rigidbody2D>().linearVelocity = shootDirection * (bulletStats.bulletSpeed+bulletSpeedAmp);
    
}

}
