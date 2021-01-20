using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {

        int x, y;
        Bitmap bmp;
        int n;
        Bitmap copy;
        Graphics h;

        public Main()
        {
            InitializeComponent();
        }
        //функция для сортировки цветов по убыванию
        //функція для сортування кольорів за зменшенням
        private static int CompareColors(Colors y, Colors x)
        {
            if (x.amount == 0)
            {
                if (y.amount == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y.amount == 0)
                {
                    return 1;
                }
                else
                {
                    int retval = x.amount.CompareTo(y.amount);
                    return retval;
                }
            }
        }
        //создает класс цвет и его свойства
        //створює клас колір та його властивості
        public List<Colors> colors;
        public class Colors
        {
            public int red;
            public int green;
            public int blue;
            public string name;
            public int amount = 0;
            public Colors()
            {
                red = 0; green = 0; blue = 0; amount = 0;
            }
            public Colors(int r, int g, int b, string s)
            {
                red = r; green = g; blue = b; amount++; name = s;
            }
            public void AddAmount()
            {
                amount++;
            }
        };
        // Завантажує кольори з тектового файла
        void Load_Colors() // Загружает цвета из текстового файла
        {
            FileStream fs = new FileStream("Colors.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string s;
            string s1 = "";
            string[] s2;
            int i = 0, r = 0, g = 0, b = 0;
            while(sr.Peek() > -1)
            {
                if (i == 5) i = 0;
                s = sr.ReadLine();
                if (i == 1) s1 = s;
                if (i == 2)
                {
                    s2 = s.Split(' ');
                    r = Convert.ToInt32(s2[0]);
                    g = Convert.ToInt32(s2[1]);
                    b = Convert.ToInt32(s2[2]);
                }
                if (i == 3)
                {
                    colors.Add(new Colors(r, g, b, s1));
                }
                i++;
            }
            sr.Close();
        }
        //знаходження середнього кольоу в секції
        //нахождение среднего цвета в секции
        Colors SrColor(int i, int k)
        {
            Color p;
            Colors p2 = new Colors();
            int r;
            int g;
            int b;
            int sumr = 0, sumg = 0, sumb = 0;
            int srr, srg, srb;
            sumr = 0;
            sumg = 0;
            sumb = 0;
            if ((n + i < x) && (n + k < y))
            {
                for (int z = i; z < i + n; z++)
                {
                    for (int t = k; t < k + n; t++)
                    {
                        p = bmp.GetPixel(z, t);
                        r = p.R;
                        g = p.G;
                        b = p.B;
                        sumr += r;
                        sumg += g;
                        sumb += b;
                    }
                }
            }
            srr = (int)sumr / (n * n);
            srg = (int)sumg / (n * n);
            srb = (int)sumb / (n * n);
            p2.red = srr;
            p2.green = srg;
            p2.blue = srb;
            return p2;
        }
        //зафарбовування секції середнім кольором
        //заливка секции средним цветом
        void DrawPixel(int i, int k, Color c)
        {
            if ((n + i < x) && (n + k < y))
            {
                for (int z = i; z < i + n; z++)
                {
                    for (int t = k; t < k + n; t++)
                    {
                        bmp.SetPixel(z, t, c);
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            
            //завантаження зображення
            //загрузка картинки
            Graphics g1 = pictureBox1.CreateGraphics();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Image files (*.BMP, *.JPG, *.JPEG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.jpeg;*.gif;*.tif;*.png;*.ico;*.emf;*.wmf";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = img;
            }
            n = int.Parse(comboBox1.Text);
            x = pictureBox1.Width;
            y = pictureBox1.Height;
            bmp = (Bitmap)pictureBox1.Image;
            colors = new List<Colors>();
            Load_Colors();
            flowLayoutPanel1.Controls.Clear();
            Colors t;
            int d;


            button2.Enabled = true;
            button3.Enabled = true;
            label2.Visible = true;
            label3.Visible = true;


            //додавання до списку кольорів
            //добавление в список цветов
            for (int i = 0; i < x; i += n)
            {
                for (int k = 0; k < y; k += n)
                {
                    t = SrColor(i, k);
                    int min = 10000, imin = 0;
                    for(int l = 0; l < colors.Count(); l++)
                    {
                        d = Convert.ToInt32(Math.Sqrt(Math.Pow(t.red - colors[l].red, 2) + Math.Pow(t.green - colors[l].green, 2) + Math.Pow(t.blue - colors[l].blue, 2)));
                        if (d < min)
                        {
                            min = d;
                            imin = l;
                        }
                    }
                    colors[imin].AddAmount();
                    DrawPixel(i, k, Color.FromArgb(colors[imin].red, colors[imin].green, colors[imin].blue));
                }
            }

            //перетворюємо наш набір секцій в нове поле для малювання
            //делаем из нашего набора секций новое поле для рисования
            Rectangle rect1 = new Rectangle(0,0,x - x%n,y - y%n);
            copy = bmp.Clone(rect1, bmp.PixelFormat);

            //сітка
            //сетка
            h = Graphics.FromImage(copy);
            for (int z = 0; z <= pictureBox1.Width; z += n)
            {
                h.DrawLine(Pens.Gray, z, 0, z, pictureBox1.Height);
            }
            for (int z = 0; z <= pictureBox1.Height; z += n)
            {
                h.DrawLine(Pens.Gray, 0, z, pictureBox1.Width, z);
            }

            //сортування кольорів за зменшенням та виведення на форму
            //сортировка цветов по убыванию и вывод на форму
            colors.Sort(CompareColors);
            pictureBox1.Image = copy;

            for(int l = 0; l < colors.Count(); l++)
            {
                IColor us1 = new IColor();
                us1.pictureBox1.BackColor = Color.FromArgb(255, colors[l].red, colors[l].green, colors[l].blue);
                us1.label3.Text = colors[l].name;
                us1.label4.Text = colors[l].amount.ToString();
                flowLayoutPanel1.Controls.Add(us1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //збереження
            //сохранение
            saveFileDialog1.Filter = "Image files (*.BMP, *.JPG, *.JPEG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.jpeg;*.gif;*.tif;*.png;*.ico;*.emf;*.wmf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //виклик підменю кольорів
           //вызов подменю цветов
                ColorMenu menu1 = new ColorMenu();
                menu1.Owner = this;
                menu1.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        //клік мишею по кольору викликає його назву
        //при клике мышкой по цвету высвечивается его название
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
           int i,k;
           string s="";
           i = e.X;
           k = e.Y;
           Color p = bmp.GetPixel(i, k);
           Load_Colors();
            for (int l = 0; l < colors.Count; l++ )
            {
                if (p.R == colors[l].red && p.G == colors[l].green && p.B == colors[l].blue) 
                s = colors[l].name;
            }
            label2.Text = s;
 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
    }
}
