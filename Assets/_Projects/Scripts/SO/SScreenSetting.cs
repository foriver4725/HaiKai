﻿using General;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "ScreenSetting", menuName = "SO/ScreenSetting")]
    public class SScreenSetting : ScriptableObject
    {
        [SerializeField, Header("スクリーン設定")]
        private SerializedScreenSetting _serializedScreenSetting;
        internal SerializedScreenSetting SerializedScreenSetting => _serializedScreenSetting;
    }
}