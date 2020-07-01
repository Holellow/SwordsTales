using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snare : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Transform snareTransform;
    
    private float[] attackdetails = new float[2];
    
    private void Start()
    {
        attackdetails[1] = snareTransform.position.x;
        attackdetails[0] = damage;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("Damage",attackdetails);
        }
    }
}
