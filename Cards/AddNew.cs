using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cards
{
    public partial class AddNew : Form
    {
        public AddNew()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Вы не написали текст!");
                return;
            }
            if (Program.Cards.ContainsKey(textBox1.Text))
            {
                MessageBox.Show("Элемент с таким названием уже существует!");
                return;
            }

            Program.Cards.Add(textBox1.Text, textBox2.Text);
            Program.Macroses.SaveMacros();
            Program.Macroses.ReRender();

            this.Close();
        }
    }
}
