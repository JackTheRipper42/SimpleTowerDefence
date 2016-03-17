using System;

namespace Assets.Scripts.Binding
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PropertyKindAttribute : Attribute
    {
        private readonly PropertyKind _propertyKind;

        public PropertyKindAttribute(PropertyKind propertyKind)
        {
            _propertyKind = propertyKind;
        }

        public PropertyKind PropertyKind
        {
            get { return _propertyKind; }
        }
    }
}