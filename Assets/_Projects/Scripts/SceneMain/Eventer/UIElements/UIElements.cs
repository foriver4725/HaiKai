﻿using System;
using System.Threading;
using UnityEngine;
using SceneGeneral;
using Sirenix.OdinInspector;
using System.Collections.ObjectModel;

namespace Main.Eventer.UIElements
{
    [Serializable]
    public sealed class UIElements
    {
        [SerializeField, Required, SceneObjectsOnly]
        private ReticleClass _reticle;
        public ReticleClass Reticle => _reticle;

        [SerializeField, Required, SceneObjectsOnly]
        private BlackImageClass _blackImage;
        public BlackImageClass BlackImage => _blackImage;

        [SerializeField, Required, SceneObjectsOnly]
        private LogTextClass _logText;
        public LogTextClass LogText => _logText;

        [SerializeField, Required, SceneObjectsOnly]
        private ManageItemUI _manageItemUI;

        [SerializeField, Required, SceneObjectsOnly]
        private LabelClass _cutSceneSkipLabel;
        public LabelClass CutSceneSkipLabel => _cutSceneSkipLabel;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _daughterKnife;
        public UIItemClass DaughterKnife => _daughterKnife;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _warehouseKeyDoubled;
        public UIItemClass WarehouseKeyDoubled => _warehouseKeyDoubled;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _warehouseKey;
        public UIItemClass WarehouseKey => _warehouseKey;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _cup;
        public UIItemClass Cup => _cup;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _cupFilledWithBlood;
        public UIItemClass CupFilledWithBlood => _cupFilledWithBlood;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _kokeshiSecretKey;
        public UIItemClass KokeshiSecretKey => _kokeshiSecretKey;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _glassShard;
        public UIItemClass GlassShard => _glassShard;

        [SerializeField, Required, AssetsOnly]
        private UIItemClass _keyInDoorPuzzleSolving;
        public UIItemClass KeyInDoorPuzzleSolving => _keyInDoorPuzzleSolving;

        [SerializeField, Required, SceneObjectsOnly]
        private ViewingItemClass _kokeshiScroll;
        public ViewingItemClass KokeshiScroll => _kokeshiScroll;

        private UIItemClass[] _keysInFinalKey2Door = null;
        public ReadOnlyCollection<UIItemClass> KeysInFinalKey2Door
        {
            get
            {
                _keysInFinalKey2Door ??= new UIItemClass[] { _kokeshiSecretKey, _keyInDoorPuzzleSolving };
                return Array.AsReadOnly(_keysInFinalKey2Door);
            }
        }

        // 最初に呼ぶこと！
        public void Init()
        {
            Init(_daughterKnife);
            Init(_warehouseKeyDoubled);
            Init(_warehouseKey);
            Init(_cup);
            Init(_cupFilledWithBlood);
            Init(_kokeshiSecretKey);
            Init(_glassShard);
            Init(_keyInDoorPuzzleSolving);

            void Init(UIItemClass uiItemClass) => uiItemClass?.Init(_manageItemUI);
        }

        public bool IsHoldingAnyItem()
        {
            if (_daughterKnife?.IsHolding() is true) return true;
            if (_warehouseKeyDoubled?.IsHolding() is true) return true;
            if (_warehouseKey?.IsHolding() is true) return true;
            if (_cup?.IsHolding() is true) return true;
            if (_cupFilledWithBlood?.IsHolding() is true) return true;
            if (_kokeshiSecretKey?.IsHolding() is true) return true;
            if (_glassShard?.IsHolding() is true) return true;
            if (_keyInDoorPuzzleSolving?.IsHolding() is true) return true;
            return false;
        }
    }

    public static class UIElementsEx
    {
        public static UIItemClass IsHoldingAny(this ReadOnlyCollection<UIItemClass> uiItemClasses)
        {
            foreach (var uiItemClass in uiItemClasses)
            {
                if (uiItemClass.IsHolding() is true) return uiItemClass;
            }

            return null;
        }
    }
}