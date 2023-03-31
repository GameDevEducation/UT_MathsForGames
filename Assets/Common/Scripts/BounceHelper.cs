using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceHelper : MonoBehaviour
{
    [SerializeField] GameObject BounceMeshGO;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBounce();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBounce(Time.deltaTime);
    }

    void UpdateBounce(float deltaTime = 0f)
    {
    }
}
