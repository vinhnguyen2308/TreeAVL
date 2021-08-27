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
    class DrawTreeAVL
    {
        // LH    -1  Cây con trái cao hơn
        // EH     0  Hai cây con bằng nhau
        // RH     1  Cây con phải cao hơn
        public void DrawTree(TreeAVL T, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.SmoothingMode = SmoothingMode.HighQuality;
            DrawAllLine(T.root, graph, bm, f);
            DrawAllNode(T.root, graph, bm, f);
            f.pictureBox1.Image = bm;
            f.pictureBox1.Show();
        }

        // Vẽ đường nhánh của cây
        private void DrawAllLine(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            if (node.pLeft != null)
                graph.DrawLine(new Pen(Color.Black, 2f), node.x, node.y, node.pLeft.x, node.pLeft.y);
            if (node.pRight != null)
                graph.DrawLine(new Pen(Color.Black, 2f), node.x, node.y, node.pRight.x, node.pRight.y);
            if (node.pLeft != null)
                DrawAllLine(node.pLeft, graph, bm, f);
            if (node.pRight != null)
                DrawAllLine(node.pRight, graph, bm, f);
        }

        private void DrawAllNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            DrawNode(node, graph, bm, f);
            if (node.pLeft != null)
                DrawAllNode(node.pLeft, graph, bm, f);
            if (node.pRight != null)
                DrawAllNode(node.pRight, graph, bm, f);
        }

        // Vẽ node khi đã đứng yên
        public void DrawNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.DrawEllipse (new Pen(Color.Green, 5), node.x - f.r, node.y - f.r, 2 * f.r, 2 * f.r);
            graph.FillEllipse (Brushes.Aqua, node.x - f.R, node.y - f.R, 2 * f.R, 2 * f.R);

            //   Vẽ key và chỉ số cân bằng
            graph.DrawString(node.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, node.x - f.r / 2, node.y - f.r / 2 );
            switch (node.Blance)
            {
                case -1: graph.DrawString("", new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, node.x + 3 - f.r / 2, node.y + f.r ); break;
                case 0: graph.DrawString("", new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, node.x + 3 - f.r / 2, node.y + f.r ); break;
                case 1: graph.DrawString("", new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, node.x + 3 - f.r / 2, node.y + f.r ); break;
            }
        }

        // Vẽ node di chuyển 
        public void DrawNode(int key, float x, float y, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.DrawEllipse (new Pen(Color.Red, 5), x - f.r, y - f.r, 2 * f.r, 2 * f.r);
            graph.FillEllipse(Brushes.Orange, x - f.R, y - f.R, 2 * f.R, 2 * f.R);
            //
            graph.DrawString(key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, x - f.r / 2, y - f.r / 2);
        }

        // Vẽ node được chọn.
        public void DrawSelectNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.FillEllipse (Brushes.Green, node.x - f.r, node.y - f.r, 2 * f.r, 2 * f.r);
            graph.FillEllipse(Brushes.GhostWhite, node.x - f.R, node.y - f.R, 2 * f.R, 2 * f.R);
            graph.DrawString(node.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, node.x - f.r / 2, node.y - f.r / 2);
            f.pictureBox1.Image = bm;
        }

        // Tim phan tu the mang
        public void DrawStandForNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.FillEllipse(Brushes.Blue, node.x - f.r, node.y - f.r, 2 * f.r, 2 * f.r);
            graph.FillEllipse(Brushes.DarkCyan, node.x - f.R, node.y - f.R, 2 * f.R, 2 * f.R);

            graph.DrawString(node.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, node.x - f.r / 2, node.y - f.r / 2 );
            f.pictureBox1.Image = bm;
        }
        // Hàm Di chuyển Node
        public void MoveNode(int key, ref float x1, ref float y1, float x2, float y2, TreeAVL T, Graphics graph, Bitmap bm, Form1 f, ref int tocDo)
        {
            if (y1 == y2)
            {
                if (x1 < x2)
                {
                    while (x1 < x2)
                    {
                        x1 += 2; if (x1 > x2) x1 = x2;
                        graph.Clear(Color.White);
                        DrawTree(T, graph, bm, f);
                        DrawNode(key, x1, y1, graph, bm, f);
                        Thread.Sleep(tocDo);
                        Application.DoEvents();
                        f.pictureBox1.Image = bm;
                    }
                }
                else if (x1 > x2)
                {
                    while (x1 > x2)
                    {
                        x1 -= 2; if (x1 < x2) x1 = x2;
                        graph.Clear(Color.White);
                        DrawTree(T, graph, bm, f);
                        DrawNode(key, x1, y1, graph, bm, f);
                        Thread.Sleep(tocDo);
                        Application.DoEvents();
                        f.pictureBox1.Image = bm;
                    }

                }
            }
            else if (y1 < y2)
            {
                if (x1 > x2)
                {
                    float k = (2f / (x1 - x2)) * (80);

                    while (x1 > x2 && y1 < y2)
                    {
                        x1 -= 2; if (x1 < x2) x1 = x2;
                        y1 += k; if (y1 > y2 || x1 == x2) y1 = y2;
                        graph.Clear(Color.White);
                        DrawTree(T, graph, bm, f);
                        DrawNode(key, x1, y1, graph, bm, f);
                        Thread.Sleep(tocDo);
                        Application.DoEvents();
                        f.pictureBox1.Image = bm;
                    }
                }
                if (x1 < x2)
                {
                    float k = (2f / (x2 - x1)) * (80);
                    while (x1 < x2 && y1 < y2)
                    {
                        x1 += 2; if (x1 > x2) x1 = x2;
                        y1 += k; if (y1 > y2 || x1 == x2) y1 = y2;
                        graph.Clear(Color.White);
                        DrawTree(T, graph, bm, f);
                        DrawNode(key, x1, y1, graph, bm, f);
                        Thread.Sleep(tocDo);
                        Application.DoEvents();
                        f.pictureBox1.Image = bm;
                    }
                }
            }
            else if (y1 > y2)
            {
                if (x1 == x2)
                {
                    while (y1 > y2)
                    {
                        y1 -= 2; if (y1 < y2) y1 = y2;
                        graph.Clear(Color.White);
                        DrawTree(T, graph, bm, f);
                        DrawNode(key, x1, y1, graph, bm, f);
                        Thread.Sleep(tocDo);
                        Application.DoEvents();
                        f.pictureBox1.Image = bm;
                    }
                }
            }
        }

        public void MoveNode(ref TreeAVL T, float[,] PosArr, Graphics graph, Bitmap bm, Form1 f, ref int tocDo)
        {
            bool flag = false;//BIẾN CỜ KIỂM TRA XEM CÁC NODE ĐÃ MOVE HẾT TỚI VỊ TRÍ CHƯA?

            float[] A = new float[PosArr.Length];
            for (int i = 0; i < f.KeyArr.Length; i++)
                A[i] = 80 * 1.5f / Math.Abs(PosArr[i, 3] - PosArr[i, 1]);

            while (!flag)
            {
                int i = 0;
                float[,] B = new float[f.KeyArr.Length, 4];
                f.SavePos(T.root, B, ref i);
                for (int j = 0; j < f.KeyArr.Length; j++)
                {
                    flag = true;
                    for (int k = 0; k < f.KeyArr.Length; k++)
                    {
                        if (B[j, 0] == PosArr[k, 0])
                        {
                            if (B[j, 1] != PosArr[k, 1] || B[j, 2] != PosArr[k, 2] || B[j, 3] != PosArr[k, 3])
                            {
                                flag = false; break;
                            }
                        }
                    }
                    if (flag == false) break;
                }
                ChinhToaDo(ref T.root, PosArr, A, ref flag, f);
                graph.Clear(Color.White);
                DrawTree(T, graph, bm, f);
                Thread.Sleep(tocDo);
                Application.DoEvents();
                f.pictureBox1.Image = bm;
            }

        }

        private void ChinhToaDo(ref Node root, float[,] PosArr, float[] A, ref bool flag, Form1 f)
        {
            for (int i = 0; i < f.KeyArr.Length; i++)
            {
                if (root.key == PosArr[i, 0])
                {
                    //A
                    if (root.x > PosArr[i, 1] && root.y > PosArr[i, 2])
                    {
                        root.x -= 1.5f; root.y -= A[i];
                        if (root.x < PosArr[i, 1])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //B
                    if (root.x == PosArr[i, 1] && root.y > PosArr[i, 2])
                    {
                        root.y -= 1.5f;
                        if (root.y < PosArr[i, 2])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //C
                    if (root.x < PosArr[i, 1] && root.y > PosArr[i, 2])
                    {
                        root.x += 1.5f; root.y -= A[i];
                        if (root.x > PosArr[i, 1])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //D
                    if (root.x > PosArr[i, 1] && root.y == PosArr[i, 2])
                    {
                        root.x -= 1.5f;
                        if (root.x < PosArr[i, 1])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //E
                    if (root.x < PosArr[i, 1] && root.y == PosArr[i, 2])
                    {
                        root.x += 1.5f;
                        if (root.x > PosArr[i, 1])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //F
                    if (root.x > PosArr[i, 1] && root.y < PosArr[i, 2])
                    {
                        root.x -= 1.5f; root.y += A[i];
                        if (root.x < PosArr[i, 1])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //G
                    if (root.x == PosArr[i, 1] && root.y < PosArr[i, 2])
                    {
                        root.y += 1.5f;
                        if (root.y > PosArr[i, 2])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                    //H
                    if (root.x < PosArr[i, 1] && root.y < PosArr[i, 2])
                    {
                        root.x += 1.5f; root.y += A[i];
                        if (root.x > PosArr[i, 1])
                        {
                            root.x = PosArr[i, 1]; root.y = PosArr[i, 2];
                        }
                    }
                }

            }
            if (root.pLeft != null)
                ChinhToaDo(ref root.pLeft, PosArr, A, ref flag, f);
            if (root.pRight != null)
                ChinhToaDo(ref root.pRight, PosArr, A, ref flag, f);
        }
    }
}
