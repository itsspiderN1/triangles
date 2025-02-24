using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject player;

    public int speed;

    public int health;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject bullet;

    public bool normalType;
    public bool dualType;
    public bool sniperType;
    
    public float range;

    public Transform guntip;

    public Transform guntip2;

    private bool canShoot;

     public Color flashColor;
   public Color regularColor;
   public float flashDur;
   public int numberOfFlashes;
   private SpriteRenderer rend;





     private Room currentRoom; 
     

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        timeBtwShots = startTimeBtwShots;
        rend=GetComponent<SpriteRenderer>();
    }

   
    void Update()
    { 
        Health();
         player = GameObject.FindGameObjectWithTag("Player");
        if(PlayerInSameRoom())
        {
            Movement();
       
        Shooting();
        }
        
    }

    void Shooting()
    {
        

        if(normalType)
        {

        
        if(timeBtwShots<=0 && canShoot)
        {
            
            var firedbullet = Instantiate(bullet, guntip.position, Quaternion.identity);
            if(sniperType)
            {
                firedbullet.GetComponent<EnemyBullet>().speed = 15;
            }
            timeBtwShots = startTimeBtwShots;
        }
        else
        timeBtwShots-=Time.deltaTime;
        }
        else if(dualType)
        {
             if(timeBtwShots<=0 && canShoot)
        {
            Instantiate(bullet, guntip.position, Quaternion.identity);
            Instantiate(bullet, guntip2.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
         else
        timeBtwShots-=Time.deltaTime;
        
        }

        
    }
    
   

    void Health()
    {
        if (health <= 0)
        {
            if (currentRoom != null)
            {
                currentRoom.RemoveEnemy(this);
            }
            Destroy(gameObject);
        }
    }

    void Movement()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
      
      
        
            if(distance>range)
            {
                canShoot = false;
                 transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else {
                transform.position = this.transform.position;
                canShoot=true;
            }
            
        
       
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    public IEnumerator FlashCo()
    {
        int temp = 0;
        while(temp < numberOfFlashes)
        {
            rend.color = flashColor;
            yield return new WaitForSeconds(flashDur);
            rend.color = regularColor;
            yield return new WaitForSeconds(flashDur);
            temp++;
        }
    }
    bool PlayerInSameRoom()
    {
        Room playerRoom = player.GetComponent<PlayerMovement>().CurrentRoom;
        return playerRoom == currentRoom; // Only move if in the same room
    }

    public void SetRoom(Room room)
    {
        currentRoom = room;  // Assign the room this enemy belongs to
    }
}
