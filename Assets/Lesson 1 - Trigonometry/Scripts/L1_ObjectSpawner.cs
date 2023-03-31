using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_ObjectSpawner : MonoBehaviour
{
    enum ESpawnMode
    {
        FixedNumber,
        ApproximateSpacing
    }

    [SerializeField] GameObject PrefabToSpawn;
    [SerializeField] ESpawnMode SpawnMode = ESpawnMode.FixedNumber;
    [SerializeField] bool AlwaysUpdate = true;

    [Header("Fixed Number")]
    [SerializeField] int NumToSpawn = 16;

    [Header("Approximate Spacing")]
    [SerializeField] float ApproximateSpacing = 1.0f;

    List<GameObject> SpawnedObjects = new();

    // Start is called before the first frame update
    void Start()
    {
        RefreshSpawnedObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (AlwaysUpdate)
            RefreshSpawnedObjects();
    }

    void RefreshSpawnedObjects()
    {
        int expectedNumObjects = 0;

        // first  - figure out how many we expect to have

        bool objectListChanged = expectedNumObjects != SpawnedObjects.Count;

        // second - remove extras
        while (SpawnedObjects.Count > expectedNumObjects)
        {
            var spawnedObject = SpawnedObjects[^1];

            SpawnedObjects.RemoveAt(SpawnedObjects.Count - 1);

            Destroy(spawnedObject);
        }

        // third - spawn missing ones
        while (SpawnedObjects.Count < expectedNumObjects)
        {
            var spawnedObject = GameObject.Instantiate(PrefabToSpawn, transform);
            SpawnedObjects.Add(spawnedObject);
        }
    }
}
