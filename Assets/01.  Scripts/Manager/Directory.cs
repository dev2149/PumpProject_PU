using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directory : MonoBehaviour
{
    #region Singleton
    private static Directory _Instance;
    public static Directory Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindObjectOfType<Directory>();
                if(_Instance == null)
                {
                    GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Directory"));
                    _Instance = obj.GetComponent<Directory>();
                }
                _Instance.Init();
                DontDestroyOnLoad(_Instance);
            }
            return _Instance;
        }
    }
    #endregion
    public Dictionary<string, Step> stepPlanDic = new Dictionary<string, Step>();
    public List<string> stepInfoNameList = new List<string>();
    public SceneController sceneController { get; private set; }
    public SoundManager soundManager { get; private set; }
    public StepDataController stepDataController { get; private set; }
    public VideoPlayerController videoPlayerController { get; private set; }
    public bool Initialize { get; private set; }
    void Init()
    {
        if (Initialize.Equals(false))
        {
            sceneController = GetComponentInChildren<SceneController>();
            soundManager = GetComponentInChildren<SoundManager>();
            stepDataController = GetComponentInChildren<StepDataController>();
            videoPlayerController = GetComponentInChildren<VideoPlayerController>();
            sceneController.Init();
            soundManager.Init();
            stepDataController.Init();
            videoPlayerController.Init();
            StartCoroutine(CheckInit());
        }
    }
    IEnumerator CheckInit()
    {
        while (true)
        {
            if (sceneController.Initialization && soundManager.Initialization
                && stepDataController.Initialization && videoPlayerController.Initialization)
            {
                Initialize = true;
                yield break;
            }
            yield return null;
        }
    }
    public void StartProject()
    {
        Debug.Log("Start");
    }
    public void SetStepClassDataInfo(Step _info)
    {
        stepPlanDic.Add(_info.name, _info);
        stepInfoNameList.Add(_info.name);
    }
}
