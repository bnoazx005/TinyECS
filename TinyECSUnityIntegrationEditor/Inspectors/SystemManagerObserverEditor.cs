using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEditor;
using UnityEngine;


namespace TinyECSUnityIntegrationEditor.Inspectors
{
    [CustomEditor(typeof(SystemManagerObserver), true)]
    public class SystemManagerObserverEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            ISystemManager worldContext = (target as SystemManagerObserver)?.SystemManager;
            
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("System Manager Statistics", EditorStyles.boldLabel);
            
            EditorGUILayout.EndVertical();
        }
    }
}
