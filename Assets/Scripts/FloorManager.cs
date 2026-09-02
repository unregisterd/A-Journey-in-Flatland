using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] floorPrefabs;
    public void SpawnFloor()
    {
        int r = Random.Range(0,floorPrefabs.Length);
        GameObject floor=Instantiate(floorPrefabs[r],transform);//将创建出来的物件设定在FloorManager物件下，作为其子物件
        floor.transform.position = new Vector3(Random.Range(-7.28f,-0.77f),-5f,0f);
    }
}
