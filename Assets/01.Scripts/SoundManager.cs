using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SoundManager : MMSingleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;         // BGM 전용 AudioSource (씬에 1개만 필요)
    [SerializeField] private float bgmFadeDuration = 1f;     // BGM 전환 시 페이드 시간
    [SerializeField] private BgmData bgmData;                // 상황별 BGM 목록 데이터
    private BgmType currentBgmType;                          // 현재 재생 중인 BGM 타입 (중복 재생 방지용)


    [SerializeField] private AudioSource sfxSourcePrefab;    // SFX 재생용 프리팹 (AudioSource만 붙어있는 오브젝트)
    [SerializeField] private int sfxPoolSize = 10;           // 동시에 재생 가능한 SFX 개수
    private List<AudioSource> sfxPool = new List<AudioSource>();

    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    protected override void Awake()
    {
        base.Awake();
        InitSfxPool();
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    // SFX 풀 초기화
    private void InitSfxPool() 
    {
        for (int i = 0; i < sfxPoolSize; i++) 
        {
            AudioSource source = Instantiate(sfxSourcePrefab, transform);
            source.playOnAwake = false;
            sfxPool.Add(source);
        }
    }

    // 풀에서 재생 중이 아닌 오디오소스 하나 찾기(없으면 가장 오래된 걸 사용)
    private AudioSource GetAvailableSfxSource() 
    {
        foreach (var source in sfxPool) 
        {
            if (!source.isPlaying) return source;
        }
        return sfxPool[0];
    }

    // BGM(타입으로 재생)
    public void  PlayBGM(BgmType type, bool loop = true) 
    {
        if (currentBgmType == type && bgmSource.isPlaying) return;

        BgmEntry entry = bgmData.bgmList.Find(b => b.type == type);
        if (entry == null || entry.clip == null)
        {
            return;
        }

        currentBgmType = type;
        PlayBGM(entry.clip, loop);

    }

    //BGM (클립 직접 지정)
    public void PlayBGM(AudioClip clip, bool loop = true) 
    {
        if (clip == null) return;
        StopAllCoroutines();
        StartCoroutine(FadeBGM(clip, loop));
    }

    public void StopBGM() 
    {
        bgmSource.Stop();
    }

    private IEnumerator FadeBGM(AudioClip newclip, bool loop = true) 
    {
        //기본 곡 페이드 아웃
        float startVolume = bgmSource.volume;
        while (bgmSource.volume > 0f) 
        {
            bgmSource.volume -= startVolume * Time.deltaTime / bgmFadeDuration;
            yield return null;
        }

        //클립 교체
        bgmSource.clip = newclip;
        bgmSource.loop = loop;
        bgmSource.Play();

        //새 곡 페이드 인
        while (bgmSource.volume < bgmVolume) 
        {
            bgmSource.volume += bgmVolume * Time.deltaTime / bgmFadeDuration;
            yield return null;
        }
        bgmSource.volume = bgmVolume;
    }

    //SFX
    public void PlaySFX(AudioClip clip) 
    {
        if (clip == null) return;
        AudioSource source = GetAvailableSfxSource();
        source.clip = clip;
        source.volume = sfxVolume;
        source.Play();
    }

    // 볼륨 조절 UI
    public void SetBgmVolume(float volume) 
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
        SaveVolumeSettings();
    }

    public void SetSfxVolume(float volume) 
    {
        sfxVolume = Mathf.Clamp01(volume);
        SaveVolumeSettings();
    }

    //볼륨 저장/로드
    private void SaveVolumeSettings() 
    {
        SaveManager.Instance.CurrentData.bgmVolume = bgmVolume;
        SaveManager.Instance.CurrentData.sfxVolume = sfxVolume;
    }

    private void LoadVolumeSettings() 
    {
        bgmVolume = SaveManager.Instance.CurrentData.bgmVolume;
        sfxVolume = SaveManager.Instance.CurrentData.sfxVolume;
        bgmSource.volume = bgmVolume;
    }

}
