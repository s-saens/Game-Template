using UnityEngine;
using UnityEngine.UI;
using UITween;

public class BackGround : MonoBehaviour
{
    public const float defaultAlpha = 0.9f;

    private RawImage image;

    private void Awake()
    {
        image = this.GetComponent<RawImage>();
        gameObject.SetActive(true);
        image.SetColor(a:0);
    }

    public void Set(float to = defaultAlpha, bool raycast = true)
    {
        StopAllCoroutines();
        
        image.raycastTarget = raycast;

        StartCoroutine(0f.To_Lerp(to, 0.2f, (v) => image.SetColor(a:v)));
    }

    public void Off()
    {
        StopAllCoroutines();

        image.raycastTarget = false;
        
        StartCoroutine(image.color.a.To_Lerp(0, 0.2f, (v) => image.SetColor(a:v)));
    }
}