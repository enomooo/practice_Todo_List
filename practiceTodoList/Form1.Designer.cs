namespace practiceTodoList
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtTitle = new TextBox();
            dtpDueDate = new DateTimePicker();
            chkCompleted = new CheckBox();
            txtNotes = new TextBox();
            btnAdd = new Button();
            btnDelete = new Button();
            todoGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)todoGridView).BeginInit();
            SuspendLayout();
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(308, 568);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(200, 27);
            txtTitle.TabIndex = 1;
            // 
            // dtpDueDate
            // 
            dtpDueDate.Location = new Point(514, 568);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new Size(166, 27);
            dtpDueDate.TabIndex = 2;
            // 
            // chkCompleted
            // 
            chkCompleted.AutoSize = true;
            chkCompleted.BackColor = SystemColors.ButtonHighlight;
            chkCompleted.Location = new Point(686, 570);
            chkCompleted.Name = "chkCompleted";
            chkCompleted.Size = new Size(61, 24);
            chkCompleted.TabIndex = 3;
            chkCompleted.Text = "完了";
            chkCompleted.UseVisualStyleBackColor = false;
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(762, 568);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(391, 27);
            txtNotes.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = SystemColors.Highlight;
            btnAdd.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnAdd.Location = new Point(308, 601);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(675, 30);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "追加";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = SystemColors.ActiveBorder;
            btnDelete.Location = new Point(983, 601);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(170, 30);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "削除";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // todoGridView
            // 
            todoGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            todoGridView.Dock = DockStyle.Fill;
            todoGridView.Location = new Point(0, 0);
            todoGridView.Name = "todoGridView";
            todoGridView.RowHeadersWidth = 51;
            todoGridView.Size = new Size(1681, 793);
            todoGridView.TabIndex = 7;
            todoGridView.CellContentClick += todoGridView_CellContentClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1681, 793);
            Controls.Add(btnDelete);
            Controls.Add(btnAdd);
            Controls.Add(txtNotes);
            Controls.Add(chkCompleted);
            Controls.Add(dtpDueDate);
            Controls.Add(txtTitle);
            Controls.Add(todoGridView);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)todoGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtTitle;
        private DateTimePicker dtpDueDate;
        private CheckBox chkCompleted;
        private TextBox txtNotes;
        private Button btnAdd;
        private Button btnDelete;
        private DataGridView todoGridView;
    }
}
