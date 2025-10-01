using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practiceTodoList
{
    /// <summary>
    /// todoリストの操作・保存・表示更新を担うロジック制御クラス
    /// UI(Form)とデータ(TodoManager)の橋渡し訳
    /// </summary>
    internal class TodoController
    {
        private readonly TodoManager manager;
        private readonly DataGridView grid;
        private List<TodoItem> todos;

        /// <summary>
        /// コントローラクラスのコンストラクタ。
        /// 保存先と表示先を受け取る
        /// </summary>
        public TodoController(string filePath, DataGridView gridView)
        {
            manager = new TodoManager(filePath);
            grid = gridView;
            todos = manager.LoadTodos();

            TodoGridViewSetup.SetupColumns(grid);
            grid.CellFormatting += TodoGridViewSetup.ApplyFormatting;

            RefreshDisplay();
        }

        /// <summary
        /// todoリストを追加・保存・更新する
        /// </summary>
        /// <param name="sender">イベント発火元("追加"ボタン)</param>
        /// <param name="e">イベント引数</param>
        public void AddTodo(string title, string notes, DateTime dueDate)
        {
            // 空白でtodoを登録しようとすると、エラーの警告を出す
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("タイトルを入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 入力内容のTodoItem作成
            todos.Add(new TodoItem
            {
                Title = title,
                DueDate = dueDate,
                Notes = notes,
                IsCompleted = false,
            });
            SaveAndRefresh();
        }

        /// <summary>
        /// 指定行のTodoを完了状態に切り替える
        /// </summary>
        /// <param name="rowIndex">指定の行</param>
        public void ToggleCompletion(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= todos.Count) return;

            // todoの完了<=>未完了のトグルスイッチ
            todos[rowIndex].IsCompleted = !todos[rowIndex].IsCompleted;

            SaveAndRefresh();
        }

        /// <summary>
        /// 指定行のTodoを削除
        /// </summary>
        /// <param name="rowIndex">指定の行</param>
        public void DeleteTodo(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= todos?.Count) return;
            todos?.RemoveAt(rowIndex);
            SaveAndRefresh();
        }

        /// <summary>
        /// Todoリストを保存し、表示を更新する
        /// </summary>
        public void SaveAndRefresh()
        {
            manager.SaveTodos(todos);
            RefreshDisplay();
        }

        /// <summary>
        /// 表示を更新
        /// </summary>
        public void RefreshDisplay()
        {
            TodoGridViewSetup.DisplayTodos(grid, todos);
        }
    }
}
