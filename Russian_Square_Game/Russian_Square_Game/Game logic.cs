using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Russian_Square_Game
{
    internal class Game_logic
    {
        private block_code mainsqure;
        private int col = 4;
        private int row = 4;
        private Russian_Square form;
        private block_code[,] squares;
        private int squarewidth;
        private Color[] colors = { Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.White};
        private Random Random = new Random();
        private DateTime StartTime;
        private Label timeLabel;
        private System.Windows.Forms.Timer gameTimer;
        private bool a = false;
        private int level = 0;

        public Game_logic(Russian_Square frm, int width = 75)
        {
            this.form = frm;
            this.squarewidth = width;
        }

        public void start()
        {
            levelNext();
            squares = new block_code[row, col];
            if (mainsqure != null && mainsqure.panel != null)
            {
                form.Controls.Remove(mainsqure.panel);
                mainsqure.panel.Dispose();
            }

            int tool = (form.Width / 2) - ((col / 2) * squarewidth);
            int arz = (form.Height / 2) - ((row / 2) * squarewidth);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    squares[i, j] = new block_code(form, squarewidth, colors[Random.Next(colors.Length)]);
                    squares[i, j].showSquare(tool + j * squarewidth, arz + i * squarewidth);
                }
            }

            mainsqure = new block_code(form, squarewidth, colors[Random.Next(colors.Length)]);
            mainsqure.showSquare(tool - squarewidth, arz);

            // راه‌اندازی کرنومتر
            if(level == 1)
            {
                StartTime = DateTime.Now;
                SetupTimerAndLabels();
            }

        }        
        private void SetupTimerAndLabels()
        {
            timeLabel = new Label();
            timeLabel.Name = "timeLabel";
            timeLabel.Location = new Point(10, 10);
            timeLabel.Text = "Time : 00:00";
            form.Controls.Add(timeLabel);
            timeLabel.BringToFront();

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 10;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - StartTime;
            timeLabel.Text = $"Time : {elapsed:mm\\:ss\\.ff}";
        }

        public void moveSquare(Direction direction)
        {
            int tol = (form.Width / 2) - ((col / 2) * squarewidth);
            int arz = (form.Height / 2) - ((row / 2) * squarewidth);

            int currentX = mainsqure.panel.Location.X;
            int currentY = mainsqure.panel.Location.Y;

            // مرزها
            int leftBound = tol - squarewidth;
            int rightBound = tol + (col * squarewidth);
            int topBound = arz - squarewidth;
            int bottomBound = arz + (row * squarewidth);

            int newX = currentX;
            int newY = currentY;

            switch (direction)
            {
                case Direction.Up:
                    if (currentY > topBound)
                    {
                        newY = currentY - squarewidth;
                    }
                    break;
                case Direction.Down:
                    if (currentY < bottomBound)
                    {
                        newY = currentY + squarewidth;
                    }
                    break;
                case Direction.Left:
                    if (currentX > leftBound)
                    {
                        newX = currentX - squarewidth;
                    }
                    break;
                case Direction.Right:
                    if (currentX < rightBound)
                    {
                        newX = currentX + squarewidth;
                    }
                    break;
            }

            // بررسی برخورد
            int hitRow = -1;
            int hitCol = -1;
            if (newX >= tol && newX < tol + col * squarewidth && newY >= arz && newY < arz + row * squarewidth)
            {
                hitRow = (newY - arz) / squarewidth;
                hitCol = (newX - tol) / squarewidth;
                // حرکت هنگام وارد شدن
                SpinCycle(direction, hitRow, hitCol);
                CheckRemove();
            }
            else
            {
                mainsqure.showSquare(newX, newY);
            }
        }
        private void SpinCycle(Direction direction, int rowIndex, int colIndex)
        {
            int tol = (form.Width / 2) - ((col / 2) * squarewidth);
            int arz = (form.Height / 2) - ((row / 2) * squarewidth);

            block_code squareThatComesOut = null;

            switch (direction)
            {
                case Direction.Up:
                    squareThatComesOut = squares[0, colIndex];
                    for (int i = 0; i < row - 1; i++)
                    {
                        squares[i, colIndex] = squares[i + 1, colIndex];
                    }
                    squares[row - 1, colIndex] = mainsqure;
                    break;

                case Direction.Down:
                    squareThatComesOut = squares[row - 1, colIndex];
                    for (int i = row - 1; i > 0; i--)
                    {
                        squares[i, colIndex] = squares[i - 1, colIndex];
                    }
                    squares[0, colIndex] = mainsqure;
                    break;

                case Direction.Left:
                    squareThatComesOut = squares[rowIndex, 0];
                    for (int j = 0; j < col - 1; j++)
                    {
                        squares[rowIndex, j] = squares[rowIndex, j + 1];
                    }
                    squares[rowIndex, col - 1] = mainsqure;
                    break;

                case Direction.Right:
                    squareThatComesOut = squares[rowIndex, col - 1];
                    for (int j = col - 1; j > 0; j--)
                    {
                        squares[rowIndex, j] = squares[rowIndex, j - 1];
                    }
                    squares[rowIndex, 0] = mainsqure;
                    break;
            }

            UpdateAllPositions();

            if (squareThatComesOut != null)
            {
                mainsqure = squareThatComesOut;

                switch (direction)
                {
                    case Direction.Up:
                        {
                            mainsqure.showSquare(tol + colIndex * squarewidth, arz - squarewidth);
                        }
                        break;
                    case Direction.Down:
                        {
                            mainsqure.showSquare(tol + colIndex * squarewidth, arz + row * squarewidth);
                        }
                        break;
                    case Direction.Left:
                        {
                            mainsqure.showSquare(tol - squarewidth, arz + rowIndex * squarewidth);
                        }
                        break;
                    case Direction.Right:
                        {
                            mainsqure.showSquare(tol + col * squarewidth, arz + rowIndex * squarewidth);
                        }
                        break;
                }
            }
        }
        private void UpdateAllPositions()
        {
            int tol = (form.Width / 2) - ((col / 2) * squarewidth);
            int arz = (form.Height / 2) - ((row / 2) * squarewidth);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (squares[i, j] != null)
                    {
                        squares[i, j].showSquare(tol + j * squarewidth, arz + i * squarewidth);
                    }
                }
            }
        }
        private bool IsRowSameColor(int rowIndex)
        {
            Color? firstColor = null;
            bool oneSquareExist = false;
            for (int j = 0; j < col; j++)
            {
                if (squares[rowIndex, j] != null)
                {
                    firstColor = squares[rowIndex, j].panel.BackColor;
                    oneSquareExist = true;
                    break;
                }
            }
            if (!oneSquareExist)
            {
                return false;
            }
            for (int j = 0; j < col; j++)
            {
                if (squares[rowIndex, j].panel.BackColor != firstColor.Value)
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsColumnSameColor(int colIndex)
        {
            Color? firstColor = null;
            bool oneSquareExist = false;

            for (int i = 0; i < row; i++)
            {
                if (squares[i, colIndex] != null)
                {
                    firstColor = squares[i, colIndex].panel.BackColor;
                    oneSquareExist = true;
                    break;
                }
            }
            if (!oneSquareExist)
            {
                return false;
            }
            for (int i = 0; i < row; i++)
            {
                if (squares[i, colIndex].panel.BackColor != firstColor.Value)
                {
                    return false;
                }
            }
            return true;
        }

        // بررسی و پاک کردن سطر/ستون‌های هم‌رنگ
        private void CheckRemove()
        {
            bool removedSomething;

            do
            {
                removedSomething = false;
                List<int> rowsToRemove = new List<int>();
                List<int> colsToRemove = new List<int>();

                for (int i = 0; i < row; i++)
                {
                    if (IsRowSameColor(i))
                    {
                        rowsToRemove.Add(i);
                    }
                }
                for (int j = 0; j < col; j++)
                {
                    if (IsColumnSameColor(j))
                    {
                        colsToRemove.Add(j);
                    }
                }
                if (rowsToRemove.Count == 0 && colsToRemove.Count == 0)
                {
                    break;
                }
                rowsToRemove.Sort((a, b) => b.CompareTo(a));
                foreach (int rowIndex in rowsToRemove)
                {
                    RemoveRow(rowIndex);
                    removedSomething = true;
                }

                colsToRemove.Sort((a, b) => b.CompareTo(a));
                foreach (int colIndex in colsToRemove)
                {
                    RemoveColumn(colIndex);
                    removedSomething = true;
                }
                if (removedSomething)
                {
                    FillEmptySpaces();
                }

            } while (removedSomething);

            if (IsGameOver())
            {
                // محاسبه زمان پایان بازی
                TimeSpan finishTime = DateTime.Now - StartTime;
                if (gameTimer != null)
                {
                    gameTimer.Stop();
                }
                string message = $"Game finished\n" +
                                $"Time : {finishTime:mm\\:ss\\.ff}\n";
                DialogResult result = MessageBox.Show(message, "you have won", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

                if (result == DialogResult.Cancel)
                {
                    Application.Exit();
                }
                else
                {
                    level = 0;
                    a = true;
                    start();
                }
            }
        }
        private bool IsGameOver()
        {
            // 1. اگر شبکه کاملاً خالی شده
            if (row == 0 || col == 0)
            {
                if (level == 3)
                    return true;
                else
                    start();
            }

            // 2. اگر شبکه 1x1 شده و مربع اصلی بیرون است
            if (row == 1 && col == 1)
            {
                // بررسی کن آیا مربعی در شبکه هست
                bool hasSquare = false;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (squares[i, j] != null)
                        {
                            hasSquare = true;
                            break;
                        }
                    }
                    if (hasSquare)
                    {
                        break;
                    }
                }

                // اگر مربعی در شبکه نیست، بازی تمام شده
                if (!hasSquare)
                {
                    return true;
                }
            }
            if (mainsqure == null || mainsqure.panel == null)
            {
                return true;
            }

            // بررسی مختصات مربع اصلی
            int tol = (form.Width / 2) - ((col / 2) * squarewidth);
            int arz = (form.Height / 2) - ((row / 2) * squarewidth);

            int currentX = mainsqure.panel.Location.X;
            int currentY = mainsqure.panel.Location.Y;

            // اگر مربع اصلی کاملاً بیرون از محدوده است
            if (currentX < tol - squarewidth || currentX > tol + col * squarewidth ||
                currentY < arz - squarewidth || currentY > arz + row * squarewidth)
            {
                bool canMove = false;

                // بالا
                if (currentY > arz - squarewidth)
                {
                    canMove = true;
                }
                // پایین
                if (currentY < arz + row * squarewidth)
                {
                    canMove = true;
                }
                // چپ
                if (currentX > tol - squarewidth)
                {
                    canMove = true;
                }
                // راست
                if (currentX < tol + col * squarewidth)
                {
                    canMove = true;
                }
                return !canMove;
            }

            return false;
        }

        private void RemoveRow(int rowIndex)
        {
            // حذف مربع‌ها از فرم
            for (int j = 0; j < col; j++)
            {
                if (squares[rowIndex, j] != null)
                {
                    if (squares[rowIndex, j] == mainsqure)
                    {
                        squares[rowIndex, j] = null;
                    }
                    else
                    {
                        form.Controls.Remove(squares[rowIndex, j].panel);
                        squares[rowIndex, j] = null;
                    }
                }
            }
            //  ایجاد آرایه جدید
            block_code[,] newSquares = new block_code[row - 1, col];
            //  کپی داده‌ها
            int newRow = 0;
            for (int i = 0; i < row; i++)
            {
                if (i == rowIndex) continue;

                for (int j = 0; j < col; j++)
                {
                    newSquares[newRow, j] = squares[i, j];
                }
                newRow++;
            }

            //  شکل جدید بعد حذف سطر
            squares = newSquares;
            row--;
        }
        // همون اتفاق با ستون
        private void RemoveColumn(int colIndex)
        {
            for (int i = 0; i < row; i++)
            {
                if (squares[i, colIndex] != null)
                {
                    if (squares[i, colIndex] == mainsqure)
                    {
                        squares[i, colIndex] = null;
                    }
                    else
                    {
                        form.Controls.Remove(squares[i, colIndex].panel);
                        squares[i, colIndex] = null;
                    }
                }
            }
            block_code[,] newSquares = new block_code[row, col - 1];
            for (int i = 0; i < row; i++)
            {
                int newCol = 0;
                for (int j = 0; j < col; j++)
                {
                    if (j == colIndex) continue;
                    newSquares[i, newCol] = squares[i, j];
                    newCol++;
                }
            }
            squares = newSquares;
            col--;
        }
        private void FillEmptySpaces()
        {
            // سطرهای خالی حذف 
            for (int i = row - 1; i >= 0; i--)
            {
                bool isEmpty = true;
                for (int j = 0; j < col; j++)
                {
                    if (squares[i, j] != null)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    RemoveRow(i);
                }
            }
            //ستون‌های خالی حذف 
            for (int j = col - 1; j >= 0; j--)
            {
                bool isEmpty = true;
                for (int i = 0; i < row; i++)
                {
                    if (squares[i, j] != null)
                    {
                        isEmpty = false;
                        break;
                    }
                }
                if (isEmpty)
                {
                    RemoveColumn(j);
                }
            }
            UpdateAllPositions();
        }
        public void levelNext()
        {
            level++;
            switch (level)
            {
                case 1:
                    row = 4;
                    col = 4;
                    break;
                case 2:
                    row = 5;
                    col = 5;
                    break;
                case 3:
                    row = 6;
                    col = 6;
                    break;

            }
        }
    }
}
