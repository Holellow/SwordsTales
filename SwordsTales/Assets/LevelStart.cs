using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    [SerializeField] public float transitionTime = 1f;

    public Animator transition;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    
}
