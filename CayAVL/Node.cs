using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace TreeAVL
{
   public  class Node
    {
        public int Blance;             // Chỉ số cân bằng
        public int key;                // Giá trị của khóa
        public Node pLeft;             // Node trái
        public Node pRight;            // Node phải
        public float x, y, Father;      

        // khởi tạo node chưa có nhánh
        public Node(int key)
        {
            this.key = key;
            pLeft = null;
            pRight = null;
            Blance = 0;
        }

        // khởi tạo node có nhánh trái nhánh phải
        public Node(int key, Node pLeft, Node pRight)
        {
            this.key = key;
            this.pLeft = pLeft;
            this.pRight = pRight;
            Blance = 0;
        }


        public void GetPos(Node node, Form1 f)
        {
            if (node != null)
            {
                GetNodePos(node, f);
                node.GetPos(node.pLeft, f);
                node.GetPos(node.pRight, f);
            }
        }

        private void GetNodePos(Node node, Form1 f)
        {
            if (node.key > key)
            {
                node.x = x + Convert.ToInt32(Math.Abs((x - Father) / 2));//XÁC ĐỊNH X, Y CỦA NODE PHẢI                
            }
            else
            {
                node.x = x - Convert.ToInt32(Math.Abs((x - Father) / 2));//XÁC ĐỊNH X, Y CỦA NODE TRÁI
            }
            node.y = y + 80;
            node.Father = x;
        }
    }
}
