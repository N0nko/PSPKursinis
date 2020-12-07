using _2050;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace _2050.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void CreateMapTest()
        {

        }

        [TestMethod()]
        public void MoveBlocksTest()
        {
            BoardInteractions board = new BoardInteractions();
            board.MoveBlocks(2, 2, 1, 0);
            Assert.AreEqual(board.cells[3, 2].ItemLabel.Text, "");
        }

        [TestMethod()]
        public void ColorSumTest()
        {
            BoardInteractions board = new BoardInteractions();
            board.cells[1, 1].ColorSum(2, 2);
            Assert.AreEqual(board.cells[1, 1].ItemImage.BackColor , Color.FromArgb(242, 177, 121));
        }

        [TestMethod()]
        public void CreateScoreTextTest()
        {
            BoardInteractions board = new BoardInteractions();
            board.CreateScoreText();
            Assert.AreEqual(board.ScoreLabel.Text, "Score: 0");
        }
    }
}


