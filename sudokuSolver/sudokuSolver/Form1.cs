namespace sudokuSolver
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    class SudokuForm : Form
    {
        private const int N = 9;
        private const int CellSize = 50;
    
        private TextBox[,] textBoxes;
    
        public SudokuForm()
        {
            InitializeUI();
        }
    
        private void InitializeUI()
        {
            textBoxes = new TextBox[N, N];
    
            this.Text = "Sudoku Solver";
            this.Size = new Size(N * CellSize + 20, N * CellSize + 100);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
    
            // 创建文本框并添加到窗体
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    textBoxes[i, j] = new TextBox();
                    textBoxes[i, j].Size = new Size(CellSize, CellSize);
                    textBoxes[i, j].Location = new Point(j * CellSize + 10, i * CellSize + 10);
                    textBoxes[i, j].TextAlign = HorizontalAlignment.Center;
                    textBoxes[i, j].MaxLength = 1;
                    textBoxes[i, j].KeyPress += TextBox_KeyPress;
                    bool isBlue = (i % 6 < 3) ^ (j % 6 < 3);
                    if(isBlue)
                    {
                        textBoxes[i, j].BackColor = Color.LightBlue;
                    }
                    else
                    {
                        textBoxes[i, j].BackColor = Color.LightGray;
                    }

    
                    this.Controls.Add(textBoxes[i, j]);
                }
            }
    
            // 添加解决按钮
            Button solveButton = new Button();
            solveButton.Text = "Solve";
            solveButton.Size = new Size(80, 30);
            solveButton.Location = new Point((N * CellSize - 80) / 2 + 10, N * CellSize + 20);
            solveButton.Click += SolveButton_Click;
    
            this.Controls.Add(solveButton);
        }
    
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许输入数字和删除键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) || e.KeyChar == '0')
            {
                e.Handled = true;
            }
        }
    
        private void SolveButton_Click(object sender, EventArgs e)
        {
            int[,] sudoku = new int[N, N];
    
            // 从文本框获取数独数据
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    if (!string.IsNullOrEmpty(textBoxes[i, j].Text))
                    {
                        sudoku[i, j] = int.Parse(textBoxes[i, j].Text);
                    }
                }
            }
    
            // 解决数独
            if (SolveSudoku(sudoku))
            {
                // 将解答写回文本框
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < N; ++j)
                    {
                        textBoxes[i, j].Text = sudoku[i, j].ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("No solution exists.", "Sudoku Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    
        // 使用回溯算法解决数独
        private static bool SolveSudoku(int[,] sudoku)
        {
            for (int row = 0; row < N; ++row)
            {
                for (int col = 0; col < N; ++col)
                {
                    if (sudoku[row, col] == 0)
                    {
                        for (int num = 1; num <= N; ++num)
                        {
                            if (IsSafe(sudoku, row, col, num))
                            {
                                sudoku[row, col] = num;
    
                                if (SolveSudoku(sudoku))
                                {
                                    return true;
                                }
    
                                sudoku[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    
        private static bool IsSafe(int[,] sudoku, int row, int col, int num)
        {
            for (int i = 0; i < N; ++i)
            {
                if (sudoku[row, i] == num || sudoku[i, col] == num)
                {
                    return false;
                }
            }
    
            int startRow = row - row % 3;
            int startCol = col - col % 3;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (sudoku[i + startRow, j + startCol] == num)
                    {
                        return false;
                    }
                }
            }
    
            return true;
        }
    }
}




