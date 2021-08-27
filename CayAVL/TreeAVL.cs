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
    class TreeAVL
    {
        public Node root;
        public float x = 15, y = 45, xAdd, yAdd, xFatherAdd;

        public TreeAVL()
        {
            root = null;
        }

        // Quay  L-L
        private void rotateLL(ref Node T)
        {
            Node T1 = T.pLeft;
            T.pLeft = T1.pRight;
            T1.pRight = T;
            switch (T1.Blance)
            {
                case -1:
                    T.Blance = 0;
                    T1.Blance = 0;
                    break;
                case 0:
                    T.Blance = -1;
                    T1.Blance = 1;
                    break;
            }
            T = T1;
        }

        // Quay R -R
        private void rotateRR(ref Node T)
        {
            Node T1 = T.pRight;
            T.pRight = T1.pLeft;
            T1.pLeft = T;
            switch (T1.Blance)
            {
                case 1:
                    T.Blance = 0;
                    T1.Blance = 0;
                    break;
                case 0:
                    T.Blance = 1;
                    T1.Blance = -1;
                    break;
            }
            T = T1;
        }

        // Quay L -R
        private void rotateLR(ref Node T)
        {
            Node T1 = T.pLeft;
            Node T2 = T1.pRight;
            T.pLeft = T2.pRight;
            T2.pRight = T;
            T1.pRight = T2.pLeft;
            T2.pLeft = T1;

            switch (T2.Blance)
            {
                case -1:
                    T.Blance = 1;
                    T1.Blance = 0;
                    break;

                case 0:
                    T.Blance = 0;
                    T1.Blance = 0;
                    break;

                case 1:
                    T.Blance = 0;
                    T1.Blance = -1;
                    break;
            }

            T2.Blance = 0;
            T = T2;

        }

        // Quay R -L
        private void rotateRL(ref Node T)
        {
            Node T1 = T.pRight;
            Node T2 = T1.pLeft;
            T.pRight = T2.pLeft;
            T2.pLeft = T;
            T1.pLeft = T2.pRight;
            T2.pRight = T1;
            switch (T2.Blance)
            {
                case 1:
                    T.Blance = -1;
                    T1.Blance = 0;
                    break;

                case 0:
                    T.Blance = 0;
                    T1.Blance = 0;
                    break;

                case -1:
                    T.Blance = 0;
                    T1.Blance = 1;
                    break;
            }
            T2.Blance = 0;
            T = T2;
        }



    
        // cân bằng cây khi bị lệch về bên trái
        public int balanceLeft(ref Node T)
        {
            Node T1 = T.pLeft;
            switch (T1.Blance)
            {
                case -1: rotateLL(ref T); return 2;
                case 0: rotateLL(ref T); return 1;
                case 1: rotateLR(ref T); return 2;
            }
            return 0;
        }

       
        // Cân bằng cây khi bị lệch về bên phải
        public int balanceRight(ref Node T)
        {
            Node T1 = T.pRight;
            switch (T1.Blance)
            {
                case -1: rotateRL(ref T); return 2;
                case 0: rotateRR(ref T); return 1;
                case 1: rotateRR(ref T); return 2;
            }
            return 0;
        }


        //  Hàm thêm Node       
        public int insertNode(ref Node T, int key)
        {
            int res;
            if (T != null)
            {
                if (T.key == key) return 0; //ĐÃ CÓ
                if (T.key > key)
                {
                    res = insertNode(ref T.pLeft, key);
                    if (res < 2) return res;
                    switch (T.Blance)
                    {
                        case 1: T.Blance = 0;
                            return 1;
                        case 0: T.Blance = -1;
                            return 2;
                        case -1: balanceLeft(ref T);
                            return 1;
                    }
                }
                else
                {
                    res = insertNode(ref T.pRight, key);
                    if (res < 2) return res;
                    switch (T.Blance)
                    {
                        case -1: T.Blance = 0;
                            return 1;
                        case 0: T.Blance = 1;
                            return 2;
                        case 1: balanceRight(ref T);
                            return 1;
                    }
                }
            }
            T = new Node(key);
            if (T == null) return -1; //THIẾU BỘ NHỚ	        
            return 2; //THÀNH CÔNG, CHIỀU CAO TĂNG
        }

        public int insertNode(ref Node root, int key, TreeAVL T, DrawTreeAVL draw, Graphics graph, Bitmap bm, Form1 f, ref int tocDo)
        {
            int res;
            if (root != null)
            {
                Thread.Sleep(tocDo);
                if (root == T.root)
                {
                    //VẼ NODE CẦN ADD Ở GÓC TRÁI PICTURE BOX VÀ DI CHUYỂN DẦN VÀO NÚT ROOT
                    x = 15; y = 45;
                    draw.DrawNode(key, 15, 45, graph, bm, f);
                    draw.DrawTree(T, graph, bm, f);
                    f.pictureBox1.Image = bm;                
                    draw.MoveNode(key, ref x, ref y, root.x, root.y, T, graph, bm, f, ref tocDo);
                    Thread.Sleep(tocDo);
                }

                if (root.key == key)
                {                   
                    return 0; //ĐÃ CÓ
                }
                if (root.key > key)
                {                 
                    if (root.pLeft != null)
                    {
                        draw.MoveNode(key, ref x, ref y, root.pLeft.x, root.pLeft.y, T, graph, bm, f, ref tocDo);
                    }
                    else
                    {
                        xAdd = root.x - Convert.ToInt32(Math.Abs(root.x - root.Father) / 2);
                        yAdd = y + 80;
                        xFatherAdd = root.x;
                        draw.MoveNode(key, ref x, ref y, xAdd, yAdd, T, graph, bm, f, ref tocDo);
                        graph.DrawLine(new Pen(Color.DarkGreen, 1f), xAdd, yAdd, root.x, root.y);
                        draw.DrawNode(root, graph, bm, f);
                        draw.DrawNode(key, xAdd, yAdd, graph, bm, f);
                        Application.DoEvents();
                        f.pictureBox1.Image = bm;
                    }
                    res = insertNode(ref root.pLeft, key, T, draw, graph, bm, f, ref tocDo);
                    if (res < 2) return res;
                    switch (root.Blance)
                    {
                        case 1: root.Blance = 0;
                            return 1;
                        case 0: root.Blance = -1;
                            return 2;
                        case -1: balanceLeft(ref root);
                            return 1;
                    }
                }
                else
                {
                    if (root.pRight != null)
                    {
                        draw.MoveNode(key, ref x, ref y, root.pRight.x, root.pRight.y, T, graph, bm, f, ref tocDo);
                    }
                    else
                    {
                        xAdd = root.x + Convert.ToInt32(Math.Abs(root.x - root.Father) / 2);
                        yAdd = y + 80;
                        xFatherAdd = root.x;
                        draw.MoveNode(key, ref x, ref y, xAdd, yAdd, T, graph, bm, f, ref tocDo);

                        graph.DrawLine(new Pen(Color.DarkGreen, 1f), xAdd, yAdd, root.x, root.y);
                        draw.DrawNode(root, graph, bm, f);
                        draw.DrawNode(key, xAdd, yAdd, graph, bm, f);
                        f.pictureBox1.Image = bm;
                        Application.DoEvents();
                        Thread.Sleep(tocDo);
                    }
                    res = insertNode(ref root.pRight, key, T, draw, graph, bm, f, ref tocDo);
                    if (res < 2) return res;
                    switch (root.Blance)
                    {
                        case -1: root.Blance = 0;
                            return 1;
                        case 0: root.Blance = 1;
                            return 2;
                        case 1: balanceRight(ref root);
                            return 1;
                    }
                }
            }

            root = new Node(key);
            if (root == null) return -1; //THIẾU BỘ NHỚ
            return 2; //THÀNH CÔNG, CHIỀU CAO TĂNG
        }

        // Ham Tim Node
        public Node SearchNode(int key, Node node)
        {
            if (node == null)
                return null;
            else
            {
                if (node.key == key)
                    return node;
                else if (key < node.key)
                    return SearchNode(key, node.pLeft);
                else
                    return SearchNode(key, node.pRight);
            }
        } 
        public Node SearchNode(int key, Node node, Graphics graph, Bitmap bm, DrawTreeAVL draw, TreeAVL T, Form1 f)
        {
            if (node == null)
                return null;
            else
            {
                graph.Clear(Color.DarkGray);
                draw.DrawTree(T, graph, bm, f);
                f.pictureBox1.Image = bm;

                draw.DrawSelectNode(node, graph, bm, f);
                f.pictureBox1.Image = bm;
                Thread.Sleep(500);
                Application.DoEvents();

                if (node.key == key)
                    return node;
                else if (key < node.key)
                {
                    return SearchNode(key, node.pLeft, graph, bm, draw, T, f);
                }
                else
                {
                    return SearchNode(key, node.pRight, graph, bm, draw, T, f);
                }
            }
        }

        // Hàm xóa Node
        public int delNode(ref Node root, int key, TreeAVL T, DrawTreeAVL draw, Graphics graph, Bitmap bm, Form1 f, ref int tocDo)
        {
            if (root == T.root)
            {
                x = T.root.x; y = T.root.y;
            }
            Thread.Sleep(tocDo);
            int res;
            if (root == null) return 0;
            if (root.key > key)
            {
                draw.MoveNode(key, ref x, ref y, root.pLeft.x, root.pLeft.y, T, graph, bm, f, ref tocDo);

                res = delNode(ref root.pLeft, key, T, draw, graph, bm, f, ref tocDo);
                if (res < 2) return res;
                switch (root.Blance)
                {
                    case -1: root.Blance = 0;
                        return 2;
                    case 0: root.Blance = 1;
                        return 1;
                    case 1: return balanceRight(ref root);
                    default: return -2;
                }
            }

            if (root.key < key)
            {
                draw.MoveNode(key, ref x, ref y, root.pRight.x, root.pRight.y, T, graph, bm, f, ref tocDo);

                res = delNode(ref root.pRight, key, T, draw, graph, bm, f, ref tocDo);
                if (res < 2) return res;
                switch (root.Blance)
                {
                    case 1: root.Blance = 0;
                        return 2;
                    case 0: root.Blance = -1;
                        return 1;
                    case -1: return balanceLeft(ref root);
                    default: return -2;
                }
            }
            else
            { 

                HieuUng(root, draw, graph, bm, f, ref tocDo);
   

                Node p = root;
                if (root.pLeft == null)
                {
                    root = root.pRight; res = 2;
                }
                else if (root.pRight == null)
                {
                    root = root.pLeft; res = 2;
                }
                else
                { //T CÓ CẢ 2 CON
                    res = searchStandFor(ref p, ref root.pRight, T, draw, graph, bm, f, ref tocDo);
                    if (res < 2) return res;
                    switch (root.Blance)
                    {
                        case 1: root.Blance = 0;
                            return 2;
                        case 0: root.Blance = -1;
                            return 1;
                        case -1: return balanceLeft(ref root);
                        default: return -2;
                    }
                }

                return res;
            }
        }
        private int searchStandFor(ref Node p, ref Node q, TreeAVL T, DrawTreeAVL draw, Graphics graph, Bitmap bm, Form1 f, ref int tocDo)
        {
            Thread.Sleep(tocDo);
            graph.Clear(Color.Honeydew);
            draw.DrawTree(T, graph, bm, f);
            draw.DrawSelectNode(p, graph, bm, f);
            draw.DrawStandForNode(q, graph, bm, f);
            f.pictureBox1.Image = bm;
            Application.DoEvents();

            int res;
            if (q.pLeft != null)
            {
                res = searchStandFor(ref p, ref q.pLeft, T, draw, graph, bm, f, ref tocDo);
                if (res < 2) return res;
                switch (q.Blance)
                {
                    case -1: q.Blance = 0;
                        return 2;
                    case 0: q.Blance = 1;
                        return 1;
                    case 1: return balanceRight(ref q);
                    default: return -2;
                }
            }
            else
            {
                HieuUng(q, draw, graph, bm, f, ref tocDo);
                p.key = q.key;
                p = q;
                q = q.pRight;
                return 2;
            }
        }

        public void HieuUng(Node q, DrawTreeAVL draw, Graphics graph, Bitmap bm, Form1 f, ref int tocDo)
        {
            graph.FillEllipse(Brushes.Yellow, q.x - f.r, q.y - f.r, 2 * f.r, 2 * f.r);
            graph.FillEllipse(Brushes.LightPink, q.x - f.R, q.y - f.R, 2 * f.R, 2 * f.R);
            graph.DrawString(q.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, q.x - f.r / 2, q.y - f.r / 2 );

            f.pictureBox1.Image = bm;
            Thread.Sleep(tocDo);
            Application.DoEvents();

            draw.DrawStandForNode(q, graph, bm, f);
            Thread.Sleep(tocDo);
            f.pictureBox1.Image = bm;
            Application.DoEvents();

            graph.FillRectangle(Brushes.Yellow, q.x - f.r, q.y - f.r, 2 * f.r, 2 * f.r);
            graph.FillRectangle(Brushes.YellowGreen, q.x - f.R, q.y - f.R, 2 * f.R, 2 * f.R);
            graph.DrawString(q.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, q.x - f.r / 2, q.y - f.r / 2 );

            f.pictureBox1.Image = bm;
            Thread.Sleep(tocDo);
            Application.DoEvents();

            draw.DrawStandForNode(q, graph, bm, f);
            Thread.Sleep(tocDo);
            f.pictureBox1.Image = bm;
            Application.DoEvents();
        }

    }
}

