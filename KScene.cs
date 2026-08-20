#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using static Kingfisher.KScene.Libs.KUtils;

namespace Kingfisher.KScene
{
    public static class KScene
    {
        #region Field

        public const string DataFileName = "KScene Data.asset";

        private const string DebugLogPrefix = "[KSCN-DEBUG]";
        private const string DebugTimeFormat = "HH:mm:ss.fff";

        private const string DefaultBookmarkNamePrefix = "Bookmark ";

        public static KSceneData Data;

        #endregion

        #region Entry Point

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (KSceneMenu.PluginDisabled) return;

            Data = Libs.KData.Load<KSceneData>(DataFileName);

            if (Data)
            {
                Libs.KData.Autosave(Data, DataFileName);

                return;
            }

            Data = Libs.KData.Create<KSceneData>(DataFileName);
        }

        public static void DeleteData()
        {
            Libs.KData.Delete(DataFileName);

            if (Data) Object.DestroyImmediate(Data);

            Data = null;
        }

        #endregion

        #region Public Methods

        public static KSceneData.Bookmark AddBookmark(SceneView sceneView)
        {
            EnsureData();

            if (!sceneView) return null;

            Undo.RecordObject(Data, "Add Bookmark");

            var bookmark = new KSceneData.Bookmark
            {
                name = DefaultBookmarkNamePrefix + ++Data.bookmarkCounter,
                pivot = sceneView.pivot,
                rotation = sceneView.rotation,
                size = sceneView.size,
                orthographic = sceneView.orthographic,
            };

            Data.bookmarks.Add(bookmark);

            Data.Dirty();

            LogDebug(nameof(AddBookmark), bookmark.name);

            return bookmark;
        }

        public static void JumpTo(SceneView sceneView, KSceneData.Bookmark bookmark)
        {
            if (bookmark == null || !sceneView) return;

            sceneView.LookAt(bookmark.pivot, bookmark.rotation, bookmark.size, bookmark.orthographic, true);

            LogDebug(nameof(JumpTo), bookmark.name);
        }

        public static void Rename(KSceneData.Bookmark bookmark, string newName)
        {
            if (bookmark == null || Data == null) return;

            Undo.RecordObject(Data, "Rename Bookmark");

            bookmark.name = newName;

            Data.Dirty();
        }

        public static void RemoveBookmark(KSceneData.Bookmark bookmark)
        {
            if (bookmark == null || Data == null) return;

            Undo.RecordObject(Data, "Remove Bookmark");

            Data.bookmarks.Remove(bookmark);

            Data.Dirty();

            LogDebug(nameof(RemoveBookmark), bookmark.name);
        }

        #endregion

        #region Private Methods

        private static void EnsureData()
        {
            if (Data) return;

            Data = Libs.KData.Load<KSceneData>(DataFileName) ?? Libs.KData.Create<KSceneData>(DataFileName);

            Libs.KData.Autosave(Data, DataFileName);
        }

        private static void LogDebug(string methodName, string bookmarkName)
        {
            if (!KSceneMenu.DebugLoggingEnabled) return;

            Debug.Log($"{DebugLogPrefix} {methodName} '{bookmarkName}' @ {DateTime.UtcNow.ToString(DebugTimeFormat)}");
        }

        #endregion
    }
}
#endif
