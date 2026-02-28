using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform floorTransform;        // drag your Floor here
    public Collider floorCollider;          // drag Floor's collider here
    public GameObject collectiblePrefab;    // your prefab

    [Header("Timing")]
    public float spawnInterval = 2.0f;

    [Header("Placement")]
    public float spawnHeightAboveFloor = 0.25f; // how high above board
    public float edgePadding = 0.5f;            // keep away from edges

    float t;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        t += Time.deltaTime;
        if (t >= spawnInterval)
        {
            t = 0f;
            SpawnOnFloor();
        }
    }

    void SpawnOnFloor()
    {
        if (floorTransform == null || floorCollider == null || collectiblePrefab == null) return;

        Bounds b = floorCollider.bounds;

        // Pick random point inside the collider bounds (with padding)
        float x = Random.Range(b.min.x + edgePadding, b.max.x - edgePadding);
        float z = Random.Range(b.min.z + edgePadding, b.max.z - edgePadding);

        // Raycast DOWN to find the actual floor surface (works even if tilted)
        Vector3 rayStart = new Vector3(x, b.max.y + 5f, z);
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 50f))
        {
            // Make sure we hit the floor collider
            if (hit.collider == floorCollider)
            {
                GameObject c = Instantiate(collectiblePrefab);

                Renderer r = c.GetComponent<Renderer>();

                float objectHeight = 0.5f;
                if (r != null)
                {
                    objectHeight = r.bounds.extents.y; // half height of the object
                }

                Vector3 spawnPos = hit.point + hit.normal * (objectHeight + spawnHeightAboveFloor);

                c.transform.position = spawnPos;
                c.transform.SetParent(floorTransform, true);

                // Optional: orient it to floor normal (nice if tilted)
                c.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
}