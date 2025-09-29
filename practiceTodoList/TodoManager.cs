using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection.Metadata.Ecma335;

namespace practiceTodoList
{
    public class TodoManager
    {
        private const string FilePath = "todos.json";
        public List<TodoItem> LoadTodos()
        {
            if(!File.Exists(FilePath))
                return new List<TodoItem>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        }
        public void SaveTodos(List<TodoItem> todos)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(todos,options);
            File.WriteAllText(FilePath, json); 
        }
    }
}
