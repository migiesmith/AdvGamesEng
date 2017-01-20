using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRing : MonoBehaviour
{

    public int numPoints = 90;
    public float radius = 1.0f;
    public float thickness = 0.1f;

    public Material material;

    private GameObject ringGO;
    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start() {

        ringGO = new GameObject("Ring");
        ringGO.transform.parent = this.transform;
        ringGO.transform.position = new Vector3(0.0f, 1.0f, 0.0f);

        lineRenderer = ringGO.AddComponent<LineRenderer>();

        lineRenderer.material = material;
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
        lineRenderer.numPositions = numPoints + 1;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.useWorldSpace = false;

        lineRenderer.numCapVertices = 2;

        float step = 2 * Mathf.PI / numPoints;

        for (int i = 0; i < numPoints; i++) {
            float theta = i * step;
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);

            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
        lineRenderer.SetPosition(numPoints, lineRenderer.GetPosition(0));

    }

    // Update is called once per frame
    void Update() {

    }
}
