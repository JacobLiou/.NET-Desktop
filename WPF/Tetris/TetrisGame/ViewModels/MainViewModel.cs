using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using TetrisGame.Models;

namespace TetrisGame.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private GameBoard gameBoard;
        private Block currentBlock;
        private Block nextBlock;
        private DispatcherTimer gameTimer;
        private int score;
        private bool isGameOver;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool[,] GameGrid => gameBoard.Grid;
        public Block CurrentBlock => currentBlock;
        public Block NextBlock => nextBlock;
        public int Score
        {
            get => score;
            private set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public bool IsGameOver
        {
            get => isGameOver;
            private set
            {
                isGameOver = value;
                OnPropertyChanged(nameof(IsGameOver));
            }
        }

        public ICommand StartGameCommand { get; private set; }
        public ICommand MoveLeftCommand { get; private set; }
        public ICommand MoveRightCommand { get; private set; }
        public ICommand RotateCommand { get; private set; }
        public ICommand DropCommand { get; private set; }

        public MainViewModel()
        {
            gameBoard = new GameBoard(20, 10);
            InitializeCommands();
            InitializeTimer();
        }

        private void InitializeCommands()
        {
            StartGameCommand = new RelayCommand(StartGame);
            MoveLeftCommand = new RelayCommand(() => MoveBlock(-1, 0));
            MoveRightCommand = new RelayCommand(() => MoveBlock(1, 0));
            RotateCommand = new RelayCommand(RotateBlock);
            DropCommand = new RelayCommand(DropBlock);
        }

        private void InitializeTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(500);
        }

        private void StartGame()
        {
            gameBoard.Clear();
            Score = 0;
            IsGameOver = false;
            SpawnNewBlock();
            gameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (!MoveBlock(0, 1))
            {
                PlaceBlock();
                ClearLines();
                SpawnNewBlock();

                if (!IsValidMove(currentBlock))
                {
                    GameOver();
                }
            }
        }

        private void SpawnNewBlock()
        {
            if (nextBlock == null)
            {
                nextBlock = Block.GetRandomBlock();
            }

            currentBlock = nextBlock;
            nextBlock = Block.GetRandomBlock();
            currentBlock.X = gameBoard.Width / 2 - 2;
            currentBlock.Y = 0;

            OnPropertyChanged(nameof(CurrentBlock));
            OnPropertyChanged(nameof(NextBlock));
        }

        private bool MoveBlock(int deltaX, int deltaY)
        {
            Block movedBlock = currentBlock.Clone();
            movedBlock.X += deltaX;
            movedBlock.Y += deltaY;

            if (IsValidMove(movedBlock))
            {
                currentBlock = movedBlock;
                OnPropertyChanged(nameof(CurrentBlock));
                return true;
            }

            return false;
        }

        private void RotateBlock()
        {
            Block rotatedBlock = currentBlock.Clone();
            rotatedBlock.Rotate();

            if (IsValidMove(rotatedBlock))
            {
                currentBlock = rotatedBlock;
                OnPropertyChanged(nameof(CurrentBlock));
            }
        }

        private void DropBlock()
        {
            while (MoveBlock(0, 1)) { }
        }

        private bool IsValidMove(Block block)
        {
            return gameBoard.IsValidMove(block);
        }

        private void PlaceBlock()
        {
            gameBoard.PlaceBlock(currentBlock);
            OnPropertyChanged(nameof(GameGrid));
        }

        private void ClearLines()
        {
            int linesCleared = gameBoard.ClearFullLines();
            Score += linesCleared * 100;
        }

        private void GameOver()
        {
            IsGameOver = true;
            gameTimer.Stop();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
