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
    public partial class Form2 : Form
    {
        public static key kl;

        public Form2(key Key)
        {
            InitializeComponent();
            kl = Key;
        }

        private int i = 3;

        private void button1_Click(object sender, EventArgs e)
        {
            string oldpas = textBox1.Text;
            string newpas = textBox2.Text;
            string pass;
            FileStream file = new FileStream("pass.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            pass = reader.ReadLine();
            reader.Close();
            oldpas = Encode(oldpas, kl.Val);
            if (pass == oldpas)
            {
                FileStream wfile = new FileStream("pass.txt", FileMode.Truncate);
                StreamWriter writer = new StreamWriter(wfile);
                Random rnd = new Random();
                int i = 1 + rnd.Next(100);
                newpas = Encode(newpas, GenerateKeyWord(newpas.Length, i));
                writer.WriteLine(newpas);
                writer.Close();
                MessageBox.Show("Your new key: " + GenerateKeyWord(newpas.Length, i));
                kl.Val = GenerateKeyWord(newpas.Length, i);
            }
            else
            {
                i--;
                MessageBox.Show("Wrong password or key! You have " + i + " chances!");
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

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
