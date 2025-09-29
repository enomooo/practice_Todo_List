using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceTodoList
{
    public class TodoItem
    {
        public string? Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Notes { get; set; }
    }
}
