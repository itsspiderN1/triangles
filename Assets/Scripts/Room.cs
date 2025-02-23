using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    public Vector2Int RoomIndex { get; set; }

    private List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        CloseAllDoors();
    }

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
            OpenAllDoors();
        }
    }

    private void CloseAllDoors()
    {
        if (topDoor) topDoor.SetActive(false);
        if (bottomDoor) bottomDoor.SetActive(false);
        if (leftDoor) leftDoor.SetActive(false);
        if (rightDoor) rightDoor.SetActive(false);
    }

    private void OpenAllDoors()
    {
        if (topDoor) topDoor.SetActive(true);
        if (bottomDoor) bottomDoor.SetActive(true);
        if (leftDoor) leftDoor.SetActive(true);
        if (rightDoor) rightDoor.SetActive(true);
    }
}
