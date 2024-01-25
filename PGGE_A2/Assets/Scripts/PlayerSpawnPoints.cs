using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoints : MonoBehaviour
{
    public List<Transform> mSpawnPoints = new List<Transform>();

    public Transform GetSpawnPoint()
    {
        //added brackets to the if statement it make it more readable
        if (mSpawnPoints.Count == 0) 
        {
            return this.transform;
        }
        return mSpawnPoints[Random.Range(0, mSpawnPoints.Count)].transform;
    }
}
