using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2Int direction; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            Debug.Log("ss");

            if (player != null)
            {
                player.TeleportTo(direction);
            }
        }
    }
}
