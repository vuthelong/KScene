#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace Kingfisher.KScene
{
    [Overlay(typeof(SceneView), "K-Scene Bookmarks", true)]
    public class KSceneOverlay : IMGUIOverlay
    {
        #region Field

        private const float MinListWidth = 200f;
        private const float JumpButtonWidth = 34f;
        private const float DeleteButtonWidth = 20f;
        private const float AddButtonHeight = 20f;

        private const string DisabledLabel = "K-Scene is disabled.";
        private const string EmptyLabel = "No bookmarks yet.";
        private const string AddButtonLabel = "+ Save current view";

        private static readonly GUIContent JumpButtonContent = new("Go", "Jump the Scene View camera to this bookmark");
        private static readonly GUIContent DeleteButtonContent = new("x", "Delete this bookmark");

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

            DrawBookmarks(sceneView);

            if (GUILayout.Button(AddButtonLabel, GUILayout.Height(AddButtonHeight)))
                KScene.AddBookmark(sceneView);

            GUILayout.EndVertical();
        }

        #endregion

        #region Private Methods

        private static void DrawBookmarks(SceneView sceneView)
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
                if (DrawRow(sceneView, data.bookmarks[i])) toRemove = data.bookmarks[i];
            }

            if (toRemove != null) KScene.RemoveBookmark(toRemove);
        }

        private static bool DrawRow(SceneView sceneView, KSceneData.Bookmark bookmark)
        {
            var isDeleted = false;

            GUILayout.BeginHorizontal();

            var newName = EditorGUILayout.TextField(bookmark.name);

            if (newName != bookmark.name) KScene.Rename(bookmark, newName);

            if (GUILayout.Button(JumpButtonContent, GUILayout.Width(JumpButtonWidth)))
                KScene.JumpTo(sceneView, bookmark);

            if (GUILayout.Button(DeleteButtonContent, GUILayout.Width(DeleteButtonWidth)))
                isDeleted = true;

            GUILayout.EndHorizontal();

            return isDeleted;
        }

        #endregion
    }
}
#endif
