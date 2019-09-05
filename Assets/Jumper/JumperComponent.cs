﻿using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Jumper
{
    public class JumperComponent : MonoBehaviour
    {
        public ISkippable skippable;
        
        public void CallSkipCommand()
        {
            if (GameConfig.Instance.clientID == 19)
            {
                skippable.SkipCommand();
            }
        }
        
        public void CallISkipCommand(MonoBehaviour target)
        {
            if (GameConfig.Instance.clientID != 19) return;
            var skipper = target as ISkippable;
            skipper?.SkipCommand();
        }
    }
}