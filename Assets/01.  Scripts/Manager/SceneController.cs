using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
[System.Serializable]
public enum SceneIdx
{
    Title,
    Decomposition,
    Tutorial
}
[System.Serializable]
public enum Difficulty
{
    Nomal,
    Hard
}
public class SceneController : MonoBehaviour
{
    public bool Initialization { get; private set; }
    public SceneIdx _CurrentSceneIdx { get; private set; }
    public Difficulty _difficulty;
    public void Init()
    {
        _CurrentSceneIdx = SceneIdx.Title;
        Initialization = true;
    }
    public void LoadScene(SceneIdx _idx)
    {
        _CurrentSceneIdx = _idx;
        Directory.Instance.soundManager.AllStopSound();// 씬 이동 할때 사운드 스탑
        StartCoroutine(ChangeScene(_CurrentSceneIdx));
        //if (!_CurrentSceneIdx.Equals(_idx)){
        //    _CurrentSceneIdx = _idx;
        //    Directory.Instance.soundManager.AllStopSound();// 씬 이동 할때 사운드 스탑
        //    StartCoroutine(ChangeScene(_CurrentSceneIdx));
        //}
    }
    public void StartOnMoveTitle(SceneIdx _idx)
    {
        if (!_CurrentSceneIdx.Equals(_idx))
        {
            _CurrentSceneIdx = _idx;
            SceneManager.LoadScene((int)_idx);
        }
    }
    public IEnumerator ChangeScene(SceneIdx _idx)
    {
        SteamVR_Fade.View(Color.black, 1);
        SteamVR_Fade.Start(Color.black, 1);
        yield return new WaitForSeconds(1f);
        Debug.Log(_idx);
        SceneManager.LoadScene((int)_idx);
        SteamVR_Fade.View(Color.clear, 1);
        SteamVR_Fade.Start(Color.clear, 1);
    }
}
