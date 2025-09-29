namespace practiceTodoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();      //�t�H�[����UI��������
            this.Load += Form1_Load;    //�t�H�[�����\������钼�O��Form1_Load���Ăяo��
        }
        private void SetupListViewColumns()
        {
            todoListView.Columns.Clear();

            todoListView.Columns.Add("�^�C�g��", 200);
            todoListView.Columns.Add("����",180);
            todoListView.Columns.Add("����", 60);
            todoListView.Columns.Add("����", 400);
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            // ListView�̗��ǉ�
            SetupListViewColumns();

            //JSON����ToDo��ǂݍ���ŕ\��
            var manager = new TodoManager();
            var todos = manager.LoadTodos();
            DisplayTodos(todos);
        }
        private void DisplayTodos(List<TodoItem> list)
        {
            todoListView.Items.Clear();     //�\����������

            foreach (var todo in list)
            {
                var item = new ListViewItem(todo.Title);                //�^�C�g��(�ŏ��̗�)
                item.SubItems.Add(todo.DueDate.ToShortDateString());    //����
                item.SubItems.Add(todo.IsCompleted ? "?" : "");        //�������
                item.SubItems.Add(todo.Notes);                          //����

                todoListView.Items.Add(item);   //ListView�ɒǉ�
            }
        }
        private List<TodoItem> todos = new();   //�t�H�[���S�̂ŕێ�����ToDo���X�g(�O��̃��X�g���܂߂ĂƂ����Ӗ��Ńt�H�[���S��)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("�^�C�g������͂��Ă��������B", "���̓G���[", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var newTodo = new TodoItem()
            {
                Title = txtTitle.Text,
                DueDate = dtpDueDate.Value,
                IsCompleted = chkCompleted.Checked,
                Notes = txtNotes.Text,
            };

            todos.Add(newTodo);     //todos���X�g��newTodo��ǉ�

            var manager = new TodoManager();
            manager.SaveTodos(todos);       //JSON�ɕۑ�

            DisplayTodos(todos);        //ListView�ɍĕ\��
            ClearInputFields();
        }
        private void ClearInputFields()
        {
            txtTitle.Text = "";
            dtpDueDate.Value = DateTime.Today;
            chkCompleted.Checked = false;
            txtNotes.Text = "";
        }
    }
}
