using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

   [SerializeField] private GameObject[] enemyPrefab;
[SerializeField] private int minEnemiesPerRoom = 1;
[SerializeField] private int maxEnemiesPerRoom = 3;


   
    int roomWidth = 20;  
    int roomHeight = 12;  

    int gridSizeX = 10;
    int gridSizeY = 10;

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] roomGrid;

    private int roomCount;

    private bool generationComplete = false;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
        }
        else if (roomCount < minRooms)
        {
            Debug.Log("roomCount was less than the minimum amount of rooms. trying again");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation complete, {roomCount} rooms created");
            generationComplete = true;
        }

        if (roomQueue.Count == 0 && roomCount < minRooms)
{
    Debug.Log("Not enough rooms. Restarting generation...");
    RegenerateRooms();
}
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

   private bool TryGenerateRoom(Vector2Int roomIndex)
{
    int x = roomIndex.x;
    int y = roomIndex.y;

    if (roomCount >= maxRooms || roomQueue.Count + roomCount >= maxRooms)
    return false;
  if (Random.value < 0.05f && roomIndex != Vector2Int.zero)
    return false;

   if (CountAdjacentRooms(roomIndex) > 2)
    return false;

    roomQueue.Enqueue(roomIndex);
    roomGrid[x, y] = 1;
    roomCount++;

    var newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
    newRoom.GetComponent<Room>().RoomIndex = roomIndex;
    newRoom.name = $"Room-{roomCount}";
    roomObjects.Add(newRoom);

    OpenDoors(newRoom, x, y);

    // Spawn enemies in the room
    SpawnEnemies(newRoom);

    return true;
}

private void SpawnEnemies(GameObject room)
{
    int enemyCount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);
    Room roomScript = room.GetComponent<Room>();
    for (int i = 0; i < enemyCount; i++)
    {
        Vector3 spawnPosition = GetRandomPositionInRoom(room);
        int randomvalue=Random.Range(0, enemyPrefab.Length);
        var newEnemy = Instantiate(enemyPrefab[randomvalue], spawnPosition, Quaternion.identity);
          Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                if (enemyScript != null && roomScript != null)
                {
                    roomScript.AddEnemy(enemyScript);
                    Debug.Log("Added Enemy in room");
                }
    }
}


private Vector3 GetRandomPositionInRoom(GameObject room)
{
    float roomX = room.transform.position.x;
    float roomY = room.transform.position.y;

    float offsetX = Random.Range(-roomWidth / 3f, roomWidth / 3f);
    float offsetY = Random.Range(-roomHeight / 3f, roomHeight / 3f);

    return new Vector3(roomX + offsetX, roomY + offsetY, 0);
}

    private int regenerationAttempts = 0;
private int maxRegenerationAttempts = 3;

private void RegenerateRooms()
{
    if (regenerationAttempts >= maxRegenerationAttempts)
    {
        Debug.LogError("Dungeon generation failed after multiple attempts.");
        return;
    }

    regenerationAttempts++;
    roomObjects.ForEach(Destroy);
    roomObjects.Clear();
    roomGrid = new int[gridSizeX, gridSizeY];
    roomQueue.Clear();
    roomCount = 0;
    generationComplete = false;

    Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
    StartRoomGenerationFromRoom(initialRoomIndex);
}


    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.down);
            bottomRoomScript.OpenDoor(Vector2Int.up);
        }
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.up);
            topRoomScript.OpenDoor(Vector2Int.down);
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++;
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++;
        if (y > 0 && roomGrid[x, y - 1] != 0) count++;
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++;

        return count;
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }

    public GameObject GetRoomAt(Vector2Int index)
{
    return roomObjects.Find(room => room.GetComponent<Room>().RoomIndex == index);
}

}