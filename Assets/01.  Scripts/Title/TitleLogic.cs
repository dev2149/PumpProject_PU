using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogic : MonoBehaviour
{
    public enum StartList
    {
        Start,
        Slect,
        Nomal,
        Hard

    }
    [SerializeField] SettingUI setting;
    [SerializeField] GameObject[] _TitleMenuGroup;
    [SerializeField] AudioClip _ButtonClick;
    float narration_lenght_temp = 0.0f;

    void Start()
    {
        Directory.Instance.StartProject();
        _TitleMenuGroup = new GameObject[GameObject.Find("WorldCanvas").transform.childCount];
        setting = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand").
    transform.Find("MenuCanvas").transform.Find("MenuBtnAction").GetComponent<SettingUI>();
        ChildLoad();
    }
    void ChildLoad()
    {
        for (int i = 0; i < _TitleMenuGroup.Length; i++)
        {
            _TitleMenuGroup[i] = GameObject.Find("WorldCanvas").transform.GetChild(i).gameObject;
        }
        Directory.Instance.soundManager.AllStopSound();
    }
    public void OnClickEventOpen(int type)
    {
        switch ((StartList)type)
        {
            case StartList.Start:
                OpenPanelAllActiveNone();
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _TitleMenuGroup[0].SetActive(true);
                break;
            case StartList.Slect:
                OpenPanelAllActiveNone();
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.T_15 ,out narration_lenght_temp);
                _TitleMenuGroup[1].SetActive(true);
                break;
            case StartList.Nomal:
                OpenPanelAllActiveNone();
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _TitleMenuGroup[2].SetActive(true);
                Directory.Instance.soundManager.StopNarrationSound();
                Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.T_16, out narration_lenght_temp);
                Directory.Instance.sceneController._difficulty = Difficulty.Nomal;
                break;
            case StartList.Hard:
                OpenPanelAllActiveNone();
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                _TitleMenuGroup[2].SetActive(true);
                Directory.Instance.soundManager.StopNarrationSound();
                Directory.Instance.soundManager.PlayNarrationSound(SoundManager.NarrationIndex.T_16, out narration_lenght_temp);
                Directory.Instance.sceneController._difficulty = Difficulty.Hard;
                break;
        }
    }
    public void OpenPanelAllActiveNone()
    {
        for (int i = 0; i < _TitleMenuGroup.Length; i++)
        {
            _TitleMenuGroup[i].gameObject.SetActive(false);
        }
    }
    #region ClickEvent
    public void OnClickScene(int _idx)
    {
        SceneIdx buttonType = (SceneIdx)_idx;
        switch (buttonType)
        {
            case SceneIdx.Title:
                Directory.Instance.sceneController.LoadScene(SceneIdx.Title);
                break;
            case SceneIdx.Decomposition:
                Directory.Instance.sceneController.LoadScene(SceneIdx.Decomposition);
                break;
            case SceneIdx.Tutorial:
                Directory.Instance.sceneController.LoadScene(SceneIdx.Tutorial);
                break;
        }
    }
    public void CloaseSettingUi()
    {
        setting.ExitSettingUi();
    }
    public void CloaseSettingUI_0()
    {
        setting.NotInGame();
    }
    public void QuitApp()
    {
        Debug.Log("종료!!");
        Application.Quit();
    }
    #endregion
}
