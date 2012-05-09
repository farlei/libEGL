using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EditorMapa2D
{
    public class Setor
    {
        public int map_width;
        public int map_height;
        public int tile_width;
        public int tile_height;

        public Setor(int mapW, int mapH, int tileW, int tileH)
        {
            map_width = mapW;
            map_height = mapH;
            tile_width = tileW;
            tile_height = tileH;
        }

        public static Point ConvertPoint(int pos, int tileW, int tileH)
        {
            Point p = new Point();

            int y = pos / tileH;

            int x = pos < tileW ? pos : pos - (tileW * y);

            p.X = x * tileW;
            p.Y = y * tileH;

            return p;
        }

        public static int ConvertNumero(Point e, int tileW, int tileH, int mapW)
        {
            int x = (e.X / tileW);
            int y = (e.Y / tileH);

            int i = 0;

            if (y > 0)
                i = ((mapW * y) + x);
            else
                i = x;

            return i;
        }
    }
    
    public class Tileset
    {
        private string name;//salva
        public Dictionary<string, List<bool>> walk_region;//salva
        public byte[] buffer;//salva
        public Dictionary<string, Setor> setor;//salva
        public Dictionary<string, Dictionary<string, Image>> tile_image;//salva?
        private StringBuilder code_builder;
        private string code;
        private string code_sector;
        private int i;
        private int j;
        private Image image;
        private Setor setor_actual;
        public int tileset_code;

        public Tileset() : this(string.Empty) { }
        public Tileset(string path)
        {
            tileset_code = 0;
            code_builder = new StringBuilder();
            walk_region = new Dictionary<string, List<bool>>();
            setor = new Dictionary<string, Setor>();
            setor_actual = null;
            tile_image = new Dictionary<string, Dictionary<string, Image>>();

            if (path != string.Empty)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Image foto = new Bitmap(fs);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        foto.Save(stream, foto.RawFormat);
                        buffer = stream.ToArray();
                    }
                }
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Setor setorActual
        {
            get { return setor_actual; }
        }

        public string getCode(int x, int y)
        {
            return getCode(x, y, 0, 0);
        }
        public string getCode(int x, int y, int w, int h)
        {
            code_builder.Remove(0, code_builder.Length);
            code_builder.Append(x);
            code_builder.Append("x");
            code_builder.Append(y);
            if (w > 0)
            {
                code_builder.Append("x");
                code_builder.Append(w);
            }
            if (h > 0)
            {
                code_builder.Append("x");
                code_builder.Append(h);
            }
            return code_builder.ToString();
        }

        public void selectSetor(int tileW, int tileH)
        {
            code = AddWalkRegion(tileW, tileH);

            setor_actual = setor[code];
        }

        public int maxWidthActual(int tileW)
        {
            return Convert.ToInt32(image.Width / tileW) * tileW;
        }

        public int maxHeightActual(int tileH)
        {
            return Convert.ToInt32(image.Height / tileH) * tileH;
        }

        private string AddWalkRegion(int tileW, int tileH)
        {
            code = getCode(tileW, tileH);
            if (!setor.ContainsKey(code))
            {
                checkImage();

                Setor tmp = new Setor(Convert.ToInt32(image.Width / tileW), Convert.ToInt32(image.Height / tileH), tileW, tileH);

                setor.Add(code, tmp);

                List<bool> walk = new List<bool>();
                for (i = 0; i < (tmp.map_width * tmp.map_height); i++)
                {
                    walk.Add(true);
                }
                walk_region.Add(code, walk);

                tile_image.Add(code, new Dictionary<string, Image>());
            }
            return code;
        }

        public static Rectangle convert_tile_crop(string tile_crop)
        {
            char[] sep = {'x'};
            string[] valores = tile_crop.Split(sep);
            return new Rectangle(Convert.ToInt32(valores[0]), Convert.ToInt32(valores[1]), Convert.ToInt32(valores[2]), Convert.ToInt32(valores[3]));
        }

        private string AddCropImage(Rectangle r)
        {
            Dictionary<string, Image> list = tile_image[code];

            code_sector = getCode(r.X, r.Y, r.Width, r.Height);

            if (!list.ContainsKey(code_sector))
            {
                Bitmap textura = new Bitmap(image);

                textura = textura.Clone(r, image.PixelFormat);

                list.Add(code_sector, textura);
            }
            return code_sector;
        }

        private void checkImage()
        {
            if (image == null)
            {
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    image = Bitmap.FromStream(ms);
                }
            }
        }

        public bool setWalkRegion(Point e, int tileW, int tileH)
        {
            if (tileW > 0 && tileH > 0)
            {
                code = AddWalkRegion(tileW, tileH);

                List<bool> list = walk_region[code];

                i = Setor.ConvertNumero(e, tileW, tileH, setor[code].map_width);

                if (list.Count > i)
                {
                    list[i] = !list[i];
                    return true;
                }
            }
            return false;
        }

        public string CropImage(int tileW, int tileH, Rectangle r)
        {
            code = AddWalkRegion(tileW, tileH);

            string retorno = AddCropImage(r);

            return retorno;
        }

        public Image getImageCrop(string tileCode, string tileCrop)
        {
            if (!tile_image.ContainsKey(tileCode))
                return null;

            Dictionary<string, Image> list = tile_image[tileCode];

            if (list.ContainsKey(tileCrop))
            {
                return list[tileCrop];
            }
            else
            {
                return null;
            }
        }

        public Image Paint()
        {
            return Paint(0, 0);
        }

        public Image Paint(int tileW, int tileH)
        {
            Image bitmap;
            try
            {
                checkImage();

                bitmap = new Bitmap(image);

                Graphics e = Graphics.FromImage(bitmap);

                if (tileW > 0 && tileH > 0)
                {
                    selectSetor(tileW, tileH);

                    List<bool> list = walk_region[code];

                    for (i = 0; i < setor_actual.map_height; i++)
                    {
                        for (j = 0; j < setor_actual.map_width; j++)
                        {
                            Point loc = new Point((j * tileW) + (tileW / 2) - (global::EditorMapa2D.Properties.Resources.cross.Width / 2), (i * tileH) + (tileH / 2) - (global::EditorMapa2D.Properties.Resources.cross.Height / 2));
                            int w = Setor.ConvertNumero(new Point((j * tileW), (i * tileH)), tileW, tileH, setor_actual.map_width);
                            if (list.Count > w && list[w])
                            {
                                e.DrawImage(global::EditorMapa2D.Properties.Resources.cicle, loc);
                            }
                            else
                            {
                                e.DrawImage(global::EditorMapa2D.Properties.Resources.cross, loc);
                            }
                        }
                    }

                    for (i = 0; i < bitmap.Height; i += tileH)
                    {
                        e.DrawLine(Pens.Black, 0, i, bitmap.Width, i);
                    }

                    for (i = 0; i < bitmap.Width; i += tileW)
                    {
                        e.DrawLine(Pens.Black, i, 0, i, bitmap.Height);
                    }
                    
                }

                e.Dispose();
            }
            finally { }
            return bitmap;
        }
    }
}
