using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private float   visionRange   = 5.0f;
    [SerializeField] private float   visionAngle   = 60.0f;
    [SerializeField] private int     rayCount      = 20;
    [SerializeField] private Color   normalColor   = new Color(1f, 1f, 0f, 0.3f);
    [SerializeField] private Color   alertColor    = new Color(1f, 0f, 0f, 0.5f);

    private Mesh         mesh;
    private MeshFilter   meshFilter;
    private MeshRenderer meshRenderer;
    private Enemy        enemy;

    void Awake()
    {
        meshFilter            = GetComponent<MeshFilter>();
        meshRenderer          = GetComponent<MeshRenderer>();
        mesh                  = new Mesh();
        meshFilter.mesh       = mesh;

        enemy = GetComponentInParent<Enemy>();
    }

    void LateUpdate()
    {
        DrawCone();
        UpdateColor();
    }

    void DrawCone()
    {
        Vector3[] vertices  = new Vector3[rayCount + 2];
        int[]     triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        float angleStep  = (visionAngle * 2) / rayCount;
        float startAngle = -visionAngle;

        for (int i = 0; i <= rayCount; i++)
        {
            float   angle   = startAngle + angleStep * i;
            Vector3 dir     = Quaternion.Euler(0, 0, angle) * Vector3.right;
            vertices[i + 1] = dir * visionRange;
        }

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3]     = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices  = vertices;
        mesh.triangles = triangles;
    }

    void UpdateColor()
    {
        if (enemy == null) return;

        bool isAlert = enemy.GetState() == Enemy.State.Frozen || enemy.GetState() == Enemy.State.Flee;
        meshRenderer.material.color = isAlert ? alertColor : normalColor;
    }
}
