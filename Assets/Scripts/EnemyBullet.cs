using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int speed;

    private Transform player;
    private Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        target = new Vector2(player.position.x, player.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position= Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(transform.position.x == target.x && transform.position.y == target.y)
        Destroy(gameObject);
    }public Room CurrentRoom { get; private set; }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<PlayerMovement>();
            player.KnockBack(transform.position);
            Destroy(gameObject);
        }
    }


}
