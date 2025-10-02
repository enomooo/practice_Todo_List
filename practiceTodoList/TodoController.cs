using System;
using System.Collections.Generic;

// BindingList<T>のために必要
using System.ComponentModel;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace practiceTodoList
{
    /// <summary>
    /// todoリストの操作・保存・表示更新を担うロジック制御クラス
    /// UI(Form)とデータソースを通じて連携し、依存しない(蜜結合の解消)
    /// </summary>
    internal class TodoController
    {
        private readonly TodoManager manager;

        // データバインディング用のリストを保持
        private readonly BindingList<TodoItem> todos;

        /// <summary>
        /// Form1 (View)がデータソースとしてバインドするためのプロパティ
        /// </summary>
        public BindingList<TodoItem> TodoList => todos;

        /// <summary>
        /// コントローラクラスのコンストラクタ。
        /// 保存先を受け取る
        /// </summary>
        public TodoController(string filePath)
        {
            manager = new TodoManager(filePath);

            // 初期化時には空のBindingListを作成
            todos = new BindingList<TodoItem>();
        }

        /// <summary>
        /// データを非同期でファイルから読み込み、リストに設定する
        /// </summary>
        public async Task LoadTodosAsync()
        {
            // TodoManagerの非同期メソッドをawaitで呼び出し
            List<TodoItem> loadedList = await manager.LoadTodosAsync();

            // 読み込んだアイテムをBindingListに追加
            todos.Clear();
            foreach (var item in loadedList)
            {
                todos.Add(item);
            }
        }

        /// <summary
        /// todoリストを追加
        /// 非同期で保存
        /// </summary>
        public async Task AddTodoAsync(string title, string notes, DateTime dueDate)
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
            await SaveTodosAsync();
        }

        /// <summary>
        /// 指定行のTodoを完了状態に切り替え,非同期で保存する
        /// </summary>
        /// <param name="rowIndex">指定の行</param>
        public async Task ToggleCompletionAsync(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= todos.Count) return;

            // todoの完了<=>未完了のトグルスイッチ
            //todos[rowIndex].IsCompleted = !todos[rowIndex].IsCompleted;

            // BindingList<T>は要素の中身の変更を自動検知しないので、手動でReset
            todos.ResetItem(rowIndex);

            await SaveTodosAsync();
        }

        /// <summary>
        /// 指定行のTodoを削除して、非同期で保存
        /// </summary>
        /// <param name="rowIndex">指定の行</param>
        public async Task DeleteTodoAsync(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= todos.Count) return;

            //BindingListのRemoveAtは、UIに自動で削除を通知します
            todos.RemoveAt(rowIndex);

            await SaveTodosAsync();
        }

        /// <summary>
        /// Todoリストを非同期で保存
        /// BindingListがUIを自動更新するので、DisplayRefreshはいらない
        /// </summary>
        public async Task SaveTodosAsync()
        {
            await manager.SaveTodosAsync(new List<TodoItem>(todos));
        }
    }
}
