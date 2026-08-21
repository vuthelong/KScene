#if UNITY_EDITOR
using UnityEditor;
using static Kingfisher.KScene.Libs.KUtils;

namespace Kingfisher.KScene
{
    public class KSceneMenu
    {
        #region Field

        private const string KeyPrefix = "KScene-kingfisher-";

        private const string PluginDisabledKey = KeyPrefix + "pluginDisabled";
        private const string SmoothTransitionsEnabledKey = KeyPrefix + "smoothTransitionsEnabled";

        private const string ExportMenuPath = "Tools/Kingfisher/KScene/Export Bookmarks...";
        private const string ImportMenuPath = "Tools/Kingfisher/KScene/Import Bookmarks...";

        private const string ExportDialogTitle = "Export K-Scene Bookmarks";
        private const string ImportDialogTitle = "Import K-Scene Bookmarks";
        private const string DefaultExportFileName = "KScene Bookmarks";
        private const string JsonExtension = "json";

        public static readonly string[] SettingsLayout =
        {
            "# Bookmarks",
            "SmoothTransitionsEnabled|Smooth camera transitions",
        };

        #endregion

        #region Property

        public static bool SmoothTransitionsEnabled { get => EditorPrefsCached.GetBool(SmoothTransitionsEnabledKey, false); set => EditorPrefsCached.SetBool(SmoothTransitionsEnabledKey, value); }

        public static bool PluginDisabled
        {
            get => EditorPrefsCached.GetBool(PluginDisabledKey, false);
            set
            {
                EditorPrefsCached.SetBool(PluginDisabledKey, value);

                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
        }

        #endregion

        #region Method

        public static void DeleteData() => KScene.DeleteData();

        [MenuItem(ExportMenuPath)]
        private static void ExportBookmarks()
        {
            var path = EditorUtility.SaveFilePanel(ExportDialogTitle, "", DefaultExportFileName, JsonExtension);

            if (string.IsNullOrEmpty(path)) return;

            KScene.ExportBookmarks(path);
        }

        [MenuItem(ImportMenuPath)]
        private static void ImportBookmarks()
        {
            var path = EditorUtility.OpenFilePanel(ImportDialogTitle, "", JsonExtension);

            if (string.IsNullOrEmpty(path)) return;

            KScene.ImportBookmarks(path);
        }

        #endregion
    }
}
#endif
