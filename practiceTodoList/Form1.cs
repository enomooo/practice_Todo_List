using System;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace practiceTodoList
{
    public partial class Form1 : Form
    {
         private TodoController? controller;
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

            todoGridView.CellFormatting += todoGridView_CallFormatting;
        }
        public void Form1_Load(object? sender, EventArgs e)
        {
            // todoGridViewを定義しているメソッドなのでまず呼び出す
            SetupDataGridView();

            string filePath = "todos.json";
         controller = new TodoController(filePath, todoGridView);
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

        /// <summary
        /// Todoリストを追加するメソッド
        /// </summary>
        /// <param name="sender">イベント発火元("追加"ボタン)</param>
        /// <param name="e">イベント引数</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
         if (controller == null) return;

         controller.AddTodo(txtTitle.Text, txtNotes.Text, dtpDueDate.Value);
         ClearInputFields();
        }

        /// <summary>
        /// 入力フォームを初期化するメソッド(登録後、書いたTodoを初期化)
        /// </summary>
        private void ClearInputFields()
        {
            txtTitle.Text = "";
            dtpDueDate.Value = DateTime.Today;
            txtNotes.Text = "";
        }

        /// <summary>
        /// Todoリスト削除メソッド
        /// ヘッダーをクリックしないとRowの指定をできない
        /// </summary>
        /// <param name="sender">"削除ボタン"</param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        { 
            if(controller == null) return;
         
            // 現在、選択されている行の数をCount == 0、つまり指定されていないのに削除しようとしたらエラー
            if (todoGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除するタスクを選択してください。", "操作エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

         controller.DeleteTodo(todoGridView.SelectedRows[0].Index);
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

        /// <summary>
        /// セルの値が変更された時に発火
        /// 「完了」チェックボックスが変更された場合、対応する TodoItem の状態を更新。
        /// JSONも更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void todoGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
             if (controller == null) return;
             // 「完了」列が変更されたら、値を保存して表示の変更
             if (todoGridView.Columns[e.ColumnIndex].DataPropertyName == "IsCompleted")
             {
             controller.ToggleCompletion(e.RowIndex);
             }
        }
    }
 }  