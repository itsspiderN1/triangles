using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    private bool isLeft = false;
    private bool isRight=false;
    private bool isTop = false;

    private bool isBottom=false;

    public Vector2Int RoomIndex { get; set; }


    private List<Enemy> enemies = new List<Enemy>();

     public void AddEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            enemy.SetRoom(this);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
         
        }
    }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up && topDoor != null)
        {
            
           // topDoor.SetActive(true);
            AssignDoorDirection(topDoor, Vector2Int.up);
            isTop=true;
        }

        if (direction == Vector2Int.down && bottomDoor != null)
        {
           
            //bottomDoor.SetActive(true);
            AssignDoorDirection(bottomDoor, Vector2Int.down);
            isBottom=true;
        }

        if (direction == Vector2Int.left && leftDoor != null)
        {
          
           // leftDoor.SetActive(true);
            AssignDoorDirection(leftDoor, Vector2Int.left);
            isLeft = true;
        }

        if (direction == Vector2Int.right && rightDoor != null)
        {
            
            //rightDoor.SetActive(true);
            isRight=true;
            AssignDoorDirection(rightDoor, Vector2Int.right);
        }
    }

    void Update()
    {
        if(isTop && enemies.Count == 0)
        {
            topDoor.SetActive(true);
        }
        else 
        topDoor.SetActive(false);
        if(isBottom && enemies.Count == 0)
        {
            bottomDoor.SetActive(true);
        }
        else 
        bottomDoor.SetActive(false);
        if(isLeft && enemies.Count == 0)
        {
            leftDoor.SetActive(true);
        }
        else 
        leftDoor.SetActive(false);
        if(isRight && enemies.Count == 0)
        {
            rightDoor.SetActive(true);
        }
        else 
        rightDoor.SetActive(false);

    }

    private void AssignDoorDirection(GameObject door, Vector2Int dir)
    {
        Door doorScript = door.GetComponent<Door>();
        if (doorScript != null)
        {
            doorScript.direction = RoomIndex + dir;
        }
    }
}
