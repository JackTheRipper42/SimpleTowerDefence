using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public interface IBinding : IDisposable
    {
        CultureInfo Culture { get; set; }

        void Close();
    }
}