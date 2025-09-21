using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static string spawnPoint;
    void Start()
    {
        if (!string.IsNullOrEmpty(spawnPoint))
        {
            GameObject point = GameObject.Find(spawnPoint);
            if (point != null)
            {
                transform.position = point.transform.position;
            }
        }
    }
}
