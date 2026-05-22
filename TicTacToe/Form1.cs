using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        // Игровое поле 3x3: '' = пусто, 'X' = крестик, 'O' = нолик
        private char[,] board = new char[3, 3];

        // Кнопки игрового поля (9 штук)
        private Button[,] cells = new Button[3, 3];

        // Текущий игрок: 'X' или 'O'
        private char currentPlayer = 'X';

        // Игра закончена?
        private bool gameOver = false;

        public Form1()
        {
            InitializeComponent();
            CreateBoard();
            ResetGame();
        }

        // Создаём 9 кнопок и добавляем их в panelBoard
        private void CreateBoard()
        {
            int size = 96;   // размер одной клетки
            int gap = 4;     // зазор между клетками

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button btn = new Button();

                    // Позиция кнопки внутри панели
                    btn.Location = new Point(col * (size + gap), row * (size + gap));
                    btn.Size = new Size(size, size);

                    // Шрифт для X и O
                    btn.Font = new Font("Segoe UI", 32F, FontStyle.Bold);

                    // Стиль кнопки
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(180, 190, 210);
                    btn.FlatAppearance.BorderSize = 2;
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.FromArgb(50, 50, 80);
                    btn.Cursor = Cursors.Hand;
                    btn.Text = "";

                    // Сохраняем строку и столбец в Tag, чтобы знать какая кнопка нажата
                    btn.Tag = new Point(row, col);

                    // Подписываемся на событие нажатия
                    btn.Click += Cell_Click;

                    cells[row, col] = btn;
                    panelBoard.Controls.Add(btn);
                }
            }
        }

        // Обработчик нажатия на клетку поля
        private void Cell_Click(object sender, EventArgs e)
        {
            if (gameOver) return;

            Button btn = (Button)sender;
            Point pos = (Point)btn.Tag;
            int row = pos.X;
            int col = pos.Y;

            // Если клетка уже занята — ничего не делаем
            if (board[row, col] != '\0') return;

            // Ставим символ текущего игрока
            board[row, col] = currentPlayer;

            // Окрашиваем текст: X — синий, O — красный
            btn.ForeColor = (currentPlayer == 'X')
                ? Color.FromArgb(60, 100, 200)
                : Color.FromArgb(200, 60, 60);

            btn.Text = currentPlayer.ToString();
            btn.Enabled = false; // кнопка больше не нажимается

            // Проверяем результат
            if (CheckWin(currentPlayer))
            {
                lblStatus.Text = $"Победил игрок: {currentPlayer}! 🎉";
                lblStatus.ForeColor = Color.FromArgb(0, 140, 60);
                gameOver = true;
                HighlightWinningCells(currentPlayer);
                return;
            }

            if (IsBoardFull())
            {
                lblStatus.Text = "Ничья!";
                lblStatus.ForeColor = Color.FromArgb(130, 80, 0);
                gameOver = true;
                return;
            }

            // Меняем игрока
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
            lblStatus.Text = $"Ход игрока: {currentPlayer}";
            lblStatus.ForeColor = Color.FromArgb(50, 50, 80);
        }

        // Проверка победителя
        private bool CheckWin(char player)
        {
            // Строки
            for (int r = 0; r < 3; r++)
                if (board[r, 0] == player && board[r, 1] == player && board[r, 2] == player)
                    return true;

            // Столбцы
            for (int c = 0; c < 3; c++)
                if (board[0, c] == player && board[1, c] == player && board[2, c] == player)
                    return true;

            // Диагонали
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
                return true;

            return false;
        }

        // Поле заполнено?
        private bool IsBoardFull()
        {
            foreach (char c in board)
                if (c == '\0') return false;
            return true;
        }

        // Подсветка победных клеток зелёным фоном
        private void HighlightWinningCells(char player)
        {
            Color highlight = Color.FromArgb(210, 240, 210);

            // Строки
            for (int r = 0; r < 3; r++)
                if (board[r, 0] == player && board[r, 1] == player && board[r, 2] == player)
                    for (int c = 0; c < 3; c++) cells[r, c].BackColor = highlight;

            // Столбцы
            for (int c = 0; c < 3; c++)
                if (board[0, c] == player && board[1, c] == player && board[2, c] == player)
                    for (int r = 0; r < 3; r++) cells[r, c].BackColor = highlight;

            // Главная диагональ
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                for (int i = 0; i < 3; i++) cells[i, i].BackColor = highlight;

            // Побочная диагональ
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            {
                cells[0, 2].BackColor = highlight;
                cells[1, 1].BackColor = highlight;
                cells[2, 0].BackColor = highlight;
            }
        }

        // Сброс игры в начальное состояние
        private void ResetGame()
        {
            board = new char[3, 3];
            currentPlayer = 'X';
            gameOver = false;

            lblStatus.Text = "Ход игрока: X";
            lblStatus.ForeColor = Color.FromArgb(50, 50, 80);

            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                {
                    cells[r, c].Text = "";
                    cells[r, c].Enabled = true;
                    cells[r, c].BackColor = Color.White;
                    cells[r, c].ForeColor = Color.FromArgb(50, 50, 80);
                }
        }

        // Кнопка «Новая»
        private void btnRestart_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        // Сохранение игры в текстовый файл
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Файл сохранения (*.ttt)|*.ttt|Все файлы (*.*)|*.*";
                dlg.DefaultExt = "ttt";
                dlg.Title = "Сохранить игру";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(dlg.FileName))
                    {
                        writer.WriteLine(currentPlayer);   // текущий игрок
                        writer.WriteLine(gameOver ? "1" : "0");

                        for (int r = 0; r < 3; r++)
                            for (int c = 0; c < 3; c++)
                                writer.WriteLine(board[r, c] == '\0' ? "." : board[r, c].ToString());
                    }
                    MessageBox.Show("Игра сохранена!", "Сохранение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Загрузка игры из файла
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Файл сохранения (*.ttt)|*.ttt|Все файлы (*.*)|*.*";
                dlg.Title = "Загрузить игру";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(dlg.FileName);

                        currentPlayer = lines[0][0];
                        gameOver = lines[1] == "1";

                        // Сбрасываем визуал
                        for (int r = 0; r < 3; r++)
                            for (int c = 0; c < 3; c++)
                            {
                                cells[r, c].Text = "";
                                cells[r, c].Enabled = true;
                                cells[r, c].BackColor = Color.White;
                            }

                        board = new char[3, 3];

                        int idx = 2;
                        for (int r = 0; r < 3; r++)
                        {
                            for (int c = 0; c < 3; c++)
                            {
                                string val = lines[idx++];
                                if (val == ".") { board[r, c] = '\0'; continue; }

                                board[r, c] = val[0];
                                cells[r, c].Text = val;
                                cells[r, c].ForeColor = (val == "X")
                                    ? Color.FromArgb(60, 100, 200)
                                    : Color.FromArgb(200, 60, 60);
                                cells[r, c].Enabled = false;
                            }
                        }

                        if (gameOver)
                        {
                            lblStatus.Text = "Игра окончена. Нажмите «Новая»";
                            lblStatus.ForeColor = Color.FromArgb(130, 80, 0);
                        }
                        else
                        {
                            lblStatus.Text = $"Ход игрока: {currentPlayer}";
                            lblStatus.ForeColor = Color.FromArgb(50, 50, 80);
                            // Включаем только пустые клетки
                            for (int r = 0; r < 3; r++)
                                for (int c = 0; c < 3; c++)
                                    if (board[r, c] == '\0') cells[r, c].Enabled = true;
                        }

                        MessageBox.Show("Игра загружена!", "Загрузка",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при загрузке файла.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
