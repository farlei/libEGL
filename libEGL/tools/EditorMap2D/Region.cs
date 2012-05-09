using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditorMapa2D
{
    public class quad
    {
        public int i;
        public Point p;

        public quad(int idx, Point point)
        {
            i = idx;
            p = point;
        }
    }
    
    public class Tile
    {
        public Point point;
        public string tile_code;
        public string tile_crop;
        public int tileset_code;

        public Tile()
        {
            tile_code = string.Empty;
            tile_crop = string.Empty;
            tileset_code = -1;
            point = Point.Empty;
        }

        public Tile(int tilesetCode, string tileCode, string tileCrop, int x, int y) : this(tilesetCode, tileCode, tileCrop, new Point(x, y)) { }

        public Tile(int tilesetCode, string tileCode, string tileCrop, Point location)
        {
            tile_code = tileCode;
            tile_crop = tileCrop;
            tileset_code = tilesetCode;
            point = location;
        }

        public static bool operator ==(Tile a, Tile b)
        {
            if (a.tile_code == b.tile_code && a.tile_crop == b.tile_crop && a.tileset_code == b.tileset_code)
                return true;
            else
                return false;
        }

        public static bool operator !=(Tile a, Tile b)
        {
            if (a.point != b.point || a.tile_code != b.tile_code || a.tile_crop != b.tile_crop || a.tileset_code != b.tileset_code)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return tile_crop;
        }
    }

    public class Region : Setor
    {
        public Dictionary<int, Layer> layer;
        public string name;

        public Region(int mapW, int mapH, int tileW, int tileH)
            : base(mapW, mapH, tileW, tileH)
        {
            layer = new Dictionary<int, Layer>();
        }

        public int addLayer()
        {
            return addLayer(new Layer());
        }

        public int addLayer(Layer new_layer)
        {
            int i = 0;
            while (layer.ContainsKey(i))
            {
                i++;
            }
            layer.Add(i, new_layer);
            return i;
        }
    }
    
    public class Layer
    {
        public Dictionary<int, Dictionary<int, Tile>> tiles;
        public Dictionary<int, Dictionary<int, int>> evento;
        public bool visible;
        public string name;

        public Layer()
        {
            tiles = new Dictionary<int, Dictionary<int, Tile>>();
            evento = new Dictionary<int, Dictionary<int, int>>();
            visible = true;
            name = "Layer";
        }

        public void addEvent(Point p, int event_code)
        {
            Dictionary<int, int> tmp;
            if (!evento.ContainsKey(p.X))
            {
                evento.Add(p.X, new Dictionary<int, int>());
            }
            tmp = evento[p.X];
            if (!tmp.ContainsKey(p.Y))
            {
                tmp.Add(p.Y, event_code);
            }
            else
            {
                tmp[p.Y] = event_code;
            }
        }

        public void deleteEvent(Point p)
        {
            Dictionary<int, int> tmp;
            if (evento.ContainsKey(p.X))
            {          
                tmp = evento[p.X];
                if(tmp.ContainsKey(p.Y))
                    tmp.Remove(p.Y);

                if (tmp.Count == 0)
                    evento.Remove(p.X);
            }
        }

        public void addTile(Tile tile)
        {
            Dictionary<int, Tile> tmp;
            if (!tiles.ContainsKey(tile.point.X))
            {
                tiles.Add(tile.point.X, new Dictionary<int,Tile>());
            }
            tmp = tiles[tile.point.X];
            if (!tmp.ContainsKey(tile.point.Y))
            {
                tmp.Add(tile.point.Y, tile);
            }
            else
            {
                tmp[tile.point.Y] = tile;
            }
        }

        public Tile this[Point point]
        {
            get 
            {
                if (tiles.ContainsKey(point.X))
                {
                    Dictionary<int, Tile> tmp = tiles[point.X];
                    if (tmp.ContainsKey(point.Y))
                    {
                        return tmp[point.Y];
                    }
                    else
                    {
                        return new Tile();
                    }
                }
                else
                {
                    return new Tile();
                }
            }
        }
    }
}
