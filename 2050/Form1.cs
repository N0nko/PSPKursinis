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
    class Cell
    {

        Point position;
        Size size;

        int itemID, itemValue;
        private Label itemLabel;
        private PictureBox itemImage;

        //transform
        public Point Position { get => position; set => position = value; }
        public Size Size { get => size; set => size = value; }
        //game values
        public int ItemID { get => itemID; set => itemID = value; }
        public int ItemValue { get => itemValue; set => itemValue = value; }
        public Label ItemLabel { get => itemLabel; set => itemLabel = value; }
        public PictureBox ItemImage { get => itemImage; set => itemImage = value; }



        public void CreateCell(Point pos, Size dims, int id, PictureBox image)
        {
            Position = pos;
            Size = dims;
            ItemID = id;
            ItemImage = image;

        }

    }

    public partial class Form1 : Form
    {
        public int[,] board = new int[4, 4];
        public Label[,] labels = new Label[4, 4];
        public PictureBox[,] background = new PictureBox[4, 4];
        List<Cell> cells = new List<Cell>();
        Size cellSize;
        Label scoreLabel;
        int score = 0;

        //ui settings
        public int blockSize = 50;
        public int blockOffset = 5;

        public int initialOffsetX = 20;
        public int initialOffsetY = 80;

        Label label1;
        PictureBox labelBox;


        //game settings
        int cellMatrixCount = 4;
        int faceVariable = 2;

        public Form1()
        {
            cellSize = new Size(blockSize, blockSize);
            int[,] map = new int[faceVariable, faceVariable];
            Label[,] labels = new Label[faceVariable, faceVariable];
            PictureBox[,] pics = new PictureBox[faceVariable, faceVariable];

            InitializeComponent();
            KeyDown += new KeyEventHandler(OnKeyDown);

            CreateMap();

            CreateScoreText();

            CreateStartingBlock(0, 0, 0, 0);
            CreateStartingBlock(0, 1, blockSize + blockOffset, 0);

        }



        

        void CreateScoreText()
        {
            labelBox = new PictureBox();
            label1 = new Label();
            label1.Text = "Score: 0";
            label1.Size = new Size(100, 15);
         
            label1.TextAlign = ContentAlignment.BottomLeft;
            labelBox.Controls.Add(label1);
            labelBox.Location = new Point(20, 10);
            labelBox.Size = new Size(100, 25); 
            labelBox.BackColor = Color.FromArgb(238, 228, 218);
            this.Controls.Add(labelBox);

        }


        List<PictureBox> CreateMap()
        {
            List<PictureBox> pictureBoxes = new List<PictureBox>();
            scoreLabel = new Label();
            scoreLabel.Location = new Point(0, 20);
            
            for (int i = 0; i < cellMatrixCount; i++)
                for (int j = 0; j < cellMatrixCount; j++)
                {
                    //transform
                    Point cellPos = new Point(initialOffsetX + (blockSize + blockOffset) * j, initialOffsetY + (blockSize + blockOffset) * i);
                    PictureBox emptyCell = new PictureBox();

                    emptyCell.Location = cellPos;
                    emptyCell.Size = cellSize;
                    emptyCell.BackColor = Color.FromArgb(204, 192, 179);

                    this.Controls.Add(emptyCell);

                    pictureBoxes.Add(emptyCell);

                }
            return pictureBoxes;
        }

        void InsertRandomBox()
        {
            Random rng = new Random();
            int a = rng.Next(0, cellMatrixCount);
            int b = rng.Next(0, cellMatrixCount);
        

            //sticks on full board
            while (background[a, b] != null)
            {
                a = rng.Next(0, cellMatrixCount);
                b = rng.Next(0, cellMatrixCount);

            }

            CreateStartingBlock(a, b, b * (blockSize + blockOffset), a * (blockSize + blockOffset));
        }

        void CreateStartingBlock(int i, int j, int offsetX, int offsetY)
        {

            board[i, j] = 1;

            background[i, j] = new PictureBox();
            labels[i, j] = new Label();
            labels[i, j].Text = faceVariable.ToString();
            labels[i, j].Size = cellSize;
            labels[i, j].TextAlign = ContentAlignment.MiddleCenter;
            background[i, j].Controls.Add(labels[i, j]);
            background[i, j].Location = new Point(initialOffsetX + offsetX, initialOffsetY + offsetY);
            background[i, j].Size = cellSize;
            background[i, j].BackColor = Color.FromArgb(238, 228, 218);
            this.Controls.Add(background[i, j]);
            background[i, j].BringToFront();


        }

        void ColorSum(int sum, int i, int j)
        {
            if (sum % (faceVariable * Math.Pow(2, 10)) == 0) background[i, j].BackColor = Color.FromArgb(252, 132, 3);
            else if (sum % (faceVariable * Math.Pow(2, 9)) == 0) background[i, j].BackColor = Color.FromArgb(252, 190, 3);
            else if (sum % (faceVariable * Math.Pow(2, 8)) == 0) background[i, j].BackColor = Color.FromArgb(252, 211, 3);
            else if (sum % (faceVariable * Math.Pow(2, 7)) == 0) background[i, j].BackColor = Color.FromArgb(252, 252, 3);
            else if (sum % (faceVariable * Math.Pow(2, 6)) == 0) background[i, j].BackColor = Color.FromArgb(232, 252, 3);
            else if (sum % (faceVariable * Math.Pow(2, 5)) == 0) background[i, j].BackColor = Color.FromArgb(246, 94, 59);
            else if (sum % (faceVariable * Math.Pow(2, 4)) == 0) background[i, j].BackColor = Color.FromArgb(245, 149, 99);
            else if (sum % (faceVariable * Math.Pow(2, 3)) == 0) background[i, j].BackColor = Color.FromArgb(242, 177, 121);
            else background[i, j].BackColor = Color.FromArgb(237, 224, 200);
        }

        void MoveBlocks(int i, int j, int dirH, int dirV)
        {

            if (board[i, j] == 0)
            {

                board[i + (1 * dirV), j + (1 * dirH)] = 0;
                board[i, j] = 1;
                background[i, j] = background[i + (1 * dirV), j + (1 * dirH)];
                background[i + (1 * dirV), j + (1 * dirH)] = null;
                labels[i, j] = labels[i + (1 * dirV), j + (1 * dirH)];
                labels[i + (1 * dirV), j + 1 * dirH] = null;
                background[i, j].Location = new Point(background[i, j].Location.X - ((blockSize + blockOffset) * dirH), background[i, j].Location.Y - ((blockSize + blockOffset) * dirV));

            }
            else
            {
                int a = int.Parse(labels[i, j].Text);
                int b = int.Parse(labels[i + (1 * dirV), j + (1 * dirH)].Text);
                if (a == b)
                {

                    labels[i, j].Text = (a + b).ToString();
                    score += (a + b);
                    ColorSum(a + b, i, j);
                    label1.Text = "Score: " + score;
                    board[i + (1 * dirV), j + (1 * dirH)] = 0;
                    this.Controls.Remove(background[i + (1 * dirV), j + (1 * dirH)]);
                    this.Controls.Remove(labels[i + (1 * dirV), j + (1 * dirH)]);
                    background[i + (1 * dirV), j + (1 * dirH)] = null;
                    labels[i + (1 * dirV), j + (1 * dirH)] = null;

                }
            }


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

                            if (board[i, l] == 1)
                            {
                                for (int j = l + 1; j < cellMatrixCount; j++)
                                {
                                    MoveBlocks(i, j, -1, 0);
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
                            if (board[i, l] == 1)
                            {
                                for (int j = l - 1; j >= 0; j--)
                                {
                                    MoveBlocks(i, j, 1, 0);
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
                            if (board[i, l] == 1)
                            {
                                for (int j = i + 1; j < cellMatrixCount; j++)
                                {
                                    MoveBlocks(j, l, 0, -1);
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
                            if (board[i, l] == 1)
                            {
                                for (int j = i - 1; j >= 0; j--)
                                {
                                    MoveBlocks(j, l, 0, 1);
                                    blockMoved = true;
                                }
                            }
                        }
                    }
                    break;

            }
            if (blockMoved)
                InsertRandomBox();

        }
    }
}
