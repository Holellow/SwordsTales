using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    private GameObject _gameObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _gameObject = other.gameObject;
        Destroy(_gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
