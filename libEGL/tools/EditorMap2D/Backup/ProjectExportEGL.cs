using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditorMapa2D
{
    public class ProjectExportEGL
    {
        private int image_index;
        private string path_export;
        private World world_export;
        private string path_image = "imagens\\";
        private string name_image = "img";
        Dictionary<int, ProjectExportEGLTileSet> tilesets;
        List<ProjectExportEGLRegion> maps;
        private int i;
        private int j;

        public ProjectExportEGL(string path, ref World world)
        {
            image_index = 0;
            path_export = path;
            world_export = world;
            tilesets = new Dictionary<int, ProjectExportEGLTileSet>();
            maps = new List<ProjectExportEGLRegion>();

            if (!Directory.Exists(path_export + "\\" + path_image))
                Directory.CreateDirectory(path_export + "\\" + path_image);
        }

        public void process()
        {
            foreach (Region region in world_export.regions.Values)
                process_region(region);

            for (i = 0; i < maps.Count; i++)
            {
                CreateFile(maps[i]);
            }

            CreateFileEvent();
        }

        private void process_region(Region region)
        {
            try
            {
                ProjectExportEGLRegion tmp;

                tmp.map_w = region.map_width;
                tmp.map_h = region.map_height;
                tmp.tile_w = region.tile_width;
                tmp.tile_h = region.tile_height;
                tmp.name = region.name;

                tmp.layers = new ProjectExportEGLLayer[region.layer.Count];

                int k = 0;
                foreach (Layer layer in region.layer.Values)
                {
                    ProjectExportEGLLayer tmpLayer;

                    tmpLayer.matrix = new int[tmp.map_w, tmp.map_h];
                    tmpLayer.walks = new int[tmp.map_w, tmp.map_h];
                    tmpLayer.events = new int[tmp.map_w, tmp.map_h];

                    for (i = 0; i < tmpLayer.matrix.GetLength(0); i++)
                    {
                        for (j = 0; j < tmpLayer.matrix.GetLength(1); j++)
                        {
                            tmpLayer.matrix[i, j] = -1;
                            tmpLayer.walks[i, j] = 1;
                            tmpLayer.events[i, j] = 0;
                        }
                    }

                    tmpLayer.name = layer.name;

                    tmpLayer.idx_img = new List<int>();

                    foreach (int x in layer.tiles.Keys)
                    {
                        Dictionary<int, Tile> tiles = layer.tiles[x];
                        foreach (int y in tiles.Keys)
                        {
                            Tile tile = tiles[y];
                            int index_image = -1;
                            int[,] walk = get_image_crop(tile, out index_image);
                            try
                            {
                                int m = x / tmp.tile_w;
                                int n = y / tmp.tile_h;

                                int q = walk.GetLength(0);
                                int w = walk.GetLength(1);

                                if (!tmpLayer.idx_img.Contains(index_image))
                                    tmpLayer.idx_img.Add(index_image);

                                if (tmpLayer.matrix.GetLength(0) > m && tmpLayer.matrix.GetLength(1) > n)
                                {
                                    tmpLayer.matrix[m, n] = index_image;

                                    try
                                    {
                                        for (i = 0; i < q; i++)
                                        {
                                            for (j = 0; j < w; j++)
                                            {
                                                if (tmpLayer.walks.GetLength(0) > (i + m) && tmpLayer.walks.GetLength(1) > (j + n))
                                                {
                                                    if (tmpLayer.walks[(i + m), (j + n)] != 0)
                                                        tmpLayer.walks[(i + m), (j + n)] = walk[i, j];
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string error = ex.Message;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string error = ex.Message;
                            }
                        }
                    }

                    foreach (int x in layer.evento.Keys)
                    {
                        Dictionary<int, int> evento = layer.evento[x];
                        foreach (int y in evento.Keys)
                        {
                            int m = x / tmp.tile_w;
                            int n = y / tmp.tile_h;

                            int q = tmpLayer.events.GetLength(0);
                            int w = tmpLayer.events.GetLength(1);

                            if(q > m && w > n)
                                tmpLayer.events[m, n] = evento[y];
                        }
                    }

                    tmp.layers[k] = tmpLayer;
                    k++;
                }
                maps.Add(tmp);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private int[,] get_image_crop(Tile tile, out int index_image)
        {
            int[,] arr = new int[0, 0];
            index_image = -1;
            try
            {
                if (world_export.tilesets.ContainsKey(tile.tileset_code))
                {
                    if (!tilesets.ContainsKey(tile.tileset_code))
                        tilesets.Add(tile.tileset_code, new ProjectExportEGLTileSet());

                    Tileset tileset = world_export.tilesets[tile.tileset_code];
                    ProjectExportEGLTileSet tmp = tilesets[tile.tileset_code];

                    if (tileset.tile_image.ContainsKey(tile.tile_code))
                    {
                        if (!tmp.tile.ContainsKey(tile.tile_code))
                            tmp.tile.Add(tile.tile_code, new Dictionary<string, ProjectExportEGLTile>());

                        Dictionary<string, Image> list_image = tileset.tile_image[tile.tile_code];
                        Dictionary<string, ProjectExportEGLTile> listTile = tmp.tile[tile.tile_code];

                        if (list_image.ContainsKey(tile.tile_crop))
                        {
                            if (!listTile.ContainsKey(tile.tile_crop))
                            {
                                Image image = list_image[tile.tile_crop];

                                using (FileStream fs = new FileStream(path_export + "\\" + path_image + name_image + image_index + ".png", FileMode.Create))
                                {
                                    image.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                }
                                index_image = image_index;

                                image_index++;
                                ProjectExportEGLTile tmpTile = new ProjectExportEGLTile();

                                tmpTile.image_idx = index_image;

                                List<bool> list_walk = tileset.walk_region[tile.tile_code];
                                Setor setor = tileset.setor[tile.tile_code];
                                Rectangle sector = Tileset.convert_tile_crop(tile.tile_crop);

                                int x = sector.X;
                                int y = sector.Y;

                                int iX = x / setor.tile_width;
                                int iY = y / setor.tile_height;

                                int iW = sector.Width / setor.tile_width;
                                int iH = sector.Height / setor.tile_height;

                                arr = new int[iW, iH];

                                int k = 1;

                                for (i = 0; i < arr.GetLength(0); i++)
                                {
                                    for (j = 0; j < arr.GetLength(1); j++)
                                    {
                                        k = Setor.ConvertNumero(new Point(((i + iX) * setor.tile_width), ((j + iY) * setor.tile_height)), setor.tile_width, setor.tile_height, setor.map_width);
                                        if (list_walk.Count > k)
                                        {
                                            arr[i, j] = list_walk[k] ? 1 : 0;
                                        }
                                    }
                                }

                                tmpTile.walk_region = arr;

                                listTile.Add(tile.tile_crop, tmpTile);
                            }
                            else
                            {
                                ProjectExportEGLTile tmpTile = listTile[tile.tile_crop];

                                index_image = tmpTile.image_idx;

                                arr = tmpTile.walk_region;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return arr;
        }

        private void CreateFile(ProjectExportEGLRegion map)
        {
            for (j = 0; j < map.layers.Length; j++)
            {
                ProjectExportEGLLayer lyrs = map.layers[j];
                using (StreamWriter fs = new StreamWriter(path_export + "\\" + FormatName(map.name) + "_" + FormatName(lyrs.name) + ".txt"))
                {
                    fs.Write(map.map_w);
                    fs.Write(" ");
                    fs.WriteLine(map.map_h);
                    fs.Write(map.tile_w);
                    fs.Write(" ");
                    fs.WriteLine(map.tile_h);

                    Dictionary<int, int> troca_code = new Dictionary<int,int>();
                    for (int k = 0; k < lyrs.idx_img.Count; k++)
                    {
                        if (!troca_code.ContainsKey(lyrs.idx_img[k]))
                        {
                            troca_code.Add(lyrs.idx_img[k], k);
                            fs.WriteLine(k + " " + path_image + name_image + lyrs.idx_img[k] + ".png");
                        }
                    }

                    fs.WriteLine("*");

                    int p;
                    for (int k = 0; k < lyrs.matrix.GetLength(1); k++)
                    {
                        for (int u = 0; u < lyrs.matrix.GetLength(0); u++)
                        {
                            if (troca_code.ContainsKey(lyrs.matrix[u, k]))
                            {
                                p = troca_code[lyrs.matrix[u, k]];
                            }
                            else
                            {
                                p = lyrs.matrix[u, k];
                            }
                            if ((u + 1) == lyrs.matrix.GetLength(0))
                            {
                                fs.WriteLine(p);
                            }
                            else
                            {
                                fs.Write(p);
                                fs.Write(" ");
                            }
                            
                        }
                    }

                    fs.WriteLine("*");
                    for (int k = 0; k < lyrs.walks.GetLength(1); k++)
                    {
                        for (int u = 0; u < lyrs.walks.GetLength(0); u++)
                        {
                            if ((u + 1) == lyrs.walks.GetLength(0))
                            {
                                fs.WriteLine(lyrs.walks[u, k]);
                            }
                            else
                            {
                                fs.Write(lyrs.walks[u, k]);
                                fs.Write(" ");
                            }
                        }
                    }

                    fs.WriteLine("*");
                    for (int k = 0; k < lyrs.events.GetLength(1); k++)
                    {
                        for (int u = 0; u < lyrs.events.GetLength(0); u++)
                        {
                            if ((u + 1) == lyrs.events.GetLength(0))
                            {
                                fs.WriteLine(lyrs.events[u, k]);
                            }
                            else
                            {
                                fs.Write(lyrs.events[u, k]);
                                fs.Write(" ");
                            }
                        }
                    }
                }
            }
        }

        private void CreateFileEvent()
        {
            if (world_export.events.Count > 0)
            {
                using (StreamWriter fs = new StreamWriter(path_export + "\\events_switch.txt"))
                {
                    fs.WriteLine("switch(code_event)");
                    fs.WriteLine("{");
                    foreach (int k in world_export.events.Keys)
                    {
                        fs.WriteLine("    case " + k + ": //" + world_export.events[k]);
                        fs.WriteLine("        break;");
                    }
                    fs.WriteLine("}");
                }
            }
        }

        private string FormatName(string name)
        {
            name = name.Trim();
            name = name.Replace(" ", "_");
            name = name.ToLower();
            return retiraAcentos(name);
        }

        private string retiraAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
    }

    public struct ProjectExportEGLRegion
    {
        public int map_w;
        public int map_h;
        public int tile_w;
        public int tile_h;
        public string name;

        public ProjectExportEGLLayer[] layers;
    }

    public struct ProjectExportEGLLayer
    {
        public int[,] matrix;
        public int[,] walks;
        public int[,] events;
        public List<int> idx_img;
        public string name;
    }

    public class ProjectExportEGLTileSet
    {
        public Dictionary<string, Dictionary<string, ProjectExportEGLTile>> tile;

        public ProjectExportEGLTileSet()
        {
            tile = new Dictionary<string, Dictionary<string, ProjectExportEGLTile>>();
        }
    }
    
    public class ProjectExportEGLTile
    {
        public int image_idx;
        public int[,] walk_region;
    }
}
