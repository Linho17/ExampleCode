
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    private static SpawnControl instance;
    [SerializeField] private IcebergsPool icebergsPool;
    [SerializeField] private SpawnFish spawnFish;
    [SerializeField] private SpawnSkua spawnSkua;
    [SerializeField] private KillerWhale killerWhale;
    [SerializeField] private SpawnOrca spawnOrca;

    [SerializeField] private float timeDelaySpawn;
    [SerializeField] private float speed;

    private List<Enemy> enemies = new List<Enemy>();

    public bool isSpawning { set; get; }
    private bool spawningIceberg;
    private bool spawningKillerWhale;
    private bool spawningOrca;
    private bool spawningSkua;
    private bool spawningFish;

    int currentIndex;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentIndex = -1;
    }

    public static void AddEnemy(Enemy enemy)
    {
        instance.enemies.Add(enemy);
        enemy.SetSpeedMove(instance.speed);
    }
    public static void RemoveEnemy(Enemy enemy)
    {
        instance.enemies.Remove(enemy);
    }

     


    public IEnumerator WaveOne()
    {
        isSpawning = true;

        icebergsPool.StartSpawn();
       
        yield return new WaitForSeconds(timeDelaySpawn*1.5f);
        killerWhale.StartingPosition();
        
        Enemy temp;

        for (int i = 2; i >= 0; i--)
        {
            temp = spawnOrca.Spawn(i);
            temp.SetSpeedMove(speed);
            AddEnemy(temp);
            yield return new WaitForSeconds(timeDelaySpawn);

        }

        yield return new WaitForSeconds(timeDelaySpawn*5f);


        killerWhale.SetTargetFollow();
       

        yield return new WaitForSeconds(timeDelaySpawn*3f);

        for (int i = 4; i >= 0; i--)
        {
            temp = spawnSkua.Spawn();
            temp.SetSpeedMove(speed);
            AddEnemy(temp);
            yield return new WaitForSeconds(timeDelaySpawn*10f);

        }

        isSpawning = false;

    }

    public void NextSpawn()
    {
        currentIndex++;
        switch (currentIndex)
        {
            case 0: 
                StartCoroutine(One());
                break;
            case 1:
                StartCoroutine(Two());
                break;
            case 2:
                StartCoroutine(Three());
                break;
            case 3:
                StartCoroutine(Four());
                break;
            case 4:
                StartCoroutine(Five());
                currentIndex = 0;
                speed += 1.5f;

                break;

        }
    }

    public IEnumerator WaveTwo(int num)
    {
        isSpawning = true;

        icebergsPool.StopSpawn();
        killerWhale.SetTargetRightBorder();

        yield return new WaitForSeconds(timeDelaySpawn*5f);
        Enemy temp;
        for (int i = num; i > 0; i--)
        {
            temp = spawnOrca.Spawn();
            temp.SetSpeedMove(speed);
            AddEnemy(temp);
            yield return new WaitForSeconds(timeDelaySpawn*5f);

        }

        isSpawning = false;
    }

    public IEnumerator One()
    {
        isSpawning = true;

        icebergsPool.StartSpawn();
        killerWhale.StartingPosition();
        killerWhale.SetTargetFollow();


        yield return new WaitForSeconds(10);
      
        isSpawning = false;
    }

    public IEnumerator Two()
    {
        isSpawning = true;
        
        spawnSkua.StartSpawn(timeDelaySpawn * 10);
        yield return new WaitForSeconds(10);
       
        isSpawning = false;
    }

    public IEnumerator Three()
    {
        isSpawning = true;
        killerWhale.SetTargetRightBorder();
        spawningFish = true;
        yield return new WaitForSeconds(5);

        spawnOrca.StartSpawn(timeDelaySpawn * 5);


        yield return new WaitForSeconds(10);
      
        isSpawning = false;
    }

    public IEnumerator Four()
    {
        isSpawning = true;

        icebergsPool.StopSpawn();
        spawnSkua.StopSpawn();
        spawnOrca.StopSpawn();

        yield return new WaitForSeconds(timeDelaySpawn * 5f);
        spawnOrca.StartSpawn(timeDelaySpawn*3f);
        yield return new WaitForSeconds(10);
        
        isSpawning = false;
    }


    public IEnumerator Five()
    {
        isSpawning = true;
        icebergsPool.StartSpawn();
        spawnOrca.StopSpawn();

        yield return new WaitForSeconds(timeDelaySpawn * 1.5f);
        killerWhale.StartingPosition();
        

        spawningFish = false;

        for (int i = 2; i >= 0; i--)
        {
            spawnOrca.Spawn(i);

            yield return new WaitForSeconds(timeDelaySpawn);

        }



        while (true)
        {
            yield return new WaitForSeconds(1);
            print(enemies.Count);
            if (enemies.Count <= 3)
            {
                break;
            }
        }
        killerWhale.SetTargetFollow();
        
        yield return new WaitForSeconds(5);

      
       
        isSpawning = false;
    }



    private void Update()
    {
        if (spawningFish)
        {
            spawnFish.Spawn(10);
        }
       
    }

    
}
