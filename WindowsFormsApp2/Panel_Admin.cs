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
using System.Reflection;

namespace WindowsFormsApp2
{
    public partial class Panel_Admin : Form
    {
        database database =new database();
        public Panel_Admin()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }


        private void createColonus()
        {
            dataGridView1.Columns.Add("id_user", "ID");
            dataGridView1.Columns.Add("login", "Логин");
            dataGridView1.Columns.Add("Password", "Пароль");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "Isadmin";
            dataGridView1 .Columns.Add(checkColumn);
        }


        private void ReadSingleRow(IDataRecord record)
        {
            dataGridView1.Rows.Add(record.GetInt32(0),record.GetString(1),record.GetString(2),record.GetBoolean(3));
        }

        private void RefreshDataGrid()
        {
            dataGridView1.Rows.Clear();

            string queryString = $"select * FROM register;";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());


            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            { 
                ReadSingleRow(reader);
            }
            reader.Close();

            database.closeConnection();
        }

        private void Panel_Admin_Load(object sender, EventArgs e)
        {
            createColonus();
            RefreshDataGrid();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++) 
            {
                
                var id = dataGridView1.Rows[index].Cells[0].Value.ToString();

                var isadmin = dataGridView1.Rows[index].Cells[3].Value.ToString();

                var changeQuery = $"update register set is_admin='{isadmin}' where id_user = '{id}'";

                var command =new SqlCommand(changeQuery, database.getConnection());
                command.ExecuteNonQuery();

            }
            database.closeConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            database.openConnection();

            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;


            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);

            var deleteQuery = $"delete from register Where id_user ={id}";

            var command = new SqlCommand(deleteQuery, database.getConnection());
            command.ExecuteNonQuery();
            database.closeConnection();

            RefreshDataGrid();
        }
    }
}
