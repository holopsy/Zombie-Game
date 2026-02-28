using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int points = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Assumption: zombies have tag "Player" (set it!)
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(points);
            Destroy(gameObject);
        }
    }
}