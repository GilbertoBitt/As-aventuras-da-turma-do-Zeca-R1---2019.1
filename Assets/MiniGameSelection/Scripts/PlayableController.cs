using System.Collections;
using System.Collections.Generic;
using com.csutil;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class PlayableController : MonoBehaviour
{
    private PlayableDirector _playableDirector;

    public double time;
    // Start is called before the first frame update
    void Start()
    {
        _playableDirector = GetComponent(typeof(PlayableDirector)) as PlayableDirector;
    }

    public void PausePlayableDirector()
    {
        time = _playableDirector.time;
        _playableDirector.Pause();
        _playableDirector.enabled = false;

    }

    [Button("Resume Playable")]
    public void ResumePlayableDirector()
    {
        _playableDirector.initialTime = time;
        _playableDirector.time = time;
        _playableDirector.Play();
        _playableDirector.enabled = true;
    }
}
