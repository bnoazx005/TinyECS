using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEditor;
using UnityEngine;


namespace TinyECSUnityIntegrationEditor.Inspectors
{
    [CustomEditor(typeof(EntityObserver), true)]
    public class EntityObserverEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            EntityObserver entityObserver = target as EntityObserver;

            IEntity entity = entityObserver.Entity;

            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            EditorGUILayout.LabelField("Entity Info:", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Id", entity.Id.ToString());
            EditorGUILayout.LabelField("Name", entity.Name);

            EditorGUILayout.Separator();
            GUILayout.Space(45.0f);

            EditorGUILayout.EndVertical();
        }
    }
}
