using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecompositionStep : MonoBehaviour
{
    [SerializeField] private DecompositionObject.RemoveType _RemoveType;

    [SerializeField] private List<DecompositionPart> _DecompositionParts;
    [SerializeField] private GameObject _RemoveObject;

    private int _PartSize;
    private int _CurrentDecomposition;

    private bool _IsComplete;
    public bool IsComplete { get { return _IsComplete; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        _PartSize = _DecompositionParts.Count;

        _CurrentDecomposition = 0;
    }

    public void NextStep()
    {
        for (int i = 0; i < _DecompositionParts.Count; i++)
        {
            _DecompositionParts[i].NextSetup();
        }

        if (_RemoveType.Equals(DecompositionObject.RemoveType.Fade))
        {
            _RemoveObject.SetActive(false);

        }
        else if (_RemoveType.Equals(DecompositionObject.RemoveType.Animation))
        {
            _RemoveObject.GetComponent<Animator>().SetTrigger("PartAnimation");
        }

        _IsComplete = true;
    }

    public void PrevStep()
    {
        for (int i = 0; i < _DecompositionParts.Count; i++)
        {
            _DecompositionParts[i].PrevSetup();
        }

        if(_RemoveType.Equals(DecompositionObject.RemoveType.Fade))
        {
            _RemoveObject.SetActive(true);

        }
        else if(_RemoveType.Equals(DecompositionObject.RemoveType.Animation))
        {
            _RemoveObject.GetComponent<Animator>().SetTrigger("ReturnAnimation");
        }

        _IsComplete = false;
    }
}
