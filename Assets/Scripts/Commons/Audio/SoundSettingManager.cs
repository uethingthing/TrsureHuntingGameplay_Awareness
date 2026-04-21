using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 音設定UI
/// </summary>
public class SoundSettingManager : MonoBehaviour
{
    //--------------------------------------------
    // コンポーネント
    //--------------------------------------------

    [SerializeField, Header("音設定ボタン")]
    private Button m_soundSettingButton;

    [SerializeField, Header("音設定UI")]
    private GameObject m_soundSettingUi;

    [SerializeField, Header("Bgm音量設定用ボタン"), Tooltip("Bgm On/Offを設定出来る")]
    private Button m_bgmVolumeButton;

    /// <summary>
    /// Bgm音量設定用ボタンの画像
    /// </summary>
    private Image m_bgmVolumeImage;

    [SerializeField, Header("Bgm音量調整用スライダー")]
    private Slider m_bgmVolumeSlider;

    [SerializeField, Header("Se音量設定用ボタン"), Tooltip("Se On/Offを設定出来る")]
    private Button m_seVolumeButton;

    /// <summary>
    /// Se音量設定用ボタンの画像
    /// </summary>
    private Image m_seVolumeImage;

    [SerializeField, Header("Se音量調整用スライダー")]
    private Slider m_seVolumeSlider;

    [SerializeField, Header("決定ボタン")]
    private Button m_okButton;

    [SerializeField, Header("サウンドOn/Offに使用するスプライト")]
    private List<Sprite> m_soundSpriteList;

    //--------------------------------------------
    // 初期化
    //--------------------------------------------

    void Awake()
    {
        m_bgmVolumeImage = m_bgmVolumeButton.GetComponent<Image>();
        m_seVolumeImage = m_seVolumeButton.GetComponent<Image>();

        // ボタン登録
        m_soundSettingButton.onClick.AddListener(() => OnClick_SoundSettingButton());
        m_bgmVolumeButton.onClick.AddListener(() => OnClick_BgmVolumeButton());
        m_seVolumeButton.onClick.AddListener(() => OnClick_SeVolumeButton());
        m_okButton.onClick.AddListener(() => OnClick_OkButton());

        // スライダー登録
        m_bgmVolumeSlider.onValueChanged.AddListener((float value) => OnValueChange_BgmVolumeSlider(value));
        m_seVolumeSlider.onValueChanged.AddListener((float value) => OnValueChange_SeVolumeSlider(value));
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        m_soundSettingUi.SetActive(false);
        CheckSoundSetting();
    }

    /// <summary>
    /// 音設定の状態を調べる
    /// </summary>
    private void CheckSoundSetting()
    {
        // 現在の音量の値をスライダーに反映
        m_bgmVolumeSlider.value = m_bgmVolumeSlider.maxValue * AudioManager.I.GetBgmVolumeMag();
        m_seVolumeSlider.value  = m_seVolumeSlider.maxValue * AudioManager.I.GetSeVolumeMag();

        // 現在の音量の状態をボタンの画像に反映
        m_bgmVolumeImage.sprite = m_soundSpriteList[(int)AudioManager.I.GetBgmFlg()];
        m_seVolumeImage.sprite  = m_soundSpriteList[(int)AudioManager.I.GetSeFlg()];
    }

    //--------------------------------------------
    // ボタン
    //--------------------------------------------

    /// <summary>
    /// 音設定UI表示するボタン押下時呼び出されるメソッド
    /// </summary>
    private void OnClick_SoundSettingButton()
    {
        AudioManager.I.PlaySe(AudioNames.ButtonSE);
        m_soundSettingUi.SetActive(true);
    }

    /// <summary>
    /// Bgm音量設定用ボタン押下時呼び出されるメソッド
    /// Bgm On/Off設定
    /// </summary>
    private void OnClick_BgmVolumeButton()
    {
        var currentBgmFlg = AudioManager.I.GetBgmFlg();

        switch (currentBgmFlg)
        {
            case SoundFlg.ON:
                AudioManager.I.SetBgmFlg(SoundFlg.OFF);
                break;
            case SoundFlg.OFF:
                AudioManager.I.SetBgmFlg(SoundFlg.ON);
                AudioManager.I.PlayBgm(AudioManager.I.CurrentBgMKey, 0.5f);
                break;
        }

        // 画像を切り替える
        m_bgmVolumeImage.sprite = m_soundSpriteList[(int)AudioManager.I.GetBgmFlg()];
    }

    /// <summary>
    /// Se音量設定用ボタン押下時呼び出されるメソッド
    /// Se On/Off設定
    /// </summary>
    private void OnClick_SeVolumeButton()
    {
        var currentSeFlg = AudioManager.I.GetSeFlg();

        switch (currentSeFlg)
        {
            case SoundFlg.ON:
                AudioManager.I.SetSeFlg(SoundFlg.OFF);
                break;
            case SoundFlg.OFF:
                AudioManager.I.SetSeFlg(SoundFlg.ON);
                break;
        }

        // 画像を切り替える
        m_seVolumeImage.sprite = m_soundSpriteList[(int)AudioManager.I.GetSeFlg()];
    }

    /// <summary>
    /// 決定ボタン押下時呼び出されるメソッド
    /// 音設定UIを非表示
    /// </summary>
    private void OnClick_OkButton()
    {
        AudioManager.I.PlaySe(AudioNames.ButtonSE);
        m_soundSettingUi.SetActive(false);
    }

    //--------------------------------------------
    // スライダー
    //--------------------------------------------

    /// <summary>
    /// Bgm音量調整時に呼び出されるメソッド
    /// </summary>
    private void OnValueChange_BgmVolumeSlider(float value)
    {
        AudioManager.I.SetBgmVolumeMag(value / m_bgmVolumeSlider.maxValue);
        Debug.Log("BGM音量倍率" + AudioManager.I.GetBgmVolumeMag());
    }

    /// <summary>
    /// Se音量調整時に呼び出されるメソッド
    /// </summary>
    private void OnValueChange_SeVolumeSlider(float value)
    {
        AudioManager.I.SetSeVolumeMag(value / m_seVolumeSlider.maxValue);
        Debug.Log("SE音量倍率" + AudioManager.I.GetSeVolumeMag());
    }
}
