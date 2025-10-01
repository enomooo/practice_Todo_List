using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace practiceTodoList
{
    /// <summary>
    /// Todoリストの保存・読み込みを担う永続化クラス
    /// JSONファイルを通じて、内部状態List<TodoItem>を外部に記録・復元する
    /// </summary>
    public class TodoManager
    {
        /// <summary>
        /// 保存先のファイルパス"todos.json"
        /// </summary>
        private readonly string FilePath;

        /// <summary>
        /// JsonSerializerOptionsを一度だけ生成し、再利用
        /// </summary>
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,

            // 日本語のエスケープを緩和
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        /// <summary>
        /// 保存先ファイルパスを指定
        /// コンストラクタなので、クラス名と同じ
        /// クラスのインスタンスを初期化
        /// </summary>
        /// <param name="filePath">保存先のファイルパス</param>
        public TodoManager(string filePath = "todos.json")
        {
            FilePath = filePath;
        }

        /// <summary>
        /// TodoリストをJson形式で非同期に保存
        /// </summary>
        /// <param name="todos"></param>
        public async Task SaveTodosAsync(List<TodoItem> todos)
        {
            // todosがnullの場合は処理を中断 (外部でnew List<TodoItem>()が渡されるのが理想)
            if (todos == null)
            {
                Debug.WriteLine("保存対象が空です。");
                return;
            }

            try
            {
                // リストをjson文字列に変換
                string json = JsonSerializer.Serialize(todos, _options);

                // 非同期でファイルに書き込む
                await File.WriteAllTextAsync(FilePath, json);
            }
            catch (Exception ex)
            {
                // ファイルアクセスエラーなどを補足
                Debug.WriteLine($"ファイル保存に失敗しました: {ex.Message}");
            }
        }

        /// <summary>
        /// jsonファイルからtodoリストを非同期に読み込む
        /// </summary>
        /// <returns>読み込まれたtodoリスト</returns>
        public async Task<List<TodoItem>> LoadTodosAsync()
        {
            // ファイルが存在しない場合は空のリストを返す
            if (!File.Exists(FilePath))
                return new List<TodoItem>();
           
            try
            { 
               // 非同期でファイルを読み込む
                var json = await File.ReadAllTextAsync(FilePath);

                // デシリアライズ(復元)。結果がnullの場合新しい空のリストを返す。
                return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            }
            catch (JsonException ex)
            {
                // Jsonフォーマットエラー(ファイルの内容が壊れている)
                Debug.WriteLine($"エラー: JSONデータの読み込みに失敗しました: {ex.Message}");
                return new List<TodoItem>();
            }
            catch (IOException ex)
            {
                // ファイルアクセスエラー
                Debug.WriteLine($"エラー: ファイルの読み込みに失敗しました: {ex.Message}");
                return new List<TodoItem>();
            }
        }
    }
}
