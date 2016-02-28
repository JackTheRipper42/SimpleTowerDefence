using System;
using JetBrains.Annotations;
using UnityEngine;


namespace Assets.Scripts.Level
{
    public class ChunkData
    {
        private readonly int _size;
        private readonly Position _position;
        private readonly int[] _data;
        private readonly WorldData _worldData;

        public ChunkData(int size, Position position, [NotNull] WorldData worldData)
        {
            if (worldData == null)
            {
                throw new ArgumentNullException("worldData");
            }

            _size = size;
            _position = position;
            _worldData = worldData;
            _data = new int[size*size];
        }

        public int Size
        {
            get { return _size; }
        }

        public Position Position
        {
            get { return _position; }
        }

        public WorldData WorldData
        {
            get { return _worldData; }
        }

        public bool RendererUpdated { get; set; }


        public int GetId(Position localPosition)
        {
            return _data[GetIndex(localPosition)];
        }

        public void SetId(Position localPosition, int id)
        {
            var index = GetIndex(localPosition);
            var oldId = _data[index];

            if (id != oldId)
            {

                _data[index] = id;
                RendererUpdated = false;
            }
        }

        //public void SetWorldData([NotNull] WorldData worldData)
        //{
        //    if (worldData == null)
        //    {
        //        throw new ArgumentNullException("worldData");
        //    }

        //    _worldData = worldData;
        //}

        private int GetIndex(Position localPosition)
        {
            return localPosition.X + localPosition.Z*_size;
        }
    }
}
