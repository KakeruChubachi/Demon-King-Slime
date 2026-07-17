using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;//¶¬‚µ‚½‚¢“G
    public float spawnInterval = 2f;//‰½•b‚²‚Æ‚Éo‚·‚©
    float nowTime = 0;//ŽžŠÔŒo‰ß‚Ì‹L˜^

    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= spawnInterval)
        {
            Instantiate(enemyPrefab, transform.position,Quaternion.identity );
            nowTime = 0;
        }
    }
}
