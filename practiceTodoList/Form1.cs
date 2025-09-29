namespace practiceTodoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();      //フォームのUIを初期化
            this.Load += Form1_Load;    //フォームが表示される直前にForm1_Loadを呼び出す
        }
        private void SetupListViewColumns()
        {
            todoListView.Columns.Clear();

            todoListView.Columns.Add("タイトル", 200);
            todoListView.Columns.Add("期限", 180);
            todoListView.Columns.Add("完了", 60);
            todoListView.Columns.Add("メモ", 400);
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            // ListViewの列を追加
            SetupListViewColumns();

            //JSONからToDoを読み込んで表示
            var manager = new TodoManager();
            this.todos = manager.LoadTodos();
            DisplayTodos(this.todos);
        }
        private void DisplayTodos(List<TodoItem> list)
        {
            todoListView.Items.Clear();     //表示を初期化

            foreach (var todo in list)
            {
                var item = new ListViewItem(todo.Title);                //タイトル(最初の列)
                item.SubItems.Add(todo.DueDate.ToShortDateString());    //期限
                item.SubItems.Add(todo.IsCompleted ? "✓" : "");        //完了状態
                item.SubItems.Add(todo.Notes);                          //メモ

                todoListView.Items.Add(item);   //ListViewに追加
            }
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

            DisplayTodos(todos);        //ListViewに再表示
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
            if (todoListView.SelectedIndices.Count == 0)
            {
                MessageBox.Show("削除するタスクを選択してください。","操作エラー",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int index = todoListView.SelectedIndices[0];    //選択された行のインデックス
            todos.RemoveAt(index);                          //リストから削除
            var manager = new TodoManager();
            manager.SaveTodos(todos);        //JSONに全てのフィールドを保存

            DisplayTodos(todos);    //ディスプレイ更新
        }
    }
}
