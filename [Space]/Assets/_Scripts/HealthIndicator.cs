using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    private PieChartMesh pieChart;
    public float delay = 0.1f;
    public Material mainMaterial;
    private Material[] materials;
    private const int segments = 2;
    public Color healthColor;
    public Color backgroundColor;

    public float maxHealthValue = 100.0f;
    public float healthValue = 90.0f;

    void Start()
    {
        materials = new Material[segments];

        materials[0] = new Material(mainMaterial);
        materials[0].color = backgroundColor;

        materials[1] = new Material(mainMaterial);
        materials[1].color = healthColor;
        if(mainMaterial.HasProperty("_EmissionColor"))
        {
            materials[0].SetColor("_EmissionColor", backgroundColor);
            materials[1].SetColor("_EmissionColor", healthColor);
        }
        if (pieChart == null)
            pieChart = gameObject.AddComponent<PieChartMesh>();
        if (pieChart != null)
        {
            float[] pieData = new float[] { maxHealthValue - healthValue, healthValue };
            pieChart.Init(pieData, 100, 0, 100, materials, delay);
            pieChart.Draw(pieData);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("a"))
            updateHealth(this.healthValue * 0.9f);
    }


    public void updateHealth(float hp)
    {
        this.healthValue = Mathf.Min(hp, this.maxHealthValue);

        float[] pieData = new float[] { maxHealthValue - healthValue, healthValue };
        pieChart.delay = this.delay;
        pieChart.Draw(pieData);
    }

    public void updateHealth(float maxHP, float hp)
    {
        if (pieChart == null)
            pieChart = gameObject.AddComponent<PieChartMesh>();
        this.maxHealthValue = maxHP;
        updateHealth(hp);
    }

}
