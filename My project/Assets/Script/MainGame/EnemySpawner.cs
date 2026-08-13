using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;//¶¬‚µ‚½‚¢“G
    public float spawnInterval = 2f;//‰½•b‚²‚Æ‚Éo‚·‚©
    public EnemySpawnenPosition spawnPosition;
    float nowTime = 0;//ŽžŠÔŒo‰ß‚Ì‹L˜^
    public bool isSpawn = true;//OnOff
    public float spawnTime = 10f;// ‰½•bŒã‚É¶¬‚ðŽ~‚ß‚é‚©
    float spawnElapsedTime = 0f;// ¶¬‚ðŠJŽn‚µ‚Ä‚©‚ç‚ÌŒo‰ßŽžŠÔ

    private void Update()
    {
        if (isSpawn == false)
        {
            return;
        }

        spawnElapsedTime += Time.deltaTime;
        if (spawnElapsedTime >= spawnTime)
        {
            isSpawn = false;
            return;
        }

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
