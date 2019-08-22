﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEditorPlacedHitObject : MonoBehaviour {

    private float timer;
    private float destroyTime = 0.2f; // Hit object destroy time

    // Use this for initialization
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increments timer
        timer += Time.deltaTime;

        // Destroy the game object
        if (timer >= destroyTime)
        {
            DestroyHitObject();
        }
    }

    // Destory hit object
    public void DestroyHitObject()
    {
        Destroy(gameObject);
    }
}
