using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{

    private bool fix;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private RuntimeAnimatorController playerContr;
    [SerializeField] private PlayableDirector _director;

    private void OnEnable()
    {
        playerContr = playerAnim.runtimeAnimatorController;
        bool fix = false;
        playerAnim.runtimeAnimatorController = null;
    }

    void Update()
    {
        if (_director.state != PlayState.Playing && !fix)
        {
            fix = true;
            playerAnim.runtimeAnimatorController = playerContr;
        } 
    }
}
