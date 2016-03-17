using System;
using UnityEngine;

namespace Assets.Scripts.Binding
{
    [Serializable]
    public struct CategoryFoldout
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        [SerializeField] private string _name;
        [SerializeField] private bool _foldout;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        public CategoryFoldout(string name, bool foldout)
        {
            _name = name;
            _foldout = foldout;
        }

        public string Name
        {
            get { return _name; }
        }

        public bool Foldout
        {
            get { return _foldout; }
        }
    }
}
