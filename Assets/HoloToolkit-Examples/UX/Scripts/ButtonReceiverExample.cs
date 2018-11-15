﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Receivers;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity.Examples
{
    public class ButtonReceiverExample : InteractionReceiver
    {
        public GameObject textObjectState;
        private TextMesh txt;

        void Start()
        {
            txt = textObjectState.GetComponentInChildren<TextMesh>();
        }

        protected override void FocusEnter(GameObject obj, PointerSpecificEventData eventData) {
            Debug.Log(obj.name + " : FocusEnter");
            txt.text = obj.name + " : FocusEnter";
        }

        protected override void FocusExit(GameObject obj, PointerSpecificEventData eventData) {
            Debug.Log(obj.name + " : FocusExit");
            txt.text = obj.name + " : FocusExit";
        }

        protected override void InputDown(GameObject obj, InputEventData eventData) {
            Debug.Log(obj.name + " : InputDown");
            txt.text = obj.name + " : InputDown";
        }

        protected override void InputUp(GameObject obj, InputEventData eventData) {
            Debug.Log(obj.name + " : InputUp");
            txt.text = obj.name + " : InputUp";
        }
    }
}
