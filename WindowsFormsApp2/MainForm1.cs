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
using static System.Windows.Forms.AxHost;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

namespace WindowsFormsApp2
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class MainForm1 : Form
    {
        private readonly CheckUser _user;
        database database = new database();
        int selectedRow;

        public MainForm1(CheckUser user)
        {
            StartPosition = FormStartPosition.CenterScreen;
            _user = user;
            InitializeComponent();

            // Подписываемся на события отрисовки строк
            dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView1_RowPostPaint);
            
        }

        // Метод для нумерации строк в dataGridView1
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        // Метод для нумерации строк в dataGridView2
       

        private void IsAdmin()
        {
            управлениеToolStripMenuItem.Enabled = _user.IsAdmin;
            btNew.Enabled = _user.IsAdmin;
            BtDell.Enabled = _user.IsAdmin;
            BtSave.Enabled = _user.IsAdmin;
            BtChange.Enabled = _user.IsAdmin;
            butNew2.Enabled = _user.IsAdmin;
            butChange2.Enabled = _user.IsAdmin;
            butDell2.Enabled = _user.IsAdmin;
            butNew2.Enabled = _user.IsAdmin;
            butSave2.Enabled = _user.IsAdmin;
        }

        private void createColonus()
        {
            //columns для data gridView1
            dataGridView1.Columns.Add("Инвентаризационный номер", "Инвентаризационный номер");
            dataGridView1.Columns.Add("Помещение", "Помещение");
            dataGridView1.Columns.Add("Тип компьютера", "Тип компьютера");
            dataGridView1.Columns.Add("ОС", "Операционная система");
            dataGridView1.Columns.Add("Статус", "Статус компьютера");
            dataGridView1.Columns.Add("Периферия", "Периферия ");

            //columns для data gridView2

            dataGridView2.Columns.Add("id2", "ㅤ№");

            dataGridView2.Columns["id2"].Visible = false;

            dataGridView2.Columns.Add("[Инвентаризационный номер компьютера]", "Инвентаризационный номер компьютера ");
            dataGridView2.Columns.Add("Тип", "Тип комплектующих ");
            dataGridView2.Columns.Add("[Название комплектующих]", "Название комплектующих ");
            dataGridView2.Columns.Add("[Статус комплектующих]", "Статус комплектующих");
            dataGridView2.Columns.Add("Характеристики", "Характеристики");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView2.Columns.Add("IsNew", String.Empty);
            dataGridView2.Columns["IsNew"].Visible = false;

            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns["IsNew"].Visible = false;

            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ClearFilds()
        {
            textBox1.Text = "";
            textBox3.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            richTextBox2.Text = "";
            comboBox6.SelectedIndex = -1;
        }

        private void ReadSingleRow(DataGridView dqw, IDataRecord record)
        {
            dqw.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dqw)
        {
            dqw.Rows.Clear();

            string queryString = $"select * from computers";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dqw, reader);
            }
            reader.Close();
        }

        private void MainForm1_Load(object sender, EventArgs e)
        {
            LoadKabinetNames();
            IsAdmin();
            createColonus();
            RefreshDataGrid(dataGridView1);
            RefreshDataGrid2(dataGridView2);
            LoadComboBox3();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[0].Value.ToString();
                textBox3.Text = row.Cells[1].Value.ToString();
                comboBox2.Text = row.Cells[2].Value.ToString();
                comboBox6.Text = row.Cells[3].Value.ToString();
                comboBox1.Text = row.Cells[4].Value.ToString();
                richTextBox2.Text = row.Cells[5].Value.ToString();
            }
        }

        private void PicUpdate_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void btNew_Click(object sender, EventArgs e)
        {
            Form_Add3 addFm = new Form_Add3();
            addFm.Show();
        }

        private void Search(DataGridView dqw)
        {
            dqw.Rows.Clear();

            string searchString = "SELECT * FROM computers WHERE CONCAT([Инвентаризационный номер],Помещение,[Тип компьютера],ОС,Статус, Периферия) LIKE @searchText";

            SqlCommand command = new SqlCommand(searchString, database.getConnection());
            command.Parameters.AddWithValue("@searchText", "%" + textBoxSearch.Text + "%");

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dqw, reader);
            }

            reader.Close();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
        }

        private void update()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[6].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var deleteQuery = $"DELETE FROM computers WHERE [Инвентаризационный номер] = '{id}'";

                    var command = new SqlCommand(deleteQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var kabinet = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var tip_computers = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var os = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var statuss = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var dop_infa = dataGridView1.Rows[index].Cells[5].Value.ToString();

                    var changeQuery = $"update computers set Помещение = '{kabinet}',[Тип компьютера] = '{tip_computers}',ОС = '{os}',Статус = '{statuss}',Периферия = '{dop_infa}' where [Инвентаризационный номер] = '{id}'";

                    var command = new SqlCommand(changeQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            database.closeConnection();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void BtDell_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                try
                {
                    update();
                    deleteRow();
                    ClearFilds();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            try
            {
                update();
                ClearFilds();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = textBox1.Text;
            var kabinet = textBox3.Text;
            var os = comboBox6.Text;
            var tip_computers = comboBox2.Text;
            var statuss = comboBox1.Text;
            var dop_infa = richTextBox2.Text;

            dataGridView1.Rows[selectedRowIndex].SetValues(id, kabinet, tip_computers, os, statuss, dop_infa);
            dataGridView1.Rows[selectedRowIndex].Cells[6].Value = RowState.Modified;
        }

        private void BtChange_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView1.CurrentCell == null || dataGridView1.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Change();
                update();
                RefreshDataGrid(dataGridView1);
            }
        }

        private void добавитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUser1 newfm = new NewUser1();
            newfm.Show();
        }

        private void управлениеПровамиПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel_Admin plf = new Panel_Admin();
            plf.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ClearFilds();
        }

        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }

        private List<string> GetInventNumbers()
        {
            List<string> inventNumbers = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Инвентаризационный номер"].Value != null)
                {
                    inventNumbers.Add(row.Cells["Инвентаризационный номер"].Value.ToString());
                }
            }

            return inventNumbers;
        }

        private void LoadComboBox3()
        {
            List<string> inventNumbers = GetInventNumbers();
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(inventNumbers.ToArray());
        }

        // DataGridView2 methods
        private void ReadSingleRow2(DataGridView dqw2, IDataRecord record)
        {
            // Используем GetInt32 для ID2 и преобразуем в строку для отображения
            dqw2.Rows.Add(
                record.GetInt32(0).ToString(), // Преобразуем int в string
                record.GetString(1),
                record.GetString(2),
                record.GetString(3),
                record.GetString(4),
                record.GetString(5),
                RowState.ModifiedNew
            );
        }

        private void RefreshDataGrid2(DataGridView dqw2)
        {
            dqw2.Rows.Clear();

            string queryString2 = $"select * from com";
            SqlCommand command = new SqlCommand(queryString2, database.getConnection());
            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow2(dqw2, reader);
            }
            reader.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];

                
                comboBox3.Text = row.Cells[1].Value.ToString();
                comboBox4.Text = row.Cells[2].Value.ToString();
                textBox7.Text = row.Cells[3].Value.ToString();
                comboBox5.Text = row.Cells[4].Value.ToString();
                richTextBox1.Text = row.Cells[5].Value.ToString();
            }
        }

        private void picRefresh2_Click(object sender, EventArgs e)
        {
            LoadKabinetNames();
            RefreshDataGrid2(dataGridView2);
            LoadComboBox3();
        }



        private List<string> _allKabinetNames = new List<string>();

        private void LoadKabinetNames()
        {
            _allKabinetNames.Clear();

            string query = "SELECT DISTINCT Помещение FROM computers";
            SqlCommand command = new SqlCommand(query, database.getConnection());

            database.openConnection();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                _allKabinetNames.Add(reader["Помещение"].ToString().Trim());
            }

            reader.Close();
            database.closeConnection();
        }






        private void Search2(DataGridView dqw2)
        {
            dqw2.Rows.Clear();
            string searchText = textBox6.Text.Trim();

            string searchString = @"SELECT c.* FROM COM c
                         INNER JOIN computers pc 
                         ON c.[Инвентаризационный номер компьютера] = pc.[Инвентаризационный номер]
                         WHERE 1=1";

            SqlCommand command = new SqlCommand(searchString, database.getConnection());
            bool isKabinetSearch = false;

            // 1. Проверка явного указания кабинета через "к:"
            if (searchText.StartsWith("к:", StringComparison.OrdinalIgnoreCase))
            {
                string kabinet = searchText.Substring(2).Trim();
                searchString += " AND pc.Помещение = @kabinet";
                command.Parameters.AddWithValue("@kabinet", kabinet);
                isKabinetSearch = true;
            }
            // 2. Проверка на точное совпадение с кабинетом
            else if (_allKabinetNames.Contains(searchText, StringComparer.OrdinalIgnoreCase))
            {
                searchString += " AND pc.Помещение = @kabinet";
                command.Parameters.AddWithValue("@kabinet", searchText);
                isKabinetSearch = true;
            }
            // 3. Поиск по инвентарному номеру
            else if (Regex.IsMatch(searchText, @"^инв:\s*\S+", RegexOptions.IgnoreCase))
            {
                string invNumber = searchText.Split(new[] { ':' }, 2)[1].Trim();
                searchString += " AND c.[Инвентаризационный номер компьютера] = @invNum";
                command.Parameters.AddWithValue("@invNum", invNumber);
            }
            // 4. Общий поиск
            else if (!string.IsNullOrEmpty(searchText))
            {
                searchString += @" AND (
                            pc.Помещение LIKE @searchText OR
                            CONCAT(c.[Инвентаризационный номер компьютера], c.Тип, 
                                  c.[Название комплектующих], c.[Статус комплектующих], 
                                  c.Характеристики) LIKE @searchText
                          )";
                command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
            }

            command.CommandText = searchString;
            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow2(dqw2, reader);
            }
            reader.Close();

       
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Search2(dataGridView2);
        }

        private void deleteRow2()
        {
            int index = dataGridView2.CurrentCell.RowIndex;
            dataGridView2.Rows[index].Cells[6].Value = RowState.Deleted;
            dataGridView2.Rows[index].Visible = false;

            if (dataGridView2.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView2.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
        }

        private void ClearFilds2()
        {
            
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            richTextBox1.Text = "";
            textBox7.Text = "";
        }

        private void butDell2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell != null)
            {
                try
                {
                    update2();
                    deleteRow2();
                    ClearFilds2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void picClear2_Click(object sender, EventArgs e)
        {
            ClearFilds2();
        }

        private void update2()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView2.Rows[index].Cells[6].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery = $"DELETE FROM COM WHERE id2 = {id}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                    var inventpc = dataGridView2.Rows[index].Cells[1].Value.ToString();
                    var tip = dataGridView2.Rows[index].Cells[2].Value.ToString();
                    var name = dataGridView2.Rows[index].Cells[3].Value.ToString();
                    var stat = dataGridView2.Rows[index].Cells[4].Value.ToString();
                    var dop_infa2 = dataGridView2.Rows[index].Cells[5].Value.ToString();

                    var changeQuery = $"update com set [Инвентаризационный номер компьютера] = '{inventpc}', Тип = '{tip}',[Название комплектующих] = '{name}',[Статус комплектующих] = '{stat}',Характеристики = '{dop_infa2}' WHERE id2 = {id}";

                    var command = new SqlCommand(changeQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            database.closeConnection();
        }

        private void butSave2_Click(object sender, EventArgs e)
        {
            ClearFilds2();
            update2();
        }

        private void Change2()
        {
            var selectedRowIndex = dataGridView2.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView2.Rows[selectedRowIndex].Cells[0].Value);
            var inventpc = comboBox3.Text;
            var tip = comboBox4.Text;
            var name = textBox7.Text;
            var stat = comboBox5.Text;
            var dop_infa2 = richTextBox1.Text;

            dataGridView2.Rows[selectedRowIndex].SetValues(id, inventpc, tip, name, stat, dop_infa2);
            dataGridView2.Rows[selectedRowIndex].Cells[6].Value = RowState.Modified;
        }

        private void butChange2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell == null)
            {
                MessageBox.Show("Выберите строку для изменения!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Change2();
                update2();
                RefreshDataGrid2(dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка изменения: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e) { }

        private void butNew2_Click(object sender, EventArgs e)
        {
            Form_add1 addFm = new Form_add1();
            addFm.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void tabPage2_Click(object sender, EventArgs e)
        {
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void picturOtchet_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем новую книгу Excel
                using (var workbook = new XLWorkbook())
                {
                    // Добавляем лист для данных о компьютерах
                    var computersWorksheet = workbook.Worksheets.Add("Компьютеры");

                    // Экспорт данных из dataGridView1
                    ExportDataGridToExcel(dataGridView1, computersWorksheet);

                    // Добавляем лист для данных о комплектующих
                    var componentsWorksheet = workbook.Worksheets.Add("Комплектующие");

                    // Экспорт данных из dataGridView2
                    ExportDataGridToExcel(dataGridView2, componentsWorksheet);

                    // Предлагаем пользователю выбрать место для сохранения
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel файлы (*.xlsx)|*.xlsx",
                        Title = "Сохранить отчет",
                        FileName = $"Отчет_по_компьютерам_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Отчет успешно сохранен!", "Успех",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportDataGridToExcel(DataGridView dataGrid, IXLWorksheet worksheet)
        {
            // Заголовки столбцов
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visible)
                {
                    worksheet.Cell(1, i + 1).Value = dataGrid.Columns[i].HeaderText;
                }
            }

            // Данные
            for (int row = 0; row < dataGrid.Rows.Count; row++)
            {
                if (dataGrid.Rows[row].Visible) // Пропускаем скрытые строки
                {
                    int colIndex = 0;
                    for (int col = 0; col < dataGrid.Columns.Count; col++)
                    {
                        if (dataGrid.Columns[col].Visible)
                        {
                            var value = dataGrid.Rows[row].Cells[col].Value;
                            worksheet.Cell(row + 2, colIndex + 1).Value = value?.ToString() ?? string.Empty;
                            colIndex++;
                        }
                    }
                }
            }

            // Автонастройка ширины столбцов
            worksheet.Columns().AdjustToContents();
        }
    }
}