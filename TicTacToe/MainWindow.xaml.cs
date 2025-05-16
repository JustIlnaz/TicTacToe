using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isXTurn = true;
        private readonly string[] board = new string[9];
        private readonly BitmapImage cross;
        private readonly BitmapImage circle;

        public MainWindow()
        {
            InitializeComponent();

            // загружаем изображения 
            var uriCross = new Uri("https://cdn1.iconfinder.com/data/icons/chunk-mini/16/X-1024.png ");
            var uriCircle = new Uri("https://sneg.top/uploads/posts/2023-04/1681161366_sneg-top-p-nolik-iz-fiksikov-kartinki-instagram-1.png ");

            cross = new BitmapImage(uriCross);
            circle = new BitmapImage(uriCircle);

            // обработка ошибок загрузки 
            cross.DownloadFailed += (s, e) => MessageBox.Show("Ошибка загрузки крестика!");
            circle.DownloadFailed += (s, e) => MessageBox.Show("Ошибка загрузки нолика!");
        }

        private void CellClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int index = int.Parse(button.Tag?.ToString() ?? "0");

            if (board[index] != null || CheckWinner()) return;

            if (isXTurn)
            {
                button.Content = new System.Windows.Controls.Image { Source = cross, Stretch = System.Windows.Media.Stretch.Uniform };
                board[index] = "X";
                txtStatus.Text = "Ходит: O";
            }
            else
            {
                button.Content = new System.Windows.Controls.Image { Source = circle, Stretch = System.Windows.Media.Stretch.Uniform };
                board[index] = "O";
                txtStatus.Text = "Ходит: X";
            }

            isXTurn = !isXTurn;

            if (CheckWinner())
            {
                string winner = isXTurn ? "O" : "X";
                txtStatus.Text = $"Победил: {winner}!";
                DisableAllButtons();
            }
            else if (IsBoardFull())
            {
                txtStatus.Text = "Ничья!";
            }
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
                if (cell == null) return false;
            }
            return true;
        }

        private void DisableAllButtons()
        {
            foreach (var child in ((Grid)this.Content).Children)
            {
                if (child is Button btn)
                    btn.IsEnabled = false;
            }
        }
    }
}