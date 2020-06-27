using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float _activeTime = 0.1f;
    private float _timeActivated;
    private float _alpha;
    [SerializeField]
    private float _alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;
    [SerializeField]
    private Transform check;
    private Transform _player;
    private SpriteRenderer _sr;
    private SpriteRenderer _playerSr;

    private Color color;

    private void OnEnable()
    {
        
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        check = _player.transform;
        _playerSr = _player.GetComponent<SpriteRenderer>();
        
       
        _alpha = _alphaSet;
        _sr.sprite = _playerSr.sprite;
        _sr.flipX = _playerSr.flipX;
        transform.position = _player.position;
        transform.rotation = _player.rotation;
        _timeActivated = Time.time;
    }

    private void Update()
    {
        
        _alpha *= alphaMultiplier;
        color = new Color(1f,1f,1f,_alpha);
        _sr.color = color;

        if (Time.time >= _timeActivated + _activeTime)
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
