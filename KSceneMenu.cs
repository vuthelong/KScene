#if UNITY_EDITOR
using static Kingfisher.KScene.Libs.KUtils;

namespace Kingfisher.KScene
{
    public class KSceneMenu
    {
        #region Field

        private const string KeyPrefix = "KScene-kingfisher-";

        private const string PluginDisabledKey = KeyPrefix + "pluginDisabled";
        private const string DebugLoggingKey = KeyPrefix + "debugLoggingEnabled";

        public static readonly string[] SettingsLayout =
        {
            "# Debug",
            "DebugLoggingEnabled|Enable debug logging",
        };

        #endregion

        #region Property

        public static bool DebugLoggingEnabled { get => EditorPrefsCached.GetBool(DebugLoggingKey, false); set => EditorPrefsCached.SetBool(DebugLoggingKey, value); }

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

        #endregion
    }
}
#endif
