using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Log_in : Form
    {

        database database = new database();
        public Log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            var loginUser = textBox1.Text;
            var passwordUser = textBox2.Text;


            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();

            string querystring = $"select id_user,login_user,password_user,is_admin from register where login_user = '{loginUser}' and password_user = '{passwordUser}'";


            SqlCommand command = new SqlCommand(querystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (string.IsNullOrWhiteSpace(loginUser) || string.IsNullOrWhiteSpace(passwordUser))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерываем выполнение метода, если поля пустые
            }

            if (table.Rows.Count == 1)
            {

               var user = new CheckUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));

                MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MainForm1 frm = new MainForm1(user);
                this.Hide();
                frm.ShowDialog();
                this.Show();
                

            }

            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Аккаунт не существует!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

        }
        private void Sing_Up_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•';
            pictureBox2.Visible = false;
            textBox1.MaxLength = 50;
            textBox2.MaxLength = 50;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•'; // Скрываем пароль (включаем символ пароля)
            pictureBox1.Visible = true; // Показываем pictureBox1
            pictureBox2.Visible = false; // Скрываем pictureBox2
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0'; // Показываем пароль (отключаем символ пароля)
            pictureBox1.Visible = false; // Скрываем pictureBox1
            pictureBox2.Visible = true; // Показываем pictureBox2
        }
    }
    }
