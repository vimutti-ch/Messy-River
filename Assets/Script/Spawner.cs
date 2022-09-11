using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obj;

    [Range(1, 20)] [SerializeField] private float spawnDelayMin;
    [Range(1, 20)] [SerializeField] private float spawnDelayMax;
    private float spawnDelay;

    public bool gameOver = false;

    [SerializeField] private GameObject[] spawnlog;

    private Transform[] spawnPos;
    private float[] speed;
    private bool[] direction;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = new Transform[spawnlog.Length];
        speed = new float[spawnlog.Length];
        direction = new bool[spawnlog.Length];

        for (int i = 0; i < spawnlog.Length; i++)
        {
            spawnPos[i] = spawnlog[i].GetComponent<Transform>();
            speed[i] = spawnlog[i].GetComponent<RandomSpeed>().Get();
            direction[i] = spawnlog[i].GetComponent<RandomSpeed>().Invert;
        }

        for (int i = 0; i < spawnlog.Length; i++)
        {
            spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            StartCoroutine(Spawn(spawnPos[i], spawnDelay, speed[i], direction[i]));
        }
    }

    IEnumerator Spawn(Transform pos, float spawnDelay, float speed, bool direction)
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(spawnDelay);

            Vector3 spawnPosition = new Vector3(pos.position.x, pos.position.y + 0.2f, (int)pos.position.z);
            int percent = Random.Range(0, 100);
            int length = -1;

            if (percent <= 40) length = 0;
            else if (percent > 40 && percent <= 80) length = 1;
            else if (percent > 80) length = 2;

            var objects = Instantiate(obj[length], spawnPosition, transform.rotation);
            var item = objects.GetComponent<Obstacle>();
            item.Set(speed);
            item.SetDirection(direction);
        }


        yield return null;
    }
}
