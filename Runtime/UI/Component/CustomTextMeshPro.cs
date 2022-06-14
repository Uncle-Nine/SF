﻿using System;
using System.Globalization;
using Framework.MessageCenter;
using Framework.UI.Wrap.Base;
using TMPro;
using UnityEngine;

namespace Framework.UIComponent
{
    public class CustomTextMeshPro : TextMeshProUGUI, IFieldChangeCb<bool>, IFieldChangeCb<string>, IFieldChangeCb<int>, IFieldChangeCb<float>, IFieldChangeCb<long>, IFieldChangeCb<double>
    {
        [SerializeField]
        private Color32 customOutLineColor;
        [SerializeField, Range(0f, 1f)]
        private float customOutLineWidth;
        [SerializeField]
        private string languageKey;
        
        public static event Func<string, string> GetLanguageStr;

        protected override void Awake()
        {
            base.Awake();
            Message.defaultEvent.Register(this,"Language", () =>
            {
                if (GetLanguageStr != null && !string.IsNullOrEmpty(languageKey)) text = GetLanguageStr(languageKey);
            });
            if (GetLanguageStr != null && !string.IsNullOrEmpty(languageKey)) text = GetLanguageStr(languageKey);
        }

        protected override void Start()
        {
            base.Start();
            outlineColor = customOutLineColor;
            outlineWidth = customOutLineWidth;
        }

        Action<bool> IFieldChangeCb<bool>.GetFieldChangeCb()
        {
            return b =>
            {
                text = b.ToString();
            };
        }

        Action<string> IFieldChangeCb<string>.GetFieldChangeCb()
        {
            return b =>
            {
                text = b.ToString();
            };
        }

        Action<int> IFieldChangeCb<int>.GetFieldChangeCb()
        {
            return b =>
            {
                text = b.ToString();
            };
        }

        Action<float> IFieldChangeCb<float>.GetFieldChangeCb()
        {
            return b =>
            {
                text = b.ToString(CultureInfo.InvariantCulture);
            };
        }

        Action<long> IFieldChangeCb<long>.GetFieldChangeCb()
        {
            return b =>
            {
                text = b.ToString();
            };
        }

        Action<double> IFieldChangeCb<double>.GetFieldChangeCb()
        {
            return b =>
            {
                text = b.ToString(CultureInfo.InvariantCulture);
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Message.defaultEvent.Unregister(this);
        }
    }
}