using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAssemblyOutLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Nomal))
        {
            this.gameObject.SetActive(true);
        }
        if (Directory.Instance.sceneController._difficulty.Equals(Difficulty.Hard))
        {
            this.gameObject.SetActive(false);
        }
    }

}
