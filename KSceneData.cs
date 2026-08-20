#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kingfisher.KScene
{
    public class KSceneData : ScriptableObject
    {
        #region Field

        public List<Bookmark> bookmarks = new();

        public int bookmarkCounter;

        #endregion

        #region Nested Type

        [Serializable]
        public class Bookmark
        {
            public string name = "";

            public Vector3 pivot;
            public Quaternion rotation = Quaternion.identity;
            public float size = 10f;
            public bool orthographic;
        }

        #endregion
    }
}
#endif
