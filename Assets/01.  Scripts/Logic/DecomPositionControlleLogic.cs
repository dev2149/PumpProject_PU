using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Valve.VR.Extras;
public class DecomPositionControlleLogic : MonoBehaviour
{
    [SerializeField] Sprite[] _ContensImages;
    [SerializeField] Image _CurrentContentsSprite;
    [SerializeField] Text _MSDSPage;
    [SerializeField] GameObject[] _MSDSCOll;
    [SerializeField] MSDSButtonActive _MSDSIDX;
    [SerializeField] private Sprite _Disabled;
    int _MSDS_Number = 0;
    [SerializeField] private GameObject[] _PanelGroup;
    float narration_lenght_temp = 0.0f;
    [SerializeField] private SteamVR_LaserPointer _LaserPoint;
    [SerializeField] private SteamVR_LaserPointer _LaserPoint1;
    [SerializeField] AudioClip _ButtonClick;
    [SerializeField] AudioClip _ComplateCound;
    [SerializeField] private GameObject[] PumpLogicObject;

    [SerializeField] private AssemblyObject _DeComObject;
    [SerializeField] private AssemblyObject _AssemblyObject;
    // Guide UI
    [SerializeField] private Image _GuideImage;
    [SerializeField] private GameObject[] _TrayImage;
    string narration_lenght_text_temp;
    [SerializeField] GameObject _ToolName;
    [SerializeField] GameObject _PlayerObj;
    [SerializeField] GameObject[] _Tray;
    [SerializeField] GameObject[] _CrossGuide;
    // video
    [SerializeField] VideoHandle _VideoStreaming;
    [SerializeField] GameObject _StartTeeveImage;
    [SerializeField] RawImage _VideoOverlay;
    [SerializeField] GameObject _Thumbnail;
    [SerializeField] Text _ThumbnailTxT;
    int count = 0;
    // 축정렬 박스 콜라이더
    [SerializeField] BoxCollider[] _AxisBoxCollider;
    // TooTip
    [SerializeField] GameObject _MainCanvas;
    [SerializeField] GameObject[] _ObjectIndex;
    private void Start()
    {
        StartCoroutine(AccidentCase());
        ChildLaod();
    }
    void ChildLaod()
    {
        _LaserPoint.active = true;
        _MSDSPage.text = "1 / 3";
        _GuideImage = GameObject.Find("GuideCanvas").transform.Find("BG").GetComponent<Image>();
        _GuideImage.gameObject.SetActive(false);
        Step _ExplantionGuideStep = Directory.Instance.stepPlanDic["Decom1"];
        _GuideImage.GetComponentInChildren<Text>().text = _ExplantionGuideStep.explanation0;
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
        {
            _TrayImage[0].SetActive(true);
            _TrayImage[1].SetActive(true);
        }
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
        {
            _TrayImage[0].SetActive(false);
            _TrayImage[1].SetActive(false);
            _ToolName.SetActive(false);
            _Tray[0].SetActive(false);
        }
        _CrossGuide[0].SetActive(false);
        _CrossGuide[1].SetActive(false);
        //_Tray[0].SetActive(true);
        //_Tray[1].SetActive(false);
        _MSDSCOll[0].GetComponent<BoxCollider>().enabled = false;
        _MSDSCOll[1].GetComponent<BoxCollider>().enabled = true;
        _StartTeeveImage.SetActive(true);
        _VideoOverlay.gameObject.SetActive(false);
        _Tray[0].GetComponentInChildren<Text>().text = "분해된 부품들을 볼수 있습니다.";
        _Thumbnail.SetActive(false);
        _ThumbnailTxT.text = "펌프 분리 방법";
        for (int i = 0; i < _ObjectIndex.Length; i++)
        {
            _ObjectIndex[i].SetActive(false);
        }
    }
    IEnumerator AccidentCase()
    {
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_17, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    public void NextPanel(int _num)
    {
        Directory.Instance.soundManager.StopNarrationSound();
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        _PanelGroup[_num - 1].SetActive(false);
        _PanelGroup[_num].SetActive(true);
        switch (_num)// 중간 나래이션 넣기
        {
            case 2:
                Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_19, out narration_lenght_temp);
                break;
        }
    }
    // 비디오
    public void FirstVideoStreaming()
    {
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
        {
            Directory.Instance.soundManager.StopNarrationSound();
            _PanelGroup[2].SetActive(false);
            Directory.Instance.soundManager.PlaySound(_ButtonClick);
            StartCoroutine(ThumbNailStart());
        }
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
        {
            Directory.Instance.soundManager.StopNarrationSound();
            _PanelGroup[2].SetActive(false);
            Directory.Instance.soundManager.PlaySound(_ButtonClick);
            _VideoOverlay.gameObject.SetActive(false);
            _StartTeeveImage.SetActive(true);
            StartCoroutine(StartHardLigic());
        }
    }
    // 비디오 처음 시작 추가
    private IEnumerator ThumbNailStart()
    {
        _StartTeeveImage.SetActive(false);
        _Thumbnail.SetActive(true);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_20, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        StartCoroutine(StreamingVideo());
    }
    private IEnumerator StreamingVideo()
    {
        Directory.Instance.soundManager.NextThumbnailSound(SoundManager.ThumbNailStream.TH_1, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _StartTeeveImage.SetActive(false);
        _Thumbnail.SetActive(false);
        _VideoOverlay.gameObject.SetActive(true);
        _VideoStreaming.Init();
        _VideoStreaming.ChildLoad();
        _VideoStreaming._myVedeo.clip = Directory.Instance.videoPlayerController._VideoGroup[0];
        _VideoStreaming.Play();
        while (_VideoStreaming._VideoValue.value < 0.99f)
        {
            //_VideoStreaming._VideoValue.value = (float)_VideoStreaming._myVedeo.frame / (float)_VideoStreaming._myVedeo.clip.frameCount;
            yield return null;
        }
        _VideoStreaming.Stop();
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_21, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    #region hardMode
    IEnumerator StartHardLigic()
    {
        _MainCanvas.SetActive(false);
        _Tray[0].SetActive(false);
        _ToolName.SetActive(false);
        PumpLogicObject[0].SetActive(true);
        _LaserPoint.active = false;
        _LaserPoint1.active = false;
        while (_DeComObject.Processivity != 20.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        _ObjectIndex[0].SetActive(true);
        while (_DeComObject.Processivity != 40.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while (_DeComObject.Processivity != 60.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while (_DeComObject.Processivity != 80.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while (_DeComObject.Processivity != 100.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        yield return new WaitForSeconds(1.0f);
        //_Tray[0].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        PumpLogicObject[0].SetActive(false);
        PumpLogicObject[1].SetActive(true);
        while ((int)_AssemblyObject.Processivity != 3)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 10)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 23)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 26)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 30)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 36)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 40)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 43)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 46)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 60)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 73)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while ((int)_AssemblyObject.Processivity != 86)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        Debug.Log((int)_AssemblyObject.Processivity);

        while ((int)_AssemblyObject.Processivity != 100)
        {

        Debug.Log((int)_AssemblyObject.Processivity);


            yield return null;
        }
        Debug.Log((int)_AssemblyObject.Processivity);


        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_16, "Assy16"));
        StartCoroutine(ComplateFIO());
        _Tray[0].SetActive(false);
        _LaserPoint.active = true;
        _LaserPoint1.active = true;
        _PanelGroup[3].SetActive(true);
    }
    #endregion
    public void VideoCloaseButton()
    {
        switch (count)
        {
            case 0:
                StartCoroutine(StartDecomposition());
                count++;
                break;
            case 1:
                StopAllCoroutines();
                StartCoroutine(StartAssembly());
                count++;
                break;
            case 2:
                AssemblyComplateCheck();
                count++;
                break;
            case 3:
                LastVideo();
                break;
        }
    }
    #region 분해 시작
    IEnumerator StartDecomposition()
    {
        _VideoStreaming._myVedeo.Stop();
        _StartTeeveImage.SetActive(true);
        _VideoOverlay.gameObject.SetActive(false);
        Directory.Instance.soundManager.StopNarrationSound();
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_18, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        PumpLogicObject[0].SetActive(true);
        _LaserPoint.active = false;
        _LaserPoint1.active = false;
        _GuideImage.gameObject.SetActive(true);
        //여기서부터 Decompositionsound / StopNarrationSound를 활용한 Play
        StartCoroutine(NextStepNarration(SoundManager.DeComPositionIndex.D_1, "Decom1"));
        while (_DeComObject.Processivity != 20.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        _ObjectIndex[0].SetActive(true);
        StartCoroutine(NextStepNarration(SoundManager.DeComPositionIndex.D_2, "Decom2"));
        while (_DeComObject.Processivity != 40.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        _ObjectIndex[1].SetActive(true);
        StartCoroutine(NextStepNarration(SoundManager.DeComPositionIndex.D_3, "Decom3"));
        while (_DeComObject.Processivity != 60.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        _ObjectIndex[2].SetActive(true);
        StartCoroutine(NextStepNarration(SoundManager.DeComPositionIndex.D_4, "Decom4"));
        yield return new WaitForSeconds(narration_lenght_temp);
        StartCoroutine(NextStepNarration(SoundManager.DeComPositionIndex.D_5, "Decom5"));
        while (_DeComObject.Processivity != 80.0f)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        _ObjectIndex[3].SetActive(true);
        StartCoroutine(NextStepNarration(SoundManager.DeComPositionIndex.D_6, "Decom6"));
        while (_DeComObject.Processivity != 100.0f)
        {
            yield return null;
        }
        // 나레이션
        _StartTeeveImage.SetActive(false);
        _Thumbnail.SetActive(true);
        _ThumbnailTxT.text = "부품 교체 방법";
        _GuideImage.gameObject.SetActive(false);
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        _ObjectIndex[4].SetActive(true);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_22, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _LaserPoint.active = true;
        _LaserPoint1.active = true;
        _VideoOverlay.gameObject.SetActive(true);
        _Thumbnail.SetActive(false);
        _VideoStreaming._myVedeo.clip = Directory.Instance.videoPlayerController._VideoGroup[1];
        _VideoStreaming.Init();
        _VideoStreaming.Play();
        while (_VideoStreaming._VideoValue.value < 0.99f)
        {
            //_VideoStreaming._VideoValue.value = (float)_VideoStreaming._myVedeo.frame / (float)_VideoStreaming._myVedeo.clip.frameCount;
            yield return null;
        }
        _VideoStreaming.Stop();

        Directory.Instance.soundManager.NextThumbnailSound(SoundManager.ThumbNailStream.TH_2, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    #endregion
    #region 조립 시작
    IEnumerator StartAssembly()
    {
        _LaserPoint.active = false;
        _LaserPoint1.active = false;
        _VideoStreaming.Stop();
        _StartTeeveImage.SetActive(true);
        _VideoOverlay.gameObject.SetActive(false);
        Directory.Instance.soundManager.StopNarrationSound();
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        _GuideImage.gameObject.SetActive(true);
        _Tray[0].SetActive(true);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_2, "Assy2"));
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _MainCanvas.SetActive(false);
        PumpLogicObject[0].SetActive(false);
        PumpLogicObject[1].SetActive(true);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_3, "Assy3"));
        _Tray[0].GetComponentInChildren<Text>().text = "조립 하기 위한 부품이 바닥에 떨어 졌거나\n" +
    "조립대에서 놓쳤을때 처음생성된 위치로,\n" +
    "부품이 다시 재 생성 됩니다.";
        while ((int)_AssemblyObject.Processivity != 3)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_4, "Assy4"));
        while ((int)_AssemblyObject.Processivity != 10)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_5, "Assy5"));
        while ((int)_AssemblyObject.Processivity != 23)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_6, "Assy6"));
        while ((int)_AssemblyObject.Processivity != 26)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_7, "Assy7"));
        while ((int)_AssemblyObject.Processivity != 30)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_8, "Assy8"));
        while ((int)_AssemblyObject.Processivity != 36)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_9, "Assy9"));
        while ((int)_AssemblyObject.Processivity != 40)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_10, "Assy10"));
        while ((int)_AssemblyObject.Processivity != 43)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_11, "Assy11"));
        while ((int)_AssemblyObject.Processivity != 46)
        {
            yield return null;
        }
        _CrossGuide[1].SetActive(true);
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_12, "Assy12"));
        while ((int)_AssemblyObject.Processivity != 60)
        {
            yield return null;
        }
        _CrossGuide[1].SetActive(false);
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_13, "Assy13"));
        while ((int)_AssemblyObject.Processivity != 73)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);


        _StartTeeveImage.SetActive(false);
        _GuideImage.gameObject.SetActive(false);
        _Thumbnail.SetActive(true);
        _ThumbnailTxT.text = "축 정렬 방법";
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_24, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _LaserPoint.active = true;
        _LaserPoint1.active = true;
        _VideoOverlay.gameObject.SetActive(true);
        _Thumbnail.SetActive(false);
        _VideoStreaming._myVedeo.clip = Directory.Instance.videoPlayerController._VideoGroup[2];
        _VideoStreaming.Init();
        _VideoStreaming.Play();
        //StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_14, "Assy14"));
        while ((int)_AssemblyObject.Processivity != 86)
        {
            yield return null;
        }
        _CrossGuide[0].SetActive(true);
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
        while (_VideoStreaming._VideoValue.value < 0.99f)
        {
            //_VideoStreaming._VideoValue.value = (float)_VideoStreaming._myVedeo.frame / (float)_VideoStreaming._myVedeo.clip.frameCount;
            yield return null;
        }
        Directory.Instance.soundManager.NextThumbnailSound(SoundManager.ThumbNailStream.TH_3, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    #endregion
    private void AssemblyComplateCheck()
    {
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        StartCoroutine(ComplateCheck());
    }
    IEnumerator ComplateCheck()
    {
        Directory.Instance.soundManager.StopNarrationSound();
        _VideoStreaming.Stop();
        _Thumbnail.SetActive(false);
        _StartTeeveImage.SetActive(true);
        _VideoOverlay.gameObject.SetActive(false);
        _GuideImage.gameObject.SetActive(true);
        _LaserPoint.active = false;
        _LaserPoint1.active = false;

        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_15, "Assy15"));
        while ((int)_AssemblyObject.Processivity != 100)
        {
            yield return null;
        }
        Directory.Instance.soundManager.PlaySound(_ComplateCound);
  
        _Thumbnail.SetActive(true);
        _StartTeeveImage.SetActive(false);
        _ThumbnailTxT.text = "펌프 스위칭 방법";
        _GuideImage.gameObject.SetActive(false);
        Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.D_25, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        _LaserPoint.active = true;
        _LaserPoint1.active = true;
        _GuideImage.gameObject.SetActive(false);
        _VideoOverlay.gameObject.SetActive(true);
        _Thumbnail.SetActive(false);
        _VideoStreaming._myVedeo.clip = Directory.Instance.videoPlayerController._VideoGroup[3];
        _VideoStreaming.Init();
        _VideoStreaming.Play();
        while (_VideoStreaming._VideoValue.value < 0.99f)
        {
            //_VideoStreaming._VideoValue.value = (float)_VideoStreaming._myVedeo.frame / (float)_VideoStreaming._myVedeo.clip.frameCount;
            yield return null;
        }
        Directory.Instance.soundManager.NextThumbnailSound(SoundManager.ThumbNailStream.TH_3, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    private void LastVideo()
    {
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        StartCoroutine(ExperienceComplate());
    }
    IEnumerator ExperienceComplate()
    {
        _VideoStreaming._myVedeo.Stop();
        _StartTeeveImage.SetActive(true);
        _VideoOverlay.gameObject.SetActive(false);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_17, "Assy17"));
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
        StartCoroutine(NextStepNarrationAssembly(SoundManager.AssemblyIndex.A_16, "Assy16"));
        StartCoroutine(ComplateFIO());
        _Tray[0].SetActive(false);
        _PanelGroup[3].SetActive(true);
    }
    #region MSDS
    public void OperatorPlus()
    {
        _MSDS_Number++;
        MSDSChangePage();
    }
    public void OperatorMinus()
    {
        _MSDS_Number--;
        MSDSChangePage();
    }
    private void MSDSChangePage()
    {
        switch (_MSDS_Number)
        {
            case 0:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _MSDSPage.text = "1 / 3";
                SetSprite(_ContensImages[0]);
                _MSDSCOll[0].GetComponent<BoxCollider>().enabled = false;
                _MSDSCOll[1].GetComponent<BoxCollider>().enabled = true;
                break;
            case 1:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _MSDSPage.text = "2 / 3";
                SetSprite(_ContensImages[1]);
                _MSDSCOll[0].GetComponent<BoxCollider>().enabled = true;
                _MSDSCOll[1].GetComponent<BoxCollider>().enabled = true;
                break;
            case 2:
                _MSDSIDX._MyCollider.enabled = true;
                _MSDSIDX.SetSprite(_Disabled);
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _MSDSPage.text = "3 / 3";
                SetSprite(_ContensImages[2]);
                _MSDSCOll[0].GetComponent<BoxCollider>().enabled = true;
                _MSDSCOll[1].GetComponent<BoxCollider>().enabled = false;
                _MSDSCOll[1].GetComponent<Image>().sprite = _ContensImages[3];
                break;
        }
    }
    void SetSprite(Sprite _sprite)
    {
        _CurrentContentsSprite.overrideSprite = _sprite;
    }
    #endregion
    void ResetGuideText(string _name, out string _val) // 설명 가이드 Out 함수
    {
        Step _ExplantionGuideStep = Directory.Instance.stepPlanDic[_name];
        string _ExplantionText = _ExplantionGuideStep.explanation0;
        _val = _ExplantionText;
    }
    public void ReturnPlayPanel(int _idx)
    {
        switch (_idx) 
        {
            case 0:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _PanelGroup[3].SetActive(false);
                _PanelGroup[4].SetActive(true);
                break;
            case 1:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                Directory.Instance.sceneController._difficulty = Difficulty.Nomal;
                Directory.Instance.sceneController.LoadScene(SceneIdx.Decomposition);
                break;
            case 2:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                Directory.Instance.sceneController._difficulty = Difficulty.Hard;
                Directory.Instance.sceneController.LoadScene(SceneIdx.Decomposition);
                break;
            case 3:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _PanelGroup[4].SetActive(false);
                _PanelGroup[3].SetActive(true);
                break;
        }
    }
    IEnumerator NextStepNarration(SoundManager.DeComPositionIndex _idx, string _name)
    {
        ResetGuideText(_name, out narration_lenght_text_temp);
        _GuideImage.GetComponentInChildren<Text>().text = narration_lenght_text_temp;
        Directory.Instance.soundManager.PlayDeComPositionSound(_idx, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    IEnumerator NextStepNarrationAssembly(SoundManager.AssemblyIndex _idx, string _name)
    {
        ResetGuideText(_name, out narration_lenght_text_temp);
        _GuideImage.GetComponentInChildren<Text>().text = narration_lenght_text_temp;
        Directory.Instance.soundManager.PlayAssemblySound(_idx, out narration_lenght_temp);
        yield return new WaitForSeconds(narration_lenght_temp + 1.0f);
    }
    public IEnumerator ComplateFIO()
    {
        SteamVR_Fade.View(Color.black, 1);
        SteamVR_Fade.Start(Color.black, 1);
        yield return new WaitForSeconds(1f);
        _GuideImage.gameObject.SetActive(false);
        _PlayerObj.transform.position = new Vector3(-0.07f, _PlayerObj.transform.position.y, 0.49f);
        SteamVR_Fade.View(Color.clear, 1);
        SteamVR_Fade.Start(Color.clear, 1);
    }
}