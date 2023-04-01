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

    [SerializeField] float SpawnDistance = 5.0f;

    [SerializeField] float PhaseMultiplier = 1.0f;
    float CachedPhaseMultiplier = 0f;

    [SerializeField] float DistanceVariation = 0.5f;
    [SerializeField] float DistanceVariationFrequency = 0f;

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
        if (SpawnMode == ESpawnMode.FixedNumber)
            expectedNumObjects = NumToSpawn;
        else
        {
            float circumference = 2f * SpawnDistance * Mathf.PI;
            expectedNumObjects = Mathf.RoundToInt(circumference / ApproximateSpacing);
        }

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

        // position all the spawned objects
        float angleIncrement = 2f * Mathf.PI / SpawnedObjects.Count;
        for (int index = 0; index < SpawnedObjects.Count; index++)
        {
            float angleToSpawnAt = index * angleIncrement;
            float distanceToSpawnAt = SpawnDistance;
            distanceToSpawnAt += DistanceVariation * Mathf.Sin(angleToSpawnAt * DistanceVariationFrequency);

            Vector3 position = new Vector3(Mathf.Sin(angleToSpawnAt) * distanceToSpawnAt,
                                           0f,
                                           Mathf.Cos(angleToSpawnAt) * distanceToSpawnAt);

            SpawnedObjects[index].transform.localPosition = position;

            if (objectListChanged || (PhaseMultiplier != CachedPhaseMultiplier))
                SpawnedObjects[index].GetComponent<BounceHelper>().SetPhase(angleToSpawnAt * PhaseMultiplier);
        }

        CachedPhaseMultiplier = PhaseMultiplier;
    }
}
