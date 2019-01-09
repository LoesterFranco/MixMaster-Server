using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager.Data
{
    public struct Tile
    {
        public byte restriction;
        public short warpID;
    }

    public struct Layer
    {
        public short id;
        public short originX;
        public short originY;
        public short endX;
        public short endY;
        public byte effect;
    }

    public class XMap
    {
        public string name;
        private Tile[,] tiles;
        private Layer[] layers;
       

        public void ResizeLayer(int length)
        {
            layers = new Layer[length];
        }

        public void ResizeMap(int x, int y)
        {
            tiles = new Tile[x, y];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            tiles[x, y] = tile;
        }

        public void SetLayer(int index, Layer layer)
        {
            layers[index] = layer;
        }


    }

    
    public class MapManager
    {
        public List<XMap> maps;
        private int mapsLoaded = 0;
        public MapManager()
        {
            maps = new List<XMap>();
        }

        public void LoadAllMaps()
        {
            string dir = Directory.GetCurrentDirectory() + @"/Map/";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string[] files = Directory.GetFiles(dir, "*.xmp");
            foreach(string file in files)
            {
                string name = Path.GetFileName(file);
                byte[] file_bytes = File.ReadAllBytes(file);
                ParseMapData(file_bytes, name);
            }
            Init.logger.ConsoleLog($"[MapManager] {mapsLoaded} mapas foram carregados com sucesso!", ConsoleColor.Cyan);
        }

        private void ParseMapData(byte[] data, string name)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    XMap map = new XMap();
                    br.BaseStream.Seek(9, SeekOrigin.Current);

                    int sizeX = br.ReadInt32();
                    int sizeY = br.ReadInt32();
                    map.ResizeMap(sizeX, sizeY);
                    br.BaseStream.Seek(4, SeekOrigin.Current);

                    for(int x = 0; x < sizeX; x++)
                    {
                        for(int y = 0; y < sizeY; y++)
                        {
                            byte restrict = br.ReadByte();
                            short warp = br.ReadInt16();
                            br.ReadInt16();
                            Tile tile = new Tile();
                            tile.restriction = restrict;
                            tile.warpID = warp;
                            map.SetTile(x, y, tile);
                        }
                    }

                    int objHeaders = br.ReadInt32();
                    br.BaseStream.Seek(objHeaders * 16, SeekOrigin.Current);

                    int nObjects = br.ReadInt32();
                    br.BaseStream.Seek(nObjects * 34, SeekOrigin.Current);

                    byte layers_num = br.ReadByte();
                    map.ResizeLayer(layers_num);
                    for(int i = 0; i < layers_num; i++)
                    {
                        Layer layer = new Layer();
                        layer.id = br.ReadInt16();
                        layer.originX = br.ReadInt16();
                        layer.originY = br.ReadInt16();
                        layer.endX = br.ReadInt16();
                        layer.endY = br.ReadInt16();

                        map.SetLayer(i, layer);
                    }

                    map.name = name;
                    maps.Add(map);
                    mapsLoaded++;
                    
                }
            }
        }

    }
}
