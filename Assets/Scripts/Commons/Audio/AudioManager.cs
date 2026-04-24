using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// サウンド用Flg
/// </summary>
public enum SoundFlg
{
    /// <summary>
    /// サウンドを鳴らす
    /// </summary>
    ON,

    /// <summary>
    /// サウンドを鳴らさない
    /// </summary>
    OFF,
}

/// <summary>
/// Audio Manager Class
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    //--------------------------------------------
    // 定数
    //--------------------------------------------

    /// <summary>
    /// デフォルトのBgm音量
    /// </summary>
    private const float DEFALUT_BGM_VOLUME = 0.5f;

    /// <summary>
    /// デフォルトのSe音量
    /// </summary>
    private const float DEFALUT_SE_VOLUME = 0.5f;

    //--------------------------------------------
    // 設定
    //--------------------------------------------

    /// <summary>
    /// 現在再生中のBGMキー
    /// </summary>
    public AudioNames CurrentBgMKey { get; private set; }
    
    /// <summary>
    /// 現在再生中のBGMの音量
    /// </summary>
    private float m_CurrentBGMVolume = 0;

    /// <summary>
    /// Bgmの音量の倍率
    /// この値を調整してBgmの音量調整をしている
    /// </summary>
    private float m_BgmVolumeMag = 0.2f;

    /// <summary>
    /// Seの音量の倍率
    /// この値を調整してSeの音量調整をしている
    /// </summary>
    private float m_SeVolumeMag = 0.2f;

    /// <summary>
    /// BGMが再生できるかどうか
    /// </summary>
    private SoundFlg m_PlayBgmFlg = SoundFlg.ON;

    /// <summary>
    /// SEが再生できるかどうか
    /// </summary>
    private SoundFlg m_PlaySeFlg = SoundFlg.ON;

    //--------------------------------------------
    // データ
    //--------------------------------------------

    /// <summary>
    /// BGM音源
    /// </summary>
    [SerializeField]
    private AudioClipSetObject m_BgmSet;

    /// <summary>
    /// SE音源
    /// </summary>
    [SerializeField]
    private AudioClipSetObject m_SeSet;

    /// <summary>
    /// BGM一覧
    /// </summary>
    private Dictionary<string, AudioClip> m_BgmList = new Dictionary<string, AudioClip>();

    /// <summary>
    /// SE一覧
    /// </summary>
    private Dictionary<string, AudioClip> m_SeList = new Dictionary<string, AudioClip>();

    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    /// <summary>
    /// BGM再生オブジェクト
    /// </summary>
    [SerializeField]
    private AudioSource m_BgmSource;

    /// <summary>
    /// SE再生オブジェクト
    /// </summary>
    [SerializeField]
    private AudioSource m_SeSource;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    /// <summary>
    /// Awake
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        CurrentBgMKey = AudioNames.None;

        // BGM一覧
        for (int i = 0; i < m_BgmSet.Audios.Count; i++)
        {
            m_BgmList.Add(m_BgmSet.Audios[i].Key.ToString(), m_BgmSet.Audios[i].Clip);
        }

        // SE一覧
        for (int i = 0; i < m_SeSet.Audios.Count; i++)
        {
            m_SeList.Add(m_SeSet.Audios[i].Key.ToString(), m_SeSet.Audios[i].Clip);
        }
    }

    //--------------------------------------------
    // 再生
    //--------------------------------------------

    /// <summary>
    /// 指定したBGMを再生する
    /// </summary>
    /// <param name="audioName">再生するBGMの名前</param>
    /// <param name="volume">音量</param>
    /// <param name="isLoop">BGMをループするか？</param>
    public void PlayBgm(AudioNames audioName, float volume = DEFALUT_BGM_VOLUME, bool isLoop = true)
    {
        // ループを設定
        m_BgmSource.loop = isLoop;

        // 現在のBGMキーを設定
        CurrentBgMKey = audioName;

        // 現在のBGMの音量設定
        m_CurrentBGMVolume = volume;

        // 再生不可の場合再生しない
        if (m_PlayBgmFlg == SoundFlg.OFF || CurrentBgMKey == AudioNames.None)
        {
            return;
        }

        string keyName = CurrentBgMKey.ToString();
        if (!m_BgmList.ContainsKey(keyName))
        {
            Logging.Debug($"{keyName}はありません");
            return;
        }

        // 再生中のBGMと指定されたBGMが異なる場合、BGMを変更する
        if (m_BgmSource.clip != m_BgmList[keyName])
        {
            m_BgmSource.volume = m_CurrentBGMVolume * m_BgmVolumeMag;
            m_BgmSource.time = 0.0f;
            m_BgmSource.Stop();
            m_BgmSource.clip = m_BgmList[audioName.ToString()];

            m_BgmSource.Play();
        }
    }

    /// <summary>
    /// 指定したSEを再生する
    /// </summary>
    /// <param name="audioName">再生するSEの名前</param>
    /// <param name="volume">音量</param>
    public void PlaySe(AudioNames audioName, float volume = DEFALUT_SE_VOLUME)
    {
        // 再生不可の場合再生しない
        if (m_PlaySeFlg == SoundFlg.OFF) { return; }

        string keyName = audioName.ToString();
        if (!m_SeList.ContainsKey(keyName))
        {
            Logging.Debug($"{keyName}はありません");
            return;
        }

        m_SeSource.volume = volume * m_SeVolumeMag;
        m_SeSource.PlayOneShot(m_SeList[keyName]);
    }

    //--------------------------------------------
    // 停止
    //--------------------------------------------

    /// <summary>
    /// 再生中のSEを停止する
    /// </summary>
    public void StopSe()
    {
        m_SeSource.Stop();
        m_SeSource.clip = null;
    }

    /// <summary>
    /// 再生中のBGMを停止する
    /// </summary>
    public void StopBgm()
    {
        m_BgmSource.Stop();
        m_BgmSource.clip = null;
    }

    /// <summary>
    /// 再生中のBGMを一時停止する
    /// </summary>
    public void PauseBgm()
    {
        m_BgmSource.Stop();
    }

    //--------------------------------------------
    // 再生設定
    //--------------------------------------------

    /// <summary>
    /// BGM再生の可否を設定する
    /// </summary>
    /// <param name="flag"></param>
    public void SetBgmFlg(SoundFlg flag)
    {
        m_PlayBgmFlg = flag;

        // 再生中のBGMを停止する
        if (m_PlayBgmFlg == SoundFlg.OFF)
        {
            StopBgm();
        }
    }

    /// <summary>
    /// SE再生の可否を設定する
    /// </summary>
    /// <param name="flag"></param>
    public void SetSeFlg(SoundFlg flag)
    {
        m_PlaySeFlg = flag;
    }

    /// <summary>
    /// BGM再生可否を取得する
    /// </summary>
    /// <returns></returns>
    public SoundFlg GetBgmFlg()
    {
        return m_PlayBgmFlg;
    }

    /// <summary>
    /// SE再生可否を取得する
    /// </summary>
    /// <returns></returns>
    public SoundFlg GetSeFlg()
    {
        return m_PlaySeFlg;
    }

    /// <summary>
    /// BGMが再生中か？
    /// </summary>
    /// <returns></returns>
    public bool IsPlayingBgm()
    {
        return m_BgmSource.isPlaying;
    }

    //--------------------------------------------
    // 音量
    //--------------------------------------------

    /// <summary>
    /// BGM音量の倍率を設定
    /// </summary>
    /// <param name="value"></param>
    public void SetBgmVolumeMag(float value)
    {
        m_BgmVolumeMag = value;
        m_BgmSource.volume = m_CurrentBGMVolume * m_BgmVolumeMag;
    }

    /// <summary>
    /// SE音量の倍率を設定
    /// </summary>
    /// <param name="value">倍率の値</param>
    public void SetSeVolumeMag(float value)
    {
        m_SeVolumeMag = value;
    }

    /// <summary>
    /// BGM音量の倍率を取得する
    /// </summary>
    /// <returns></returns>
    public float GetBgmVolumeMag()
    {
        return m_BgmVolumeMag;
    }

    /// <summary>
    /// SE音量の倍率を取得する
    /// </summary>
    /// <returns></returns>
    public float GetSeVolumeMag()
    {
        return m_SeVolumeMag;
    }
}
