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
            txtNotes = new TextBox();
            btnAdd = new Button();
            btnDelete = new Button();
            todoGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)todoGridView).BeginInit();
            SuspendLayout();
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(298, 366);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(300, 27);
            txtTitle.TabIndex = 1;
            // 
            // dtpDueDate
            // 
            dtpDueDate.Location = new Point(92, 366);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new Size(200, 27);
            dtpDueDate.TabIndex = 2;
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(604, 366);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(420, 27);
            txtNotes.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = SystemColors.Highlight;
            btnAdd.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnAdd.Location = new Point(124, 421);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(671, 30);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "追加";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = SystemColors.ActiveBorder;
            btnDelete.Location = new Point(801, 421);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(166, 30);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "削除";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // todoGridView
            // 
            todoGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            todoGridView.Location = new Point(28, 39);
            todoGridView.Name = "todoGridView";
            todoGridView.RowHeadersWidth = 51;
            todoGridView.Size = new Size(1060, 277);
            todoGridView.TabIndex = 7;
            todoGridView.CellContentClick += todoGridView_CellContentClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1116, 544);
            Controls.Add(btnDelete);
            Controls.Add(btnAdd);
            Controls.Add(txtNotes);
            Controls.Add(dtpDueDate);
            Controls.Add(txtTitle);
            Controls.Add(todoGridView);
            Name = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)todoGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtTitle;
        private DateTimePicker dtpDueDate;
        private TextBox txtNotes;
        private Button btnAdd;
        private Button btnDelete;
        private DataGridView todoGridView;
    }
}
