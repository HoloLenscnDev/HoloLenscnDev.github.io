﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Editor;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.WindowsMixedReality.Editor
{
    [CustomEditor(typeof(WindowsMixedRealityCameraSettingsProfile))]
    public class WindowsMixedRealityCameraSettingsProfileInspector : BaseMixedRealityToolkitConfigurationProfileInspector
    {
        private const string ProfileTitle = "Windows Mixed Reality Camera Settings";
        private const string ProfileDescription = "";

        private SerializedProperty renderFromPVCameraForMixedRealityCapture;

        private readonly GUIContent pvCameraRenderingTitle = new GUIContent("Render from PV Camera (Align holograms)");

        private const string MRCDocURL = "https://docs.microsoft.com/en-us/windows/mixed-reality/mixed-reality-capture-for-developers#render-from-the-pv-camera-opt-in";

        protected override void OnEnable()
        {
            base.OnEnable();

            renderFromPVCameraForMixedRealityCapture = serializedObject.FindProperty("renderFromPVCameraForMixedRealityCapture");
        }

        public override void OnInspectorGUI()
        {
            RenderProfileHeader(ProfileTitle, ProfileDescription, target);

            using (new GUIEnabledWrapper(!IsProfileLock((BaseMixedRealityProfile)target)))
            {
                serializedObject.Update();

                EditorGUILayout.Space();
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Mixed Reality Capture Settings (Experimental)", EditorStyles.boldLabel);
                    InspectorUIUtility.RenderDocumentationButton(MRCDocURL);
                }

                EditorGUILayout.HelpBox("Render from PV Camera is supported on Unity 2018.4.13f1 or newer and 2019.3 or newer. Enabling the feature on other versions may result in incorrect capture behavior.", MessageType.Info);

                EditorGUILayout.PropertyField(renderFromPVCameraForMixedRealityCapture, pvCameraRenderingTitle);

                serializedObject.ApplyModifiedProperties();
            }
        }

        protected override bool IsProfileInActiveInstance()
        {
            var profile = target as BaseMixedRealityProfile;

            return MixedRealityToolkit.IsInitialized && profile != null &&
                   MixedRealityToolkit.Instance.HasActiveProfile &&
                   MixedRealityToolkit.Instance.ActiveProfile.CameraProfile != null &&
                   MixedRealityToolkit.Instance.ActiveProfile.CameraProfile.SettingsConfigurations != null &&
                   MixedRealityToolkit.Instance.ActiveProfile.CameraProfile.SettingsConfigurations.Any(s => s.SettingsProfile == profile);
        }
    }
}