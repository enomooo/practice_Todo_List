using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace practiceTodoList
{
    /// <summary>
    /// DataGridViewの表示やフォーマット、処理を担うクラス
    /// </summary>
    public static class TodoGridViewSetup
    {

        /// <summary>
        /// DataGridViewの列構成を初期化、定義する
        /// </summary>
        public static void SetupColumns(DataGridView grid)
        {
            // 既存のtodoGridViewを初期化
            grid.Columns.Clear();

            // 下でWidthを指定するので自動調整をOFF
            grid.AutoGenerateColumns = false;

            // 各列の定義(期日、タイトル、メモ、完了)
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "期日",
                DataPropertyName = "DueDate",
                Width = 200
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "タイトル",
                DataPropertyName = "Title",
                Width = 300
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "メモ",
                DataPropertyName = "Notes",
                Width = 420
            });

            grid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "完了",
                DataPropertyName = "IsCompleted",
                Width = 80
            });
        }

        /// <summary>
        /// 指定されたTodoItemリストを期限順に並べ替え、DataGridViewに表示する
        /// </summary>
        /// <param name="list">表示するTodoItemのリスト</param>
        public static void DisplayTodos(DataGridView grid, List<TodoItem> todos)
        {
            // 表示用に、期限順で並べ替えたリストを作成
            var sorted = todos.OrderBy(todo => todo.DueDate).ToList();

            // DataGridViewのデータソースをクリアして、sortedを代入
            if (todos == null || todos.Count == 0)
            {
                grid.DataSource = null;
                grid.DataSource = sorted;
            }
        }

        /// <summary>
        /// 表示時の見た目（フォーマット）を動的に整えるフォーマット処理
        /// 期日列の表示形式の統一
        /// 完了済みtodoは打ち消し線とグレー表示
        /// </summary>
        /// <param name="sender">todoGridView</param>
        /// <param name="e"></param>
        public static void ApplyFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            // 期限列を「yyyyy/MM/dd」形式に統一(今フォーマットしようとしているセルが、DueDate列でDateTime型なら)
            if (grid.Columns[e.ColumnIndex].DataPropertyName == "DueDate" && e.Value is DateTime dt)
            {
                e.Value = dt.ToString("yyyy/MM/dd");
                e.FormattingApplied = true;
            }

            // 完了済みなら打ち消し線+グレー表示(IsNewRow は、DataGridView が自動的に生成する「新規入力用の空行)
            if (e.RowIndex >= 0 && !grid.Rows[e.RowIndex].IsNewRow)
            {
                // 今処理している行に対応するTodoItemオブジェクトを取得
                var item = (TodoItem)grid.Rows[e.RowIndex].DataBoundItem;

                // もしitemが完了なら、スタイルの変更
                if (item.IsCompleted)
                {
                    e.CellStyle!.Font = new Font(grid.Font, FontStyle.Strikeout);
                    e.CellStyle.ForeColor = Color.Gray;

                    // 独自スタイル適応済みと明示
                    e.FormattingApplied = true;
                }
            }
        }

    }
}
