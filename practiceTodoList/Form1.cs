namespace practiceTodoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();      //フォームのUIを初期化
            this.Load += Form1_Load;    //フォームが表示される直前にForm1_Loadを呼び出す

            /*ヘッダーを消すと選択出来なくなり、Todo削除が出来なくなる*/
            //todoGridView.RowHeadersVisible = false;     //一番左の列の空白(ヘッダー)を非表示

            todoGridView.CellValueChanged += todoGridView_CellValueChanged; //true or falseの状態変化の保存・表示
            todoGridView.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (todoGridView.IsCurrentCellDirty)
                {
                    todoGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };

            todoGridView.CellContentClick += todoGridView_CellContentClick;
            todoGridView.CellFormatting += todoGridView_CallFormatting;  //true or falseの状態変化を検知して打消し線を表示
        }

        private void SetupDataGridView()
        {
            todoGridView.Columns.Clear();

            todoGridView.AutoGenerateColumns = false;

            todoGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "期日",
                DataPropertyName = "DueDate",
                Width = 200
            });

            todoGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "タイトル",
                DataPropertyName = "Title",
                Width = 300
            });

            todoGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "メモ",
                DataPropertyName = "Notes",
                Width = 420
            });

            todoGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "完了",
                DataPropertyName = "IsCompleted",
                Width = 80
            });
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            SetupDataGridView();    //class_Form1にあるメソッド＝列を定義しているので呼び出す

            //JSONからToDoを読み込んで表示
            var manager = new TodoManager();
            this.todos = manager.LoadTodos();

            DisplayTodosInGrid(this.todos);
        }

        //最新状態のTodoリストを表示するメソッド(立ち上げ時、リスト変更時に呼び出し更新)
        private void DisplayTodosInGrid(List<TodoItem> list)
        {
            var sorted = list.OrderBy(todo => todo.DueDate).ToList();
            todoGridView.DataSource = null;
            todoGridView.DataSource = sorted;

            todos = todos.OrderBy(todo => todo.DueDate).ToList();
        }

        //フォーム全体で保持するToDoリスト(前回のリストも含めてという意味でフォーム全体)
        private List<TodoItem> todos = new();

        //Todoリストを登録するメソッド
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
                Notes = txtNotes.Text,
            };

            todos.Add(newTodo);     //todosリストにnewTodoを追加

            var manager = new TodoManager();
            manager.SaveTodos(todos);       //JSONに保存

            DisplayTodosInGrid(todos);        //ListViewに再表示
            ClearInputFields();
        }

        //入力フォームを初期化するメソッド(登録後、書いたTodoを初期化)
        private void ClearInputFields()
        {
            txtTitle.Text = "";
            dtpDueDate.Value = DateTime.Today;
            txtNotes.Text= "";
        }

        //Todoリスト削除メソッド
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (todoGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除するタスクを選択してください。", "操作エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedTodo = todoGridView.SelectedRows[0].Index;    //選択された行のインデックス
            todos.RemoveAt(selectedTodo);                          //リストから削除
            var manager = new TodoManager();
            manager.SaveTodos(todos);        //JSONに全てのフィールドを保存

            DisplayTodosInGrid(todos);    //ディスプレイ更新
        }

        //表示時の見た目（フォーマット）を動的に整えるためのメソッド
        private void todoGridView_CallFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            //期限列を「yyyyy/MM/dd」形式に統一
            if (todoGridView.Columns[e.ColumnIndex].DataPropertyName == "DueDate" && e.Value is DateTime dt)
            {
                e.Value = dt.ToString("yyyy/MM/dd");
                e.FormattingApplied = true; 
            }

            //完了済みなら打ち消し線+グレー表示
            if (e.RowIndex >= 0 && !todoGridView.Rows[e.RowIndex].IsNewRow)
            {
                var item = (TodoItem)todoGridView.Rows[e.RowIndex].DataBoundItem;
                if (item.IsCompleted)
                {
                    e.CellStyle!.Font = new Font(todoGridView.Font, FontStyle.Strikeout);
                    e.CellStyle.ForeColor = Color.Gray;
                    e.FormattingApplied = true;
                }
            }
        }

        //セルクリック時に発火
        //完了ボタンのクリックした時のメソッド
        private void todoGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)    //e.RowIndexは選択されたRow
            {
                var item = (TodoItem)todoGridView.Rows[e.RowIndex].DataBoundItem;
                item.IsCompleted = true;

                var manager = new TodoManager();
                manager.SaveTodos(todos);

                DisplayTodosInGrid(todos);
            }
            else
            {

            }
        }

        //セルの値が変更された時に発火
        //チェックボックスの変更を自動で保存
        private void todoGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (todoGridView.Columns[e.ColumnIndex].DataPropertyName == "IsCompleted")
            {
                var item = (TodoItem)todoGridView.Rows[e.RowIndex].DataBoundItem;

                item.IsCompleted = (bool)todoGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                                 //選択されたRowの選択されたColumn(cell)のValue(bool)

                var manager = new TodoManager();
                manager.SaveTodos(todos);
            }
        }
    }
}   
