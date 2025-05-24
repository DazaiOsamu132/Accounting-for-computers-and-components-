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

namespace WindowsFormsApp2
{
    public partial class Form_add1 : Form
    {
        database database = new database();

        public Form_add1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();
            var inventpc = comboBox1.Text;
            var tip = comboBox2.Text;
            var stat = comboBox3.Text;
            var name = textBox1.Text;
            var dop_infa2 = richTextBox1.Text;

            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox2.Text) ||
                string.IsNullOrWhiteSpace(comboBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Все поля кроме поля \"Характеристики\" должны быть заполнены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Если все проверки пройдены, добавляем запись в базу данных
            string addQuery = $"INSERT INTO com ([Инвентаризационный номер компьютера], Тип, [Название комплектующих], [Статус комплектующих], Характеристики) VALUES ('{inventpc}', '{tip}', '{name}', '{stat}', '{dop_infa2}')";

            var command = new SqlCommand(addQuery, database.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            database.closeConnection();
        }

        private void Form_add1_Load(object sender, EventArgs e)
        {
            LoadComboBoxData(comboBox1, "SELECT [Инвентаризационный номер] FROM computers");
        }

        private void LoadComboBoxData(ComboBox comboBox, string query)
        {
            try
            {
                database.openConnection();

                using (SqlCommand command = new SqlCommand(query, database.getConnection()))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBox.Items.Clear(); // Очистка ComboBox перед добавлением новых данных

                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader[0].ToString()); // Добавление данных в ComboBox
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                database.closeConnection();
            }
        }
    }
}