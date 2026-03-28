using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimatedSpriteSet
{
    public float animationTime = 1f;
    public bool loop = false;
    public List<Sprite> sprites = new List<Sprite>();
}