using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class VideoHandle : MonoBehaviour
{
    [SerializeField] VideoButtonClickEvent _VideoClickPlay;
    [SerializeField] VideoButtonClickEvent _VideoClickMute;
    public VideoPlayer _myVedeo;
    public Slider _VideoValue;
    [SerializeField] bool _isPlayFlag;
    [SerializeField] bool _isMuteFlag;
    [SerializeField] private Sprite[] _Hightlighted;
    [SerializeField] private Sprite[] _Disabled;
    [SerializeField] private Sprite[] _Selected;

    [SerializeField] Image[] _ChangeImage;
    public void Init()
    {
        _myVedeo = GetComponent<VideoPlayer>();
        _isPlayFlag = false;
        _VideoClickPlay._Hightlighted = _Hightlighted[1];
        _VideoClickPlay._Disabled = _Disabled[1];
        _VideoClickPlay._Selected = _Selected[1];
        _VideoClickMute._Hightlighted = _Hightlighted[2];
        _VideoClickMute._Disabled = _Disabled[2];
        _VideoClickMute._Selected = _Selected[2];
        _isMuteFlag = true; // 동영상 시작 할때 펄스 해주고 함수 실행 필요함
        _VideoClickPlay.SetImagesInit();
        _VideoClickMute.SetImagesInit();
        MuteClip();
    }
    public void ChildLoad()
    {
        _VideoValue.value = (float)_myVedeo.frame / (float)_myVedeo.clip.frameCount;
    }
    public void Play()
    {
        _isPlayFlag = !_isPlayFlag;
        if (_isPlayFlag)
        {
            _VideoClickPlay._Hightlighted = _Hightlighted[1];
            _VideoClickPlay._Disabled = _Disabled[1];
            _VideoClickPlay._Selected = _Selected[1];
            _myVedeo.Play();
            StartCoroutine(PlaySlide());
        }
        else
        {
            _VideoClickPlay._Hightlighted = _Hightlighted[0];
            _VideoClickPlay._Disabled = _Disabled[0];
            _VideoClickPlay._Selected = _Selected[0];
            _myVedeo.Pause();
        }
    }
    public void Stop()
    {
        _VideoClickPlay._Hightlighted = _Hightlighted[0];
        _VideoClickPlay._Disabled = _Disabled[0];
        _VideoClickPlay._Selected = _Selected[0];
        _VideoClickPlay.SetSprite(_VideoClickPlay._Disabled);
        _isPlayFlag = false;
        _myVedeo.Stop();
        _VideoValue.value = (float)_myVedeo.frame / (float)_myVedeo.clip.frameCount;
    }
    public void MuteClip()
    {
        _isMuteFlag = !_isMuteFlag;
        if (_isMuteFlag)
        {
            _myVedeo.SetDirectAudioMute(0, _isMuteFlag);
            _VideoClickMute._Hightlighted = _Hightlighted[3];
            _VideoClickMute._Disabled = _Disabled[3];
            _VideoClickMute._Selected = _Selected[3];
        }
        else
        {
            _myVedeo.SetDirectAudioMute(0, _isMuteFlag);
            _VideoClickMute._Hightlighted = _Hightlighted[2];
            _VideoClickMute._Disabled = _Disabled[2];
            _VideoClickMute._Selected = _Selected[2];
        }
    }
    IEnumerator PlaySlide()
    {
        while(_VideoValue.value < 0.99f)
        {
            _VideoValue.value = (float)_myVedeo.frame / (float)_myVedeo.clip.frameCount;
            yield return null;
        }
    }
    public void PlayerLenth(int _num , out float _f)
    {
        _f = (float)Directory.Instance.videoPlayerController._VideoGroup[_num].length;
    }
}