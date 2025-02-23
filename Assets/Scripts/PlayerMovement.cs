using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{

    public int speed;
    private Rigidbody2D rb;

    private Vector2 moveDirection;

    private Vector2 mousePos;

     private DungeonGenerator dungeon;

     private Camera mainCamera;

    public bool canMove = true;

     public float knockbackForce;

     public Color flashColor;
   public Color regularColor;
   public float flashDur;
   public int numberOfFlashes;
   private SpriteRenderer rend;

   private Stats stats;


     
     
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        dungeon = FindObjectOfType<DungeonGenerator>();
        mainCamera = Camera.main;
        rend=GetComponent<SpriteRenderer>();
        stats = GetComponent<Stats>();
    }

    void FixedUpdate()
    {
       PlayerInput();
       Move();
       Rotate();
    }

    void PlayerInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
    }

    void Rotate()
    {
        Vector2 aimDirection = mousePos - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    void Move()
    {
       if(canMove)
       rb.linearVelocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

  public void TeleportTo(Vector2Int targetRoomIndex)
    {
        if (dungeon == null) return;

        GameObject targetRoom = dungeon.GetRoomAt(targetRoomIndex);

        if (targetRoom != null)
        {
            Vector3 newPosition = targetRoom.transform.position;
            
            // Move the player to the new room
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            // Move the camera to the new room smoothly
            StartCoroutine(SmoothCameraTransition(newPosition));
        }
    }

    private IEnumerator SmoothCameraTransition(Vector3 targetPosition)
    {
        float duration = 0.5f; // Smooth transition time
        float elapsedTime = 0f;
        Vector3 startCameraPos = mainCamera.transform.position;
        Vector3 targetCameraPos = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startCameraPos, targetCameraPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetCameraPos;
    }

     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            canMove=false;
            if(stats.invincible==false)
            stats.Health--;
            Debug.Log("attacked");
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.linearVelocity = knockbackDirection * knockbackForce;
            stats.invincible = true;
            

           
  StartCoroutine(EnableMovementAfterDelay(0.25f)); 
   StartCoroutine(FlashCo());
        }
    }
    private IEnumerator FlashCo()
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
    IEnumerator EnableMovementAfterDelay(float delay)
    {
        stats.invincible=false;
        yield return new WaitForSeconds(delay);
        canMove = true; // Enable movement after the specified delay
    }

   public Room CurrentRoom { get; private set; }

void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Room"))
    {
        CurrentRoom = other.GetComponent<Room>();
        Debug.Log($"[Player] Entered Room: {CurrentRoom.name}");
    }
}
}
