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
            ISystemManager systemManager = (target as SystemManagerObserver)?.SystemManager;
            
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("System Manager Statistics", EditorStyles.boldLabel);

            GUILayout.Space(25.0f);

            EditorGUILayout.LabelField("Active Systems List:", EditorStyles.boldLabel);

            ISystemIterator iter = systemManager.GetSystemIterator();

            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("Name\t\tType");

            while (iter.MoveNext())
            {
                _displaySystemInfo(iter.Get());
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        protected void _displaySystemInfo(ISystem system)
        {
            EditorGUILayout.LabelField($"{system.GetType().Name}\t\t{_getSystemTypeMask(system)}");
        }

        protected E_SYSTEM_TYPE _getSystemTypeMask(ISystem system)
        {
            E_SYSTEM_TYPE systemTypeMask = E_SYSTEM_TYPE.ST_UNKNOWN;

            if (system is IInitSystem)
            {
                systemTypeMask |= E_SYSTEM_TYPE.ST_INIT;
            }

            if (system is IUpdateSystem)
            {
                systemTypeMask |= E_SYSTEM_TYPE.ST_UPDATE;
            }

            if (system is IReactiveSystem)
            {
                systemTypeMask |= E_SYSTEM_TYPE.ST_REACTIVE;
            }

            return systemTypeMask;
        }
    }
}
