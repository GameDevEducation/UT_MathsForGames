using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_CircleManager : MonoBehaviour
{
    [SerializeField] GameObject MovingGO;
    [SerializeField] GameObject HypotenuseGO;
    [SerializeField] GameObject AdjacentGO;
    [SerializeField] GameObject OppositeGO;
    [SerializeField] GameObject CornerGO;
    [SerializeField] GameObject CornerMarkerGO;

    [SerializeField] GraphHelper OppositeValueGraph;
    [SerializeField] GraphHelper AdjacentValueGraph;

    [SerializeField] TMPro.TextMeshProUGUI ValueDisplayUI;

    [SerializeField] float AngularSpeed = 30f;

    [SerializeField] float CurrentAngle = 0f;

    [SerializeField] float OrbitDistance = 3f;

    [SerializeField] bool WrapAngles = true;

    // Start is called before the first frame update
    void Start()
    {
        HypotenuseGO.transform.localScale = new Vector3(1, OrbitDistance, 1);

        OppositeValueGraph.ClearGraph();
        AdjacentValueGraph.ClearGraph();

        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAngle += Time.deltaTime * AngularSpeed;

        if (WrapAngles && CurrentAngle >= 360f)
        {
            CurrentAngle -= 360f;

            OppositeValueGraph.ClearGraph();
            AdjacentValueGraph.ClearGraph();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Sine (aka sin): Sine (Angle) = Opposite / Hypotenuse
        // Opposite = Sine(Angle) * Hypotensuse
        float orbitX = Mathf.Sin(CurrentAngle * Mathf.Deg2Rad) * OrbitDistance;

        // Cosine (aka cos): Cosine (Angle) = Adjacent / Hypotenuse
        // Adjacent = Cosine(Angle) * Hypotenuse
        float orbitY = Mathf.Cos(CurrentAngle * Mathf.Deg2Rad) * OrbitDistance;

        MovingGO.transform.position = Vector3.up * orbitY + Vector3.right * orbitX;

        HypotenuseGO.transform.localScale = new Vector3(1, OrbitDistance, 1);
        HypotenuseGO.transform.eulerAngles = new Vector3(0, 0, -CurrentAngle);

        OppositeGO.transform.localScale = new Vector3(1, orbitY, 1);
        AdjacentGO.transform.localScale = new Vector3(1, orbitX, 1);
        CornerGO.transform.position = CornerMarkerGO.transform.position;

        ValueDisplayUI.text = $"<b>Angle          </b>: {CurrentAngle:0.##} Degrees" + System.Environment.NewLine
                            + $"<b>Angle          </b>: {(CurrentAngle * Mathf.Deg2Rad):0.##} Radians" + System.Environment.NewLine
                            + $"<b>Opposite Length</b>: {orbitY:0.##}" + System.Environment.NewLine
                            + $"<b>Adjacent Length</b>: {orbitX:0.##}";

        OppositeValueGraph.AddPoint(CurrentAngle % 360f, orbitY);
        AdjacentValueGraph.AddPoint(CurrentAngle % 360f, orbitX);
    }
}
