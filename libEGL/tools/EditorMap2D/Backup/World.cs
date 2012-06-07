using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditorMapa2D
{
    public class World
    {
        public Dictionary<int, Region> regions;
        public Dictionary<int, Tileset> tilesets;
        public Dictionary<int, Dictionary<int, Image>> region_image;
        public Dictionary<int, string> events;

        private int i;

        public World()
        {
            regions = new Dictionary<int, Region>();
            tilesets = new Dictionary<int, Tileset>();
            region_image = new Dictionary<int,Dictionary<int,Image>>();
            events = new Dictionary<int, string>();
        }

        public int AddTileset(string path)
        {
            i = 0;
            while (tilesets.ContainsKey(i))
            {
                i++;
            }
            tilesets.Add(i, new Tileset(path));
            tilesets[i].tileset_code = i;
            return i;
        }

        public int AddEvent(string name)
        {
            i = 1;
            while (events.ContainsKey(i))
            {
                i++;
            }
            events.Add(i, name);
            return i;
        }

        public int AddEvent(int code, string name)
        {
            if (!events.ContainsKey(code))
            {
                events.Add(code, name);
            }
            else
            {
                if (events[code] != name)
                    return AddEvent(name);
            }
            return code;
        }

        public void delete_event(int event_code)
        {
            if (events.ContainsKey(event_code))
                events.Remove(event_code);
        }

        public void edit_event(int event_code, string event_name)
        {
            if (events.ContainsKey(event_code))
                events[event_code] = event_name;
        }

        public int AddTileset(Tileset tile)
        {
            while (tilesets.ContainsKey(tile.tileset_code))
            {
                tile.tileset_code++;
            }
            tilesets.Add(tile.tileset_code, tile);
            return tile.tileset_code;
        }

        public string get_event(int i)
        {
            if(events.ContainsKey(i))
                return events[i];
            return string.Empty;
        }

        public Image getImage(int i, int j, bool redesenha)
        {
            if (regions.ContainsKey(i))
            {
                int w = regions[i].tile_width * regions[i].map_width;
                int h = regions[i].map_height * regions[i].tile_height;
                Dictionary<int, Layer> list = regions[i].layer;

                if (!region_image.ContainsKey(i))
                    region_image.Add(i, new Dictionary<int, Image>());

                if(list.ContainsKey(j))
                {
                    Layer lyr = list[j];
                    if (redesenha || !region_image[i].ContainsKey(j))
                    {
                        Image image = new Bitmap(w, h);

                        Graphics g = Graphics.FromImage(image);
                        foreach (Dictionary<int, Tile> coluna in lyr.tiles.Values)
                        {
                            foreach (Tile tile in coluna.Values)
                            {
                                if (tilesets.ContainsKey(tile.tileset_code))
                                {
                                    Tileset tset = tilesets[tile.tileset_code];
                                    Image img = tset.getImageCrop(tile.tile_code, tile.tile_crop);
                                    if (img != null)
                                    {
                                        g.DrawImage(img, tile.point);
                                    }
                                }
                            }
                        }

                        if (!region_image[i].ContainsKey(j))
                            region_image[i].Add(j, image);
                        else
                            region_image[i][j] = image;

                    }
                    return region_image[i][j];
                }
            }
            return null;
        }

        public int AddRegion(int mapW, int mapH, int tileW, int tileH)
        {
            i = 0;
            while (regions.ContainsKey(i))
            {
                i++;
            }
            regions.Add(i, new Region(mapW, mapH, tileW, tileH));
            return i;
        }

        public int AddRegion(Region region)
        {
            i = 0;
            while (regions.ContainsKey(i))
            {
                i++;
            }
            regions.Add(i, region);
            return i;
        }
    }
}
