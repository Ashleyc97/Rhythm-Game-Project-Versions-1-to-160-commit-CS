﻿using System;
using System.Collections.Generic;
[Serializable]
public class Beatmap
{
    /*
    public float hitObjectPositionX;
    public float hitObjectPositionY;
    public float hitObjectPositionZ;
    */

    public List<float> HitObjectSpawnTime = new List<float>();

    public List<float> PositionX = new List<float>();
    public List<float> PositionY = new List<float>();
    public List<float> PositionZ = new List<float>();
}
