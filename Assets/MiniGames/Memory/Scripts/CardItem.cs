﻿using System;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
using UnityEditor;
#endif
using UnityEngine;

namespace MiniGames.Memory.Scripts
{
    public class CardItem : ScriptableObject
    {
        public int idItem;
        public Sprite SpriteItem;
        public string NameItem;
        public AudioClip AudioClipItem;
    }
}