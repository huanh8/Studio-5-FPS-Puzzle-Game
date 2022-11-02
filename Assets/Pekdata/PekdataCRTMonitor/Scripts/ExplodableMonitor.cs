﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableMonitor : MonoBehaviour
{
    [SerializeField]
    private GameObject screenExplosionParticleSystem;
    [SerializeField]
    private GameObject screenOff;
    [SerializeField]
    private GameObject screenOn;
    [SerializeField]
    private GameObject shards;
    private bool broken;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision col){
        Debug.Log("Collision");
        Debug.Log(col.gameObject.name);
        if ((col.gameObject.name =="Floor Tile") && (!broken))
        {
             Debug.Log(col.gameObject.name);
            broken = true;
            screenOff.SetActive(false);
            screenOn.SetActive(false);
            shards.SetActive(true);
            Rigidbody[] shardRBs = GetComponentsInChildren<Rigidbody>();
            screenExplosionParticleSystem.SetActive(true);
            foreach (Rigidbody shardRB in shardRBs){
                float randomForce = Random.Range(1,5);
                float randomRotationX = Random.Range(-20,20);
                float randomRotationY = Random.Range(-20,20);
                float randomRotationZ = Random.Range(-20,20);
                shardRB.transform.Rotate(randomRotationX,randomRotationY,randomRotationZ);
                shardRB.AddRelativeForce(Vector3.forward * randomForce,ForceMode.Impulse);
            }
        }

    }
}