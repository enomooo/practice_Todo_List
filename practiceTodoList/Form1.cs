namespace practiceTodoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // フォームのUIを初期化
            InitializeComponent();

            // フォームが表示される直前にForm1_Loadを呼び出す
            this.Load += Form1_Load;

            // DataGridViewのイベント登録 = "完了"のtrue or falseの状態変化の保存・表示
            todoGridView.CellValueChanged += todoGridView_CellValueChanged;

            // 現在のセルがDirtyになったら発火 (Dirty=編集中で何か書いているが、未確定という意味) ((s, e)は、(sender, EventArgs)の略)
            todoGridView.CurrentCellDirtyStateChanged += (s, e) =>
            {
                // 現在のセルがDirtyなら確定させた時に、CellValueChangedイベントを発火
                if (todoGridView.IsCurrentCellDirty)
                {
                    // CommitEditは編集中のセルの変更を確定させるためのもの
                    todoGridView.CommitEdit(DataGridViewDataErrorContexts.Commit); 
                }
            };

            todoGridView.CellContentClick += todoGridView_CellContentClick;
            todoGridView.CellFormatting += todoGridView_CallFormatting;
        }

        /// <summary>
        /// todoGridViewの列構成を初期化、定義する
        /// </summary>
        private void SetupDataGridView()
        {
            // 既存のtodoGridViewを初期化
            todoGridView.Columns.Clear();

            // 下でWidthを指定するので自動調整をOFF
            todoGridView.AutoGenerateColumns = false;

            // 各列の定義(期日、タイトル、メモ、完了)
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
            // todoGridViewを定義しているメソッドなのでまず呼び出す
            SetupDataGridView();

            // JSONからToDoを読み込んで表示
            var manager = new TodoManager();
            this.todos = manager.LoadTodos();

            DisplayTodosInGrid(this.todos);
        }

        /// <summary>
        /// 指定されたTodoItemリストを期限順に並べ替え、DataGridViewに表示する
        /// </summary>
        /// <param name="list">表示するTodoItemのリスト</param>
        private void DisplayTodosInGrid(List<TodoItem> list)
        {
            // 表示用に、期限順で並べ替えたリストを作成
            var sorted = list.OrderBy(todo => todo.DueDate).ToList();
           
            // DataGridViewのデータソースをクリアして、sortedを代入
            todoGridView.DataSource = null;
            todoGridView.DataSource = sorted;

            // todos(メモリ上のList<TodoItem>)も期限順に並べ替えて保持
            todos = todos.OrderBy(todo => todo.DueDate).ToList();
        }

        /// <summary>
        /// フォーム全体で保持するTodoリストの内部状態をtodosに記録
        /// todosはロジックに使ったり、編集、記録に使う
        /// ちなみに、DataGridViewは画面表示のためで別物
        /// </summary>
        private List<TodoItem> todos = new();

        /// <summary
        /// Todoリストを登録するメソッド
        /// </summary>
        /// <param name="sender">イベント発火元("追加"ボタン)</param>
        /// <param name="e">イベント引数</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 空白でtodoを登録しようとすると、エラーの警告を出す
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("タイトルを入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 入力内容のTodoItem作成
            var newTodo = new TodoItem()
            {
                Title = txtTitle.Text,
                DueDate = dtpDueDate.Value,
                Notes = txtNotes.Text,
            };

            // todosリストにnewTodoを追加
            todos.Add(newTodo);

            // 追加されたtodosを全部をJSONに保存
            var manager = new TodoManager();
            manager.SaveTodos(todos);

            DisplayTodosInGrid(todos);
            ClearInputFields();
        }

        /// <summary>
        /// 入力フォームを初期化するメソッド(登録後、書いたTodoを初期化)
        /// </summary>
        private void ClearInputFields()
        {
            txtTitle.Text = "";
            dtpDueDate.Value = DateTime.Today;
            txtNotes.Text= "";
        }

        /// <summary>
        /// Todoリスト削除メソッド
        /// ヘッダーをクリックしないとRowの指定をできない
        /// </summary>
        /// <param name="sender">"削除ボタン"</param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 現在、選択されている行の数をCount == 0、つまり指定されていないのに削除しようとしたらエラー
            if (todoGridView.SelectedRows.Count == 0) 
            {
                MessageBox.Show("削除するタスクを選択してください。", "操作エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 選択された行のインデックス
            var selectedTodo = todoGridView.SelectedRows[0].Index;

            // リストから削除
            todos.RemoveAt(selectedTodo);

            // JSONに全てのフィールドを保存
            var manager = new TodoManager();
            manager.SaveTodos(todos); 

            DisplayTodosInGrid(todos);
        }

        /// <summary>
        /// 表示時の見た目（フォーマット）を動的に整えるフォーマット処理
        /// 期日列の表示形式の統一
        /// 完了済みtodoは打ち消し線とグレー表示
        /// </summary>
        /// <param name="sender">todoGridView</param>
        /// <param name="e"></param>
        private void todoGridView_CallFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            // 期限列を「yyyyy/MM/dd」形式に統一(今フォーマットしようとしているセルが、DueDate列でDateTime型なら)
            if (todoGridView.Columns[e.ColumnIndex].DataPropertyName == "DueDate" && e.Value is DateTime dt)
            {
                e.Value = dt.ToString("yyyy/MM/dd");
                e.FormattingApplied = true; 
            }

            // 完了済みなら打ち消し線+グレー表示(IsNewRow は、DataGridView が自動的に生成する「新規入力用の空行)
            if (e.RowIndex >= 0 && !todoGridView.Rows[e.RowIndex].IsNewRow)
            {
                // 今処理している行に対応するTodoItemオブジェクトを取得
                var item = (TodoItem)todoGridView.Rows[e.RowIndex].DataBoundItem;
                
                // もしitemが完了なら、スタイルの変更
                if (item.IsCompleted)
                {
                    e.CellStyle!.Font = new Font(todoGridView.Font, FontStyle.Strikeout);
                    e.CellStyle.ForeColor = Color.Gray;

                    // 独自スタイル適応済みと明示
                    e.FormattingApplied = true;
                }
            }
        }

        /*セルクリック時に発火
        完了ボタンのクリックした時のメソッド*/

        /// <summary>
        /// セルクリック時に発火するイベントハンドラ
        /// 「完了」ボタンがクリックされた行の TodoItem を完了状態に更新し、
        /// 内部リストと表示を同期させる。
        /// </summary>
        /// <param name="sender">todoGridView</param>
        /// <param name="e">クリックされたセルの行・列のインデックス</param>
        private void todoGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 今処理している行に対応するTodoItemオブジェクトを取得
                var item = (TodoItem)todoGridView.Rows[e.RowIndex].DataBoundItem;

                // 該当のTodoを完了状態に更新
                item.IsCompleted = true;

                // JSONに保存
                var manager = new TodoManager();
                manager.SaveTodos(todos);

                DisplayTodosInGrid(todos);
            }
            else
            {
                // 無効なクリックを無視
            }
        }

        /*セルの値が変更された時に発火
        チェックボックスの変更を自動で保存*/
        /// <summary>
        /// セルの値が変更された時に発火
        /// 「完了」チェックボックスが変更された場合、対応する TodoItem の状態を更新。
        /// JSONも更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void todoGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            // 「完了」列が変更されたら、値を保存して表示の変更
            if (todoGridView.Columns[e.ColumnIndex].DataPropertyName == "IsCompleted")
            {
                var item = (TodoItem)todoGridView.Rows[e.RowIndex].DataBoundItem;

                // 選択されたRowの選択されたColumn(cell)のValue(bool)のTrue or FalseをTodoItemに反映
                item.IsCompleted = (bool)todoGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                var manager = new TodoManager();
                manager.SaveTodos(todos);
            }
        }
    }
}   
