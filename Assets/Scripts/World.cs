using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Assets.Scripts.Level;
using Assets.Scripts.TMX;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class World : MonoBehaviour
    {
        private const int PixelPerUnit = 32;

        public string FilePath;
        public ChunkRenderer ChunkRendererPrefab;
        public GameObject QuadColliderPrefab;
        public Material Tiles;
        public int Layer;

        private WorldData _worldData;

        public ChunkData GetChunkData(Position position)
        {
            return _worldData.GetChunkData(position);
        }

        protected virtual void Start()
        {
            CreateWorld();
        }

        private void CreateWorld()
        {
            _worldData = ImportWorldData();

            var quadCollider = Instantiate(QuadColliderPrefab);
            quadCollider.transform.parent = transform;
            var colliderWidth = _worldData.Width * _worldData.TileMeshWidth;
            var colliderHeight = _worldData.Height * _worldData.TileMeshHeight;
            quadCollider.transform.localScale = new Vector3(colliderWidth, colliderHeight, 1f);
            quadCollider.transform.position = new Vector3(colliderWidth / 2f, 0f, colliderHeight / 2f);
            quadCollider.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            quadCollider.layer = Layer;

            foreach (var chunkData in _worldData.Chunks)
            {
                var chunkRenderer = InstantiatePrefab(
                    ChunkRendererPrefab,
                    transform,
                    chunkData);
                chunkRenderer.Position = chunkData.Position;

                var meshRenderer = chunkRenderer.GetComponent<MeshRenderer>();
                meshRenderer.materials = new[] { Tiles };
            }
        }

        private WorldData ImportWorldData()
        {
            WorldData worldData;
            using (var stream = new FileStream(FilePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(map));
                var map = (map)serializer.Deserialize(stream);
                //var mapProperties = map.properties.property.ToDictionary(
                //    property => property.name,
                //    property => property.value);
                var rawTileSet = map.tileset[0];
                var tiles = rawTileSet.tile != null
                    ? rawTileSet.tile.ToDictionary(
                        tile => int.Parse(tile.id),
                        tile => tile.properties.property.ToDictionary(
                            property => property.name,
                            property => property.value))
                    : new Dictionary<int, Dictionary<string, string>>();

                //string pixelPerUnitValue;
                //var pixelPerUnit = mapProperties.TryGetValue("PixelPerUnit", out pixelPerUnitValue)
                //    ? float.Parse(pixelPerUnitValue)
                //    : 32f;

                var tileSetImage = rawTileSet.image[0];
                var imageSource = Path.GetFileNameWithoutExtension(tileSetImage.source);
                if (imageSource == null)
                {
                    throw new InvalidOperationException();
                }

                var tileSet = new TileSet(
                    int.Parse(rawTileSet.firstgid),
                    int.Parse(rawTileSet.tilewidth),
                    int.Parse(rawTileSet.tileheight),
                    int.Parse(tileSetImage.width),
                    int.Parse(tileSetImage.height),
                    PixelPerUnit,
                    imageSource,
                    tiles);

                var rawLayer = map.Items.OfType<layer>().First();
                worldData = new WorldData(
                    int.Parse(map.width),
                    int.Parse(map.height),
                    64,
                    int.Parse(map.tilewidth),
                    int.Parse(map.tileheight),
                    PixelPerUnit,
                    tileSet);

                var x = 0;
                var z = worldData.Height - 1;
                foreach (var id in rawLayer.data.Items.Select(item => int.Parse(item.gid)))
                {
                    worldData.SetId(new Position(x, z), id);
                    x++;
                    if (x >= worldData.Width)
                    {
                        x = 0;
                        z--;
                    }
                }

                //// ToDo: this is the wrong place to do this
                //var rawObjectGroup = map.Items.OfType<objectgroup>().First();
                //foreach (var obj in rawObjectGroup.@object)
                //{
                //    var position = new Vector3(
                //        float.Parse(obj.x) / pixelPerUnit,
                //        0f,
                //        worldData.Height * worldData.TileMeshHeight - float.Parse(obj.y) / pixelPerUnit);
                //    position += new Vector3(int.Parse(obj.width), 0f, int.Parse(obj.height)) / pixelPerUnit / 2f;
                //    switch (obj.type)
                //    {
                //        case "Building":
                //            InstantiatePrefab(
                //                _importer.BuildingPrefab,
                //                _importer.PlayerSide,
                //                position);
                //            break;
                //        case "Wall":
                //            InstantiatePrefab(
                //                _importer.WallPrefab,
                //                _importer.PlayerSide,
                //                position);
                //            break;
                //        default:
                //            throw new NotSupportedException(string.Format(
                //                "The object type '{0}' is not supported",
                //                obj.type));
                //    }
                //}

            }
            return worldData;
        }


        private static T InstantiatePrefab<T>(
            [NotNull] T original,
            [NotNull] Transform parent,
                        [NotNull] ChunkData chunkData)
            where T : MonoBehaviour
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (chunkData == null)
            {
                throw new ArgumentNullException("chunkData");
            }

            var prefab = InstantiatePrefab(original, parent);
            var tileSet = chunkData.WorldData.TileSet;
            prefab.transform.position = new Vector3(
                chunkData.Position.X * tileSet.TileMeshWidth,
                0f,
                chunkData.Position.Z * tileSet.TileMeshHeight);
            prefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            return prefab;
        }

        //private static GameObject InstantiatePrefab(
        //    [NotNull] GameObject original,
        //    [NotNull] Transform parent,
        //    Vector3 position)
        //{
        //    if (original == null)
        //    {
        //        throw new ArgumentNullException("original");
        //    }
        //    if (parent == null)
        //    {
        //        throw new ArgumentNullException("parent");
        //    }

        //    var prefab = Instantiate(original);
        //    prefab.transform.parent = parent;
        //    prefab.transform.position = position;
        //    return prefab;
        //}

        private static T InstantiatePrefab<T>(
            [NotNull] T original,
            [NotNull] Transform parent)
            where T : Component
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var prefab = Instantiate(original);
            prefab.transform.parent = parent;
            return prefab;
        }
    }
}
