using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EGLCort2
{
    public partial class Form1 : Form
    {
        bool box=false;
        bool movendo = false;
        Point p1;
        Point p2;
        string nome_arquivo;
        int selecionado;

        List<Box> lista_box;
        
        public Form1()
        {
            InitializeComponent();
            imagem.Visible = false;

            lista_box = new List<Box>();

            copiar.Enabled = false;
            limpa.Enabled = false;
            duplicar.Enabled = false;
            mover.Enabled = false;
            lista.Enabled = false;
            label_txtNome.Enabled = false;
            txtNome.Enabled = false;

            selecionado = -1;
        }

        private void abrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();

            diag.Filter = "BMP|*.bmp|PNG|*.png|Tudo (*.*)|*.*";

            DialogResult res = diag.ShowDialog();
            if (res == DialogResult.OK)
            {
                imagem.Visible = true;
                lista.Enabled = true;
                copiar.Enabled = true;
                limpa.Enabled = true;
                duplicar.Enabled = true;
                mover.Enabled = true;
                label_txtNome.Enabled = true;
                txtNome.Enabled = true;

                imagem.Image = Image.FromFile(diag.FileName);
                imagem.Size = imagem.Image.Size;
                nome_arquivo = diag.FileName;

                p1 = new Point(0, 0);
                p2 = new Point(imagem.Image.Size);  
            }
        }

        private void imagem_MouseDown(object sender, MouseEventArgs e)
        {
            if (movendo)
            {
                movendo = false;
                lista_box.ElementAt(selecionado).P1 = p1;
                lista_box.ElementAt(selecionado).P2 = p2;

                populaLista();

                lista.SelectedIndex = lista_box.Count - 1;
            }
            else
            {
                box = true;
                p1 = e.Location;
            }

        }

        private void imagem_MouseMove(object sender, MouseEventArgs e)
        {
            if (box)
            {
                p2 = e.Location;
                imagem.Invalidate();   
            }
            if (movendo)
            {
                int difx = e.X - p1.X; 
                int dify = e.Y - p1.Y;
                p1.X += difx;
                p1.Y += dify;
                p2.X += difx;
                p2.Y += dify;
                imagem.Invalidate();
            }

        }

        private void imagem_MouseUp(object sender, MouseEventArgs e)
        {
            if (box)
            {
                box = false;     
                p2 = e.Location;
                if (p2.X - p1.X < 0) return;
                if (p2.Y - p1.Y < 0) return;
                if (p1 == p2)
                {

                    Rectangle rtest;
                    Point pt1, pt2;
                    int i = 0;
                    foreach (Box boxp in lista_box)
                    {
                        pt1 = boxp.P1;
                        pt2 = boxp.P2;
                        rtest = new Rectangle(pt1.X, pt1.Y, pt2.X - pt1.X, pt2.Y - pt1.Y);

                        if(rtest.Contains(p1))
                        {
                             lista.SelectedIndex = i;
                        }
                        i++;
                    }
                    return;
                }

                imagem.Invalidate();

                lista_box.Add(new Box(p1, p2));

                lista.Items.Add(p1.ToString() + " - " + p2.ToString());

                lista.SelectedIndex = lista_box.Count - 1;
            }
            
        }

        private void imagem_Paint(object sender, PaintEventArgs e)
        {
            Graphics gc = e.Graphics;
            if (box)
            {
                Pen preto = new Pen(Color.Black, 1);
                gc.DrawRectangle(preto, new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y));   
            }

            if (movendo)
            {
                Pen preto = new Pen(Color.Red, 1);
                gc.DrawRectangle(preto, new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y));  
            }

            Point pt1;
            Point pt2;
            foreach(Box boxp in lista_box)
            {
                pt1 = boxp.P1;
                pt2 = boxp.P2;
                gc.DrawRectangle(new Pen(boxp.Cor), new Rectangle(pt1.X, pt1.Y, pt2.X - pt1.X, pt2.Y - pt1.Y));
            }


        }

        private void limpa_Click(object sender, EventArgs e)
        {
            lista.Items.Clear();
            lista_box.Clear();
            imagem.Invalidate();
            selecionado = -1;
        }

        private void copiar_Click(object sender, EventArgs e)
        {
            try
            {
                string inicio = txtNome.Text + ".carregar(\""+nome_arquivo+"\",";
                string texto = "";

                Point pt1;
                Point pt2;
                foreach (Box boxp in lista_box)
                {
                    pt1 = boxp.P1;
                    pt2 = boxp.P2;

                    texto += inicio + pt1.X + "," + pt1.Y + "," +
                        pt2.X + "," + pt2.Y + ");\r\n";
                }

                Clipboard.SetData(DataFormats.Text, texto);
                MessageBox.Show("Comandos copiados para o Clipboard");
            }
            catch{}


           
        }

        private void lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selecionado >= 0) lista_box.ElementAt(selecionado).Cor = Color.Black;
            if (lista.SelectedIndex < lista_box.Count)
            {
                selecionado = lista.SelectedIndex;
                lista_box.ElementAt(lista.SelectedIndex).Cor = Color.Blue;
            }

            imagem.Invalidate();
        }

        private void duplicar_Click(object sender, EventArgs e)
        {
            if (selecionado >= 0)
            {
                Box cbox = new Box(lista_box.ElementAt(selecionado).P1,lista_box.ElementAt(selecionado).P2);
                lista_box.Add(cbox);
                populaLista();
                lista.SelectedIndex = lista_box.Count - 1;
            }
        }

        private void mover_Click(object sender, EventArgs e)
        {
            if (selecionado >= 0)
            {
                movendo = true;
                p1 = lista_box.ElementAt(selecionado).P1;
                p2 = lista_box.ElementAt(selecionado).P2;
            }
        }

        private void populaLista()
        {
            string sbox;
            Point pt1, pt2;
            lista.Items.Clear();
            selecionado = -1;
            foreach (Box boxp in lista_box)
            {
                boxp.Cor = Color.Black;
                pt1 = boxp.P1;
                pt2 = boxp.P2;

                sbox =  pt1.ToString() + " - " + pt2.ToString();
                lista.Items.Add(sbox);
            }
            imagem.Invalidate();
        }

        private void ajuda_Click(object sender, EventArgs e)
        {
            //Help diag = new Help();
            //diag.ShowDialog();
        }
    }
}
