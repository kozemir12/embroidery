using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ColorMenu : Form
    {
        public ColorMenu()
        {
            InitializeComponent();
        }
        List<IColor> mc1;
        List<IColor> mc3;
        int n;
        private void ColorMenu_Load(object sender, EventArgs e)
        {
            Main m = this.Owner as Main;
            n = m.colors.Count();
            mc1 = new List<IColor>();
            mc3 = new List<IColor>();
            //вывод новосозданных элементов с изменениями для каждого цвета на панелькe
            //выведення новостворенних елементов с змінами для кожного коліра на панельці
            for (int l = 0; l < n; l++)
            {
                IColor us1 = new IColor();
                us1.pictureBox1.BackColor = Color.FromArgb(255, m.colors[l].red, m.colors[l].green, m.colors[l].blue);
                us1.label3.Text = m.colors[l].name;
                us1.label4.Text = m.colors[l].amount.ToString();
                us1.Name = l.ToString();
                flowLayoutPanel1.Controls.Add(us1);
                CheckedColor us2 = new CheckedColor();
                us2.pictureBox1.BackColor = Color.FromArgb(255, m.colors[l].red, m.colors[l].green, m.colors[l].blue);
                us2.label3.Text = m.colors[l].name;
                us2.Name = l.ToString();
                us2.textBox1.TextChanged += new EventHandler(this.TextChanged);
                flowLayoutPanel2.Controls.Add(us2);
                IColor us3 = new IColor();
                us3.pictureBox1.BackColor = Color.FromArgb(255, m.colors[l].red, m.colors[l].green, m.colors[l].blue);
                us3.label3.Text = m.colors[l].name;
                us3.Name = l.ToString();
                us3.label4.Text = m.colors[l].amount.ToString();
                flowLayoutPanel3.Controls.Add(us3);
            }
        }
        //процедура, которая при изменения исходного текста пересчитывает и выводит ост. кол-во
        //процедура, що при зміні початкового текста перелічує та виводе зост. кількість
        private void TextChanged(object sender, EventArgs e)
        {
            Main m = this.Owner as Main;
            TextBox c1 = (TextBox)sender;
            CheckedColor c2 = (CheckedColor)c1.Parent;
            int need, have, ind;
            ind = Convert.ToInt32(c2.Name);
            bool res = Int32.TryParse(c1.Text, out have);
            if(res)
            {
                IColor c3 = (IColor)flowLayoutPanel1.Controls[ind];
                need = Convert.ToInt32(c3.label4.Text);
                int rez = need - have;
                if(rez > 0)
                {
                    flowLayoutPanel3.Controls[ind].Visible = true;
                    IColor c4 = (IColor)flowLayoutPanel3.Controls[ind];
                    c4.label4.Text = rez.ToString();
                    flowLayoutPanel3.Controls.RemoveAt(ind);
                    flowLayoutPanel3.Controls.Add(c4);
                    flowLayoutPanel3.Controls.SetChildIndex(c4, ind);
                }
                else
                {
                    flowLayoutPanel3.Controls[ind].Visible = false;
                }
            }
        }
    }
}
