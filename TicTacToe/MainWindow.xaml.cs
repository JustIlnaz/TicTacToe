using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private bool isXTurn = true;
        private readonly string[] board = new string[9];
        private readonly BitmapImage cross;
        private readonly BitmapImage circle;
        private int scoreX = 0;
        private int scoreO = 0;

        public MainWindow()
        {
            InitializeComponent();

            // Загрузка изображений
            var uriCross = new Uri("https://cdn1.iconfinder.com/data/icons/chunk-mini/16/X-1024.png ");
            var uriCircle = new Uri("https://sneg.top/uploads/posts/2023-04/1681161366_sneg-top-p-nolik-iz-fiksikov-kartinki-instagram-1.png ");

            cross = new BitmapImage(uriCross);
            circle = new BitmapImage(uriCircle);

            // Обработка ошибок загрузки
            cross.DownloadFailed += (s, e) => MessageBox.Show("Ошибка загрузки крестика!");
            circle.DownloadFailed += (s, e) => MessageBox.Show("Ошибка загрузки нолика!");

            txtStatus.Text = "Ходит: X";
            UpdateScoreDisplay();
        }

        private void CellClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null || button.Content != null) return;

            int index = Convert.ToInt32(button.Tag);

            if (board[index] != null)
                return;

            board[index] = isXTurn ? "X" : "O";

            Image image = new Image();
            image.Source = isXTurn ? cross : circle;
            image.Width = 50;
            image.Height = 50;
            image.VerticalAlignment = VerticalAlignment.Center;
            image.HorizontalAlignment = HorizontalAlignment.Center;

            button.Content = image;

            if (CheckWinner())
            {
                string winner = isXTurn ? "X" : "O";
                txtStatus.Text = $"Победил: {winner}!";

                if (winner == "X") scoreX++;
                else scoreO++;

                UpdateScoreDisplay();

                AutoRestartGame();
            }
            else if (IsBoardFull())
            {
                txtStatus.Text = "Ничья!";
                AutoRestartGame();
            }
            else
            {
                isXTurn = !isXTurn;
                txtStatus.Text = $"Ходит: {(isXTurn ? "X" : "O")}";
            }
        }

        private void AutoRestartGame()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                System.Threading.Thread.Sleep(1000); // 1 секунды задержки
                RestartGame(null, null);
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
        

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            Array.Clear(board, 0, board.Length);

            foreach (var child in ((Grid)this.Content).Children)
            {
                if (child is Button btn && btn.Tag != null)
                {
                    btn.Content = null;
                    btn.IsEnabled = true;
                }
            }

            isXTurn = true;
            txtStatus.Text = "Ходит: X";
        }

        private void ResetScore(object sender, RoutedEventArgs e)
        {
            scoreX = 0;
            scoreO = 0;
            UpdateScoreDisplay();
        }

        private bool CheckWinner()
        {
            int[,] winCombos = new int[,]
            {
                {0,1,2}, {3,4,5}, {6,7,8}, // строки
                {0,3,6}, {1,4,7}, {2,5,8}, // столбцы
                {0,4,8}, {2,4,6}           // диагонали
            };

            for (int i = 0; i < winCombos.GetLength(0); i++)
            {
                int a = winCombos[i, 0];
                int b = winCombos[i, 1];
                int c = winCombos[i, 2];

                if (board[a] != null && board[a] == board[b] && board[a] == board[c])
                    return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (var cell in board)
            {
                if (cell == null)
                    return false;
            }
            return true;
        }

        private void UpdateScoreDisplay()
        {
            txtScoreX.Text = $"X: {scoreX}";
            txtScoreO.Text = $"O: {scoreO}";
        }
    }
}