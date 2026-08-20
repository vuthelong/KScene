#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using Object = UnityEngine.Object;
using static Kingfisher.KScene.Libs.KUtils;

namespace Kingfisher.KScene
{
    [Overlay(typeof(SceneView), "K-Scene Bookmarks", true)]
    public class KSceneOverlay : IMGUIOverlay
    {
        #region Field

        private const float MinListWidth = 200f;
        private const float JumpButtonWidth = 34f;
        private const float DeleteButtonWidth = 20f;
        private const float SetHomeButtonWidth = 40f;
        private const float AddButtonHeight = 20f;
        private const float ThumbnailWidth = 32f;
        private const float ThumbnailHeight = 18f;

        private const string DisabledLabel = "K-Scene is disabled.";
        private const string EmptyLabel = "No bookmarks yet.";
        private const string NoHomeLabel = "No home set.";
        private const string AddButtonLabel = "+ Save current view";
        private const string SetHomeButtonLabel = "Set";

        private static readonly GUIContent JumpButtonContent = new("Go", "Jump the Scene View camera to this bookmark");
        private static readonly GUIContent DeleteButtonContent = new("x", "Delete this bookmark");
        private static readonly GUIContent SetHomeButtonContent = new(SetHomeButtonLabel, "Save the current view as Home");

        private readonly Dictionary<KSceneData.Bookmark, Texture2D> _thumbnailCache = new();

        private string _filter = "";

        #endregion

        #region Unity Callbacks

        public override void OnGUI()
        {
            if (KSceneMenu.PluginDisabled)
            {
                GUILayout.Label(DisabledLabel);

                return;
            }

            var sceneView = containerWindow as SceneView;

            GUILayout.BeginVertical(GUILayout.MinWidth(MinListWidth));

            DrawHome(sceneView);

            this._filter = EditorGUILayout.TextField(this._filter, EditorStyles.toolbarSearchField);

            DrawBookmarks(sceneView);

            if (GUILayout.Button(AddButtonLabel, GUILayout.Height(AddButtonHeight)))
                KScene.AddBookmark(sceneView);

            GUILayout.EndVertical();
        }

        public override void OnWillBeDestroyed()
        {
            foreach (var texture in this._thumbnailCache.Values)
            {
                if (texture) Object.DestroyImmediate(texture);
            }

            this._thumbnailCache.Clear();
        }

        #endregion

        #region Private Methods

        private void DrawHome(SceneView sceneView)
        {
            var home = KScene.Data ? KScene.Data.home : null;

            GUILayout.BeginHorizontal();

            if (home != null)
            {
                DrawThumbnail(home);
                GUILayout.Label(home.name, GUILayout.ExpandWidth(true));

                if (GUILayout.Button(JumpButtonContent, GUILayout.Width(JumpButtonWidth)))
                    KScene.GoHome(sceneView);
            }
            else
            {
                GUILayout.Label(NoHomeLabel, GUILayout.ExpandWidth(true));
            }

            if (GUILayout.Button(SetHomeButtonContent, GUILayout.Width(SetHomeButtonWidth)))
                KScene.SetHome(sceneView);

            GUILayout.EndHorizontal();
        }

        private void DrawBookmarks(SceneView sceneView)
        {
            var data = KScene.Data;

            if (data == null || data.bookmarks.Count == 0)
            {
                GUILayout.Label(EmptyLabel);

                return;
            }

            KSceneData.Bookmark toRemove = null;

            for (var i = 0; i < data.bookmarks.Count; i++)
            {
                var bookmark = data.bookmarks[i];

                if (!this._filter.IsNullOrEmpty() && bookmark.name.IndexOf(this._filter, StringComparison.OrdinalIgnoreCase) < 0) continue;

                if (DrawRow(sceneView, bookmark)) toRemove = bookmark;
            }

            if (toRemove == null) return;

            ReleaseThumbnail(toRemove);

            KScene.RemoveBookmark(toRemove);
        }

        private bool DrawRow(SceneView sceneView, KSceneData.Bookmark bookmark)
        {
            var isDeleted = false;

            GUILayout.BeginHorizontal();

            DrawThumbnail(bookmark);

            var newName = EditorGUILayout.TextField(bookmark.name);

            if (newName != bookmark.name) KScene.Rename(bookmark, newName);

            if (GUILayout.Button(JumpButtonContent, GUILayout.Width(JumpButtonWidth)))
                KScene.JumpTo(sceneView, bookmark);

            if (GUILayout.Button(DeleteButtonContent, GUILayout.Width(DeleteButtonWidth)))
                isDeleted = true;

            GUILayout.EndHorizontal();

            return isDeleted;
        }

        private void DrawThumbnail(KSceneData.Bookmark bookmark)
        {
            var rect = GUILayoutUtility.GetRect(ThumbnailWidth, ThumbnailHeight, GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));
            var texture = GetThumbnail(bookmark);

            if (texture) GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit);
        }

        private Texture2D GetThumbnail(KSceneData.Bookmark bookmark)
        {
            if (bookmark.thumbnailPng == null || bookmark.thumbnailPng.Length == 0) return null;

            if (this._thumbnailCache.TryGetValue(bookmark, out var cached) && cached) return cached;

            var texture = new Texture2D(2, 2, TextureFormat.RGB24, false);

            texture.LoadImage(bookmark.thumbnailPng);

            this._thumbnailCache[bookmark] = texture;

            return texture;
        }

        private void ReleaseThumbnail(KSceneData.Bookmark bookmark)
        {
            if (!this._thumbnailCache.TryGetValue(bookmark, out var texture)) return;

            if (texture) Object.DestroyImmediate(texture);

            this._thumbnailCache.Remove(bookmark);
        }

        #endregion
    }
}
#endif
