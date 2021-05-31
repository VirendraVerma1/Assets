using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglePathGenerator : MonoBehaviour
{
    public GameObject MyEagle;
    List<Vector3> EaglePoints = new List<Vector3>();
    List<Vector3> EaglePointsNearPlayer = new List<Vector3>();
    public int AreaCover = 500;
    public int upDownFlexible = 30;
    public int minPoint = 100;
    public int maxPoint = 500;
    public int revolvePlayerArea = 200;
  

    public GameObject WaterGO;

    public GameObject TestPrefab;
    GameObject PlayerPrefab;

    Transform CurrentTerrain;

    void Start()
    {
        MyEagle.transform.position=gameObject.transform.position;
        CurrentTerrain = GameObject.FindGameObjectWithTag("TerrainObject").transform;
        SetWaterTransform();
        SetEaglePositionSpawnerTransform();
        StartCoroutine(InitailizePlayer());
        
    }

    IEnumerator InitailizePlayer()
    {
        int n=1;
        while (n > 0)
        {
            yield return new WaitForSeconds(1);
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                n = 0;
                PlayerPrefab = GameObject.FindGameObjectWithTag("Player");
            }
        }
       
    }

    void SetWaterTransform()
    {
        float y=WaterGO.transform.position.y;
        WaterGO.transform.position = new Vector3(CurrentTerrain.position.x+150, y, CurrentTerrain.position.z+150);
    }

    void SetEaglePositionSpawnerTransform()
    {
        float y = gameObject.transform.position.y;
        gameObject.transform.position = new Vector3(CurrentTerrain.position.x + 150, y, CurrentTerrain.position.z + 150);

        int randomPoint = Random.Range(minPoint, maxPoint);//points to generate

        while (randomPoint > 0)
        {
            randomPoint--;
            int randomX = Random.Range(AreaCover*-1, AreaCover);
            int randomY = Random.Range(upDownFlexible*-1, upDownFlexible);
            int randomZ = Random.Range(AreaCover*-1, AreaCover);
            Vector3 temp = new Vector3(gameObject.transform.position.x + randomX, gameObject.transform.position.y + randomY, gameObject.transform.position.z + randomZ);
            EaglePoints.Add(temp);
            Instantiate(TestPrefab, temp,gameObject.transform.rotation);
        }

    }

    public Vector3 GetRandomPoint(Vector3 oldPosition,Vector3 currentPosition)
    {
        int random=0;
        if (PlayerPrefab == null)
        {
            random = Random.Range(0, EaglePoints.Count);
            return EaglePoints[random];
        }
        else
        {
            CheckIfAreaExist(oldPosition, currentPosition);
            if (EaglePointsNearPlayer.Count == 0)
            {
                random = Random.Range(0, EaglePoints.Count);
                return EaglePoints[random];
            }
            else
            {
                random = Random.Range(0, EaglePointsNearPlayer.Count);
                return EaglePointsNearPlayer[random];
            }
        }
        
    }

    void CheckIfAreaExist(Vector3 oldPosition,Vector3 currentPosition)
    {
        float oldDistance=0;
        float newTargetDistance=99990;
        if (oldPosition != Vector3.zero)
            oldDistance = Vector3.Distance(oldPosition, currentPosition);
       

        EaglePointsNearPlayer.Clear();
        foreach (Vector3 v in EaglePoints)
        {
            float distance = Vector3.Distance(v, PlayerPrefab.transform.position);
            if (oldPosition != Vector3.zero)
            newTargetDistance = Vector3.Distance(v, oldPosition);
            if (distance < revolvePlayerArea && oldDistance < newTargetDistance)
            {
                EaglePointsNearPlayer.Add(v);
            }

        }
    }


    
}
