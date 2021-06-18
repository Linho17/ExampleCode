using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcebergsPool : MonoBehaviour
{
    public static IcebergsPool ICEBERGSPOOL;


    [SerializeField] private List<Iceberg> icebergs = new List<Iceberg>();
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float distanceBetweenIceberg;
    [SerializeField] private Iceberg prevIceberg;
    [SerializeField] private Iceberg nextIceberg;


    private Vector3 nextSpawnPoint;

    private bool isSpawnStart  = false;
    private int counterSpawnIceberg = -1;
    public bool CanSpawn => isSpawnStart && counterSpawnIceberg > 0;

    private void Awake()
    {
        if (!ICEBERGSPOOL)
        {
            ICEBERGSPOOL = this;
        }
    }

  


    private void ChooseNextIceberg()
    {
        do
        {
            nextIceberg = icebergs[Random.Range(0, icebergs.Count)];
           
        }
        while (!nextIceberg.IsReady);
        if (Mathf.Abs(spawnPoint.position.x - prevIceberg.transform.position.x) < (distanceBetweenIceberg + (nextIceberg.Size + prevIceberg.Size) * 0.5f)+10f)
            nextSpawnPoint = prevIceberg.transform.position + (distanceBetweenIceberg + (nextIceberg.Size + prevIceberg.Size) * 0.5f) * Vector3.right;
        else
            nextSpawnPoint = spawnPoint.position;
    }


    void Update()
    {
        if (CanSpawn)
        {
            if (spawnPoint.position.x >= nextSpawnPoint.x)
            {
                Spawn();
                counterSpawnIceberg--;
            }
        }
    }

    public void StartSpawn(int _counter = 100)
    {
        if (!isSpawnStart)
        {
            counterSpawnIceberg = _counter;
            ChooseNextIceberg();
            isSpawnStart = true;
        }
    }

    public void StopSpawn()
    {
        isSpawnStart = false;
    }


    private void Spawn()
    {
        nextIceberg.transform.position = nextSpawnPoint;
        nextIceberg.IsReady = false;
        prevIceberg = nextIceberg;
        ChooseNextIceberg();
    }

}
