using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [Header("Shape")]
    [SerializeField] private float visionRange = 3.5f;
    [SerializeField] private float visionAngle = 47.0f;

    [Header("Pixel Art Style")]
    [SerializeField] private int rayCount = 6;
    [SerializeField] private float pixelSnapSize = 0.25f;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(1f, 0.9f, 0f, 0.25f);
    [SerializeField] private Color frozenColor = new Color(1f, 0.5f, 0f, 0.5f);
    [SerializeField] private Color fleeColor   = new Color(1f, 0.1f, 0.1f, 0.6f);

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Enemy enemy;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        enemy = GetComponentInParent<Enemy>();
    }

    void LateUpdate()
    {
        DrawCone();
        UpdateColor();
    }

    void DrawCone()
    {
        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        float angleStep = (visionAngle * 2f) / rayCount;
        float startAngle = -visionAngle;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Vector3 tip = dir * visionRange;

            tip.x = Mathf.Round(tip.x / pixelSnapSize) * pixelSnapSize;
            tip.y = Mathf.Round(tip.y / pixelSnapSize) * pixelSnapSize;

            vertices[i + 1] = tip;
        }

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3]     = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void UpdateColor()
    {
        if (enemy == null) return;

        Color target = normalColor;

        switch (enemy.GetState())
        {
            case Enemy.State.Frozen: target = frozenColor; break;
            case Enemy.State.Flee:   target = fleeColor;   break;
        }

        meshRenderer.material.color = Color.Lerp(
            meshRenderer.material.color, target, Time.deltaTime * 8f
        );
    }
}
