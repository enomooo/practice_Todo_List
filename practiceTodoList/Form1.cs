using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practiceTodoList
{
    public partial class Form1 : Form
    {
        private readonly TodoController controller;
        private readonly TodoGridViewHelper gridHelper;
        public Form1()
        {
            // フォームのUIを初期化
            InitializeComponent();

            // ControllerとHelperの初期化
            string filePath = "todos.json";
            this.controller = new TodoController(filePath);

            // GridViewのフォントを渡してHelperをインスタンス化
            this.gridHelper = new TodoGridViewHelper(todoGridView.Font);

            // DataGridViewの初期設定
            gridHelper.SetupColumns(todoGridView);

            // todoGridViewの一番下にある新規入力用の空白行を非表示にする
            todoGridView.AllowUserToAddRows = false;

            // Controllerが持つBindingListをDataSourceに設定する = 自動更新を有効化
            todoGridView.DataSource = controller.TodoList;

            // フォームが表示される直前にLoadイベントを非同期で呼び出す
            this.Load += Form1_LoadAsync;

            // DataGridViewのイベント登録
            todoGridView.CellValueChanged += todoGridView_CellValueChangedAsync;
            
            todoGridView.CellFormatting += gridHelper.ApplyFormatting;

            todoGridView.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (todoGridView.IsCurrentCellDirty)
                {
                    todoGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };

            //ボタンイベントを非同期に対応
            btnAdd.Click += btnAdd_ClickAsync;
            btnDelete.Click += btnDelete_ClickAsync;

        }


        /// <summary>
        /// Form1_Loadを非同期メソッドとして再定義
        /// </summary>
        public async void Form1_LoadAsync(object? sender, EventArgs e)
        {
            // 非同期でデータを読み込み、BindingListに反映させる
            await controller.LoadTodosAsync();
        }

        /// <summary>
        /// Todoリストを追加するメソッド(非同期)
        /// </summary>
        private async void btnAdd_ClickAsync(object? sender, EventArgs e)
        {
            //Controllerに処理を移譲
            await controller.AddTodoAsync(txtTitle.Text, txtNotes.Text, dtpDueDate.Value);

            // Controllerが追加と保存を完了した後、UIのみをクリア
            ClearInputFields();
        }


        private async void btnDelete_ClickAsync(object? sender, EventArgs e)
        {
            if (todoGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除するタスクを選択してください。", "操作エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 選択された行のインデックスを取得し、Controllerに処理を移譲
            await controller.DeleteTodoAsync(todoGridView.SelectedRows[0].Index);
        }

        /// <summary>
        /// セルの値が変更された時に発火
        /// 「完了」✓が変更された時、対応するTodoItemの状態を更新し、JSONも更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void todoGridView_CellValueChangedAsync(object? sender, DataGridViewCellEventArgs e)
        {
            // 「完了」列が変更されたら、値を保存して表示の変更
            if (todoGridView.Columns[e.ColumnIndex].DataPropertyName == "IsCompleted")
            {
                // Controllerに処理を移譲
                await controller.ToggleCompletionAsync(e.RowIndex);
            }
        }

        /// <summary>
        /// 入力フォーム初期化
        /// </summary>
        private void ClearInputFields()
        {
            txtTitle.Text = "";
            dtpDueDate.Value = DateTime.Today;
            txtNotes.Text = "";
        }

    }
}