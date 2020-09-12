using System;
using TinyECSUnityIntegration.Impls;
using UnityEditor;
using UnityEngine;


namespace TinyECSUnityIntegrationEditor.Inspectors
{
    [CustomEditor(typeof(BaseView), true)]
    public class BaseViewEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            _showViewInfo();
            base.OnInspectorGUI();
        }

        protected void _showViewInfo()
        {
            // Available only in play mode
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal("box");

            BaseView view = target as BaseView;
            GUILayout.Label(view.LinkedEntityId.ToString());

            if (GUILayout.Button("Go To", GUILayout.ExpandWidth(false)))
            {
                EntityObserver[] entitiesObservers = GameObject.FindObjectsOfType<EntityObserver>();

                int index = Array.FindIndex(entitiesObservers, entity => (entity.mEntityId == view.LinkedEntityId));
                if (index >= 0)
                {
                    Selection.activeTransform = entitiesObservers[index].transform;
                }
                else
                {
                    Debug.LogError($"[BaseViewEditor] There is no EntityObserver with corresponding id, {view.LinkedEntityId}");
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
