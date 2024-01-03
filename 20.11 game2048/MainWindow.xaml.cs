using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20._11_game2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Cell
        {
            public int Value { get; set; }
            private Point _location;
            public Point Location {
                get
                {
                    return _location;
                }
                set
                {
                    _location = value;
                }
            }
            public Cell(Point location)
            {
                _location = location;
                Value =0;
            }
        }
        private int Sum { get; set; } = 0;
        private List<Cell> CellList { get; set; }
        private List<Cell> FreeCell { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        public void StartGame()
        {
            Score.Text = HighScore.Text = "0";
            Playground.Children.Clear();

            FreeCell = new List<Cell>();
            CellList = new List<Cell>();
            for (int i = 0; i < 4; i++) {
                for(int j=0; j< 4; j++)
                {      
                    CellList.Add(new Cell(new Point(j, i)));
                    FreeCell.Add(CellList.ElementAt(i * 4 + j));
                }
            }

            if(FreeCell.Count != 0)
            {
                DrawCells();
            }
        }

        void SpawnCell()
        {
            FreeCell.Clear();
            for (int i = 0; i < CellList.Count; i++)
            {
                if (CellList[i].Value < 2)
                {
                    FreeCell.Add(CellList[i]);
                }
            }

            Random random = new Random();

            const int SmallerNumber = 2;
            const int LargerNumber = 4;
            const int ChanceLargerNumber = 10;

            int NumToSpawn = random.Next(0, 100) <= ChanceLargerNumber ? LargerNumber : SmallerNumber;
            int spawnCellNum = random.Next(FreeCell.Count);
            Sum = CalculateSum();

            for (int i=0; i < CellList.Count;i++)
            {
                if (CellList[i].Location.X == FreeCell[spawnCellNum].Location.X && CellList[i].Location.Y == FreeCell[spawnCellNum].Location.Y)
                {
                    CellList[i].Value = NumToSpawn;
                    break;
                }
            }
        }
        private int CalculateSum()
        {
            int sum = 0;

            foreach (var cell in CellList)
            {
                sum += cell.Value;
            }

            return sum;
        }


        void DrawCells()
        {
            Playground.Children.Clear();
            SpawnCell();
            for (int i=0;i< CellList.Count; i++)
            {
                if (CellList[i].Value == 0) continue;
                AddLabel((int)CellList[i].Location.X, (int)CellList[i].Location.Y, CellList[i].Value);
            }
            Score.Text = HighScore.Text = Sum.ToString();
        }

        private void AddLabel(int X, int Y, int LabelValue)
        {
            System.Windows.Controls.Label label = new System.Windows.Controls.Label();
            label.Content = LabelValue.ToString();

            Grid.SetColumn(label, X);
            Grid.SetRow(label, Y);
            Playground.Children.Add(label);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Up && e.Key != Key.Down) return;
            switch (e.Key)
            {
                case Key.Left:
                    MoveLeft();
                    DrawCells();
                    break;
                case Key.Right:
                    MoveRight();
                    DrawCells();
                    break;
                case Key.Up:
                    MoveUp();
                    DrawCells();
                    break;
                case Key.Down:
                    MoveDown();
                    DrawCells();
                    break;

            }
        }
        private void MoveLeft()
        {
            for(int i=0; i < 4; i++)
            {
                for (int target = 0; target < 3; target++)
                {
                    for (int j = target+1; j < 4; j++)
                    {
                        if (CellList[i * 4 + target].Value == 0 || CellList[i * 4 + target].Value == CellList[i * 4 + j].Value)
                        {
                            CellList[i * 4 + target].Value += CellList[i * 4 + j].Value;
                            CellList[i * 4 + j].Value = 0;
             
                            if (CellList[i * 4 + target].Value != 0) break;
                        }
                    }
                }

            }


        }
        private void MoveRight()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int target = 3; target > 0; target--)
                {
                    for (int j = target - 1; j >= 0; j--)
                    {
                        if (CellList[i * 4 + target].Value == 0 || CellList[i * 4 + target].Value == CellList[i * 4 + j].Value)
                        {
                            CellList[i * 4 + target].Value += CellList[i * 4 + j].Value;
                            CellList[i * 4 + j].Value = 0;
                            
                            if (CellList[i * 4 + target].Value != 0) break;
                        }
                    }
                }
            }
        }

        private void MoveUp()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int target = 0; target < 4; target++)
                {
                    for (int j = target + 1; j < 4; j++)
                    {
                        if (CellList[target * 4 + i].Value == 0 || CellList[target * 4 + i].Value == CellList[j * 4 + i].Value)
                        {
                            CellList[target * 4 + i].Value += CellList[j * 4 + i].Value;
                            CellList[j * 4 + i].Value = 0;
                           
                            if (CellList[target * 4 + i].Value != 0) break;
                        }
                    }
                }
            }
        }

        private void MoveDown()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int target = 3; target > 0; target--)
                {
                    for (int j = target - 1; j >= 0; j--)
                    {
                        if (CellList[target * 4 + i].Value == 0 || CellList[target * 4 + i].Value == CellList[j * 4 + i].Value)
                        {
                            CellList[target * 4 + i].Value += CellList[j * 4 + i].Value;
                            CellList[j * 4 + i].Value = 0;
                            
                            if (CellList[target * 4 + i].Value != 0) break;
                        }
                    }
                }
            }
        }

    }
}
