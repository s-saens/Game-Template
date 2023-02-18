using UnityEngine;
using UnityEngine.UI;

public static class VectorExt
{
    private const float INF = Mathf.Infinity;

    public static void SetLocalScale(this Transform t, float x = INF, float y = INF, float z = INF)
    {
        Vector3 v = t.localScale;
        t.localScale = new Vector3 (
            IsInfinity(x) ? v.x : x,
            IsInfinity(y) ? v.y : y,
            IsInfinity(z) ? v.z : z
        );
    }
    public static void SetPosition(this Transform t, float x = INF, float y = INF, float z = INF)
    {
        Vector3 v = t.position;
        t.position = new Vector3 (
            IsInfinity(x) ? v.x : x,
            IsInfinity(y) ? v.y : y,
            IsInfinity(z) ? v.z : z
        );
    }
    public static void SetLocalPosition(this Transform t, float x = INF, float y = INF, float z = INF)
    {
        Vector3 v = t.localPosition;
        t.localPosition = new Vector3(
            IsInfinity(x) ? v.x : x,
            IsInfinity(y) ? v.y : y,
            IsInfinity(z) ? v.z : z
        );
    }

    public static void SetColor(this Graphic img, float r = INF, float g = INF, float b = INF, float a = INF)
    {
        Color c = img.color;
        img.color = new Color(
            IsInfinity(r) ? c.r : r,
            IsInfinity(g) ? c.g : g,
            IsInfinity(b) ? c.b : b,
            IsInfinity(a) ? c.a : a
        );
    }
    public static void SetColor(this SpriteRenderer spr, float r = INF, float g = INF, float b = INF, float a = INF)
    {
        Color c = spr.color;
        spr.color = new Color(
            IsInfinity(r) ? c.r : r,
            IsInfinity(g) ? c.g : g,
            IsInfinity(b) ? c.b : b,
            IsInfinity(a) ? c.a : a
        );
    }

    static bool IsInfinity(float f)
    {
        return f == INF;
    }
}