using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    #region - Variable Declaration (การประกาศตัวแปร) -

    public GameObject[] obj;
    public bool gameOver = false;

    [Range(1, 20), SerializeField] private float spawnDelayMin;
    [Range(1, 20), SerializeField] private float spawnDelayMax;
    
    [SerializeField] private GameObject[] spawner;

    private Transform[] _positionToSpawn;
    private float[] _speed;
    private bool[] _direction;
    
    private float _spawnDelay;

    #endregion
    
    void Start()
    {
        _positionToSpawn = new Transform[spawner.Length];
        _speed = new float[spawner.Length];
        _direction = new bool[spawner.Length];

        for (int i = 0; i < spawner.Length; i++)
        {
            _positionToSpawn[i] = spawner[i].GetComponent<Transform>();
            _speed[i] = spawner[i].GetComponent<RandomSpeed>().speed;
            _direction[i] = spawner[i].GetComponent<RandomSpeed>().invert;
        }

        for (int i = 0; i < spawner.Length; i++)
        {
            _spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            StartCoroutine(Spawn(_positionToSpawn[i], _spawnDelay, _speed[i], _direction[i], i));
        }
    }

    IEnumerator Spawn(Transform pos, float spawnDelay, float speed, bool direction, int positionInArray)
    {
        while (!gameOver)
        {
            Vector3 spawnPosition = new Vector3(pos.position.x, pos.position.y, (int)pos.position.z);

            int percent = Random.Range(0, 100);
            int length = -1;

            if (percent <= 40) length = 0;
            else if (percent > 40 && percent <= 80) length = 1;
            else if (percent > 80) length = 2;

            GameObject objects = Instantiate(obj[length], spawnPosition, transform.rotation);
            Obstacle item = objects.GetComponent<Obstacle>();
            
            objects.transform.SetParent(spawner[positionInArray].GetComponent<Transform>());
            
            item.SetSpeed(speed);
            item.SetDirection(direction);
            
            yield return new WaitForSeconds(spawnDelay);
        }


        yield return null;
    }
}
