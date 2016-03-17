using Assets.Scripts.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Binding
{
    public abstract class ResourceManagerBase : MonoBehaviour, ISerializationCallbackReceiver
    {
        private readonly IFormatter _formatter = new Formatter();

        [SerializeField] private CategoryFoldout[] _serializedCategoryFoldouts;
        [SerializeField] private SerializedMember[] _serializedValues;

        private List<CategoryProperties> _categoryProperties;
        private Dictionary<string, bool> _categoryFoldouts;

        protected ResourceManagerBase()
        {
            InitializeEditorData();
        }

        public IEnumerable<CategoryProperties> CategoryProperties
        {
            get { return _categoryProperties; }
        }

        public IDictionary<string, bool> CategoryFoldouts
        {
            get { return _categoryFoldouts; }
        }

        public virtual void OnBeforeSerialize()
        {
            _serializedCategoryFoldouts = _categoryFoldouts.Select(
                categoryFoldout => new CategoryFoldout(categoryFoldout.Key, categoryFoldout.Value))
                .ToArray();

            _serializedValues = GetNotifyingObjectProperties()
                .Where(property => property.Type.AssemblyQualifiedName != null)
                .Select(property => new SerializedMember(
                    property.PropertyName,
                    MemberType.Property,
                    _formatter.Serialize(property.Type, property.GetValue<object>())))
                .ToArray();
        }

        public virtual void OnAfterDeserialize()
        {
            if (_serializedCategoryFoldouts != null)
            {
                foreach (var categoryFoldout in _serializedCategoryFoldouts)
                {
                    if (_categoryFoldouts.ContainsKey(categoryFoldout.Name))
                    {
                        _categoryFoldouts[categoryFoldout.Name] = categoryFoldout.Foldout;
                    }
                }
            }

            if (_serializedValues != null)
            {
                var properties = GetNotifyingObjectProperties().ToList();
                foreach (var serializedMember in _serializedValues.Where(member => member.MemberType == MemberType.Property))
                {
                    var property = properties.FirstOrDefault(
                        propertyInfo => propertyInfo.PropertyName == serializedMember.Name);
                    if (property != null)
                    {
                        var type = Type.GetType(serializedMember.SerializedValue.AssemblyQualifiedName, false);
                        if (type == property.Type)
                        {
                            var value = _formatter.Deserialize(serializedMember.SerializedValue);
                            property.SetValue(value);
                        }
                    }
                }
            }
        }

        private IEnumerable<NotifyingObjectProperty> GetNotifyingObjectProperties()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsNotifyingObject)
                .Select(property => new NotifyingObjectProperty(property, this))
                .Where(IsSupported);
        }

        private void InitializeEditorData()
        {
            _categoryProperties = GetNotifyingObjectProperties()
                .OrderBy(property => property.DisplayName)
                .GroupBy(property => property.Category)
                .Select(group => new CategoryProperties(group.Key, group.ToArray()))
                .OrderBy(group => group.Category)
                .ToList();

            _categoryFoldouts = _categoryProperties.ToDictionary(group => group.Category, category => true);
        }

        private static bool IsNotifyingObject(PropertyInfo property)
        {
            var getter = property.GetGetMethod();
            if (getter == null || !getter.IsPublic)
            {
                return false;
            }

            var type = property.PropertyType;
            if (!type.IsGenericType)
            {
                return false;
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            return genericTypeDefinition == typeof(NotifyingObject<>);
        }

        private static bool IsSupported(NotifyingObjectProperty property)
        {
            var propertyType = property.Type;
            switch (property.PropertyKind)
            {
                case PropertyKind.Undefined:
                    return propertyType == typeof(Color) ||
                           propertyType == typeof(float) ||
                           propertyType == typeof(string) ||
                           propertyType == typeof(int) ||
                           propertyType == typeof(long) ||
                           propertyType == typeof(double) ||
                           propertyType == typeof(bool) ||
                           propertyType == typeof(Vector2) ||
                           propertyType == typeof(Vector3) ||
                           propertyType == typeof(Bounds) ||
                           propertyType == typeof(Rect) ||
                           propertyType.IsEnum ||
                           typeof(Object).IsAssignableFrom(propertyType);
                case PropertyKind.Tag:
                    return propertyType == typeof(string);
                case PropertyKind.Layer:
                    return propertyType == typeof(int);
                case PropertyKind.Passwort:
                    return propertyType == typeof(string);
                default:
                    return false;
            }
        }
    }
}
