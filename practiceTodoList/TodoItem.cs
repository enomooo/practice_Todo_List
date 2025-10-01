using System;
using System.Collections.Generic;

namespace practiceTodoList
{
    public class TodoItem
    {
        public string Title { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Notes { get; set; }
    }
}
