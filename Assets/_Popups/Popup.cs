using UnityEngine;
using System;
using System.Collections;

public class Popup : MonoBehaviour
{
    [SerializeField] private bool back = true;
    [SerializeField] private bool bgClick = true;
    [SerializeField] protected bool stopTime = false;
    [SerializeField] public bool bgRaycast = true;
    [SerializeField] public float alpha = 0.9f;
    [SerializeField] public Action whenOpen;


    public bool IsOn
    {
        get;
        protected set;
    }


    protected RectTransform rect;
    private RectTransform canvasRect;

    private void Awake()
    {
        canvasRect = transform.parent.GetComponent<RectTransform>();
    }

    public void ReOpen()
    {
        _Open();
        whenOpen?.Invoke();
        StartCoroutine(TransitionReOpen().Then(AfterOpen));
    }

    public void Open()
    {
        _Open();
        whenOpen?.Invoke();
        StartCoroutine(TransitionOpen().Then(AfterOpen));
    }

    protected virtual void _Open()
    {
        rect = this.GetComponent<RectTransform>();
        GlobalEvent.backEvent.callback += Back;
        GlobalEvent.bgClickEvent.callback += OnClickBG;

        gameObject.SetActive(true);

        StopAllCoroutines();

        IsOn = true;

        WhenOpen();
        if (stopTime) Time.timeScale = 0;
    }


    public virtual void Close()
    {
        GlobalEvent.backEvent.callback -= Back;
        GlobalEvent.bgClickEvent.callback -= OnClickBG;

        StopAllCoroutines();

        if(!IsOn) return;

        IsOn = false;
        WhenClose();
        if (stopTime) Time.timeScale = 1;
        StartCoroutine(TransitionClose().Then(AfterClose).Then(() => gameObject.SetActive(false)));
    }

    private void OnClickBG()
    {
        if(bgClick) _OnClickBG();
    }
    protected virtual void _OnClickBG()
    {
        _Back();
    }
    private void Back()
    {
        if(back) _Back();
    }
    protected virtual void _Back()
    {
        PopupController.Instance.Close();
    }

    protected virtual void WhenOpen() {}
    protected virtual void AfterOpen() {}
    protected virtual void WhenClose() {}
    protected virtual void AfterClose() {}

    public virtual IEnumerator TransitionReOpen()
    {
        yield return TransitionOpen();
    }

    protected Vector2 start {
        get {
            return Vector2.up * canvasRect.sizeDelta.y;
        } 
    }

    public virtual IEnumerator TransitionOpen()
    {
        yield return 0;
    }


    public virtual IEnumerator TransitionClose()
    {
        yield return 0;
    }
}