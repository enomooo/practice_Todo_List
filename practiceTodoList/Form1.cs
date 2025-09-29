namespace practiceTodoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();      //フォームのUIを初期化
            this.Load += Form1_Load;    //フォームが表示される直前にForm1_Loadを呼び出す
        }
        private void SetupDataGridView()
        {
            todoGridView.Columns.Clear();

            todoGridView.AutoGenerateColumns = false;
            todoGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "タイトル",
                DataPropertyName = "Title",
                Width = 200
            });

            todoGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "期限",
                DataPropertyName = "DueDate",
                Width = 120
            });

            todoGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "メモ",
                DataPropertyName = "Notes",
                Width = 300
            });

            todoGridView.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "完了",
                Text = "完了",
                UseColumnTextForButtonValue = true,
                Width = 80
            });
        }
        private void Form1_Load(object? sender, EventArgs e)
        {

            //JSONからToDoを読み込んで表示
            var manager = new TodoManager();
            this.todos = manager.LoadTodos();

            DisplayTodosInGrid(this.todos);
        }
        private void DisplayTodosInGrid(List<TodoItem> list)
        {
            todoGridView.DataSource = null;
            todoGridView.DataSource = todos;
        }
        private List<TodoItem> todos = new();   //フォーム全体で保持するToDoリスト(前回のリストも含めてという意味でフォーム全体)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("タイトルを入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var newTodo = new TodoItem()
            {
                Title = txtTitle.Text,
                DueDate = dtpDueDate.Value,
                IsCompleted = chkCompleted.Checked,
                Notes = txtNotes.Text,
            };

            todos.Add(newTodo);     //todosリストにnewTodoを追加

            var manager = new TodoManager();
            manager.SaveTodos(todos);       //JSONに保存

            DisplayTodosInGrid(todos);        //ListViewに再表示
            ClearInputFields();
        }
        private void ClearInputFields()
        {
            txtTitle.Text = "";
            dtpDueDate.Value = DateTime.Today;
            chkCompleted.Checked = false;
            txtNotes.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (todoGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除するタスクを選択してください。", "操作エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int index = todoGridView.SelectedRows[0].Index;    //選択された行のインデックス
            todos.RemoveAt(index);                          //リストから削除
            var manager = new TodoManager();
            manager.SaveTodos(todos);        //JSONに全てのフィールドを保存

            DisplayTodosInGrid(todos);    //ディスプレイ更新
        }

        private void todoGridView_CallContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (todoGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                todos[e.RowIndex].IsCompleted = true;

                var manager = new TodoManager();
                manager.SaveTodos(todos);

                DisplayTodosInGrid(todos);
            }
        }
        private void todoGridWiew_CallFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && todos[e.RowIndex].IsCompleted)
            {
                e.CellStyle!.Font = new Font(todoGridView.Font, FontStyle.Strikeout);
                e.CellStyle.ForeColor = Color.Gray;
            }
        }
        private void todoGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (todoGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                todos[e.RowIndex].IsCompleted = true;

                var manager = new TodoManager();
                manager.SaveTodos(todos);

                DisplayTodosInGrid(todos);
            }
        }
    }
}
