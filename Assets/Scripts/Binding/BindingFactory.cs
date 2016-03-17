using JetBrains.Annotations;
using System;

namespace Assets.Scripts.Binding
{
    public class BindingFactory : IBindingFactory
    {
        public IBinding CreatePropertyBinding<TSource, TTarget>(
            BindingType bindingType, 
            IDependencyProperty<TTarget> target, 
            INotifyingObject<TSource> source, 
            ValueConverter<TSource, TTarget> converter)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            return new PropertyBinding<TSource, TTarget>(bindingType, target, source, converter);
        }
    }
}
