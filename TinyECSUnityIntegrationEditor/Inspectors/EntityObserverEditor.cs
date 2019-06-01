using System;
using System.Reflection;
using TinyECS.Interfaces;
using TinyECSUnityIntegration.Impls;
using UnityEditor;
using UnityEngine;


namespace TinyECSUnityIntegrationEditor.Inspectors
{
    [CustomEditor(typeof(EntityObserver), true)]
    public class EntityObserverEditor: Editor
    {
        protected static bool[]  mComponentsFoldOutInfo = new bool[1000];

        protected EntityObserver mCurrSelectedEntity = null;

        public override void OnInspectorGUI()
        {
            EntityObserver entityObserver = target as EntityObserver;

            if (mCurrSelectedEntity != entityObserver)
            {
                Array.Clear(mComponentsFoldOutInfo, 0, mComponentsFoldOutInfo.Length);

                mCurrSelectedEntity = entityObserver;
            }

            IEntity entity = entityObserver.Entity;

            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            EditorGUILayout.LabelField("Entity Info:", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Id", entity.Id.ToString());
            EditorGUILayout.LabelField("Name", entity.Name);

            EditorGUILayout.Separator();
            GUILayout.Space(20.0f);

            // display all components that are attached to the entity
            IComponentIterator componentIter = entity.GetComponentsIterator();

            int i = 0; 

            while (componentIter.MoveNext())
            {
                _displayComponent(componentIter.Get(), i++, ref mComponentsFoldOutInfo);
            }

            EditorGUILayout.EndVertical();

            Repaint();
        }

        protected void _displayComponent(IComponent component, int componentId, ref bool[] componenentsFoldOutInfo)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            // the method returns true when we should to display detailed information to a user
            if (componenentsFoldOutInfo[componentId] = _displayComponentHeader(component, componenentsFoldOutInfo[componentId]))
            {
                _displayComponentFields(component);
            }
            
            EditorGUILayout.EndVertical();
        }

        protected bool _displayComponentHeader(IComponent component, bool prevState)
        {
            EditorGUILayout.BeginHorizontal();

            // implement fold out effect
            if (GUILayout.Button(prevState ? "-" : "+", GUILayout.ExpandWidth(false)))
            {
                return !prevState;
            }

            EditorGUILayout.LabelField(component.GetType().Name, EditorStyles.boldLabel);

            EditorGUILayout.EndHorizontal();

            return prevState;
        }

        protected void _displayComponentFields(IComponent component)
        {
            Type componentType = component.GetType();

            // display current values of the component (read only)
            FieldInfo[] componentFields = componentType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            FieldInfo currField = null;

            string fieldName = null;

            object fieldValue = null;

            for (int i = 0; i < componentFields.Length; ++i)
            {
                currField = componentFields[i];

                fieldName  = currField.Name;
                fieldValue = currField.GetValue(component);

                // display reference type's value
                if (!currField.FieldType.IsValueType)
                {
                    EditorGUILayout.ObjectField(fieldName, fieldValue as UnityEngine.Object, currField.FieldType, false);

                    continue;
                }

                EditorGUILayout.LabelField(fieldName, $"{fieldValue}");
            }
        }
    }
}
