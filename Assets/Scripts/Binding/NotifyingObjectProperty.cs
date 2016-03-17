using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Binding
{
    public class NotifyingObjectProperty
    {
        private const string GetValueMethodName = "GetValue";
        private const string SetValueMethodName = "SetValue";
        private const string DefaultCategory = "Misc";

        private readonly Type _type;
        private readonly MethodInfo _getter;
        private readonly MethodInfo _setter;
        private readonly object _notifyingObject;
        private readonly string _displayName;
        private readonly string _description;
        private readonly string _category;
        private readonly string _propertyName;
        private readonly PropertyKind _propertyKind;

        public NotifyingObjectProperty(PropertyInfo property, object obj)
        {
            _type = property.PropertyType.GetGenericArguments()[0];
            _notifyingObject = property.GetValue(obj, null);
            _getter = _notifyingObject.GetType().GetMethod(GetValueMethodName);
            _setter = _notifyingObject.GetType().GetMethod(SetValueMethodName);

            var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .Cast<DisplayNameAttribute>()
                .SingleOrDefault();

            _displayName = displayNameAttribute != null
                ? displayNameAttribute.DisplayName
                : property.Name;

            var descriptionAttribute = property.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .SingleOrDefault();

            _description = descriptionAttribute != null
                ? descriptionAttribute.Description
                : string.Empty;

            var categoryAttribute = property.GetCustomAttributes(typeof(CategoryAttribute), false)
                .Cast<CategoryAttribute>()
                .SingleOrDefault();

            _category = categoryAttribute != null
                ? categoryAttribute.Category
                : DefaultCategory;

            _propertyName = property.Name;

            var propertyKindAttribut = property.GetCustomAttributes(typeof(PropertyKindAttribute), false)
                .Cast<PropertyKindAttribute>()
                .SingleOrDefault();

            _propertyKind = propertyKindAttribut != null
                ? propertyKindAttribut.PropertyKind
                : PropertyKind.Undefined;
        }

        public Type Type
        {
            get { return _type; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Category
        {
            get { return _category; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public PropertyKind PropertyKind
        {
            get { return _propertyKind; }
        }

        public void SetValue<T>(T value)
        {
            _setter.Invoke(_notifyingObject, new object[] {value});
        }

        public T GetValue<T>()
        {
            return (T) _getter.Invoke(_notifyingObject, null);
        }
    }
}
