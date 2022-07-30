using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;
public class SoundManager : MonoBehaviour
{
    public enum NarrationIndex
    {
        S_1,
        S_2,
        S_3,
        S_4,
        S_5,
        S_6,
        S_7,
        S_8,
        S_9,
        S_10,
        S_11,
        S_12,
        S_13,
        S_14,
        T_15,
        T_16,
        D_17,
        D_18,
        D_19,
        D_20,
        D_21,
        D_22,
        D_23,
        D_24,
        D_25
    }
    public enum DeComPositionIndex
    {
        D_1,
        D_2,
        D_3,
        D_4,
        D_5,
        D_6,
        D_7,
        D_8,
        D_9,
        D_10,
        D_11,
        D_12,
        D_13,
        D_14,
        D_15,
        D_16,
    }
    public enum AssemblyIndex
    {
        A_1,
        A_2,
        A_3,
        A_4,
        A_5,
        A_6,
        A_7,
        A_8,
        A_9,
        A_10,
        A_11,
        A_12,
        A_13,
        A_14,
        A_15,
        A_16,
        A_17
    }
    public enum ThumbNailStream
    {
        TH_1,
        TH_2,
        TH_3,
        TH_4,
        TH_5
    }
    [System.Serializable]
    public enum SoundIndex
    {
        None,
        BGM,
        Effect
    }
    [SerializeField] SettingUI _SettingUI;
    public AudioSource _EffectSound;
    public AudioSource _BGMSound;
    public AudioSource _NarrationSound;

    [SerializeField] private AudioClip[] _NarrationClips;
    [SerializeField] private AudioClip[] _DecompositionClips;
    [SerializeField] private AudioClip[] _AssemblyClips;
    [SerializeField] private AudioClip[] _ThumbnailClips;
    public AudioClip _Drop;
    #region ChangeSprite
    [HideInInspector] public Sprite _EffectDisabled;
    [HideInInspector] public Sprite _EffectHighlight;
    [HideInInspector] public Sprite _BGMDisabled;
    [HideInInspector] public Sprite _BGMHighlight;
    [HideInInspector] public Sprite _NarrationDisabled;
    [HideInInspector] public Sprite _NarrationHighLight;
     #endregion
    float _TemporaryStorageEffectSound;
    float _TemporaryStorageBGMSound;
    float _TemporaryStorageNarration;
    public bool Initialization { get; private set; }
   [HideInInspector] public bool _flagEffectOnOff;
   [HideInInspector] public bool _flagBGMOnOff;
   [HideInInspector] public bool _flagNarrationOnOff;

    Teleport teleport;
    public void Init()
    {
        _flagEffectOnOff = false;
        _flagBGMOnOff = false;
        _flagNarrationOnOff = false;
        Initialization = true;
    }
    public void SetMuteSound(string _name)
    {
        switch (_name)
        {
            case "BGM":
                SetBGMSliderInit();
                break;
            case "Effect":
                SetEffectSliderInit();
                break;
            case "Narration":
                SetNarrationSliderInit();
                break;
        }
    }
    public void PlaySound(AudioClip _clip)
    {
        _EffectSound.clip = _clip;
        _EffectSound.Play();
        _BGMSound.loop = false;
    }
    public void StopSound(AudioClip _Clip)
    {
        _EffectSound.clip = _Clip;
        _EffectSound.Stop();
        _BGMSound.loop = false;
    }
    private void SetBGMSliderInit()
    {
        _SettingUI = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand")
            .transform.Find("MenuCanvas").transform.Find("MenuBtnAction").gameObject.GetComponent<SettingUI>();
        _flagBGMOnOff = !_flagBGMOnOff;
        if (_flagBGMOnOff)
        {
            _TemporaryStorageBGMSound = _BGMSound.volume;
            _SettingUI.SetBGMSound(_flagBGMOnOff);
        }
        else
        {
            _BGMSound.volume = _TemporaryStorageBGMSound;
            _SettingUI.SetBGMSound(_flagBGMOnOff , _BGMSound.volume);
            _TemporaryStorageBGMSound = 0.0f;
        }
    }
    private void SetEffectSliderInit()
    {
        _SettingUI = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand")
    .transform.Find("MenuCanvas").transform.Find("MenuBtnAction").gameObject.GetComponent<SettingUI>();
        _flagEffectOnOff = !_flagEffectOnOff;
        if (_flagEffectOnOff)
        {
            _TemporaryStorageEffectSound = _EffectSound.volume;
            _SettingUI.SetEffectSound(_flagEffectOnOff);
        }
        else
        {
            _EffectSound.volume = _TemporaryStorageEffectSound;
            _SettingUI.SetEffectSound(_flagEffectOnOff , _EffectSound.volume);
            _TemporaryStorageEffectSound = 0.0f;
        }
    }
    private void SetNarrationSliderInit()
    {
        _SettingUI = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand")
        .transform.Find("MenuCanvas").transform.Find("MenuBtnAction").gameObject.GetComponent<SettingUI>();
        _flagNarrationOnOff = !_flagNarrationOnOff;
        if (_flagNarrationOnOff)
        {
            _TemporaryStorageNarration = _NarrationSound.volume;
            _SettingUI.SetNarrationSound(_flagNarrationOnOff);
        }
        else
        {
            _NarrationSound.volume = _TemporaryStorageNarration;
            _SettingUI.SetNarrationSound(_flagNarrationOnOff, _NarrationSound.volume);
            _TemporaryStorageNarration = 0.0f;
        }
    }
    public void NextThumbnailSound(ThumbNailStream _idx , out float val)
    {
        int index = (int)_idx;
        val = _ThumbnailClips[index].length;
        _NarrationSound.clip = _ThumbnailClips[index];
        _NarrationSound.Play();
    }
    public void PlayNarrationSound(NarrationIndex narrationIndex, out float value)
    {
        int index = (int)narrationIndex;

        value = _NarrationClips[index].length;

        _NarrationSound.clip = _NarrationClips[index];
        _NarrationSound.Play();
    }
    public void PlayDeComPositionSound(DeComPositionIndex decompositionIndex, out float value)
    {
        int index = (int)decompositionIndex;

        value = _DecompositionClips[index].length;

        _NarrationSound.clip = _DecompositionClips[index];
        _NarrationSound.Play();
    }
    public void PlayAssemblySound(AssemblyIndex assemblyIndex, out float value)
    {
        int index = (int)assemblyIndex;

        value = _AssemblyClips[index].length;

        _NarrationSound.clip = _AssemblyClips[index];
        _NarrationSound.Play();
    }
    public void StopNarrationSound()
    {
        _NarrationSound.Stop();
        _NarrationSound.clip = null;
    }
    public void AllStopSound()
    {
        _EffectSound.Stop();
        _BGMSound.Stop();
        _NarrationSound.Stop();
    }
}