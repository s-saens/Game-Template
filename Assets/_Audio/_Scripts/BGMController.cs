using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMController : SingletonMono<BGMController>
{
    AudioSource[] bgmSources;
    int nowSourceIndex = 0;
    public AudioClip[] bgmClips;
    public float bgmVolume = 1;
    private AudioClip lastClip = null;

    private int nextSourceIndex {
        get {
            return (nowSourceIndex + 1) % 2;
        }
    }


    private void OnEnable()
    {
        bgmSources = GetComponentsInChildren<AudioSource>();
        StartCoroutine(BgmOn(nowSourceIndex));
        SceneManager.sceneLoaded += SetBGM;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        SceneManager.sceneLoaded -= SetBGM;
    }

    private void SetBGM(Scene scene, LoadSceneMode mode)
    {

        int nowSceneIndex = scene.buildIndex;
        AudioClip nowClip = bgmClips[nowSceneIndex];

        if(lastClip == nowClip) return;
        lastClip = nowClip;

        int lastSourceIndex = nowSourceIndex;
        nowSourceIndex = nextSourceIndex;

        StopAllCoroutines();
        StartCoroutine(BgmOff(lastSourceIndex));

        if(bgmClips[nowSceneIndex] == null)
        {
            Debug.Log("No bgm was set of index " + nowSceneIndex);
            return;
        }

        bgmSources[nowSourceIndex].clip = nowClip;
        StartCoroutine(BgmOn(nowSourceIndex));
    }

    IEnumerator BgmOn(int sourceIndex)
    {
        AudioSource s = bgmSources[sourceIndex];
        s.Play();
        yield return STween.To_Linear(0, bgmVolume, 0.01f, (v)=> s.volume = v, true);
    }
    IEnumerator BgmOff(int sourceIndex)
    {
        AudioSource s = bgmSources[sourceIndex];
        yield return STween.To_Linear(s.volume, 0, 0.01f, (v) => s.volume = v, true);
        s.Stop();
    }
}