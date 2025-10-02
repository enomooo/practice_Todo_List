using System;
using System.Drawing;
using System.Windows.Forms;

namespace practiceTodoList
{
    /// <summary>
    /// DataGridViewの表示やフォーマット、処理を担うヘルパークラス(非静的化)
    /// </summary>
    public  class TodoGridViewHelper
    {
        // パフォーマンス改善のため、打消し線用のフォントをフィールドとして保持
        private readonly Font _strikeoutFont;

        /// <summary>
        /// コンストラクタで基準となるフォントを受け取り、必要なスタイルを一度だけ作成
        /// </summary>
        /// <param name="baseFont">DataGridViewの標準フォント</param>
        public  TodoGridViewHelper(Font baseFont)
        {
            // 標準フォントを基に、打消し線スタイルを一度だけ作成
            _strikeoutFont = new Font(baseFont, FontStyle.Strikeout);
        }

        /// <summary>
        /// DataGridViewの列構成を初期化、定義する
        /// 非静的化
        /// </summary>
        public void SetupColumns(DataGridView grid)
        {
            // GridViewの初期化
            grid.Columns.Clear();
            // Columnの自動調整オフ
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
        /// 表示時の見た目（フォーマット）を動的に整えるフォーマット処理
        /// 期日列の表示形式の統一
        /// 完了済みtodoは打ち消し線とグレー表示
        /// </summary>
        /// <param name="sender">todoGridView</param>
        public  void ApplyFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            // 期限列を「yyyyy/MM/dd」形式に統一(今フォーマットしようとしているセルが、DueDate列でDateTime型なら)
            if (grid.Columns[e.ColumnIndex].DataPropertyName == "DueDate" && e.Value is System.DateTime dt)
            {
                e.Value = dt.ToString("yyyy/MM/dd");
                e.FormattingApplied = true;
            }

            // 完了済みなら打ち消し線+グレー表示(IsNewRow は、DataGridView が自動的に生成する「新規入力用の空行)
            if (e.RowIndex >= 0 && !grid.Rows[e.RowIndex].IsNewRow)
            {
                // DataBoundItemからTodoItemを取得
                if (grid.Rows[e.RowIndex].DataBoundItem is TodoItem item)
                {
                    if (item.IsCompleted)
                    {
                        //すでに作成済みのFontオブジェクトを使い回す
                        e.CellStyle!.Font = _strikeoutFont;
                        e.CellStyle.ForeColor = Color.Gray;

                        // 独自スタイル適応済みと明示
                        e.FormattingApplied = true;
                    }
                    else if (e.CellStyle!.Font != grid.Font)
                    {
                        //完了が解除された場合、スタイルをデフォルトに戻す
                        e.CellStyle.Font = grid.Font;
                        e.CellStyle.ForeColor = grid.ForeColor;
                    }
                }
            }
        }
    }
}
