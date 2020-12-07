using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2050
{
    public class Board : Form
    {
        public int[,] board;
        public Cell[,] cells;

        int blockSize;
        int blockOffset;
        int cellMatrixCount;
        int initialOffsetX;
        int initialOffsetY;
        int faceVariable;
        int score;
        Size cellSize;

        Label label1;
        PictureBox labelBox;
        Label scoreLabel;

        public int BlockSize { get => blockSize; set => blockSize = value; }
        public int BlockOffset { get => blockOffset; set => blockOffset = value; }
        public int CellMatrixCount { get => cellMatrixCount; set => cellMatrixCount = value; }
        public int InitialOffsetX { get => initialOffsetX; set => initialOffsetX = value; }
        public int InitialOffsetY { get => initialOffsetY; set => initialOffsetY = value; }
        public Size CellSize { get => cellSize; set => cellSize = value; }

        public int FaceVariable { get => faceVariable; set => faceVariable = value; }
        public int Score { get => score; set => score = value; }
        public Label Label1 { get => label1; set => label1 = value; }
        public PictureBox LabelBox { get => labelBox; set => labelBox = value; }
        public Label ScoreLabel { get => scoreLabel; set => scoreLabel = value; }

        public void InitBoard(int cellMatrixCount, int blockSize, int blockOffset, int initialOffsetX, int initialOffsetY, int faceVariable)
        {
            CellMatrixCount = cellMatrixCount;
            BlockSize = blockSize;
            BlockOffset = blockOffset;
            InitialOffsetX = initialOffsetX;
            InitialOffsetY = initialOffsetY;
            CellSize = cellSize;

            faceVariable = FaceVariable;
            Score = 0;

            cellSize = new Size(blockSize, blockSize);
            cells = new Cell[CellMatrixCount, CellMatrixCount];
            board = new int[CellMatrixCount, CellMatrixCount];


        }
    }

    public class BoardInteractions : Board {

        public void CreateScoreText()
        {
            ScoreLabel = new Label();
            ScoreLabel.Location = new Point(0, 20);

            LabelBox = new PictureBox();
            Label1 = new Label();
            Label1.Text = "Score: 0";
            Label1.Size = new Size(100, 15);

            Label1.TextAlign = ContentAlignment.BottomLeft;
            LabelBox.Controls.Add(Label1);
            LabelBox.Location = new Point(20, 10);
            LabelBox.Size = new Size(100, 25);
            LabelBox.BackColor = Color.FromArgb(238, 228, 218);
            Controls.Add(LabelBox);

        }

        public void CreateMap()
        {

            for (int i = 0; i < CellMatrixCount; i++)
                for (int j = 0; j < CellMatrixCount; j++)
                {

                    Point cellPos = new Point(InitialOffsetX + (BlockSize + BlockOffset) * j, InitialOffsetY + (BlockSize + BlockOffset) * i);

                    cells[i, j] = new EmptyCell();
                    cells[i, j].CreateCell(cellPos);


                }
        }

        public void MoveBlocks(int i, int j, int dirH, int dirV)
        {

            Cell thisCell = cells[i, j];
            Cell thatCell = cells[i + (1 * dirV), j + (1 * dirH)];

            if (board[i, j] == 0)
            {

                board[i + (1 * dirV), j + (1 * dirH)] = 0;
                board[i, j] = 1;


                thisCell.ItemImage = thatCell.ItemImage;
                thatCell.ItemImage = null;

                thisCell.ItemLabel = thatCell.ItemLabel;
                thatCell.ItemLabel = null;
                thisCell.ItemImage.Location = new Point(thisCell.ItemImage.Location.X - ((BlockSize + BlockOffset) * dirH), thisCell.ItemImage.Location.Y - ((BlockSize + BlockOffset) * dirV));
            }
            else
            {

                int a = int.Parse(thisCell.ItemLabel.Text);
                int b = int.Parse(thatCell.ItemLabel.Text);
                if (a == b)
                {

                    thisCell.ItemLabel.Text = (a + b).ToString();
                    Score += (a + b);
                    thisCell.ColorSum(a + b, FaceVariable);
                    Label1.Text = "Score: " + Score;
                    board[i + (1 * dirV), j + (1 * dirH)] = 0;
                    Controls.Remove(thatCell.ItemImage);
                    Controls.Remove(thatCell.ItemLabel);
                    thatCell.ItemImage = null;
                    thatCell.ItemLabel = null;

                }
            }

        }

    }

        public abstract class Cell : Board
        {
            private Label itemLabel;
            private PictureBox itemImage;


            public Label ItemLabel { get => itemLabel; set => itemLabel = value; }
            public PictureBox ItemImage { get => itemImage; set => itemImage = value; }

            public abstract void CreateCell(Point cellPos);
            public abstract void CreateCell(int offsetX, int offsetY);


            public void ColorSum(int sum, int faceVariable)
            {
                if (sum % (faceVariable * Math.Pow(2, 10)) == 0) ItemImage.BackColor = Color.FromArgb(252, 132, 3);
                else if (sum % (faceVariable * Math.Pow(2, 9)) == 0) ItemImage.BackColor = Color.FromArgb(252, 190, 3);
                else if (sum % (faceVariable * Math.Pow(2, 8)) == 0) ItemImage.BackColor = Color.FromArgb(252, 211, 3);
                else if (sum % (faceVariable * Math.Pow(2, 7)) == 0) ItemImage.BackColor = Color.FromArgb(252, 252, 3);
                else if (sum % (faceVariable * Math.Pow(2, 6)) == 0) ItemImage.BackColor = Color.FromArgb(232, 252, 3);
                else if (sum % (faceVariable * Math.Pow(2, 5)) == 0) ItemImage.BackColor = Color.FromArgb(246, 94, 59);
                else if (sum % (faceVariable * Math.Pow(2, 4)) == 0) ItemImage.BackColor = Color.FromArgb(245, 149, 99);
                else if (sum % (faceVariable * Math.Pow(2, 3)) == 0) ItemImage.BackColor = Color.FromArgb(242, 177, 121);
                else ItemImage.BackColor = Color.FromArgb(237, 224, 200);
            }
        }

        public class EmptyCell : Cell
        {

            public override void CreateCell(Point cellPos)
            {
                ItemImage = new PictureBox
                {
                    Location = cellPos,
                    Size = CellSize,
                    BackColor = Color.FromArgb(204, 192, 179)
                };
                Controls.Add(ItemImage);

            }

            public override void CreateCell(int offsetX, int offsetY)
            {
                throw new NotImplementedException();
            }
        }

        public class Block : Cell
        {
            public override void CreateCell(int offsetX, int offsetY)
            {

                ItemLabel = new Label
                {
                    Text = FaceVariable.ToString(),
                    Size = CellSize,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                ItemImage = new PictureBox
                {

                    Location = new Point(InitialOffsetX + offsetX, InitialOffsetY + offsetY),
                    Size = CellSize,
                    BackColor = Color.FromArgb(238, 228, 218),

                };

                ItemImage.Controls.Add(ItemLabel);

                Controls.Add(ItemImage);

                ItemImage.BringToFront();

            }

            public override void CreateCell(Point cellPos)
            {
                throw new NotImplementedException();
            }
        }


        public partial class Form1 : Form
        {

            BoardInteractions GameBoard;

            //ui settings
            public int blockSize = 50;
            public int blockOffset = 5;

            public int initialOffsetX = 20;
            public int initialOffsetY = 80;

            //game settings
            int cellMatrixCount = 4;
            int faceVariable = 2;

            public Form1()
            {
                InitializeComponent();

                KeyDown += new KeyEventHandler(OnKeyDown);

                GameBoard = new BoardInteractions();

                GameBoard.InitBoard(cellMatrixCount, blockSize, blockOffset, initialOffsetX, initialOffsetY, faceVariable);

                GameBoard.CreateScoreText();

                GameBoard.CreateMap();

               // GameBoard.CreateControl();
               // GameBoard.CreateGraphics();
               // GameBoard.BringToFront();
               //UpdateTable();

                InsertRandomBox(GameBoard.cells);

            }
            void UpdateTable()
            {

                foreach (Cell cell in GameBoard.cells)
                {
                    cell.ItemImage.BringToFront();
                    Controls.Add(cell.ItemImage);

                }
            }
            public void InsertRandomBox(Cell[,] cells)
            {
                Random rng = new Random();
                int a = rng.Next(0, cellMatrixCount);
                int b = rng.Next(0, cellMatrixCount);

                int loopsPassed = 0;

                while (GameBoard.cells[a, b].ItemImage != null)
                {
                    a = rng.Next(0, cellMatrixCount);
                    b = rng.Next(0, cellMatrixCount);
                    loopsPassed++;
                    if (loopsPassed > cellMatrixCount * cellMatrixCount)
                        return;
                }
                GameBoard.board[a, b] = 1;
                GameBoard.cells[a, b].CreateCell(b * (blockSize + blockOffset), a * (blockSize + blockOffset));

            }

            private void OnKeyDown(object sender, KeyEventArgs keyPress)
            {
                bool blockMoved = false;

                switch (keyPress.KeyCode.ToString())
                {
                    case "Right":
                        for (int i = 0; i < cellMatrixCount; i++)
                        {
                            for (int l = cellMatrixCount - 2; l >= 0; l--)
                            {

                                if (GameBoard.board[i, l] == 1)
                                {
                                    for (int j = l + 1; j < cellMatrixCount; j++)
                                    {
                                        GameBoard.MoveBlocks(i, j, -1, 0);
                                        blockMoved = true;

                                    }
                                }
                            }
                        }
                        break;
                    case "Left":

                        for (int i = 0; i < cellMatrixCount; i++)
                        {
                            for (int l = 1; l < cellMatrixCount; l++)
                            {
                                if (GameBoard.board[i, l] == 1)
                                {
                                    for (int j = l - 1; j >= 0; j--)
                                    {
                                        GameBoard.MoveBlocks(i, j, 1, 0);
                                        blockMoved = true;
                                    }
                                }
                            }
                        }
                        break;
                    case "Down":

                        for (int i = cellMatrixCount - 2; i >= 0; i--)
                        {
                            for (int l = 0; l < cellMatrixCount; l++)
                            {
                                if (GameBoard.board[i, l] == 1)
                                {
                                    for (int j = i + 1; j < cellMatrixCount; j++)
                                    {
                                        GameBoard.MoveBlocks(j, l, 0, -1);
                                        blockMoved = true;
                                    }
                                }
                            }
                        }
                        break;
                    case "Up":

                        for (int i = 1; i < cellMatrixCount; i++)
                        {
                            for (int l = 0; l < cellMatrixCount; l++)
                            {
                                if (GameBoard.board[i, l] == 1)
                                {
                                    for (int j = i - 1; j >= 0; j--)
                                    {
                                        GameBoard.MoveBlocks(j, l, 0, 1);
                                        blockMoved = true;
                                    }
                                }
                            }
                        }
                        break;

                }
                if (blockMoved)
                    InsertRandomBox(GameBoard.cells);

            }
        }
    }
