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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WindowsFormsApp2;

namespace WindowsFormsApp2
{
    public partial class Form_Add3 : Form
    {
        database database = new database();
        public Form_Add3()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form_add2_Load(object sender, EventArgs e)
        {



        }


    
    
    private void Form_Add3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        { 
    // Проверка заполнения обязательных полей
    if (string.IsNullOrWhiteSpace(textBox1.Text) ||
        string.IsNullOrWhiteSpace(textBox2.Text) ||
        string.IsNullOrWhiteSpace(comboBox1.Text) ||
        string.IsNullOrWhiteSpace(comboBox2.Text) ||
        string.IsNullOrWhiteSpace(comboBox3.Text))
    {
        MessageBox.Show("Все поля кроме поля \"Периферия\" должны быть заполнены!", 
                        "Ошибка", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
        return;
    }

    var invent = textBox1.Text;
        var kabinet = textBox2.Text;
        var tip_computers = comboBox2.Text;
        var os = comboBox3.Text;
        var statuss = comboBox1.Text;
        var dop_infa = richTextBox1.Text;

    database.openConnection();

    // Проверка на уникальность инвентаризационного номера
    string checkQuery = "SELECT COUNT(*) FROM computers WHERE [Инвентаризационный номер] = @invent";
    
    using (SqlCommand checkCommand = new SqlCommand(checkQuery, database.getConnection()))
    {
        checkCommand.Parameters.AddWithValue("@invent", invent);
        int existingCount = (int)checkCommand.ExecuteScalar();
        
        if (existingCount > 0)
        {
            MessageBox.Show("Инвентаризационный номер уже существует!", 
                            "Ошибка", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
            database.closeConnection();
            return;
        }
    }

    // Добавление новой записи
    string addQuery = @"INSERT INTO computers 
                        ([Инвентаризационный номер], Помещение, [Тип компьютера], ОС, Статус, Периферия) 
                        VALUES 
                        (@invent, @kabinet, @tip_computers, @os, @statuss, @dop_infa)";

using (SqlCommand addCommand = new SqlCommand(addQuery, database.getConnection()))
{
    addCommand.Parameters.AddWithValue("@invent", invent);
    addCommand.Parameters.AddWithValue("@kabinet", kabinet);
    addCommand.Parameters.AddWithValue("@tip_computers", tip_computers);
    addCommand.Parameters.AddWithValue("@os", os);
    addCommand.Parameters.AddWithValue("@statuss", statuss);
    addCommand.Parameters.AddWithValue("@dop_infa", dop_infa);

    addCommand.ExecuteNonQuery();
}

MessageBox.Show("Запись успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
database.closeConnection();
}

    }
}





