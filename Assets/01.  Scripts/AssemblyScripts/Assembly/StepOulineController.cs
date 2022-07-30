using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StepOulineController : MonoBehaviour
{
    public enum _Flag
    { 
        NotOrder,
        Order
    }
    public _Flag _FlagOrder; 
    [Tooltip("현재 스텝의 아웃라인")] [SerializeField] AssemblyStep _OnOutLine;
    [SerializeField] private List<Outline> _OutLine;

    // Order Component
    [SerializeField] GameObject _OutLineObject;
    [SerializeField] AssemblyPart[] _OutLineSc;
    [SerializeField] GameObject[] _OutLIneTest;
    int _OutlineCount;
    int _OrderOutLineCount = 8;
    private void Start()
    {
        _OutlineCount = _OutLine.Count;
        for (int i = 0; i < _OutlineCount; i++)
        {
            _OutLine[i].gameObject.SetActive(false);
        }
        _OutLineSc = new AssemblyPart[8];
        _OutLIneTest = new GameObject[8];
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
        {
            _FlagOrder = _Flag.NotOrder;
        }
    }
    public void SetNotOrderOutLine()
    {
        if (_OnOutLine.OutLineOn && _FlagOrder.Equals(_Flag.NotOrder) && Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
        {
            for (int i = 0; i < _OutlineCount; i++)
            {
                _OutLine[i].gameObject.SetActive(true);
            }
        }
    }
    public void OrderFirstOutLineStep()
    {
        if (_OnOutLine.OutLineOn && _FlagOrder.Equals(_Flag.Order) && Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
        {



            for (int i = 0; i < _OrderOutLineCount; i++)
            {

                //if (_OutLineObject.gameObject.transform.GetChild(i).gameObject.activeSelf)
                //{
                    _OutLineSc[i] = _OutLineObject.gameObject.transform.GetChild(i).gameObject.GetComponent<AssemblyPart>();
                //}
                _OutLIneTest[i] = _OnOutLine.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < _OutLIneTest.Length; i++)
            {
                if (_OutLineSc[i] != null && _OutLineSc[i].transform.position == _OutLIneTest[0].transform.position)
                {
                    _OutLineSc[i].gameObject.GetComponent<AssemblyPart>().ActiveHightLight(true);
                }
            }
            //_OutLineSc[4].ActiveHightLight(true);
        }
    }
    public void OutLineStep(int _num)
    {
        if (_OnOutLine.OutLineOn && _FlagOrder.Equals(_Flag.Order) && _num <= _OutlineCount)
        {

            switch (_num)
            {
                case 1:
                    for (int i = 0; i < _OutLIneTest.Length; i++)
                    {
                        if (_OutLineSc[i] != null && _OutLineSc[i].transform.position == _OutLIneTest[3].transform.position)
                        {
                            _OutLineSc[i].gameObject.GetComponent<AssemblyPart>().ActiveHightLight(true);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < _OutLIneTest.Length; i++)
                    {
                        if (_OutLineSc[i] != null && _OutLineSc[i].transform.position == _OutLIneTest[1].transform.position)
                        {
                            _OutLineSc[i].gameObject.GetComponent<AssemblyPart>().ActiveHightLight(true);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < _OutLIneTest.Length; i++)
                    {
                        if (_OutLineSc[i] != null && _OutLineSc[i].transform.position == _OutLIneTest[2].transform.position)
                        {
                            _OutLineSc[i].gameObject.GetComponent<AssemblyPart>().ActiveHightLight(true);
                        }
                    }
                    break;
            }
        }
    }
}