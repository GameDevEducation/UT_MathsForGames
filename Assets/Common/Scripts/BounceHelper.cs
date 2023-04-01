using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceHelper : MonoBehaviour
{
    [SerializeField] GameObject BounceMeshGO;

    [SerializeField] float BounceFrequency = 0.5f;
    [SerializeField] float BounceHeight = 1f;

    float CurrentTime = 0f;
    float Phase = 0f;

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
        CurrentTime += deltaTime;

        float angle = CurrentTime * 2f * Mathf.PI * BounceFrequency;
        float heightOffset = BounceHeight * Mathf.Sin(angle + Phase);

        BounceMeshGO.transform.localPosition = Vector3.up * heightOffset;
    }

    public void SetPhase(float newPhase)
    {
        Phase = newPhase;
        CurrentTime = 0f;
    }
}
