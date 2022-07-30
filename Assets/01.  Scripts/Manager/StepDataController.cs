using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class StepDataController : MonoBehaviour
{
    public bool Initialization { get; private set; }
    public Rootobject rootobject;
    public void Init()
    {
        LoadingData();
        Initialization = true;
    }
    void LoadingData()
    {
        StartCoroutine(LoadDataCoroutine());
    }
    IEnumerator LoadDataCoroutine()
    {
        TextAsset[] textAssetRootObject = Resources.LoadAll<TextAsset>("Explanation");
        string jsonDataRootObject = textAssetRootObject[0].text;
        rootobject = JsonUtility.FromJson<Rootobject>(jsonDataRootObject);
        SetClassDataController();
        yield return null;
    }
    public void SetClassDataController()
    {
        for (int i = 0; i < rootobject.step.Length; i++)
        {
            Step tempStep = new Step(rootobject.step[i]);
            Directory.Instance.SetStepClassDataInfo(tempStep);
        }
    }
}
