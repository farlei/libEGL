using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace EditorMapa2D
{
    public partial class frmMain : Form
    {
        private frmNewProject frmProject;
        private World world;
        private int code_layer = -1;
        private int code_region = -1;
        private int code_tileset = -1;

        private int width;
        private int height;

        private Rectangle mapaQSel = Rectangle.Empty;
        private Rectangle tilesetQSel = Rectangle.Empty;
        private Rectangle PencilTilesetQSel = Rectangle.Empty;
        private Point tilesetCPointIni = Point.Empty;
        private bool seleciona_titleQ = false;

        private Tileset tileset;
        private Region region;
        private Layer layer;
        private Project project;

        private frmEvent frmEvents;
        private frmSobre frmAbout;
        private StringFormat formatEventStr;
        private Font fontEvent;
        
        private string default_save;
        private string default_map;

        public frmMain()
        {
            InitializeComponent();

            frmEvents = new frmEvent();
            frmEvents.newEvent += new newEventHandler(frmEvents_newEvent);
            frmEvents.editEvent += new editEventHandler(frmEvents_editEvent);
            frmEvents.deleteEvent += new deleteEventHandler(frmEvents_deleteEvent);
            frmEvents.FormClosed += new FormClosedEventHandler(frmEvents_FormClosed);

            fontEvent = new Font(this.Font, FontStyle.Bold);

            formatEventStr = new StringFormat();
            formatEventStr.Alignment = StringAlignment.Center;
            formatEventStr.LineAlignment = StringAlignment.Center;

            EnableTools(false);
            frmAbout = new frmSobre();
            helpToolStripMenuItem.Text = String.Format("Versão {0} beta", frmAbout.AssemblyVersion);
        }

        private void EnableTools(bool condicao)
        {
            saveToolStripMenuItem.Enabled = condicao;
            SavetoolStripButton.Enabled = condicao;
            savetotoolStripMenuItem.Enabled = condicao;
            ExporttoolStripButton.Enabled = condicao;
            exporttoolStripMenuItem.Enabled = condicao;
            ErasetoolStripButton.Enabled = condicao;
            PenciltoolStripButton.Enabled = condicao;
            BuckettoolStripButton.Enabled = condicao;
            WalktoolStripButton.Enabled = condicao;
            GridMaptoolStripButton.Enabled = condicao;
            LayertoolStripButton.Enabled = condicao;
            EventostoolStripButton.Enabled = condicao;
            closeProjecttoolStripMenuItem.Enabled = condicao;
            exportMaptoolStripMenuItem.Enabled = condicao;
            ImportMaptoolStripMenuItem.Enabled = condicao;
            splitContainer1.Panel1Collapsed = !condicao;
            splitContainer2.Panel2Collapsed = !condicao;

            pencilToolStripMenuItem.Enabled = condicao;
            bucketToolStripMenuItem.Enabled = condicao;
            eraseToolStripMenuItem.Enabled = condicao;
            gridToolStripMenuItem.Enabled = condicao;
            layersToolStripMenuItem.Enabled = condicao;
            eventsToolStripMenuItem.Enabled = condicao;
            walkToolStripMenuItem.Enabled = condicao;

            if (condicao)
                splitContainer3.Panel1.BackgroundImage = global::EditorMapa2D.Properties.Resources.bg_dark;
            else
                splitContainer3.Panel1.BackgroundImage = null;

            if (condicao)
                pbMap.BackgroundImage = global::EditorMapa2D.Properties.Resources.bg_dark;
            else
                pbMap.BackgroundImage = null;
        }

        #region NEW PROJECT

        private void NewProject_Click(object sender, EventArgs e)
        {
            frmProject = new frmNewProject();
            frmProject.FormClosed += new FormClosedEventHandler(frmProject_FormClosed);
            frmProject.ShowDialog();
        }

        private void clear_project()
        {
            world = null;
            tileset = null;
            region = null;
            layer = null;

            default_save = string.Empty;

            this.Text = "Editor Map 2D";

            ErasetoolStripButton.Checked = false;
            PenciltoolStripButton.Checked = false;
            BuckettoolStripButton.Checked = false;
            WalktoolStripButton.Checked = false;
            GridMaptoolStripButton.Checked = false;
            LayertoolStripButton.Checked = false;
            EventostoolStripButton.Checked = false;

            dsControle.maps.Clear();
            dsControle.tilesets.Clear();
            dsControle.layers.Clear();

            dgTilesets.Refresh();
            dgMaps.Refresh();
            dgLayers.Refresh();

            pbTileSet.Image = null;
            pbMap.BackgroundImage = null;
            pbMap.Refresh();
        }

        private void frmProject_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmProject.isProject)
            {
                new_project_world();
                
                AddMap(frmProject.mapW, frmProject.mapH, frmProject.tileW, frmProject.tileH);
                
                btnDeleteTileset.Enabled = (dgTilesets.Rows.Count > 1);
                WalktoolStripButton.Enabled = (dgTilesets.Rows.Count > 0);
            }
        }

        private void new_project_world()
        {
            clear_project();

            EnableTools(true);

            world = new World();
        }

        #endregion

        #region EVENT MAPA GRID
        private void AddMap(int mapW, int mapH, int tileW, int tileH)
        {
            code_region = world.AddRegion(mapW, mapH, tileW, tileH);

            insert_region("Map " + code_region, code_region);

            code_layer = region.addLayer();

            layer = region.layer[code_layer];

            AddLayer(code_layer);

            selectMap();
        }

        private void insert_region(string nome, int code)
        {
            code_region = code;

            region = world.regions[code_region];

            DataTable dt = dsControle.maps;
            DataRow dr = dt.NewRow();
            dr["nm_map"] = nome;
            dr["code"] = code;
            dt.Rows.InsertAt(dr, 0);
            dgMaps.Rows[0].Selected = true;
            dgMaps.Refresh();

            region.name = nome;

            btnDeleteMap.Enabled = (dgMaps.Rows.Count > 1);
        }

        private void selectMap()
        {
            dsControle.layers.Clear();

            region = world.regions[code_region];

            txtTileWidth.ReadOnly = false;
            txtTileHeight.ReadOnly = false;

            txtMapWidth.Text = region.map_width.ToString();
            txtMapHeight.Text = region.map_height.ToString();
            txtTileWidth.Text = region.tile_width.ToString();
            txtTileHeight.Text = region.tile_height.ToString();

            width = region.map_width * region.tile_width;
            height = region.map_height * region.tile_height;

            pbMap.Width = width;
            pbMap.Height = height;

            btnChanceValue.Enabled = false;
            WalktoolStripButton.Checked = false;

            foreach (int key in region.layer.Keys)
            {
                layer = region.layer[key];

                if (layer.tiles.Count > 0)
                {
                    txtTileWidth.ReadOnly = true;
                    txtTileHeight.ReadOnly = true;
                }

                AddLayer(key, layer.name, layer.visible);
            }

            pbMap.Refresh();
        }

        private void btnNewMap_Click(object sender, EventArgs e)
        {
            AddMap(Convert.ToInt32(txtMapWidth.Text), Convert.ToInt32(txtMapHeight.Text), Convert.ToInt32(txtTileWidth.Text), Convert.ToInt32(txtTileHeight.Text));
        }

        private void btnDeleteMap_Click(object sender, EventArgs e)
        {
            //pbMap.Image.Dispose();

            int code = Convert.ToInt32(dgMaps.SelectedRows[0].Cells["mapCode"].Value);
            DataRow[] rows = dsControle.maps.Select("code=" + code);
            foreach (dsControle.mapsRow dr in rows)
            {
                dsControle.maps.RemovemapsRow(dr);

                world.regions.Remove(code);
            }
            dgMaps.Refresh();

            dgMaps.Rows[0].Selected = true;

            DataGridViewCell celula = dgMaps["mapCode", 0];

            code_region = Convert.ToInt32(celula.Value);

            selectMap();

            btnDeleteMap.Enabled = (dgMaps.Rows.Count > 1);
        }

        private void nud_TextChanged(object sender, EventArgs e)
        {
            changeNUD(sender);
        }

        private void changeNUD(object sender)
        {
            if (sender is TextBox)
            {
                btnChanceValue.Enabled = true;
                TextBox ctrl = (TextBox)sender;
                int value = -1;
                switch (ctrl.Name)
                {
                    case "txtMapWidth":
                        value = Convert.ToInt32(ctrl.Text);
                        break;
                    case "txtMapHeight":
                        value = Convert.ToInt32(ctrl.Text);
                        break;
                    case "txtTileWidth":
                        value = Convert.ToInt32(ctrl.Text);
                        break;
                    case "txtTileHeight":
                        value = Convert.ToInt32(ctrl.Text);
                        break;
                }
                if (value < 0)
                {
                    ctrl.Text = "4";
                }
            }
        }

        private void isNumericKeyPress(object sender, KeyPressEventArgs e)
        {
            WalktoolStripButton.Checked = false;
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnChanceValue_Click(object sender, EventArgs e)
        {
            btnChanceValue.Enabled = false;

            region.map_width = Convert.ToInt32(txtMapWidth.Text);
            region.map_height = Convert.ToInt32(txtMapHeight.Text);
            region.tile_width = Convert.ToInt32(txtTileWidth.Text);
            region.tile_height = Convert.ToInt32(txtTileHeight.Text);

            width = region.map_width * region.tile_width;
            height = region.map_height * region.tile_height;

            pbMap.Width = width;
            pbMap.Height = height;
        }

        private void PaintInMap(Point e)
        {
            if (tilesetQSel != Rectangle.Empty && mapaQSel != Rectangle.Empty && e.X >= 0 && e.X <= width && e.Y >= 0 && e.Y <= height)
            {
                string code_crop_image = tileset.CropImage(region.tile_width, region.tile_height, tilesetQSel);

                string tile_code = tileset.getCode(region.tile_width, region.tile_height);

                Tile tile_new = new Tile(code_tileset, tile_code, code_crop_image, mapaQSel.Location);

                Tile tile_old = layer[mapaQSel.Location];

                if (tile_old != tile_new)
                {
                    txtTileWidth.ReadOnly = true;
                    txtTileHeight.ReadOnly = true;

                    layer.addTile(tile_new);

                    if (BuckettoolStripButton.Checked)
                    {
                        List<quad> lista = new List<quad>();
                        quad item_atual;

                        lista.Add(new quad(0, new Point(mapaQSel.X - mapaQSel.Width, mapaQSel.Y)));
                        item_atual = lista[lista.Count-1];
                        if (!BucketPaint(new Rectangle(item_atual.p, mapaQSel.Size), code_crop_image, tile_code, tile_old))
                        {
                            lista.Remove(item_atual);
                        }
                        lista.Add(new quad(0, new Point(mapaQSel.X, mapaQSel.Y + mapaQSel.Height)));
                        item_atual = lista[lista.Count - 1];
                        if (!BucketPaint(new Rectangle(item_atual.p, mapaQSel.Size), code_crop_image, tile_code, tile_old))
                        {
                            lista.Remove(item_atual);
                        }
                        lista.Add(new quad(0, new Point(mapaQSel.X + mapaQSel.Width, mapaQSel.Y)));
                        item_atual = lista[lista.Count - 1];
                        if (!BucketPaint(new Rectangle(item_atual.p, mapaQSel.Size), code_crop_image, tile_code, tile_old))
                        {
                            lista.Remove(item_atual);
                        }
                        lista.Add(new quad(0, new Point(mapaQSel.X, mapaQSel.Y - mapaQSel.Height)));
                        item_atual = lista[lista.Count - 1];
                        if (!BucketPaint(new Rectangle(item_atual.p, mapaQSel.Size), code_crop_image, tile_code, tile_old))
                        {
                            lista.Remove(item_atual);
                        }

                        int top = 0;
                        Rectangle recTmp;
                        Point poiTmp;
                        while (lista.Count > 0)
                        {
                            top = (lista.Count - 1);

                            item_atual = lista[top];

                            poiTmp = item_atual.p;

                            switch(item_atual.i)
                            {
                                case 0:
                                    poiTmp = new Point(poiTmp.X, poiTmp.Y - mapaQSel.Height);
                                    break;
                                case 1:
                                    poiTmp = new Point(poiTmp.X + mapaQSel.Width, poiTmp.Y);
                                    break;
                                case 2:
                                    poiTmp = new Point(poiTmp.X, poiTmp.Y + mapaQSel.Height);
                                    break;
                                case 3:
                                    poiTmp = new Point(poiTmp.X - mapaQSel.Width, poiTmp.Y);
                                    break;
                                default:
                                    lista.Remove(item_atual);
                                    continue;
                            }

                            item_atual.i++;

                            recTmp = new Rectangle(poiTmp, mapaQSel.Size);

                            if (BucketPaint(recTmp, code_crop_image, tile_code, tile_old))
                            {
                                lista.Add(new quad(0, new Point(recTmp.X, recTmp.Y)));
                            }
                        }
                    }
                         
                }
            }
        }

        private void ClearInMap(Point e)
        {
            if (mapaQSel != Rectangle.Empty && e.X >= 0 && e.X <= width && e.Y >= 0 && e.Y <= height)
            {
                if (layer.tiles.ContainsKey(mapaQSel.Location.X))
                {
                    Dictionary<int, Tile> tmp = layer.tiles[mapaQSel.Location.X];
                    if (tmp.ContainsKey(mapaQSel.Location.Y))
                    {
                        tmp.Remove(mapaQSel.Location.Y);
                        pbMap.Refresh();
                    }
                }
            }
        }

        private bool BucketPaint(Rectangle r, string codeCropImage, string tileCode, Tile tileOld)
        {
            if (r.X < 0 || r.Y < 0 || r.Y > (pbMap.Height - r.Height) || r.X > (pbMap.Width - r.Width))
                return false;

            Tile local = layer[r.Location];

            if (tileOld == local)
            {
                layer.addTile(new Tile(code_tileset, tileCode, codeCropImage, r.Location));
                return true;
            }
            else
            {
                return false;
            }
        }

        private void pbMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (PenciltoolStripButton.Checked || BuckettoolStripButton.Checked)
            {
                PaintInMap(e.Location);
            }
            else
            {
                if (ErasetoolStripButton.Checked)
                {
                    ClearInMap(e.Location);
                }
                else
                {
                    if (EventostoolStripButton.Checked)
                    {
                        mapaQSel = new Rectangle(PegaPonto(e), new Size(region.tile_width, region.tile_height));
                        if (e.Button == MouseButtons.Right)
                            eventContextMenuStrip.Show(pbMap.PointToScreen(e.Location));
                        pbMap.Refresh();
                    }
                }
            }
        }

        private void pbMap_MouseMove(object sender, MouseEventArgs e)
        {
            if ((tilesetQSel != Rectangle.Empty && (PenciltoolStripButton.Checked || BuckettoolStripButton.Checked)) || ErasetoolStripButton.Checked)
            {
                Size tamanho = tilesetQSel.Size;
                if (ErasetoolStripButton.Checked)
                {
                    tamanho = new Size(region.tile_width, region.tile_height);
                }
                mapaQSel = new Rectangle(PegaPonto(e), tamanho);
                pbMap.Refresh();
            }
            if (e.Button == MouseButtons.Left && (PenciltoolStripButton.Checked || ErasetoolStripButton.Checked))
            {
                if (PenciltoolStripButton.Checked)
                {
                    PaintInMap(e.Location);
                }
                else
                {
                    if (ErasetoolStripButton.Checked)
                        ClearInMap(e.Location);
                }
            }
        }

        private void pbMap_Paint(object sender, PaintEventArgs e)
        {
            if (region != null && region is Region)
            {
                Dictionary<int, Layer> list = region.layer;
                foreach (int l in list.Keys)
                {
                    Layer lyr = list[l];
                    if (lyr.visible)
                    {
                        Image img = world.getImage(code_region, l, l == code_layer ? true : false);
                        if (LayertoolStripButton.Checked && l != code_layer)
                        {
                            e.Graphics.DrawImage(image_transparent(img), 0, 0);
                        }
                        else
                        {
                            e.Graphics.DrawImage(img, 0, 0);                            
                        }

                        if (EventostoolStripButton.Checked)
                        {
                            Dictionary<int, Dictionary<int, int>> tmpEventX = lyr.evento;
                            foreach (int x in tmpEventX.Keys)
                            {
                                Dictionary<int, int> tmpEventY = tmpEventX[x];
                                foreach (int y in tmpEventY.Keys)
                                {
                                    string event_code = tmpEventY[y].ToString();
                                    RectangleF area = new RectangleF((float)x, (float)y, (float)region.tile_width, (float)region.tile_height);
                                    Brush pencil = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                                    e.Graphics.FillRectangle(pencil, new Rectangle(x + 4, y + 4, region.tile_width - 7, region.tile_height - 7));
                                    Pen pen = new Pen(Color.White);
                                    pen.Width = 2f;
                                    e.Graphics.DrawRectangle(pen, new Rectangle(x + 3, y + 3, region.tile_width - 5, region.tile_height - 5));
                                    e.Graphics.DrawString(event_code, fontEvent, Brushes.Black, area, formatEventStr);
                                }
                            }
                        }
                    }
                }

                if (mapaQSel != Rectangle.Empty && (tilesetQSel != Rectangle.Empty || ErasetoolStripButton.Checked || EventostoolStripButton.Checked))
                {
                    PintaRetangulo(e, mapaQSel);
                }

                if (GridMaptoolStripButton.Checked || EventostoolStripButton.Checked)
                {
                    for (int i = 0; i < region.map_width; i++)
                    {
                        e.Graphics.DrawLine(Pens.Black, i * region.tile_width, 0, i * region.tile_width, pbMap.Height);
                    }

                    for (int i = 0; i < region.map_height; i++)
                    {
                        e.Graphics.DrawLine(Pens.Black, 0, i * region.tile_height, pbMap.Width, i * region.tile_height);
                    }
                }
            }
        }

        private Image image_transparent(Image image)
        {
            Image new_image = new Bitmap(image.Width, image.Height);


            ImageAttributes imgAttributes = new ImageAttributes();

             float[][] ptsArray = { 
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 0.5F, 0}, 
                new float[] {0, 0, 0, 0, 1}};
            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


            using (Graphics g = Graphics.FromImage(new_image))
            {
                Rectangle _rect = new Rectangle(0, 0, (int)image.Width, (int)image.Height);
                g.DrawImage(image, _rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttributes);
            }

            return new_image;
        }

        private void dgMaps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell celula = dgMaps["mapCode", e.RowIndex];

            int code = Convert.ToInt32(celula.Value);
            if (code != code_region)
            {
                code_region = code;
                selectMap();
            }

            dgMaps.Rows[e.RowIndex].Selected = true;
        }

        private void dgMaps_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 0)
            {
                region.name = dgMaps[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }
        #endregion

        #region EVENT LAYERS GRID

        private void dgLayers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgLayers.Rows[e.RowIndex].Selected = true;

            DataGridViewCell celula = dgLayers["code", e.RowIndex];

            int code = Convert.ToInt32(celula.Value);
            if (code != code_layer)
            {
                code_layer = code;
                if (region.layer.ContainsKey(code_layer))
                    layer = region.layer[code_layer];
            }

            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                celula = dgLayers["columnVisible", e.RowIndex];
                bool valor = Convert.ToBoolean(celula.Value);
                celula.Value = !valor;
                layer.visible = !valor;
            }
            dgLayers.Refresh();
            pbMap.Refresh();
        }

        private void dgLayers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds);

                DataGridViewCell celula = dgLayers["columnVisible", e.RowIndex];
                
                bool valor = Convert.ToBoolean(celula.Value);
                if (valor)
                    e.Graphics.DrawImage(global::EditorMapa2D.Properties.Resources.ico_visible, e.CellBounds.X+1, e.CellBounds.Y+1, e.CellBounds.Width, e.CellBounds.Height);

                e.Paint(e.CellBounds, ~DataGridViewPaintParts.ContentForeground & ~DataGridViewPaintParts.Background);

                e.Handled = true;
            }
        }

        private void dgLayers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                layer.name = dgLayers[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }

        private void AddLayer(int codeLayer)
        {
            layer = region.layer[codeLayer];
            AddLayer(codeLayer, "Layer " + codeLayer, true);
        }

        private void AddLayer(int codeLayer, string name, bool visible)
        {
            code_layer = codeLayer;

            DataTable dt = dsControle.layers;
            DataRow dr = dt.NewRow();
            dr["nm_layer"] = name;
            dr["code"] = codeLayer;
            dr["visible"] = visible;
            dt.Rows.InsertAt(dr, 0);
            dgLayers.Refresh();
            dgLayers.Rows[0].Selected = true;

            layer.name = name;
            layer.visible = visible;

            btnDeleteLayer.Enabled = (dgLayers.Rows.Count > 1);
        }

        private void btnNewLayer_Click(object sender, EventArgs e)
        {
            AddLayer(region.addLayer());
        }

        private void btnDeleteLayer_Click(object sender, EventArgs e)
        {
            int code = Convert.ToInt32(dgLayers.SelectedRows[0].Cells["code"].Value);
            DataRow[] rows = dsControle.layers.Select("code=" + code);
            foreach (dsControle.layersRow dr in rows)
            {
                dsControle.layers.RemovelayersRow(dr);

                region.layer.Remove(code);
            }
            dgLayers.Refresh();

            dgLayers.Rows[0].Selected = true;

            DataGridViewCell celula = dgLayers["code", 0];

            code_layer = Convert.ToInt32(celula.Value);

            layer = region.layer[code_layer];

            btnDeleteLayer.Enabled = (dgLayers.Rows.Count > 1);

            pbMap.Refresh();
        }

        #endregion

        #region EVENT TILESET GRID

        private void btnNewTileset_Click(object sender, EventArgs e)
        {
            AddTileset();
        }

        private void AddTileset()
        {
            openFile.Filter = "Fotos(*.jpg,*.gif,*.png,*.bmp,*.tif)|*.jpg;*.gif;*.png;*.bmp;*.tif|JPEG (*.jpg)|*.jpg|CompuServe GIF (*.gif)|*.gif|Portable Network Graphics (*.png)|*.png|Windows Bitmaps (*.bmp)|*.bmp|TIFF (*.tif)|*.tif";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                code_tileset = world.AddTileset(openFile.FileName);
                InsertTileset(openFile.SafeFileName, code_tileset);
            }
        }

        private void InsertTileset(string name, int code)
        {
            code_tileset = code;
            DataTable dt = dsControle.tilesets;
            DataRow dr = dt.NewRow();
            dr["nm_tileset"] = name;
            dr["code"] = code_tileset;
            dt.Rows.InsertAt(dr, 0);
            dgTilesets.Rows[0].Selected = true;
            dgTilesets.Refresh();

            tileset = world.tilesets[code_tileset];

            tileset.Name = name;

            btnDeleteTileset.Enabled = (dgTilesets.Rows.Count > 1);
            WalktoolStripButton.Enabled = (dgTilesets.Rows.Count > 0);

            checkTileSet();
        }

        private void checkTileSet()
        {
            if (code_tileset < 0 || region == null)
                return;

            if (WalktoolStripButton.Checked)
            {
                pbTileSet.Image = tileset.Paint(region.tile_width, region.tile_height);
            }
            else
            {
                pbTileSet.Image = tileset.Paint();
            }
        }

        private void dgTilesets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell celula = dgTilesets["codeDataGridViewTextBoxColumn", e.RowIndex];

            int code = Convert.ToInt32(celula.Value);
            if (code != code_tileset)
            {
                code_tileset = code;
                if (world.tilesets.ContainsKey(code_tileset))
                    tileset = world.tilesets[code_tileset];
                checkTileSet();
            }

            dgTilesets.Rows[e.RowIndex].Selected = true;
        }

        private void btnDeleteTileset_Click(object sender, EventArgs e)
        {
            pbTileSet.Image.Dispose();

            int code = Convert.ToInt32(dgTilesets.SelectedRows[0].Cells["codeDataGridViewTextBoxColumn"].Value);
            DataRow[] rows = dsControle.tilesets.Select("code=" + code);
            foreach (dsControle.tilesetsRow dr in rows)
            {
                dsControle.tilesets.RemovetilesetsRow(dr);

                world.tilesets.Remove(code);
            }
            dgTilesets.Refresh();

            dgTilesets.Rows[0].Selected = true;

            DataGridViewCell celula = dgTilesets["codeDataGridViewTextBoxColumn", 0];

            code_tileset = Convert.ToInt32(celula.Value);

            checkTileSet();

            btnDeleteTileset.Enabled = (dgTilesets.Rows.Count > 1);
        }

        private void WalktoolStripButton_CheckStateChanged(object sender, EventArgs e)
        {
            checkTileSet();
            if (WalktoolStripButton.Checked)
            {
                PenciltoolStripButton.Checked = false;
                BuckettoolStripButton.Checked = false;
            }
            pbTileSet.Refresh();
        }

        private void WalktoolStripButton_Click(object sender, EventArgs e)
        {
            dgTilesets.Focus();
        }

        private void PintaRetangulo(PaintEventArgs e, Rectangle re)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black), re);
            e.Graphics.DrawRectangle(new Pen(Color.White), re.X + 1, re.Y + 1, re.Width - 2, re.Height - 2);
            e.Graphics.DrawRectangle(new Pen(Color.White), re.X + 2, re.Y + 2, re.Width - 4, re.Height - 4);
            e.Graphics.DrawRectangle(new Pen(Color.Black), re.X + 3, re.Y + 3, re.Width - 6, re.Height - 6);
        }

        private void pbTileSet_Paint(object sender, PaintEventArgs e)
        {
            if (seleciona_titleQ && (PenciltoolStripButton.Checked || BuckettoolStripButton.Checked))
            {
                PintaRetangulo(e, tilesetQSel);
            }
        }

        private void pbTileSet_MouseDown(object sender, MouseEventArgs e)
        {
            if (WalktoolStripButton.Checked && e.Button == MouseButtons.Left)
            {
                if (tileset.setWalkRegion(e.Location, region.tile_width, region.tile_height))
                {
                    checkTileSet();
                }
            }
            if (e.Button == MouseButtons.Left && (PenciltoolStripButton.Checked || BuckettoolStripButton.Checked))
            {
                if (e.X < tileset.maxWidthActual(region.tile_width) && e.Y < tileset.maxHeightActual(region.tile_height) && e.X >= 0 && e.Y >= 0)
                {
                    pbTileSet.Refresh();
                    tilesetCPointIni = PegaPonto(e);
                }
            }
        }

        private void pbTileSet_MouseMove(object sender, MouseEventArgs e)
        {
            if (PenciltoolStripButton.Checked)
            {
                if (tilesetCPointIni != Point.Empty && e.X < tileset.maxWidthActual(region.tile_width) && e.Y < tileset.maxHeightActual(region.tile_height) && e.X >= 0 && e.Y >= 0)
                    Selecao(e);
            }
        }

        private void pbTileSet_MouseUp(object sender, MouseEventArgs e)
        {
            if (PenciltoolStripButton.Checked || BuckettoolStripButton.Checked)
            {
                if (e.X < tileset.maxWidthActual(region.tile_width) && e.Y < tileset.maxHeightActual(region.tile_height) && e.X >= 0 && e.Y >= 0)
                {
                    if (BuckettoolStripButton.Checked)
                        tilesetCPointIni = PegaPonto(e);

                    Selecao(e);
                }
                tilesetCPointIni = Point.Empty;
            }
        }

        private Point PegaPonto(MouseEventArgs e)
        {
            Point pos = new Point();

            int index = Convert.ToInt32(e.X / region.tile_width);
            pos.X = index * region.tile_width;

            index = Convert.ToInt32(e.Y / region.tile_height);
            pos.Y = index * region.tile_height;

            return pos;
        }

        private void Selecao(MouseEventArgs e)
        {
            if (pbTileSet.Image != null)
            {
                seleciona_titleQ = true;

                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;

                tileset.selectSetor(region.tile_width, region.tile_height);

                Point pos = PegaPonto(e);
                if (pos.X > pbTileSet.Width)
                {
                    pos.X = tileset.setorActual.map_width * region.tile_width - region.tile_width;
                }

                if (pos.Y > pbTileSet.Height)
                {
                    pos.Y = tileset.setorActual.map_height * region.tile_height - region.tile_height;
                }

                if (tilesetCPointIni.X <= pos.X)
                {
                    x1 = tilesetCPointIni.X;
                    x2 = pos.X - x1 + region.tile_width;
                }
                else
                {
                    x1 = pos.X;
                    x2 = tilesetCPointIni.X - x1 + region.tile_width;
                }
                if (tilesetCPointIni.Y <= pos.Y)
                {
                    y1 = tilesetCPointIni.Y;
                    y2 = pos.Y - y1 + region.tile_height;
                }
                else
                {
                    y1 = pos.Y;
                    y2 = tilesetCPointIni.Y - y1 + region.tile_height;
                }

                tilesetQSel.Location = new Point(x1, y1);
                tilesetQSel.Width = x2;
                tilesetQSel.Height = y2;

                PencilTilesetQSel = new Rectangle(tilesetQSel.Location, tilesetQSel.Size);

                pbTileSet.Refresh();
            }
        }

        #endregion

        #region EVENT MENU CLICKS
        private void closeProjecttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableTools(false);
            clear_project();
        }

        private void SavetoolStripButton_Click(object sender, EventArgs e)
        {
            bool is_save = false;
            saveFile.Filter = "Projeto Mapa 2D (*.pm2d)|*.pm2d";
            saveFile.DefaultExt = "pm2d";
            if (default_save != string.Empty)
            {
                saveFile.InitialDirectory = default_save;
                is_save = true;
            }
            else
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    default_save = saveFile.FileName;
                    is_save = true;
                }
            }
            if (is_save)
                Save();
        }

        private void savetotoolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile.Filter = "Projeto Mapa 2D (*.pm2d)|*.pm2d";
            saveFile.DefaultExt = "pm2d";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                default_save = saveFile.FileName;
                Save();
            }
        }

        private void Save()
        {
            try
            {
                project = new Project();
                foreach (int event_code in world.events.Keys)
                    project.add_event(event_code, world.events[event_code]);

                foreach (Tileset tile in world.tilesets.Values)
                    project.add_tile_set(tile);

                foreach (Region region in world.regions.Values)
                    project.add_region(region);

                using (Stream stream = File.Open(default_save, FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, project);
                }

                this.Text = "Editor Map 2D - " + default_save;
            }
            catch (IOException ex)
            {
                string error = ex.Message;
            }
        }

        private void OpentoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                openFile.Filter = "Projeto Mapa 2D (*.pm2d)|*.pm2d;";
                openFile.DefaultExt = "pm2d";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    new_project_world();

                    default_save = openFile.FileName;

                    using (Stream stream = new FileStream(default_save, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        this.Text = "Editor Map 2D - " + default_save;

                        Project projeto = (Project)formatter.Deserialize(stream);

                        int i;
                        if (projeto.events != null)
                        {
                            for (i = 0; i < projeto.events.Count; i++)
                                OpenEvent(projeto.events[i]);
                        }
                        if (projeto.regions != null)
                        {
                            for (i = 0; i < projeto.regions.Count; i++)
                                OpenRegion(projeto.regions[i]);
                        }
                        if (projeto.tilesets != null)
                        {
                            for (i = 0; i < projeto.tilesets.Count; i++)
                                OpenTileset(projeto.tilesets[i]);
                        }
                        selectMap();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private int OpenEvent(ProjectEvent pEvent)
        {
            return world.AddEvent(pEvent.code, pEvent.name);
        }

        private int OpenTileset(ProjectTileSet pTileSet)
        {
            int i = 0;
            try
            {
                Tileset tileset = new Tileset();

                tileset.Name = pTileSet.tileset_name;
                tileset.tileset_code = pTileSet.tileset_code;
                tileset.buffer = pTileSet.buffer;
                
                for (i = 0; i < pTileSet.tiles.Length; i++)
                {
                    ProjectTiles pTile = pTileSet.tiles[i];

                    Setor setor = new Setor(pTile.tileset_setor[0], pTile.tileset_setor[1], pTile.tileset_setor[2], pTile.tileset_setor[3]);

                    string code = tileset.getCode(setor.tile_width, setor.tile_height);

                    if (!tileset.setor.ContainsKey(code))
                    {
                        tileset.setor.Add(code, setor);

                        List<bool> walk_r = new List<bool>();
                        int j;
                        for (j = 0; j < pTile.tileset_walk.Length; j++)
                            walk_r.Add(pTile.tileset_walk[j]);

                        tileset.walk_region.Add(code, walk_r);

                        tileset.tile_image.Add(code, new Dictionary<string, Image>());

                        Dictionary<string, Image> tile_images = tileset.tile_image[code];
                        List<byte[]> image_l = pTile.tileset_images;
                        string[] names_l = pTile.tileset_names;

                        for (j = 0; j < names_l.Length; j++)
                        {
                            Image tile;

                            using (MemoryStream ms = new MemoryStream(image_l[j]))
                            {
                                tile = Bitmap.FromStream(ms);
                            }

                            tile_images.Add(names_l[j], tile);
                        }
                    }
                }
                i = world.AddTileset(tileset);
                InsertTileset(tileset.Name, i);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return i;
        }

        private void OpenRegion(ProjectRegion pRegion)
        {
            try
            {
                Region region = new Region(pRegion.setor[0], pRegion.setor[1], pRegion.setor[2], pRegion.setor[3]);
                region.name = pRegion.name;

                int i;
                for (i = 0; i < pRegion.layer.Length; i++)
                {
                    ProjectLayer pLayer = pRegion.layer[i];

                    Layer layer = new Layer();

                    layer.name = pLayer.name;
                    layer.visible = pLayer.visible;

                    layer.tiles = new Dictionary<int, Dictionary<int, Tile>>();

                    int j;
                    if (pLayer.tiles != null)
                    {
                        for (j = 0; j < pLayer.tiles.Length; j++)
                        {
                            ProjectTile pTile = pLayer.tiles[j];

                            if (pTile == null)
                                continue;

                            Tile tile = new Tile();

                            tile.point = new Point(pTile.point[0], pTile.point[1]);
                            tile.tile_code = pTile.tile_code;
                            tile.tile_crop = pTile.tile_crop;
                            tile.tileset_code = pTile.tileset_code;

                            if (!layer.tiles.ContainsKey(pTile.point[0]))
                            {
                                layer.tiles.Add(pTile.point[0], new Dictionary<int, Tile>());
                            }

                            Dictionary<int, Tile> coluna = layer.tiles[pTile.point[0]];

                            if (!coluna.ContainsKey(pTile.point[1]))
                            {
                                coluna.Add(pTile.point[1], tile);
                            }
                            else
                            {
                                coluna[pTile.point[1]] = tile;
                            }
                        }
                    }
                    if (pLayer.events != null)
                    {
                        for (j = 0; j < pLayer.events.GetLongLength(0); j++)
                        {
                            int code = pLayer.events[j, 0];
                            int x = pLayer.events[j, 1];
                            int y = pLayer.events[j, 2];

                            layer.addEvent(new Point(x, y), code);
                        }
                    }
                    region.addLayer(layer);
                }
                i = world.AddRegion(region);
                insert_region(region.name, i);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void ExporttoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    ProjectExportEGL export = new ProjectExportEGL(folderBrowser.SelectedPath, ref world);

                    export.process();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void exportMaptoolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile.Filter = "Mapa 2D (*.m2d)|*.m2d";
            saveFile.DefaultExt = "m2d";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                default_map = saveFile.FileName;
                ExportMap();
            }
        }

        private void ExportMap()
        {
            try
            {
                if (region != null)
                {
                    project = new Project();

                    List<int> list_event = new List<int>();
                    List<int> tilesets = new List<int>();

                    foreach (Layer lyr in region.layer.Values)
                    {
                        foreach (Dictionary<int, int> events in lyr.evento.Values)
                        {
                            foreach (int event_code in events.Values)
                            {
                                if (!list_event.Contains(event_code))
                                    list_event.Add(event_code);
                            }
                        }
                        foreach (Dictionary<int, Tile> tiles in lyr.tiles.Values)
                        {
                            foreach (Tile tl in tiles.Values)
                            {
                                if (!tilesets.Contains(tl.tileset_code))
                                    tilesets.Add(tl.tileset_code);
                            }
                        }
                    }

                    foreach (int event_code in list_event)
                        if (world.events.ContainsKey(event_code))
                            project.add_event(event_code, world.events[event_code]);

                    foreach (int tileset_code in tilesets)
                        if (world.tilesets.ContainsKey(tileset_code))
                            project.add_tile_set(world.tilesets[tileset_code]);

                    project.add_region(region);

                    using (Stream stream = File.Open(default_map, FileMode.Create, FileAccess.ReadWrite))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, project);
                    }
                }
            }
            catch (IOException ex)
            {
                string error = ex.Message;
            }
        }

        private void ImportMaptoolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.Filter = "Mapa 2D (*.m2d)|*.m2d";
            openFile.DefaultExt = "m2d";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                default_map = openFile.FileName;

                using (Stream stream = new FileStream(default_map, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    Project projeto = (Project)formatter.Deserialize(stream);

                    Dictionary<int, int> check_event = new Dictionary<int, int>();
                    Dictionary<int, int> check_tileset = new Dictionary<int, int>();

                    int i;
                    if (projeto.events != null)
                    {
                        for (i = 0; i < projeto.events.Count; i++)
                        {
                            int new_code = OpenEvent(projeto.events[i]);
                            if (new_code != projeto.events[i].code)
                            {
                                if(!check_event.ContainsKey(projeto.events[i].code))
                                    check_event.Add(projeto.events[i].code, new_code);
                            }
                        }
                    }
                    
                    if (projeto.tilesets != null)
                    {
                        bool insert = true;
                        for (i = 0; i < projeto.tilesets.Count; i++)
                        {
                            insert = true;
                            foreach (int tileset_code in world.tilesets.Keys)
                            {
                                if (projeto.tilesets[i].tileset_name == world.tilesets[tileset_code].Name)
                                {
                                    insert = false;
                                    if (projeto.tilesets[i].tileset_code != tileset_code)
                                    {
                                        if (!check_tileset.ContainsKey(projeto.tilesets[i].tileset_code))
                                            check_tileset.Add(projeto.tilesets[i].tileset_code, tileset_code);

                                        break;
                                    }
                                }
                            }
                            //Não achou..
                            if (insert)
                            {
                                int tileset_code = OpenTileset(projeto.tilesets[i]);
                                if (projeto.tilesets[i].tileset_code != tileset_code)
                                {
                                    if (!check_tileset.ContainsKey(projeto.tilesets[i].tileset_code))
                                        check_tileset.Add(projeto.tilesets[i].tileset_code, tileset_code);
                                }
                            }
                        }
                    }

                    if (projeto.regions != null)
                    {
                        for (i = 0; i < projeto.regions.Count; i++)
                        {
                            ProjectRegion pRegion = projeto.regions[i];
                            for (int j = 0; j < pRegion.layer.Length; j++)
                            {
                                ProjectLayer pLayer = pRegion.layer[j];

                                for (int k = 0; k < pLayer.events.GetLongLength(0); k++)
                                {
                                    for (int l = 0; l < pLayer.events.GetLongLength(1); l++)
                                    {
                                        if(check_event.ContainsKey(pLayer.events[k,l]))
                                        {
                                            pLayer.events[k, l] = check_event[pLayer.events[k, l]];
                                        }
                                    }
                                }

                                for (int k = 0; k < pLayer.tiles.Length; k++)
                                {
                                    ProjectTile pTile = pLayer.tiles[k];
                                    if (check_tileset.ContainsKey(pTile.tileset_code))
                                    {
                                        pLayer.tiles[k].tileset_code = check_tileset[pTile.tileset_code];
                                    }
                                }

                                pRegion.layer[j] = pLayer;
                            }
                            OpenRegion(pRegion);
                        }
                    }

                    selectMap();
                }
            }
        }

        private void sobreEditorMapa2DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout = new frmSobre();
            frmAbout.ShowDialog();
        }

        private void LayertoolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            pbMap.Refresh();
        }

        private void EventostoolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            if (EventostoolStripButton.Checked)
            {
                PenciltoolStripButton.Checked = false;
                BuckettoolStripButton.Checked = false;
                ErasetoolStripButton.Checked = false;
            }
            pbMap.Refresh();
        }

        private void GridMaptoolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            pbMap.Refresh();
        }

        private void ErasetoolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ErasetoolStripButton.Checked)
            {
                PenciltoolStripButton.Checked = false;
                BuckettoolStripButton.Checked = false;
                EventostoolStripButton.Checked = false;
            }
            tilesetQSel = Rectangle.Empty;
            mapaQSel = Rectangle.Empty;
            pbTileSet.Refresh();
            pbMap.Refresh();
        }

        private void PenciltoolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PenciltoolStripButton.Checked)
            {
                ErasetoolStripButton.Checked = false;
                WalktoolStripButton.Checked = false;
                BuckettoolStripButton.Checked = false;
                EventostoolStripButton.Checked = false;
            }
            if (PencilTilesetQSel != Rectangle.Empty)
                tilesetQSel = new Rectangle(PencilTilesetQSel.Location, PencilTilesetQSel.Size);
            else
                tilesetQSel = Rectangle.Empty;
            pbTileSet.Refresh();
            mapaQSel = Rectangle.Empty;
            pbMap.Refresh();
        }

        private void BuckettoolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BuckettoolStripButton.Checked)
            {
                ErasetoolStripButton.Checked = false;
                WalktoolStripButton.Checked = false;
                PenciltoolStripButton.Checked = false;
                EventostoolStripButton.Checked = false;
            }
            if (PencilTilesetQSel != Rectangle.Empty)
                tilesetQSel = new Rectangle(PencilTilesetQSel.Location,new Size(region.tile_width, region.tile_height));
            else
                tilesetQSel = Rectangle.Empty;
            pbTileSet.Refresh();
            mapaQSel = Rectangle.Empty;
            pbMap.Refresh();
        }

        private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenciltoolStripButton.Checked = !PenciltoolStripButton.Checked;
        }

        private void bucketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuckettoolStripButton.Checked = !BuckettoolStripButton.Checked;
        }

        private void eraseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErasetoolStripButton.Checked = !ErasetoolStripButton.Checked;
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridMaptoolStripButton.Checked = !GridMaptoolStripButton.Checked;
        }

        private void layersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayertoolStripButton.Checked = !LayertoolStripButton.Checked;
        }

        private void eventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventostoolStripButton.Checked = !EventostoolStripButton.Checked;
        }

        private void walkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WalktoolStripButton.Checked = !WalktoolStripButton.Checked;
        }
        #endregion

        #region EVENT EVENTOS :D
        private void frmEvents_newEvent()
        {
            string nome = "Evento";
            int code = world.AddEvent(nome);
            frmEvents.AddEvent(code, nome);
        }

        private void frmEvents_editEvent(int event_code, string event_name)
        {
            world.edit_event(event_code, event_name);
        }

        private void frmEvents_deleteEvent(int event_code)
        {
            world.delete_event(event_code);
            List<Point> event_del = new List<Point>();
            foreach (Region mapas in world.regions.Values)
            {
                foreach (Layer lyr in mapas.layer.Values)
                {
                    Dictionary<int, Dictionary<int, int>> tmpEventX = lyr.evento;
                    event_del.Clear();
                    foreach (int x in tmpEventX.Keys)
                    {
                        Dictionary<int, int> tmpEventY = tmpEventX[x];
                        foreach (int y in tmpEventY.Keys)
                        {
                            int code = tmpEventY[y];
                            if (!world.events.ContainsKey(code))
                            {
                                event_del.Add(new Point(x, y));
                            }
                        }
                    }
                    for (int i = 0; i < event_del.Count; i++)
                    {
                        lyr.deleteEvent(event_del[i]);
                    }
                }
            }
            pbMap.Refresh();
        }

        private void deletarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (layer != null && mapaQSel != Rectangle.Empty)
            {
                layer.deleteEvent(mapaQSel.Location);
                pbMap.Refresh();
            }
        }

        private void frmEvents_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEvents.ok)
            {
                layer.addEvent(mapaQSel.Location, frmEvents.codeEvent);
                pbMap.Refresh();
            }
        }

        private void criarEventoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEvents.clear();
            foreach (int event_code in world.events.Keys)
            {
                frmEvents.AddEvent(event_code, world.events[event_code]);
            }
            frmEvents.ShowDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja sair?", "Editor Mapa 2D", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
        #endregion
    }
}