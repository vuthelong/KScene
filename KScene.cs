#if UNITY_EDITOR
using System;
using System.IO;
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

        private const string DefaultBookmarkNamePrefix = "Bookmark ";
        private const string HomeBookmarkName = "Home";

        private const int ThumbnailWidth = 96;
        private const int ThumbnailHeight = 54;

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
                thumbnailPng = CaptureThumbnail(sceneView),
            };

            Data.bookmarks.Add(bookmark);

            Data.Dirty();

            return bookmark;
        }

        public static void JumpTo(SceneView sceneView, KSceneData.Bookmark bookmark)
        {
            if (bookmark == null || !sceneView) return;

            sceneView.LookAt(bookmark.pivot, bookmark.rotation, bookmark.size, bookmark.orthographic, !KSceneMenu.SmoothTransitionsEnabled);
        }

        public static void SetHome(SceneView sceneView)
        {
            EnsureData();

            if (!sceneView) return;

            Undo.RecordObject(Data, "Set Home Bookmark");

            Data.home = new KSceneData.Bookmark
            {
                name = HomeBookmarkName,
                pivot = sceneView.pivot,
                rotation = sceneView.rotation,
                size = sceneView.size,
                orthographic = sceneView.orthographic,
                thumbnailPng = CaptureThumbnail(sceneView),
            };

            Data.Dirty();
        }

        public static void GoHome(SceneView sceneView) => JumpTo(sceneView, Data ? Data.home : null);

        public static void ExportBookmarks(string filePath)
        {
            EnsureData();

            var list = new KSceneData.BookmarkList { bookmarks = Data.bookmarks };

            File.WriteAllText(filePath, JsonUtility.ToJson(list, true));
        }

        public static void ImportBookmarks(string filePath)
        {
            EnsureData();

            var list = JsonUtility.FromJson<KSceneData.BookmarkList>(File.ReadAllText(filePath));

            if (list?.bookmarks == null) return;

            Undo.RecordObject(Data, "Import Bookmarks");

            Data.bookmarks.AddRange(list.bookmarks);

            Data.Dirty();
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
        }

        #endregion

        #region Private Methods

        private static void EnsureData()
        {
            if (Data) return;

            Data = Libs.KData.Load<KSceneData>(DataFileName) ?? Libs.KData.Create<KSceneData>(DataFileName);

            Libs.KData.Autosave(Data, DataFileName);
        }

        private static byte[] CaptureThumbnail(SceneView sceneView)
        {
            var camera = sceneView.camera;

            if (!camera) return null;

            var renderTexture = RenderTexture.GetTemporary(ThumbnailWidth, ThumbnailHeight, 24);
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;

            camera.targetTexture = renderTexture;
            camera.Render();
            RenderTexture.active = renderTexture;

            var texture = new Texture2D(ThumbnailWidth, ThumbnailHeight, TextureFormat.RGB24, false);

            texture.ReadPixels(new Rect(0, 0, ThumbnailWidth, ThumbnailHeight), 0, 0);
            texture.Apply();

            camera.targetTexture = previousTarget;
            RenderTexture.active = previousActive;

            RenderTexture.ReleaseTemporary(renderTexture);

            var png = texture.EncodeToPNG();

            Object.DestroyImmediate(texture);

            return png;
        }

        #endregion
    }
}
#endif
