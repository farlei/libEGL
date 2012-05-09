using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace EditorMapa2D
{
    [Serializable()]
    public class Project
    {
        public List<ProjectTileSet> tilesets;
        public List<ProjectRegion> regions;
        public List<ProjectEvent> events;

        public Project()
        {
            tilesets = new List<ProjectTileSet>();
            regions = new List<ProjectRegion>();
            events = new List<ProjectEvent>();
        }

        public void add_tile_set(Tileset tileset)
        {
            try
            {
                if (tileset == null)
                    return;

                ProjectTileSet tmp = new ProjectTileSet();
                tmp.tileset_name = tileset.Name;
                tmp.buffer = tileset.buffer;
                tmp.tileset_code = tileset.tileset_code;
                tmp.tiles = new ProjectTiles[tileset.setor.Count];
                int i = 0;
                foreach (string key in tileset.setor.Keys)
                {
                    ProjectTiles tmp_tiles = new ProjectTiles();

                    if (tileset.walk_region.ContainsKey(key))
                    {
                        List<bool> walk_list = tileset.walk_region[key];

                        tmp_tiles.tileset_walk = new bool[walk_list.Count];

                        for (int j = 0; j < walk_list.Count; j++)
                        {
                            tmp_tiles.tileset_walk[j] = walk_list[j];
                        }
                    }

                    if (tileset.setor.ContainsKey(key))
                    {
                        Setor sector = tileset.setor[key];

                        tmp_tiles.tileset_setor = new int[4];

                        tmp_tiles.tileset_setor[0] = sector.map_width;
                        tmp_tiles.tileset_setor[1] = sector.map_height;
                        tmp_tiles.tileset_setor[2] = sector.tile_width;
                        tmp_tiles.tileset_setor[3] = sector.tile_height;

                    }

                    if (tileset.tile_image.ContainsKey(key))
                    {
                        Dictionary<string, Image> list = tileset.tile_image[key];

                        tmp_tiles.tileset_names = new string[list.Count];

                        tmp_tiles.tileset_images = new List<byte[]>();

                        int j = 0;
                        foreach (string names in list.Keys)
                        {
                            tmp_tiles.tileset_names[j] = names;

                            Image crop_image = new Bitmap(list[names]);

                            using (MemoryStream stream = new MemoryStream())
                            {
                                crop_image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] buffer = stream.ToArray();
                                tmp_tiles.tileset_images.Add(buffer);
                            }

                            j++;
                        }
                    }

                    tmp.tiles[i] = tmp_tiles;
                    i++;
                }
                tilesets.Add(tmp);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public void add_region(Region region)
        {
            try
            {
                if (region == null)
                    return;

                ProjectRegion tmp = new ProjectRegion();

                tmp.name = region.name;
                tmp.setor = new int[4];
                tmp.setor[0] = region.map_width;
                tmp.setor[1] = region.map_height;
                tmp.setor[2] = region.tile_width;
                tmp.setor[3] = region.tile_height;

                tmp.layer = new ProjectLayer[region.layer.Count];
                int i;
                for (i = 0; i < region.layer.Count; i++)
                {
                    Layer layer = region.layer[i];
                    ProjectLayer tmpLayer = new ProjectLayer();

                    tmpLayer.name = layer.name;
                    tmpLayer.visible = layer.visible;

                    int num = 0;
                    foreach (Dictionary<int, Tile> list in layer.tiles.Values)
                        num += list.Count;

                    tmpLayer.tiles = new ProjectTile[num];

                    int j = 0;
                    foreach (int x in layer.tiles.Keys)
                    {
                        Dictionary<int, Tile> list = layer.tiles[x];
                        foreach (int y in list.Keys)
                        {
                            Tile tile = list[y];
                            ProjectTile tmpTile = new ProjectTile();
                            tmpTile.point = new int[2];
                            tmpTile.point[0] = x;
                            tmpTile.point[1] = y;
                            tmpTile.tile_code = tile.tile_code;
                            tmpTile.tile_crop = tile.tile_crop;
                            tmpTile.tileset_code = tile.tileset_code;

                            tmpLayer.tiles[j] = tmpTile;

                            j++;
                        }
                    }

                    num = 0;
                    foreach (Dictionary<int, int> list in layer.evento.Values)
                        num += list.Count;

                    tmpLayer.events = new int[num, 3];

                    j = 0;
                    foreach (int x in layer.evento.Keys)
                    {
                        Dictionary<int, int> list = layer.evento[x];
                        foreach (int y in list.Keys)
                        {
                            int code = list[y];

                            tmpLayer.events[j, 0] = code;
                            tmpLayer.events[j, 1] = x;
                            tmpLayer.events[j, 2] = y;

                            j++;
                        }
                    }

                    tmp.layer[i] = tmpLayer;
                }
                regions.Add(tmp);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public void add_event(int event_code, string event_name)
        {
            ProjectEvent tmp = new ProjectEvent();
            tmp.code = event_code;
            tmp.name = event_name;
            events.Add(tmp);
        }
    }

    [Serializable()]
    public class ProjectRegion
    {
        public string name;
        public int[] setor;
        public ProjectLayer[] layer;
    }

    [Serializable()]
    public class ProjectLayer
    {
        public string name;
        public bool visible;
        public ProjectTile[] tiles;
        public int[,] events;
    }

    [Serializable()]
    public class ProjectTile
    {
        public int[] point;
        public int tileset_code;
        public string tile_code;
        public string tile_crop;
    }

    [Serializable()]
    public class ProjectTileSet
    {
        public int tileset_code;
        public string tileset_name;
        public byte[] buffer;
        public ProjectTiles[] tiles;
    }

    [Serializable()]
    public class ProjectTiles
    {
        public string[] tileset_names;
        public List<byte[]> tileset_images;
        public int[] tileset_setor;
        public bool[] tileset_walk;
    }

    [Serializable()]
    public class ProjectEvent
    {
        public int code;
        public string name;
    }
}
