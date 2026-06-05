#nullable disable
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class Admin : Form
    {
        private DataGridView _dbTableView;
        private DataTable _currentTable;
        private string _currentTableName = "";
        private List<ColumnMeta> _currentColumns = new List<ColumnMeta>();

        public Admin()
        {
            InitializeComponent();

            Load += Admin_Load;
        }

        private async void Admin_Load(object sender, EventArgs e)
        {
            CreateTableGridFromPlaceholder();

            try
            {
                await LoadTableListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("테이블 목록 불러오기 실패: " + ex.Message);
            }
        }

        private void CreateTableGridFromPlaceholder()
        {
            if (_dbTableView != null)
                return;

            _dbTableView = new DataGridView();
            _dbTableView.Name = "DbTableGrid";
            _dbTableView.Location = DbTable.Location;
            _dbTableView.Size = DbTable.Size;
            _dbTableView.Anchor = DbTable.Anchor;
            _dbTableView.BackgroundColor = System.Drawing.Color.White;
            _dbTableView.BorderStyle = BorderStyle.FixedSingle;
            _dbTableView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dbTableView.AllowUserToAddRows = true;
            _dbTableView.AllowUserToDeleteRows = true;
            _dbTableView.MultiSelect = false;
            _dbTableView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            _dbTableView.RowHeadersVisible = false;
            _dbTableView.DataError += DbTable_DataError;

            var parent = DbTable.Parent ?? this;
            parent.Controls.Add(_dbTableView);
            _dbTableView.BringToFront();

            DbTable.Visible = false;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadSelectedTableAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("테이블 불러오기 실패: " + ex.Message);
            }
        }

        private async void DBSave_Click(object sender, EventArgs e)
        {
            try
            {
                await SaveCurrentTableAsync();
                MessageBox.Show("저장 완료");
                await LoadSelectedTableAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 실패: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void DbTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private async Task LoadTableListAsync()
        {
            string connStr = GetConnectionString();
            EnsureDatabaseFileExists(connStr);

            var tables = new List<string>();

            await using var conn = new SqliteConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT name
                FROM sqlite_master
                WHERE type = 'table'
                  AND name NOT LIKE 'sqlite_%'
                ORDER BY name;
            ";

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tables.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }

            comboBox1.Items.Clear();
            foreach (var table in tables)
                comboBox1.Items.Add(table);

            if (comboBox1.Items.Count > 0 && comboBox1.SelectedIndex < 0)
                comboBox1.SelectedIndex = 0;
        }

        private async Task LoadSelectedTableAsync()
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("테이블을 선택하세요.");
                return;
            }

            string tableName = comboBox1.SelectedItem.ToString();
            string connStr = GetConnectionString();
            EnsureDatabaseFileExists(connStr);

            _currentTableName = tableName;
            _currentColumns = await GetTableColumnsAsync(tableName);

            var table = new DataTable(tableName);

            foreach (var col in _currentColumns)
                table.Columns.Add(col.Name, typeof(object));

            await using var conn = new SqliteConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {QuoteIdentifier(tableName)};";

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = table.NewRow();
                for (int i = 0; i < _currentColumns.Count; i++)
                    row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);

                table.Rows.Add(row);
            }

            table.AcceptChanges();

            _currentTable = table;
            _dbTableView.DataSource = _currentTable;

            ApplyGridOptions();
        }

        private void ApplyGridOptions()
        {
            if (_dbTableView == null || _dbTableView.Columns == null || _currentColumns == null)
                return;

            foreach (DataGridViewColumn gridCol in _dbTableView.Columns)
            {
                var meta = _currentColumns.FirstOrDefault(x => x.Name == gridCol.Name);
                if (meta == null)
                    continue;

                if (meta.IsPrimaryKey)
                {
                    gridCol.ReadOnly = true;
                    gridCol.DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                }
            }
        }

        private async Task SaveCurrentTableAsync()
        {
            if (_currentTable == null || string.IsNullOrWhiteSpace(_currentTableName))
            {
                MessageBox.Show("먼저 테이블을 불러오세요.");
                return;
            }

            var pkColumns = _currentColumns.Where(x => x.IsPrimaryKey).ToList();
            if (pkColumns.Count == 0)
                throw new InvalidOperationException("기본 키가 없는 테이블은 저장할 수 없습니다.");

            _dbTableView.EndEdit();

            if (_dbTableView.DataSource != null && BindingContext[_dbTableView.DataSource] is CurrencyManager cm)
                cm.EndCurrentEdit();

            string connStr = GetConnectionString();
            EnsureDatabaseFileExists(connStr);

            await using var conn = new SqliteConnection(connStr);
            await conn.OpenAsync();

            await using var dbTx = await conn.BeginTransactionAsync();
            var tx = (SqliteTransaction)dbTx;

            try
            {
                foreach (DataRow row in _currentTable.Rows.Cast<DataRow>().Where(x => x.RowState == DataRowState.Deleted).ToList())
                    await DeleteRowAsync(conn, tx, row);

                foreach (DataRow row in _currentTable.Rows.Cast<DataRow>().Where(x => x.RowState == DataRowState.Modified).ToList())
                    await UpdateRowAsync(conn, tx, row);

                foreach (DataRow row in _currentTable.Rows.Cast<DataRow>().Where(x => x.RowState == DataRowState.Added).ToList())
                    await InsertRowAsync(conn, tx, row);

                await dbTx.CommitAsync();
                _currentTable.AcceptChanges();
            }
            catch
            {
                await dbTx.RollbackAsync();
                throw;
            }
        }

        private async Task DeleteRowAsync(SqliteConnection conn, SqliteTransaction tx, DataRow row)
        {
            var pkColumns = _currentColumns.Where(x => x.IsPrimaryKey).ToList();
            var whereList = new List<string>();

            var cmd = conn.CreateCommand();
            cmd.Transaction = tx;

            for (int i = 0; i < pkColumns.Count; i++)
            {
                string paramName = $"$w{i}";
                whereList.Add($"{QuoteIdentifier(pkColumns[i].Name)} = {paramName}");
                cmd.Parameters.AddWithValue(paramName, ToDbValue(row[pkColumns[i].Name, DataRowVersion.Original]));
            }

            cmd.CommandText = $@"
                DELETE FROM {QuoteIdentifier(_currentTableName)}
                WHERE {string.Join(" AND ", whereList)};
            ";

            await cmd.ExecuteNonQueryAsync();
        }

        private async Task UpdateRowAsync(SqliteConnection conn, SqliteTransaction tx, DataRow row)
        {
            var pkColumns = _currentColumns.Where(x => x.IsPrimaryKey).ToList();
            var nonPkColumns = _currentColumns.Where(x => !x.IsPrimaryKey).ToList();

            if (nonPkColumns.Count == 0)
                return;

            var setList = new List<string>();
            var whereList = new List<string>();

            var cmd = conn.CreateCommand();
            cmd.Transaction = tx;

            for (int i = 0; i < nonPkColumns.Count; i++)
            {
                string paramName = $"$p{i}";
                setList.Add($"{QuoteIdentifier(nonPkColumns[i].Name)} = {paramName}");
                cmd.Parameters.AddWithValue(paramName, ToDbValue(row[nonPkColumns[i].Name]));
            }

            for (int i = 0; i < pkColumns.Count; i++)
            {
                string paramName = $"$w{i}";
                whereList.Add($"{QuoteIdentifier(pkColumns[i].Name)} = {paramName}");
                cmd.Parameters.AddWithValue(paramName, ToDbValue(row[pkColumns[i].Name, DataRowVersion.Original]));
            }

            cmd.CommandText = $@"
                UPDATE {QuoteIdentifier(_currentTableName)}
                SET {string.Join(", ", setList)}
                WHERE {string.Join(" AND ", whereList)};
            ";

            await cmd.ExecuteNonQueryAsync();
        }

        private async Task InsertRowAsync(SqliteConnection conn, SqliteTransaction tx, DataRow row)
        {
            var insertColumns = new List<ColumnMeta>();

            foreach (var col in _currentColumns)
            {
                object value = row[col.Name];

                if (col.IsPrimaryKey && IsNullOrEmptyValue(value))
                    continue;

                insertColumns.Add(col);
            }

            if (insertColumns.Count == 0)
                throw new InvalidOperationException("저장할 값이 없습니다.");

            var colNames = new List<string>();
            var valNames = new List<string>();

            var cmd = conn.CreateCommand();
            cmd.Transaction = tx;

            for (int i = 0; i < insertColumns.Count; i++)
            {
                string paramName = $"$p{i}";
                colNames.Add(QuoteIdentifier(insertColumns[i].Name));
                valNames.Add(paramName);
                cmd.Parameters.AddWithValue(paramName, ToDbValue(row[insertColumns[i].Name]));
            }

            cmd.CommandText = $@"
                INSERT INTO {QuoteIdentifier(_currentTableName)}
                ({string.Join(", ", colNames)})
                VALUES
                ({string.Join(", ", valNames)});
            ";

            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<List<ColumnMeta>> GetTableColumnsAsync(string tableName)
        {
            string connStr = GetConnectionString();
            var list = new List<ColumnMeta>();

            await using var conn = new SqliteConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = $"PRAGMA table_info({QuoteIdentifier(tableName)});";

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ColumnMeta
                {
                    Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Type = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    IsNotNull = !reader.IsDBNull(3) && reader.GetInt32(3) == 1,
                    IsPrimaryKey = !reader.IsDBNull(5) && reader.GetInt32(5) > 0
                });
            }

            return list;
        }

        private static string GetConnectionString()
        {
            string dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "WorldBeat",
                "worldbeat.db");

            return $"Data Source={dbPath}";
        }

        private static void EnsureDatabaseFileExists(string connStr)
        {
            string prefix = "Data Source=";
            if (!connStr.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return;

            string dbPath = connStr.Substring(prefix.Length).Trim();
            if (!File.Exists(dbPath))
                throw new FileNotFoundException("DB 파일을 찾을 수 없습니다: " + dbPath);
        }

        private static string QuoteIdentifier(string name)
        {
            return "[" + (name ?? "").Replace("]", "]]") + "]";
        }

        private static object ToDbValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return DBNull.Value;

            if (value is bool b)
                return b ? 1 : 0;

            if (value is DateTime dt)
                return dt.ToString("yyyy-MM-dd HH:mm:ss");

            return value;
        }

        private static bool IsNullOrEmptyValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return true;

            if (value is string s && string.IsNullOrWhiteSpace(s))
                return true;

            return false;
        }

        private sealed class ColumnMeta
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsNotNull { get; set; }
            public bool IsPrimaryKey { get; set; }
        }
    }
}