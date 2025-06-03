using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; //List

public class GameManager : MonoBehaviour
{

    public Transform tile;

    public Transform obstacle;

    public Vector3 startPoint = new Vector3 (0, 0, -5);

    [Range(1,15)]
    public int initSpawnNum = 10;

    public int initNoObstacles = 4;

    private Vector3 nextTileLocation;

    private Quaternion nextTileRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        // i++ is the same as ++i
        for (int i=0; i<initSpawnNum; ++i)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    /// <summary>
    /// Will spawn tile at a certain location
    /// </summary>
    /// <param name="spawnObstacles">If we should spawn an obstacle</param>>
    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);

        var nextTile = newTile.Find("Next Spawn Point");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }

    private void SpawnObstacle(Transform newTile)
    {
        var obstacleSpawnPoints = new List<GameObject>();

        foreach (Transform child in newTile)
        {
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }
        if (obstacleSpawnPoints.Count > 0)
        {
            int index = Random.Range(0, obstacleSpawnPoints.Count);

            var spawnPoint = obstacleSpawnPoints[index];

            var spawnPos = spawnPoint.transform.position;

            var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

            newObstacle.SetParent(spawnPoint.transform);

            //Addition to up the difficulty

            //-------------------------------------------------------------------------------------

            /*
            
            int chance = Random.Range(1, 3);
            if (chance > 1)
            {
                int index2 = Random.Range(0, obstacleSpawnPoints.Count);
                if (index2 == index)
                {
                    if (index2 == ((obstacleSpawnPoints.Count) - 1))
                    {
                        index2--;
                        print("Test1");
                    }
                    else
                    {
                        index2++;
                        print("Test2");
                    }
                }

                var spawnPoint2 = obstacleSpawnPoints[index2];

                var spawnPos2 = spawnPoint2.transform.position;

                var newObstacle2 = Instantiate(obstacle, spawnPos2, Quaternion.identity);

                newObstacle2.SetParent(spawnPoint2.transform);
            }

            */

            //-------------------------------------------------------------------------------------


            //Note: Window -> Rendering -> Lighting -> Environment
            //that's how to change skybox
        }
    }
}
