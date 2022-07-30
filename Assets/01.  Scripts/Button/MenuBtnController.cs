using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
public class MenuBtnController : MonoBehaviour
{
    [SerializeField] private SteamVR_LaserPointer laserPointer;
    [SerializeField] private SteamVR_LaserPointer laserPointer1;
    void Start()
    {
        laserPointer.PointerClick += PointerClick;
        laserPointer.PointerIn += PointerIn;
        laserPointer.PointerOut += PointerOut;

        laserPointer1.PointerClick += PointerClick;
        laserPointer1.PointerIn += PointerIn;
        laserPointer1.PointerOut += PointerOut;
    }
    public void PointerClick(object sender , PointerEventArgs e)
    {
        if (e.target.CompareTag("PanelBtn"))
        {
            ButtonClickEvent obj = e.target.GetComponent<ButtonClickEvent>();
            obj.PointerClick();
        }
        if (e.target.CompareTag("EffectSoundBtn"))
        {
            EffectSoundController obj = e.target.GetComponent<EffectSoundController>();
            obj.PointerClick();
        }
        if (e.target.CompareTag("BGMSoundBtn"))
        {
            BGMSoundController _obj = e.target.GetComponent<BGMSoundController>();
            _obj.PointerClick();
        }
        if (e.target.CompareTag("NarrationSoundBtn"))
        {
            NarrationSoundController obj = e.target.GetComponent<NarrationSoundController>();
            obj.PointerClick();
        }
        if (e.target.CompareTag("VideoClips"))
        {
            VideoButtonClickEvent obj = e.target.GetComponent<VideoButtonClickEvent>();
            obj.PointerClick();
        }
        if (e.target.CompareTag("MSDS"))
        {
            MSDSButtonActive obj = e.target.GetComponent<MSDSButtonActive>();
            obj.PointerClick();
        }
    }
    public void PointerIn(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("PanelBtn"))
        {
            ButtonClickEvent obj = e.target.GetComponent<ButtonClickEvent>();
            obj.PointerInside();
        }
        if (e.target.CompareTag("EffectSoundBtn"))
        {
            EffectSoundController obj = e.target.GetComponent<EffectSoundController>();
            obj.PointerInside();
        }
        if (e.target.CompareTag("BGMSoundBtn"))
        {
            BGMSoundController _obj = e.target.GetComponent<BGMSoundController>();
            _obj.PointerInside();
        }
        if (e.target.CompareTag("NarrationSoundBtn"))
        {
            NarrationSoundController _obj = e.target.GetComponent<NarrationSoundController>();
            _obj.PointerInside();
        }
        if (e.target.CompareTag("VideoClips"))
        {
            VideoButtonClickEvent obj = e.target.GetComponent<VideoButtonClickEvent>();
            obj.PointerInside();
        }
        if (e.target.CompareTag("MSDS"))
        {
            MSDSButtonActive obj = e.target.GetComponent<MSDSButtonActive>();
            obj.PointerInside();
        }
    }
    public void PointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("PanelBtn"))
        {
            ButtonClickEvent obj = e.target.GetComponent<ButtonClickEvent>();
            obj.PointerOutside();
        }
        if (e.target.CompareTag("EffectSoundBtn"))
        {
            EffectSoundController obj = e.target.GetComponent<EffectSoundController>();
            obj.PointerOutside();
        }
        if (e.target.CompareTag("BGMSoundBtn"))
        {
            BGMSoundController _obj = e.target.GetComponent<BGMSoundController>();
            _obj.PointerOutside();
        }
        if (e.target.CompareTag("NarrationSoundBtn"))
        {
            NarrationSoundController _obj = e.target.GetComponent<NarrationSoundController>();
            _obj.PointerOutside();
        }
        if (e.target.CompareTag("VideoClips"))
        {
            VideoButtonClickEvent obj = e.target.GetComponent<VideoButtonClickEvent>();
            obj.PointerOutside();
        }
        if (e.target.CompareTag("MSDS"))
        {
            MSDSButtonActive obj = e.target.GetComponent<MSDSButtonActive>();
            obj.PointerOutside();
        }
    }
}