using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;//¶¬‚µ‚½‚¢“G
    public float spawnInterval = 2f;//‰½•b‚²‚Æ‚Éo‚·‚©
    public EnemySpawnenPosition spawnPosition;
    float nowTime = 0;//ŽžŠÔŒo‰ß‚Ì‹L˜^

    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= spawnInterval)
        {
            Vector3 pos = spawnPosition.GetSpawnPosition();
            Instantiate(enemyPrefab, pos, Quaternion.identity);
//          Instantiate(enemyPrefab, transform.position,Quaternion.identity );
            nowTime = 0;
        }
    }
}
