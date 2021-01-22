using UnityEditor;
using UnityEngine;

namespace LostPolygon.uLiveWallpaper.Editor.Internal {
    internal static class MenuItems {
        [MenuItem("Tools/Lost Polygon/uLiveWallpaper", priority = 0)]
        private static void OpenLiveWallpaperBuildWindow() {
#if UNITY_ANDROID
            EditorWindow.GetWindow<LiveWallpaperBuildWindow>(true, "uLiveWallpaper", true);
#else
            ShowSwitchBuildTargetToAndroidDialog();
#endif
        }

        [MenuItem("Tools/Lost Polygon/uLiveWallpaper - Update Project %#u", priority = 1)]
        private static void RunUpdateProject() {
#if UNITY_ANDROID
            MenuItemsImplementation.UpdateProject();
#else
            ShowSwitchBuildTargetToAndroidDialog();
#endif
        }

        private static void ShowSwitchBuildTargetToAndroidDialog() {
            if (EditorUtility.DisplayDialog(
                "Wrong build target",
                "Current build target is not set to Android.\n\nSwitch build target to Android?",
                "Switch to Android",
                "Cancel")
                ) {
#if UNITY_5_6 || UNITY_5_6_OR_NEWER
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
#else
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
#endif
            }
        }
    }
}