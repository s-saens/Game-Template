using UnityEngine;
using System;
using System.Collections;

public static class STween
{
    public static IEnumerator WaitForFrames(int frames = 1)
    {
        for(int i=0 ; i<frames ; ++i) yield return 0;
    }
    public static IEnumerator WaitForFixedFrames(int frames = 1)
    {
        for (int i = 0; i < frames; ++i) yield return new WaitForFixedUpdate();
    }
    
    public static IEnumerator To_Lerp(this float value, float to, float percentage, Action<float> function, bool ignorePause = false)
    {
        while(MathF.Abs(value - to) > 0.01f)
        {
            float deltaTime = ignorePause ? Time.unscaledDeltaTime * 60 : Time.deltaTime * 60;

            value = Mathf.Lerp(value, to, percentage * deltaTime);
            function.Invoke(value);
            yield return 0;
        }
        value = to;
        function.Invoke(value);
    }

    public static IEnumerator To_Linear(this float value, float to, float speed, Action<float> function, bool ignorePause = false)
    {
        int direction = MathF.Sign(to - value);
        while (MathF.Abs(value - to) > 0)
        {
            float deltaTime = ignorePause ? Time.unscaledDeltaTime * 60 : Time.deltaTime * 60;

            value += speed * deltaTime * direction;
            if(value * direction > to) value = to;
            function.Invoke(value);
            yield return 0;
        }
        value = to;
        function.Invoke(value);
    }

    public static IEnumerator To_Lerp(this Vector2 value, Vector2 to, float percentage, Action<Vector2> function, bool ignorePause = false)
    {
        while ((value - to).magnitude > 0.01f)
        {
            float deltaTime = ignorePause ? Time.unscaledDeltaTime * 60 : Time.deltaTime * 60;

            value = Vector2.Lerp(value, to, percentage * deltaTime);
            function.Invoke(value);
            yield return 0;
        }
        value = to;
        function.Invoke(value);
    }

    public static IEnumerator To_Lerp(this Vector3 value, Vector3 to, float percentage, Action<Vector3> function, bool ignorePause = false)
    {
        while ((value - to).magnitude > 0.01f)
        {
            float deltaTime = ignorePause ? Time.unscaledDeltaTime * 60 : Time.deltaTime * 60;

            value = Vector3.Lerp(value, to, percentage * deltaTime);
            function.Invoke(value);
            yield return 0;
        }
        value = to;
        function.Invoke(value);
    }
    public static IEnumerator To_Linear(this Vector3 value, Vector3 to, float percentage, Action<Vector3> function, bool ignorePause = false)
    {
        Vector3 direction = (to - value).normalized;
        while ((value - to).magnitude > 0)
        {
            float deltaTime = ignorePause ? Time.unscaledDeltaTime * 60 : Time.deltaTime * 60;

            value = value + direction * deltaTime;
            function.Invoke(value);
            yield return 0;
        }
        value = to;
        function.Invoke(value);
    }

    public static IEnumerator To_Lerp(this Vector4 value, Vector4 to, float percentage, Action<Vector4> function, bool ignorePause = false)
    {
        while ((value - to).magnitude > 0.01f)
        {
            float deltaTime = ignorePause ? Time.unscaledDeltaTime * 60 : Time.deltaTime * 60;

            value = Vector4.Lerp(value, to, percentage * deltaTime);
            function.Invoke(value);
            yield return 0;
        }
        value = to;
        function.Invoke(value);
    }

    public static IEnumerator Wait(float time, bool ignorePause = false)
    {
        if(ignorePause) yield return new WaitForSecondsRealtime(time);
        else yield return new WaitForSeconds(time);
        yield return 0;
    }
}