using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEditor;
using UnityEngine;


namespace TinyECSUnityIntegrationEditor.Inspectors
{
    [CustomEditor(typeof(WorldContextsManager), true)]
    public class WorldContextsManagerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            IWorldContext worldContext = (target as WorldContextsManager)?.WorldContext;

            TWorldContextStats stats = worldContext.Statistics;

            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("World Context Statistics", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Active entities:", stats.mNumOfActiveEntities.ToString());
            EditorGUILayout.LabelField("Reserved entities:", stats.mNumOfReservedEntities.ToString());
            EditorGUILayout.LabelField("Total Components Count:", stats.mNumOfActiveComponents.ToString());

            EditorGUILayout.EndVertical();
        }
    }
}
