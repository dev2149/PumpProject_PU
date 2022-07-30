using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Valve.VR.Extras;
public class SettingUI : MonoBehaviour
{
    [SerializeField] private SteamVR_LaserPointer laserPointer;
    [SerializeField] private SteamVR_LaserPointer laserPointer1;
    [SerializeField] private SteamVR_Action_Boolean menuAction;
    [SerializeField] private GameObject m_MenuObject;
    public bool _IsMenuUIOpen;
    [SerializeField] SceneIdx _scneneType;
    #region SliderGroup
    MeshCollider _SliderCollider;
    bool m_SliderCollCheck;
    [SerializeField] private Slider[] _SoundVolume;
    #endregion
    void Start()
    {
        m_MenuObject = this.gameObject.transform.Find("UiList").gameObject;
        m_SliderCollCheck = false;
        _SoundVolume = new Slider[this.gameObject.transform.Find("UiList").transform.Find("MainPanel").transform.Find("SoundVolume").transform.childCount];
        for (int i = 0; i < _SoundVolume.Length; i++)
        {
            _SoundVolume[i] = this.gameObject.transform.Find("UiList").transform.Find("MainPanel").transform.Find("SoundVolume").GetChild(i).GetComponent<Slider>();
        }
        if (_SoundVolume != null)
        {
            _SoundVolume[0].value = Directory.Instance.soundManager._BGMSound.volume;
            _SoundVolume[1].value = Directory.Instance.soundManager._EffectSound.volume;
            _SoundVolume[2].value = Directory.Instance.soundManager._NarrationSound.volume;
        }
    }
    void Update()
    {
        SoundSliderController();
        SoundController();
        MenuOnOff();
        ReadMuteActive();
    }
    void ReadMuteActive()
    {
        if (Directory.Instance.soundManager._flagBGMOnOff)
        {
            _SoundVolume[0].value = 0.0f;
        }
        if (Directory.Instance.soundManager._flagEffectOnOff)
        {
            _SoundVolume[1].value = 0.0f;
        }
        if (Directory.Instance.soundManager._flagNarrationOnOff)
        {
            _SoundVolume[2].value = 0.0f;
        }
    }// 실시간 음소거 감지.
    private void SoundSliderController()
    {
        if (m_MenuObject.gameObject.activeSelf && !m_SliderCollCheck)
        {
            _SliderCollider = this.gameObject.transform.Find("UiList").transform.Find("MainPanel").gameObject.GetComponent<MeshCollider>();
            _SliderCollider.convex = true;
            m_SliderCollCheck = true;
        }
    }
    private void SoundController()
    {
        if (_SoundVolume != null)
        {
            Directory.Instance.soundManager._BGMSound.volume = _SoundVolume[0].value;
            Directory.Instance.soundManager._EffectSound.volume = _SoundVolume[1].value;
            Directory.Instance.soundManager._NarrationSound.volume = _SoundVolume[2].value;
        }
    }
    #region SoundSetting
    public void SetBGMSound(bool _b , float _Value = 0.0f)
    {
        if (_b)
        {
            _SoundVolume[0].value = _Value;
        }
        else
        {
            _SoundVolume[0].value = _Value;
        }
    }
    public void SetEffectSound(bool _b, float _Value = 0.0f)
    {
        if(_b)
        {
            _SoundVolume[1].value = _Value;
        }
        else
        {
            _SoundVolume[1].value = _Value;
        }
    }
    public void SetNarrationSound(bool _b, float _Value = 0.0f)
    {
        if (_b)
        {
            _SoundVolume[2].value = _Value;
        }
        else
        {
            _SoundVolume[2].value = _Value;
        }
    }
    #endregion
    public void ExitSettingUi()
    {
        _IsMenuUIOpen = false;
        laserPointer.active = _IsMenuUIOpen;
        laserPointer1.active = _IsMenuUIOpen;
        m_MenuObject.SetActive(_IsMenuUIOpen);
    }
    public void NotInGame()
    {
        _IsMenuUIOpen = false;
        m_MenuObject.SetActive(_IsMenuUIOpen);
    }
    private void MenuOnOff()
    {
        if (menuAction.stateDown && _scneneType.Equals(SceneIdx.Decomposition))
        {
            _IsMenuUIOpen = !_IsMenuUIOpen;
            laserPointer.active = _IsMenuUIOpen;
            laserPointer1.active = _IsMenuUIOpen;
            m_MenuObject.SetActive(_IsMenuUIOpen);
            if (_SliderCollider != null)
            {
                _SliderCollider.convex = _IsMenuUIOpen;
            }
        }
        if (menuAction.stateDown && _scneneType.Equals(SceneIdx.Title))
        {
            _IsMenuUIOpen = !_IsMenuUIOpen;
            m_MenuObject.SetActive(_IsMenuUIOpen);
            if (_SliderCollider != null)
            {
                _SliderCollider.convex = _IsMenuUIOpen;
            }
        }
        if (menuAction.stateDown && _scneneType.Equals(SceneIdx.Tutorial))
        {
            _IsMenuUIOpen = !_IsMenuUIOpen;
            laserPointer.active = _IsMenuUIOpen;
            laserPointer1.active = _IsMenuUIOpen;
            m_MenuObject.SetActive(_IsMenuUIOpen);
            if (_SliderCollider != null)
            {
                _SliderCollider.convex = _IsMenuUIOpen;
            }
        }
    }
}