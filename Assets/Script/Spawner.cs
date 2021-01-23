using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<EnemySpawnGroup> spawnGroups = new List<EnemySpawnGroup>();

    public Transform[] spawn;

    public static int enemiesAlive = 0;

    private float timeSinceLastSpawn;

    private const int MINENEMIES = 3, MAXENEMIES = 25, MAXTIME = 25;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        enemiesAlive = 0;

        while(App.Instance.isPlaying)
        {
            timeSinceLastSpawn += Time.deltaTime;

            //is it time to spawn a new group?
            if(enemiesAlive < MINENEMIES || timeSinceLastSpawn > MAXTIME && enemiesAlive < MAXENEMIES)
            {
                //Select a random spawn group, based on spawn chance
                float r = Random.Range(0, 100);
                EnemySpawnGroup sp = spawnGroups.Find(x => x.spawnChance > r);

                //spawn those mobs
                foreach (EnemySpawn es in sp.mobs)
                {
                    int amount = es.amount;

                    if (es.randomizeAmount)
                        amount = Random.Range(0, 10);

                    for (int i = 0; i < amount; i++)
                    {
                        Vector3 spawnPos = spawn[Random.Range(0, spawn.Length - 1)].position;
                        Instantiate(es.mob,spawnPos,Quaternion.identity);
                        yield return new WaitForSeconds(Random.Range(0f, 1.5f));
                    }
                }

                timeSinceLastSpawn = 0;
            }

            yield return null;
        }
    }

    [System.Serializable]
    public struct EnemySpawnGroup
    {
        public string name;
        public EnemySpawn[] mobs;
        [Range(0,100)]
        public int spawnChance;
    }

    [System.Serializable]
    public struct EnemySpawn
    {
        public string name;
        public GameObject mob;
        [Range(1,10)]
        public int amount;

        public bool randomizeAmount;
    }
}
