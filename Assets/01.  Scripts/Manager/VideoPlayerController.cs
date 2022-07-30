using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoPlayerController : MonoBehaviour
{
    public VideoClip[] _VideoGroup; 
    public bool Initialization { get; private set; }
    public void Init()
    {
        ChildLoad();
        Initialization = true;
    }
    void ChildLoad()
    {

    }
}
