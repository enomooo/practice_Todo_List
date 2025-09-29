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
            todoListView = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            txtTitle = new TextBox();
            dtpDueDate = new DateTimePicker();
            chkCompleted = new CheckBox();
            txtNotes = new TextBox();
            btnAdd = new Button();
            SuspendLayout();
            // 
            // todoListView
            // 
            todoListView.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            todoListView.FullRowSelect = true;
            todoListView.GridLines = true;
            todoListView.Location = new Point(308, 36);
            todoListView.Name = "todoListView";
            todoListView.Size = new Size(845, 528);
            todoListView.TabIndex = 0;
            todoListView.UseCompatibleStateImageBehavior = false;
            todoListView.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "タイトル";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "期限";
            columnHeader2.Width = 180;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "完了";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "メモ";
            columnHeader4.Width = 400;
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
            btnAdd.Font = new Font("Yu Gothic UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnAdd.Location = new Point(425, 601);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(591, 69);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "追加";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1439, 727);
            Controls.Add(todoListView);
            Controls.Add(btnAdd);
            Controls.Add(txtNotes);
            Controls.Add(chkCompleted);
            Controls.Add(dtpDueDate);
            Controls.Add(txtTitle);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView todoListView;
        private TextBox txtTitle;
        private DateTimePicker dtpDueDate;
        private CheckBox chkCompleted;
        private TextBox txtNotes;
        private Button btnAdd;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
    }
}
