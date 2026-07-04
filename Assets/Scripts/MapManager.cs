using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public Texture2D[] maps;

    public GameObject gemPrefab;
    public GameObject[] zombiePrefab;
    public GameObject wallPrefab;
    public GameObject pointLightPrefab;
    public int numberOfLights = 35;

    public bool zombiesCanMove = true;
    private Texture2D selectedMap;
    private List<Vector3> openPositions = new List<Vector3>();

    private Color wallColor = Color.black;

    private int gemsRemaining;
    public int zombieCount = 10;

    public static MapManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);  
    }


    private void Start()
    {
        GenerateNewMap();
        GenerateGem();
        GenerateZombie();
        GenerateLights();
    }
    public void GenerateNewMap()
    {
        openPositions.Clear();
        selectedMap = maps[Random.Range(0, maps.Length)];

        for (int x = 0; x < selectedMap.width; x++)
        {
            for (int y = 0; y < selectedMap.height; y++)
            {
               GenerateTile(x, y);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color pixelColor = selectedMap.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            openPositions.Add(new Vector3(x, 0, y));
            return;
        }
        if(pixelColor == wallColor)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
        }
    }

    private void GenerateGem()
    {
        for (int i = 0; i < 3; i++)
        {
            if (openPositions.Count == 0)
                return;

            int index = Random.Range(0, openPositions.Count);
            Instantiate(gemPrefab, openPositions[index], Quaternion.identity);
            openPositions.RemoveAt(index);
        }
        gemsRemaining = 3;
    }

    public Vector3 GetRandomPos()
    {
        if (openPositions.Count == 0) return Vector3.zero;
        return openPositions[Random.Range(0, openPositions.Count)];
    }
    private void GenerateZombie()
    {
        for (int i = 0; i < zombieCount; i++)
        {
            if (openPositions.Count == 0)
                return;

            int positionIndex = Random.Range(0, openPositions.Count);

            // Pick one of the six zombie prefabs randomly
            GameObject randomZombie =
            zombiePrefab[Random.Range(0, zombiePrefab.Length)];

            Instantiate(randomZombie,
                    openPositions[positionIndex],
                    Quaternion.identity);

            openPositions.RemoveAt(positionIndex);
        }
    }

    private void GenerateLights()
    {
        List<Vector3> placedLights = new List<Vector3>();

        for (int i = 0; i < numberOfLights; i++)
        {
            Vector3 bestPosition = Vector3.zero;
            float maxDistance = -1f;
            bool foundValidPoint = false;

            // Generate multiple candidates and pick the one furthest from all other placed lights
            for (int candidate = 0; candidate < 35; candidate++)
            {
                float randomX = Random.Range(0f, selectedMap.width);
                float randomZ = Random.Range(0f, selectedMap.height);
                Vector3 randomPos = new Vector3(randomX, 0f, randomZ);

                UnityEngine.AI.NavMeshHit hit;
                if (UnityEngine.AI.NavMesh.SamplePosition(randomPos, out hit, 2.0f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    Vector3 candidatePos = hit.position;

                    // First light can be placed anywhere valid
                    if (placedLights.Count == 0)
                    {
                        bestPosition = candidatePos;
                        foundValidPoint = true;
                        break; 
                    }

                    // Find the distance to the closest already placed light
                    float minDistanceToOther = float.MaxValue;
                    foreach (Vector3 placedLight in placedLights)
                    {
                        float dist = Vector3.Distance(candidatePos, placedLight);
                        if (dist < minDistanceToOther)
                        {
                            minDistanceToOther = dist;
                        }
                    }

                    // We want to maximize this minimum distance to spread them out evenly
                    if (minDistanceToOther > maxDistance)
                    {
                        maxDistance = minDistanceToOther;
                        bestPosition = candidatePos;
                        foundValidPoint = true;
                    }
                }
            }

            if (foundValidPoint)
            {
                placedLights.Add(bestPosition);
                Vector3 lightPosition = bestPosition + Vector3.up * 1.5f;
                Instantiate(pointLightPrefab, lightPosition, Quaternion.identity);
            }
        }
    }
    
    public void GemPickedUp()
    {
        gemsRemaining--;
        if (gemsRemaining == 0)
        {
            zombiesCanMove = false;
            UIManager.instance.ShowGameOver(true);
        }
    }
}
