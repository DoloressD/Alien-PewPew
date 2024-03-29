using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning,
        Waiting,
        Timer
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float spawnRate;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    private int waveIndex = 0;
    public float timeBetweenWaves = 5f;
    public float waveTimer;

    private float checkTimer;

    private SpawnState state = SpawnState.Timer;

    private string[] waveCompletedWords = { "Nice!", "Wow!", "Not Done Yet...", "Spawning...", "Wave Completed!" };

    private void Start()
    {
        waveTimer = timeBetweenWaves;
    }

    private void Update()
    {
        if(state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveDone();
            }
            else return;
        }

        if(waveTimer <= 0)
        {
            if(state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[waveIndex]));
            }

        }
        else
        {
            waveTimer -= Time.deltaTime;
        }
    }

    bool EnemyIsAlive()
    {
        checkTimer -= Time.deltaTime;

        if (checkTimer <= 0)
        {
            checkTimer = 1;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        TextPopup.CreateTitlePopup(_wave.name);

        state = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        var randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(_enemy, spawnPoints[randomIndex].position, transform.rotation);
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }

    void WaveDone()
    {
        var randomIndex = Random.Range(0, waveCompletedWords.Length);
        TextPopup.CreateTitlePopup(waveCompletedWords[randomIndex]);

        state = SpawnState.Timer;
        waveTimer = timeBetweenWaves;

        if(waveIndex + 1 >  waves.Length - 1)
        {
            waveIndex = 0;
            Debug.Log("All Waves Completed, going back to first wave.");
        } else
            waveIndex++;
    }
}
