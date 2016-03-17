using JetBrains.Annotations;

namespace Assets.Scripts.Binding
{
    public interface IBindingFactory
    {
        IBinding CreatePropertyBinding<TSource, TTarget>(
            BindingType bindingType, 
            [NotNull] IDependencyProperty<TTarget> target, 
            [NotNull] INotifyingObject<TSource> source, 
            [NotNull] ValueConverter<TSource, TTarget> converter);
    }
}