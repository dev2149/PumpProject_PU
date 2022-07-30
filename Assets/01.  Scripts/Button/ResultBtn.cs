using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Valve.VR.Extras;
public class ResultBtn : MonoBehaviour
{
    [SerializeField] private SettingUI setting;
    [SerializeField] private AudioClip _ButtonClick;
    private void Start()
    {
        setting = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand").
            transform.Find("MenuCanvas").transform.Find("MenuBtnAction").GetComponent<SettingUI>();
    }
    #region ClickEvent
    public void OnClickScene(int _idx)
    {
        SceneIdx buttonType = (SceneIdx)_idx;
        switch (buttonType)
        {
            case SceneIdx.Title:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                Directory.Instance.sceneController.LoadScene(SceneIdx.Title);
                break;
            case SceneIdx.Decomposition:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                Directory.Instance.sceneController.LoadScene(SceneIdx.Decomposition);
                break;
            case SceneIdx.Tutorial:
                Directory.Instance.soundManager.PlaySound(_ButtonClick);
                Directory.Instance.sceneController.LoadScene(SceneIdx.Tutorial);
                break;
        }
    }
    public void CloaseSettingUi()
    {
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        setting.ExitSettingUi();
    }
    public void CloaseSettingUI_0()
    {
        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        setting.NotInGame();
    }
    public void QuitApp()
    {

        Directory.Instance.soundManager.PlaySound(_ButtonClick);
        Debug.Log("종료!!");
        Application.Quit();
    }
    #endregion
}