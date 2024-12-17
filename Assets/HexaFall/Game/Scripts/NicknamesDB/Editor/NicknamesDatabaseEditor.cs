using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HexFall
{

    [CustomEditor(typeof(NicknamesDatabase))]
    public class NicknamesDatabaseEditor : Editor
    {
        private MethodInfo parseNamesAndAddToDBMethodInfo;

        protected void OnEnable()
        {
            parseNamesAndAddToDBMethodInfo = serializedObject.targetObject.GetType().GetMethod("ParseNamesAndAddToDB", BindingFlags.Public | BindingFlags.Instance);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.DrawDefaultInspector();


            EditorGUILayout.HelpBox("Supported separators: \";\"", MessageType.Info);
            TextAsset newPrefab = (TextAsset)EditorGUILayout.ObjectField("Parse And Add: ", null, typeof(TextAsset), true);

            if (newPrefab != null)
            {
                parseNamesAndAddToDBMethodInfo.Invoke(serializedObject.targetObject, new object[] { newPrefab });
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}