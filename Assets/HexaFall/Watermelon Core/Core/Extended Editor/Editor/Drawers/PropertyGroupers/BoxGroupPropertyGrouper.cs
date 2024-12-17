using UnityEditor;
using UnityEngine;

namespace HexFall
{
    [PropertyGrouper(typeof(BoxGroupAttribute))]
    public class BoxGroupPropertyGrouper : PropertyGrouper
    {
        public override void BeginGroup(string label)
        {
            EditorGUILayout.BeginVertical(WatermelonEditor.Styles.Skin.box);

            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayoutCustom.Header(label);
            }
        }

        public override void EndGroup()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
