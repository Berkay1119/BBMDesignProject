using System.Collections.Generic;
using UnityEngine;

namespace Backend.Managers
{
    public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int spawnCount = 10;

    [Tooltip("List of Transforms that define the spawn polygon")]
    public List<Transform> regionPoints = new List<Transform>(4);
    public Color polygonColor = Color.cyan;

    public void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 localPoint = GetRandomPointInPolygon();
            Vector2 worldPoint = transform.TransformPoint(localPoint);
            Instantiate(objectToSpawn, worldPoint, Quaternion.identity);
        }
    }

    private Vector2 GetRandomPointInPolygon()
    {
        List<Vector2> polygon = new List<Vector2>();
        foreach (var t in regionPoints)
        {
            if (t != null)
                polygon.Add(t.position);
        }

        var bounds = GetPolygonBounds(polygon);

        for (int i = 0; i < 100; i++) // Try up to 100 times
        {
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (IsPointInPolygon(point, polygon) && !Physics2D.OverlapPoint(point))
                return point;
        }

        Debug.LogWarning("No valid spawn point found without colliders.");
        return bounds.center;
    }

    private Bounds GetPolygonBounds(List<Vector2> points)
    {
        if (points.Count == 0) return new Bounds();

        Vector2 min = points[0];
        Vector2 max = points[0];
        foreach (var p in points)
        {
            min = Vector2.Min(min, p);
            max = Vector2.Max(max, p);
        }

        return new Bounds((min + max) / 2, max - min);
    }

    private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        bool inside = false;
        for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
        {
            if ((polygon[i].y > point.y) != (polygon[j].y > point.y) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / 
                (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    private void OnDrawGizmos()
    {
        if (regionPoints == null || regionPoints.Count < 2) return;

        Gizmos.color = polygonColor;

        for (int i = 0; i < regionPoints.Count; i++)
        {
            var current = regionPoints[i];
            var next = regionPoints[(i + 1) % regionPoints.Count];

            if (current != null && next != null)
            {
                Gizmos.DrawLine(current.position, next.position);
            }
        }
    }
}
}