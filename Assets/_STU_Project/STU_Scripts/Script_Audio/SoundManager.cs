using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    static public SoundManager Instance = null;

    //音效
    Dictionary<Sound, SoundPack> soundBank = new Dictionary<Sound, SoundPack>();
    [SerializeField] AudioSource sfxPlayer;
    [SerializeField] List<SoundPack> soundList = new List<SoundPack>();
    //音樂
    Dictionary<Music, MusicPack> musicBank = new Dictionary<Music, MusicPack>();
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] List<MusicPack> musicList = new List<MusicPack>();

    [SerializeField] AudioMixerSnapshot maxVolume;
    [SerializeField] AudioMixerSnapshot minVolume;

    Music currentMusic = Music.None;
    bool isPreventPlayback = false;
    Coroutine prevenPlayback;

    /// <summary>用於播放需要大量細微變化的聲音(ex.腳步聲)</summary>
    List<SoundPack> collectedSounds = new List<SoundPack>();

    bool hasSubscribeSceneChange = false;

    private void Awake()
    {
        //讓SoundManager能夠跨越場景，聲音不因轉換場景而中斷，並確保只有一個實體
        if (FindObjectsOfType<SoundManager>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //初始化音效庫
        soundBank.Clear();
        foreach (SoundPack s in soundList)
        {
            soundBank.Add(s.sound, s);
        }
        //初始化音樂庫
        musicBank.Clear();
        foreach (MusicPack m in musicList)
        {
            musicBank.Add(m.music, m);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        hasSubscribeSceneChange = true;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        isPreventPlayback = false;
        if (prevenPlayback != null)
            StopCoroutine(prevenPlayback);

        MuiscTransition(newScene);

    }

    #region 音樂

    private void MuiscTransition(Scene newScene)
    {
        maxVolume.TransitionTo(0f);
        //場景的數值須依專案調整 (目前遊戲開始於場景4)
        int mainGameScene = 4;
        if (newScene.buildIndex >= mainGameScene && currentMusic != Music.MainGame)
        {
            StartCoroutine(PlayMusic(Music.MainGame));
            currentMusic = Music.MainGame;
        }
        else if (newScene.buildIndex < mainGameScene && currentMusic != Music.Menu)
        {
            StartCoroutine(PlayMusic(Music.Menu));
            currentMusic = Music.Menu;
        }

        if (newScene.name == "GameOver")
        {
            StopMusic(currentMusic);
        }
    }

    IEnumerator PlayMusic(Music music)
    {
        while (true)
        {
            //停止播放之前的音樂，並將mixer音量降至最低
            musicPlayer.Stop();
            yield return null;
            minVolume.TransitionTo(0f);
            yield return null;
            //開始播放並依參數FadeIn
            musicPlayer.clip = musicBank[music].audioClip;
            musicPlayer.volume = musicBank[music].volume;
            musicPlayer.Play();
            maxVolume.TransitionTo(musicBank[music].fadeIn);
            //等播放到FadeOut點      
            yield return new WaitUntil(() => (musicPlayer.clip.length - musicPlayer.time) <= musicBank[music].fadeOut);
            minVolume.TransitionTo(musicBank[music].fadeOut);
            yield return new WaitForSeconds(musicBank[music].fadeOut);
        }
    }

    void StopMusic(Music music)
    {
        minVolume.TransitionTo(musicBank[music].fadeOut);
    }

    #endregion


    #region 音效
    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="sound"></param>
    public void Play(Sound sound)
    {
        StartCoroutine(PlayOnce(sound,0f));
    }

    /// <summary>
    /// 播放
    /// <para>preventTime:限制該時間內不能再播放</para>
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="preventTime"></param>
    public void Play(Sound sound,float preventTime)
    {
        StartCoroutine(PlayOnce(sound,preventTime));
    }

    /// <summary>
    /// 於場景中的指定地點播放
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="position"></param>
    public void Play(Sound sound, Vector3 position = default(Vector3))
    {
        StartCoroutine(PlayInScene(sound, position));
    }


    private IEnumerator PlayOnce(Sound sound,float preventTime = 0.1f)
    {
        sfxPlayer.clip = null;
        yield return new WaitForSeconds(soundBank[sound].offset);
        if (isPreventPlayback)
            yield break;

        sfxPlayer.PlayOneShot(soundBank[sound].clip, soundBank[sound].volume);
        if(preventTime > 0)
            prevenPlayback = StartCoroutine(PreventPlaybackTime(preventTime));
    }

    private IEnumerator PlayInScene(Sound sound, Vector3 pos)
    {
        yield return new WaitForSeconds(soundBank[sound].offset);
        AudioSource.PlayClipAtPoint(soundBank[sound].clip, pos, soundBank[sound].volume);
    }


    /// <summary>
    /// 播放UI音效
    /// </summary>
    /// <param name="sound"></param>
    public void PlayWithUI(Sound sound)
    {
        sfxPlayer.clip = soundBank[sound].clip;
        sfxPlayer.Play();
    }

    private void PlayRandomizedSound(Sound sound)
    {

    }

    private void Stop(Sound sound)
    {
        sfxPlayer.Stop();
    }

    #endregion

    IEnumerator PreventPlaybackTime(float time)
    {
        isPreventPlayback = true;
        yield return new WaitForSeconds(time);
        isPreventPlayback = false;
    }

    private void OnDisable()
    {
        if (hasSubscribeSceneChange)
            SceneManager.activeSceneChanged -= OnSceneChanged;
    }

}



public enum Sound
{
    DoorPass,
    DoorPortal,
    GetStar,
    SwitchClose,
    Die,
    Jump,
    UI_Click = 100,
    UI_Return = 101,
    UI_ScrollingSelect = 102,
}

public enum Music
{
    None,
    MainGame,
    Menu
}


[System.Serializable]
public struct SoundPack
{
    public AudioClip clip;
    public Sound sound;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 3f)] public float offset;
}

[System.Serializable]
public struct MusicPack
{
    public AudioClip audioClip;
    public Music music;
    [Range(0f, 1f)] public float volume;
    public float fadeIn;
    public float fadeOut;
}