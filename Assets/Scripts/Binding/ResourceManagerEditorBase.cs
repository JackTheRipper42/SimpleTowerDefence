using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Binding
{
    public abstract class ResourceManagerEditorBase : Editor
    {
        private const float FoldoutSpace = 12;

        private ResourceManagerBase _resourceManager;

        public virtual void OnEnable()
        {
            _resourceManager = (ResourceManagerBase) target;
        }

        public override void OnInspectorGUI()
        {
            foreach (var categoryProperties in _resourceManager.CategoryProperties)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(FoldoutSpace);
                var foldout = EditorGUILayout.Foldout(
                    _resourceManager.CategoryFoldouts[categoryProperties.Category],
                    categoryProperties.Category);
                GUILayout.EndHorizontal();

                _resourceManager.CategoryFoldouts[categoryProperties.Category] = foldout;
                if (foldout)
                {

                    foreach (var property in categoryProperties.Properties)
                    {
                        var propertyType = property.Type;

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(FoldoutSpace);
                        GUILayout.Label(new GUIContent(property.DisplayName, property.Description));
                        GUILayout.FlexibleSpace();

                        switch (property.PropertyKind)
                        {
                            case PropertyKind.Undefined:
                                if (propertyType == typeof(Color))
                                {
                                    property.SetValue(EditorGUILayout.ColorField(property.GetValue<Color>()));
                                }
                                else if (propertyType == typeof(float))
                                {
                                    property.SetValue(
                                        EditorGUILayout.DelayedFloatField(
                                            property.GetValue<float>()));
                                }
                                else if (propertyType == typeof(string))
                                {
                                    property.SetValue(
                                        EditorGUILayout.DelayedTextField(
                                            property.GetValue<string>()));
                                }
                                else if (propertyType == typeof(int))
                                {
                                    property.SetValue(EditorGUILayout.DelayedIntField(property.GetValue<int>()));
                                }
                                else if (propertyType == typeof(long))
                                {
                                    property.SetValue(EditorGUILayout.LongField(property.GetValue<long>()));
                                }
                                else if (propertyType == typeof(double))
                                {
                                    property.SetValue(EditorGUILayout.DoubleField(property.GetValue<double>()));
                                }
                                else if (propertyType == typeof(bool))
                                {
                                    property.SetValue(EditorGUILayout.Toggle(
                                        GUIContent.none,
                                        property.GetValue<bool>()));
                                }
                                else if (propertyType == typeof(Vector2))
                                {
                                    property.SetValue(EditorGUILayout.Vector2Field(GUIContent.none,
                                        property.GetValue<Vector2>()));
                                }
                                else if (propertyType == typeof(Vector3))
                                {
                                    property.SetValue(EditorGUILayout.Vector3Field(
                                        GUIContent.none,
                                        property.GetValue<Vector3>()));
                                }
                                else if (propertyType == typeof(Bounds))
                                {
                                    property.SetValue(EditorGUILayout.BoundsField(property.GetValue<Bounds>()));
                                }
                                else if (propertyType == typeof(Rect))
                                {
                                    property.SetValue(EditorGUILayout.RectField(property.GetValue<Rect>()));
                                }
                                else if (propertyType.IsEnum)
                                {
                                    var flagsAttribute = propertyType.GetCustomAttributes(typeof(FlagsAttribute), false)
                                        .Cast<FlagsAttribute>()
                                        .FirstOrDefault();

                                    property.SetValue(flagsAttribute != null
                                        ? EditorGUILayout.EnumMaskField(property.GetValue<Enum>())
                                        : EditorGUILayout.EnumPopup(property.GetValue<Enum>()));
                                }
                                else if (typeof(UnityEngine.Object).IsAssignableFrom(propertyType))
                                {
                                    property.SetValue(EditorGUILayout.ObjectField(
                                        property.GetValue<UnityEngine.Object>(),
                                        propertyType,
                                        true));
                                }
                                break;
                            case PropertyKind.Tag:
                                property.SetValue(EditorGUILayout.TagField(property.GetValue<string>()));
                                break;
                            case PropertyKind.Layer:
                                property.SetValue(EditorGUILayout.LayerField(property.GetValue<int>()));
                                break;
                            case PropertyKind.Passwort:
                                property.SetValue(EditorGUILayout.PasswordField(property.GetValue<string>()));
                                break;
                            default:
                                throw new NotSupportedException(string.Format(
                                    "The property kind '{0}' is not supported.",
                                    property.PropertyKind));
                        }

                        GUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}
