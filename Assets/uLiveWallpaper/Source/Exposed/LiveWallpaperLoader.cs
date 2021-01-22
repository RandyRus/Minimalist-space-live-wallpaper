#if UNITY_ANDROID

using UnityEngine;

namespace LostPolygon.uLiveWallpaper.Internal {
    internal static class LiveWallpaperLoader {
        [RuntimeInitializeOnLoadMethod]
        private static void OnLoad() {
            Load();
        }

        private static void Load() {
            // Call the static constructor
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(LiveWallpaper).TypeHandle);
        }
    }
}

#endif