using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PassAuth
{
    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FileInfo fileInf = new FileInfo("pass.txt");
            if (fileInf.Exists)
                groupBox1.Visible = true;
        }

        private int i = 3;

        private void button1_Click(object sender, EventArgs e)
        {
            string pas = textBox1.Text;
            string pass;
            FileInfo fileInf = new FileInfo("pass.txt");
            if (fileInf.Exists)
            {
                FileStream file = new FileStream("pass.txt", FileMode.Open);
                StreamReader reader = new StreamReader(file);
                pass = reader.ReadLine();
                reader.Close();
                key Key = new key();
                Key.Val = textBox2.Text;
                pas = Encode(pas, Key.Val);
                if (pas == pass)
                {
                    Form ifrm = new Form2(Key);
                    ifrm.Show();
                    this.Hide();
                }
                else
                {
                    i--;
                    MessageBox.Show("Wrong password or key! You have "+i+" chances!");
                }
            }
            else
            {
                FileStream file = new FileStream("pass.txt", FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                Random rnd = new Random();
                int i = 1 + rnd.Next(100);
                pas = Encode(pas, GenerateKeyWord(pas.Length, i));
                writer.WriteLine(pas);
                MessageBox.Show("Your key: " + GenerateKeyWord(pas.Length, i));
                writer.Close();
                key Key = new key();
                Key.Val = GenerateKeyWord(pas.Length, i);
                Form ifrm = new Form2(Key);
                ifrm.Show();
                this.Hide();
            }
            if (i==0)
            {
                this.Close();
            }
        }

        public static char[] characters = new char[] {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','X','X','0','1','2','3','4','5','6','7','8','9','_'};

        public int N = characters.Length;

        public string Encode(string input, string keyword)
        {
            input = input.ToUpper();
            keyword = keyword.ToUpper();

            string result = "";

            int keyword_index = 0;

            foreach (char symbol in input)
            {
                int c = (Array.IndexOf(characters, symbol) +
                    Array.IndexOf(characters, keyword[keyword_index])) % N;

                result += characters[c];

                keyword_index++;

                if ((keyword_index + 1) == keyword.Length)
                    keyword_index = 0;
            }

            return result;
        }

        public string GenerateKeyWord(int length, int startSeed)
        {
            Random rand = new Random(startSeed);

            string result = "";

            for (int i = 0; i < length; i++)
                result += characters[rand.Next(0, characters.Length)];

            return result;
        }
    }

    public class key
    {
        private string val;
        public string Val
        {
            get { return val; }
            set { val = value; }
        }
    }
}
