using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace WindowsFormsApp2
{
    public partial class NewUser1 : Form
    {
        public NewUser1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        database database = new database();


        private void buttonNew_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var password = textBox2.Text;


            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерываем выполнение метода, если поля пустые
            }


            // Проверяем, существует ли пользовательw
            if (CheckUser())
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Прерываем выполнение метода, если пользователь уже существует
            }

            string QueryString = $"insert into register (login_user, password_user,is_admin) values('{login}', '{password}',0)";

            SqlCommand command = new SqlCommand(QueryString, database.getConnection());

            database.openConnection();



                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Аккаунт успешно создан!", "Успех");
                }
                else
                {
                    MessageBox.Show("Аккаунт не создан!");
                }

                database.closeConnection();
            }
        


        private Boolean CheckUser()
        {
            var loginUser = textBox1.Text;
            var passwordUser = textBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string queryString = $"select id_user,login_user,password_user,is_admin from register where login_user = '{loginUser}' and password_user = '{passwordUser}'";

            SqlCommand command = new SqlCommand(@queryString, database.getConnection());


            adapter.SelectCommand = command;


            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {

                return true;
            }
            else
                { return false; }
        }
        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0'; // Показываем пароль (отключаем символ пароля)
            pictureBox1.Visible = false; // Скрываем pictureBox1
            pictureBox2.Visible = true; // Показываем pictureBox2
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•'; // Скрываем пароль (включаем символ пароля)
            pictureBox1.Visible = true; // Показываем pictureBox1
            pictureBox2.Visible = false; // Скрываем pictureBox2
        }

        private void NewUser1_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•';
            pictureBox2.Visible = false;
            textBox1.MaxLength = 50;
            textBox2.MaxLength = 50;
        }
    }
}
