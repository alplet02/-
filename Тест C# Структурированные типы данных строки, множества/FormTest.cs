using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace QuickTest
{
    public partial class FormTest : Form
    {
        string file_name = "t.txt";
        string[,] massiv;
        int total, kolvo_voprosov, praviln_otvetov, nepraviln_otvetov;
        static Random rand = new Random();

        public FormTest()
        {            
            InitializeComponent();
            init_test();
        }

        private void init_test()
        {
            button_next.Text = "Следующий вопрос";
            kolvo_voprosov = 9;
            praviln_otvetov = 0;
            nepraviln_otvetov = 0;
            load_file();
            radio_checked();
            radio_turn_on_off();
            label_result.Text = "";
            kolvo_voprosov--;
            show_question();
        }

        private void load_file()
        {
            try
            {
                string[] lines = File.ReadAllLines(file_name, Encoding.UTF8);
                total = lines.Length / 4;
                massiv = new string[kolvo_voprosov, 4];

                int[] temp = new int[kolvo_voprosov];
                int j;
                int k = 0;
                do
                {
                    j = rand.Next(0, total) * 4;
                    if (!temp.Contains(j))
                    {
                        massiv[k, 0] = lines[j];
                        for (int i = 1; i < 4; i++)
                            massiv[k, i] = lines[j + i];
                        temp[k] = j;
                        k++;
                    }
                } while (!(k == kolvo_voprosov));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }                                        
        }

        private void show_question()
        {
            radio_checked();
            label_question.Text = massiv[kolvo_voprosov, 0];
            radio_tags(rand.Next(1,7));
            radio_answer1.Text  = massiv[kolvo_voprosov, Convert.ToInt16(radio_answer1.Tag)];
            radio_answer2.Text  = massiv[kolvo_voprosov, Convert.ToInt16(radio_answer2.Tag)];
            radio_answer3.Text  = massiv[kolvo_voprosov, Convert.ToInt16(radio_answer3.Tag)];
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            if (kolvo_voprosov < 0) { init_test(); return; }

            if (!(radio_answer1.Checked || radio_answer2.Checked || radio_answer3.Checked))
            {
                MessageBox.Show("Выберите вариант ответа");
                return;
            }
            if ((radio_answer1.Checked & Convert.ToInt16(radio_answer1.Tag) == 1) ||
                (radio_answer2.Checked & Convert.ToInt16(radio_answer2.Tag) == 1) ||
                (radio_answer3.Checked & Convert.ToInt16(radio_answer3.Tag) == 1))
            {
                praviln_otvetov++;
                kolvo_voprosov--;
            }
            else
            {
                MessageBox.Show("Правильный вариант ответа: "+massiv[kolvo_voprosov, 1]);
                nepraviln_otvetov++;
                kolvo_voprosov--;
            }
            
            if (kolvo_voprosov < 0) 
            { 
                show_result();
                label_question.Text = "Результат теста";
                button_next.Text = "Тест заново";
                radio_turn_on_off();
                return; 
            }            
            show_question();
        }

        private void radio_turn_on_off()
        {
            if (kolvo_voprosov < 0)
            {
                radio_answer1.Visible = false;
                radio_answer2.Visible = false;
                radio_answer3.Visible = false;
            }
            else
            {
                radio_answer1.Visible = true;
                radio_answer2.Visible = true;
                radio_answer3.Visible = true;
            }
        }

        private void radio_checked()
        {
            radio_answer1.Checked = false;
            radio_answer2.Checked = false;
            radio_answer3.Checked = false;
        }


        private void radio_tags(int i)
        {
            switch (i)
            {
                case 1: radio_answer1.Tag = 1; radio_answer2.Tag = 2; radio_answer3.Tag = 3; break;
                case 2: radio_answer1.Tag = 1; radio_answer2.Tag = 3; radio_answer3.Tag = 2; break;
                case 3: radio_answer1.Tag = 2; radio_answer2.Tag = 1; radio_answer3.Tag = 3; break;
                case 4: radio_answer1.Tag = 2; radio_answer2.Tag = 3; radio_answer3.Tag = 1; break;
                case 5: radio_answer1.Tag = 3; radio_answer2.Tag = 1; radio_answer3.Tag = 2; break;
                case 6: radio_answer1.Tag = 3; radio_answer2.Tag = 2; radio_answer3.Tag = 1; break;
            }
        }

        private void show_result()
        {
            label_result.Text = "Правильных ответов: "+praviln_otvetov.ToString()+"\n"+
                "Неправильных ответов: "+nepraviln_otvetov.ToString();
        }


    }
}
