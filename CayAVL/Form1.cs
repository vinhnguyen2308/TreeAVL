using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class Form1 : Form
    {
        int tocDo = 26; // Giá trị để sử dụng cho hàm Thread.Sleep() - tăng giảm tốc độ
        int gtTocDo = 0; // biểu diễn tốc độ cao - thấp hiện tại
        Bitmap bitmap;
        Graphics graph;
        TreeAVL tree;
        Node selectNode;
        DrawTreeAVL draw;
        Stopwatch sw;
        public Label []lb;
        List<int> listvalue=new List<int> ();

        public int[] KeyArr;        // Mảng chứa các key của cây
        public float[,] PosArr, PosArr2;
        public int n;              // Số phần tử của mảng
        public int r = 16; 
        public int R = 16;
        int value;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            graph = Graphics.FromImage(bitmap);
            graph.CompositingQuality = CompositingQuality.HighQuality;
            pictureBox1.Image = bitmap;
            pictureBox1.Show();
            draw = new DrawTreeAVL();
            //  groupPanel2.Enabled = false;
            txbTocDo.Text = gtTocDo.ToString();
        }
        //
        public void GetPos()
        {
            tree.root.x = pictureBox1.Width / 2;
            tree.root.y = 45;
            tree.root.Father = pictureBox1.Width;
            tree.root.GetPos(tree.root.pLeft, this);
            tree.root.GetPos(tree.root.pRight, this);
        }
        //
        public void UpdateDel(int delKey)
        {
            int[] newArr = new int[n - 1];
            for (int i = 0; i < n - 1; i++)
            {
                if (KeyArr[i] != delKey)
                    newArr[i] = KeyArr[i];
            }
            KeyArr = new int[--n];
            KeyArr = newArr;
        }
        //
        public void LoadPos(Node root, float[,] PosArr)
        {
            for (int j = 0; j < KeyArr.Length; j++)
            {
                if (root.key == (int)PosArr[j, 0])
                {
                    root.x = PosArr[j, 1]; root.y = PosArr[j, 2]; //root.Father = PosArr[j, 3];
                    break;
                }
            }
            if (root.pLeft != null)
                LoadPos(root.pLeft, PosArr);
            if (root.pRight != null)
                LoadPos(root.pRight, PosArr);
        }
        //
        public void UpdateAdd(int value)
        {
            n++;
            int[] newArr = new int[n];
            for (int i = 0; i < n - 1; i++)
                newArr[i] = KeyArr[i];
            newArr[n - 1] = value;
            KeyArr = new int[n];
            KeyArr = newArr;
        }
        //
        public void SavePos(Node root, float[,] PosArr, ref int i)
        {
            if (root != null)
            {
                PosArr[i, 0] = (float)root.key; PosArr[i, 1] = root.x; PosArr[i, 2] = root.y; PosArr[i++, 3] = root.Father;
                if (root.pLeft != null)
                    SavePos(root.pLeft, PosArr, ref i);
                if (root.pRight != null)
                    SavePos(root.pRight, PosArr, ref i);
            }
        }
        //
       

            // Thêm node
        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnAddNode_Click(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            int value = Convert.ToInt32(txtValue.Text.ToString());
            listvalue.Add(value);
            //textBox1.Text = textBox1.Text +"   " + value;
            if (tree == null)
            {
                tree = new TreeAVL();
                tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);
                UpdateAdd(value);

                tree.root.x = pictureBox1.Width / 2;
                tree.root.y = 45;
                tree.root.Father = pictureBox1.Width;

                graph.SmoothingMode = SmoothingMode.HighQuality;
                float x = 15, y = 45;
                draw.DrawNode(value, 15, 45, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                while (x < (pictureBox1.Width / 2))
                {
                    x += 2; if (x > pictureBox1.Width / 2) x = pictureBox1.Width / 2;

                    graph.Clear(Color.White);
                    draw.DrawNode(value, x, y, graph, bitmap, this);
                    Thread.Sleep(tocDo * 40);
                    Application.DoEvents();
                    //pictureBox1.Image = bitmap;
                }

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                //pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                //pictureBox1.Image = bitmap;

                
            }
            else
            {
                selectNode = tree.root;
                int i = 0;
                float[,] PosArr = new float[KeyArr.Length + 1, 4];
                SavePos(tree.root, PosArr, ref i);

                int k = tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

                PosArr[i, 0] = (float)value; PosArr[i, 1] = tree.xAdd; PosArr[i, 2] = tree.yAdd; PosArr[i++, 3] = tree.xFatherAdd;

                float[,] PosArr2 = new float[KeyArr.Length + 1, 4];
                if (k != 0)
                {
                    UpdateAdd(value);

                    tree.root.x = pictureBox1.Width / 2;
                    tree.root.y = 45;
                    tree.root.Father = pictureBox1.Width;
                    tree.root.GetPos(tree.root.pLeft, this);
                    tree.root.GetPos(tree.root.pRight, this);

                    i = 0;
                    SavePos(tree.root, PosArr2, ref i);
                    LoadPos(tree.root, PosArr);

                    draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);

                    MessageBox.Show("Thêm thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("node đã tồn tại, vui lòng thêm node khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
            }
            sw.Stop();
            //labelX5.Text = sw.Elapsed.ToString();
        }

        // xóa node
        private void btnDeleteNode_Click(object sender, EventArgs e)
        {
            //sw = new Stopwatch();
            //sw.Start();
            //value = Convert.ToInt32(txtValue.Text.ToString());
           // Delete(value);
           // sw.Stop();
            //labelX5.Text = sw.Elapsed.ToString();
            sw = new Stopwatch();
            sw.Start();
            value = Convert.ToInt32(txtValue.Text.ToString());
            //string s = textBox1.Text;
            //textBox1.Text = s.Replace(txtValue.Text, " ");
            PosArr = new float[KeyArr.Length, 4];
            int i = 0;
            SavePos(tree.root, PosArr, ref i);
            //draw.DrawNode(value, tree.root.x, tree.root.y, graph, bitmap, this);

            tree.delNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

            UpdateDel(value);

            if (tree.root == null)
            {
                graph.Clear(Color.White);
                pictureBox1.Image = bitmap;

            }
            else
            {

                GetPos();

                PosArr2 = new float[KeyArr.Length, 4]; i = 0;
                SavePos(tree.root, PosArr2, ref i);
                LoadPos(tree.root, PosArr);

                draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            sw.Stop();
           // labelX5.Text = sw.Elapsed.ToString();

        }

        private void Delete(int value)
        {
            value = Convert.ToInt32(txtValue.Text.ToString());
            //string s = textBox1.Text;
            //textBox1.Text = s.Replace(txtValue.Text, " ");
            PosArr = new float[KeyArr.Length, 4];
            int i = 0;
            SavePos(tree.root, PosArr, ref i);
            draw.DrawNode(value, tree.root.x, tree.root.y, graph, bitmap, this);

            tree.delNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

            UpdateDel(value);

            if (tree.root == null)
            {
                graph.Clear(Color.White);
                pictureBox1.Image = bitmap;

            }
            else
            {

                GetPos();

                PosArr2 = new float[KeyArr.Length, 4]; i = 0;
                SavePos(tree.root, PosArr2, ref i);
                LoadPos(tree.root, PosArr);

                draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                MessageBox.Show("Xóa thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // tìm node
        //private void btnSearchNode_Click(object sender, EventArgs e)
        //{
        //    sw = new Stopwatch();
        //    sw.Start();
        //    value = Convert.ToInt32(txtValue.Text.ToString());
        //    Node p = tree.SearchNode(value, tree.root, graph, bitmap, draw, tree, this);

        //    graph.Clear(Color.White);
        //    draw.DrawTree(tree, graph, bitmap, this);
        //    pictureBox1.Image = bitmap;

        //    if (p == null)
        //    {
        //        selectNode = tree.root;//GÁN SELECTNODE LÀ ROOT
        //        Thread.Sleep(500);
        //        MessageBox.Show("Node không tồn tại!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        selectNode = tree.root;
        //        draw.DrawSelectNode(tree.root, graph, bitmap, this);
        //    }
        //    else
        //    {
        //        tree = new TreeAVL();


        //        tree.HieuUng(selectNode, draw, graph, bitmap, this);
        //        selectNode = p; // Gan selectNode la Node vua moi tim thay

        //        MessageBox.Show("Đã tìm thấy node!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    sw.Stop();
        //    // labelX5.Text = sw.Elapsed.ToString();
        //    // MessageBox.Show("Đã tìm thấy node!!!","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
        // tạo node
        private void btnCreate_Click(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            CreateNode();
            sw.Stop();
          //  labelX5.Text = sw.Elapsed.ToString();
        }

        private void CreateNode()
        {
            value = Convert.ToInt32(textBoxX1.Text.ToString());
            //textBox1.Text = textBox1.Text + "     " + value;
            listvalue.Add(value);
            if (tree == null)
            {
                tree = new TreeAVL();
                tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);
                UpdateAdd(value);

                tree.root.x = pictureBox1.Width / 2;
                tree.root.y = 45;
                tree.root.Father = pictureBox1.Width;

                graph.SmoothingMode = SmoothingMode.HighQuality;
                float x = 15, y = 45;
                draw.DrawNode(value, 15, 45, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                while (x < (pictureBox1.Width / 2))
                {
                    x += 2; if (x > pictureBox1.Width / 2) x = pictureBox1.Width / 2;

                    graph.Clear(Color.White);
                    draw.DrawNode(value, x, y, graph, bitmap, this);
                    Thread.Sleep(tocDo);
                    Application.DoEvents();
                    pictureBox1.Image = bitmap;
                }

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
            }
            else
            {
                selectNode = tree.root;
                int i = 0;
                float[,] PosArr = new float[KeyArr.Length + 1, 4];
                SavePos(tree.root, PosArr, ref i);

                int k = tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

                PosArr[i, 0] = (float)value; PosArr[i, 1] = tree.xAdd; PosArr[i, 2] = tree.yAdd; PosArr[i++, 3] = tree.xFatherAdd;

                float[,] PosArr2 = new float[KeyArr.Length + 1, 4];

                if (k != 0)
                {
                    UpdateAdd(value);

                    GetPos();

                    i = 0;
                    SavePos(tree.root, PosArr2, ref i);
                    LoadPos(tree.root, PosArr);

                    draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);
                }

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;

            }
            
            //groupPanel2.Enabled = true;
            textBoxX1.Text = "";
        }

       
        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void labelX1_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNode_Click_1(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            int value = Convert.ToInt32(txtValue.Text.ToString());
            listvalue.Add(value);
            //textBox1.Text = textBox1.Text +"   " + value;
            if (tree == null)
            {
                tree = new TreeAVL();
                tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);
                UpdateAdd(value);

                tree.root.x = pictureBox1.Width / 2;
                tree.root.y = 45;
                tree.root.Father = pictureBox1.Width;

                graph.SmoothingMode = SmoothingMode.HighQuality;
                float x = 15, y = 45;
                draw.DrawNode(value, 15, 45, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                while (x < (pictureBox1.Width / 2))
                {
                    x += 2; if (x > pictureBox1.Width / 2) x = pictureBox1.Width / 2;

                    graph.Clear(Color.White);
                    draw.DrawNode(value, x, y, graph, bitmap, this);
                    Thread.Sleep(tocDo);
                    Application.DoEvents();
                    pictureBox1.Image = bitmap;
                }

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;


            }
            else
            {
                selectNode = tree.root;
                int i = 0;
                float[,] PosArr = new float[KeyArr.Length + 1, 4];
                SavePos(tree.root, PosArr, ref i);

                int k = tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

                PosArr[i, 0] = (float)value; PosArr[i, 1] = tree.xAdd; PosArr[i, 2] = tree.yAdd; PosArr[i++, 3] = tree.xFatherAdd;

                float[,] PosArr2 = new float[KeyArr.Length + 1, 4];
                if (k != 0)
                {
                    UpdateAdd(value);

                    tree.root.x = pictureBox1.Width / 2;
                    tree.root.y = 45;
                    tree.root.Father = pictureBox1.Width;
                    tree.root.GetPos(tree.root.pLeft, this);
                    tree.root.GetPos(tree.root.pRight, this);

                    i = 0;
                    SavePos(tree.root, PosArr2, ref i);
                    LoadPos(tree.root, PosArr);

                    draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);

                    MessageBox.Show("Thêm thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Node đã tồn tại, vui lòng thêm Node khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
            }
            sw.Stop();
            //labelX5.Text = sw.Elapsed.ToString();
        }

        private void btnDeleteNode_Click_1(object sender, EventArgs e)
        {
            //sw = new Stopwatch();
            //sw.Start();
            //value = Convert.ToInt32(txtValue.Text.ToString());
            // Delete(value);
            // sw.Stop();
            //labelX5.Text = sw.Elapsed.ToString();
            sw = new Stopwatch();
            sw.Start();
            value = Convert.ToInt32(txtValue.Text.ToString());
            //string s = textBox1.Text;
            //textBox1.Text = s.Replace(txtValue.Text, " ");
            PosArr = new float[KeyArr.Length, 4];
            int i = 0;
            SavePos(tree.root, PosArr, ref i);
            draw.DrawNode(value, tree.root.x, tree.root.y, graph, bitmap, this);

            tree.delNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

            UpdateDel(value);

            if (tree.root == null)
            {
                graph.Clear(Color.White);
                pictureBox1.Image = bitmap;

            }
            else
            {

                GetPos();

                PosArr2 = new float[KeyArr.Length, 4]; i = 0;
                SavePos(tree.root, PosArr2, ref i);
                LoadPos(tree.root, PosArr);

                draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            sw.Stop();
            // labelX5.Text = sw.Elapsed.ToString();
        }

        private void btnNLR_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            preorder(tree.root);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        //private void BtnRandom_Click(object sender, EventArgs e)
        //{
        //    sw = new Stopwatch();
        //    sw.Start();
        //    int n = Convert.ToInt32(textBoxX1.Text.ToString());
        //    Random r = new Random();
        //    for (int j = 0; j < n; j++)
        //    {
        //        int addKey = r.Next(99);
        //        listvalue.Add(addKey);
        //        if (tree == null)
        //        {
        //            tree = new TreeAVL();
        //            tree.insertNode(ref tree.root, addKey, tree, draw, graph, bitmap, this, ref tocDo);
        //            UpdateAdd(addKey);

        //            tree.root.x = pictureBox1.Width / 2;
        //            tree.root.y = 45;
        //            tree.root.Father = pictureBox1.Width;

        //            graph.SmoothingMode = SmoothingMode.HighQuality;
        //            float x = 15, y = 45;
        //            draw.DrawNode(addKey, 15, 45, graph, bitmap, this);
        //            pictureBox1.Image = bitmap;
        //            while (x < (pictureBox1.Width / 2))
        //            {
        //                x += 2; if (x > pictureBox1.Width / 2) x = pictureBox1.Width / 2;

        //                graph.Clear(Color.White);
        //                draw.DrawNode(addKey, x, y, graph, bitmap, this);
        //                Thread.Sleep(tocDo*40);
        //                Application.DoEvents();
        //                pictureBox1.Image = bitmap;
        //            }

        //            graph.Clear(Color.White);
        //            draw.DrawTree(tree, graph, bitmap, this);
        //            pictureBox1.Image = bitmap;

        //            selectNode = tree.root;
        //            draw.DrawSelectNode(selectNode, graph, bitmap, this);
        //            pictureBox1.Image = bitmap;

        //        }
        //        else
        //        {
        //            selectNode = tree.root;
        //            int i = 0;
        //            float[,] PosArr = new float[KeyArr.Length + 1, 4];
        //            SavePos(tree.root, PosArr, ref i);

        //            int k = tree.insertNode(ref tree.root, addKey, tree, draw, graph, bitmap, this, ref tocDo);

        //            PosArr[i, 0] = (float)addKey; PosArr[i, 1] = tree.xAdd; PosArr[i, 2] = tree.yAdd; PosArr[i++, 3] = tree.xFatherAdd;

        //            float[,] PosArr2 = new float[KeyArr.Length + 1, 4];

        //            if (k != 0)
        //            {
        //                UpdateAdd(addKey);

        //                GetPos();

        //                i = 0;
        //                SavePos(tree.root, PosArr2, ref i);
        //                LoadPos(tree.root, PosArr);

        //                draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);
        //            }

        //            graph.Clear(Color.White);
        //            draw.DrawTree(tree, graph, bitmap, this);
        //            pictureBox1.Image = bitmap;

        //            selectNode = tree.root;
        //            draw.DrawSelectNode(selectNode, graph, bitmap, this);
        //            pictureBox1.Image = bitmap;

        //        }
        //    }
        //    sw.Stop();
        //   // labelX5.Text = sw.Elapsed.ToString();
        //   // btnSearchNode.Enabled = true;
        //    btnAddNode.Enabled = true;
        //    btnDeleteNode.Enabled = true;
        //}


        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < listvalue.Count; i++)
        //    {

        //        Delete(listvalue[i]);
        //    }
        //}

        private void inorder(Node root)
        {
            if (root == null)
            {
                return;
            }
            inorder(root.pLeft);
            textBox1.AppendText(root.key + " ");
            inorder(root.pRight);
        }
        private void preorder(Node root)
        {
            if (root == null)
            {
                return;
            }
            textBox1.AppendText(root.key + " ");
            preorder(root.pLeft);
            preorder(root.pRight);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            inorder(tree.root);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            postorder(tree.root);
        }

        private void btnRandom_Click_1(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            int n = Convert.ToInt32(textBoxX1.Text.ToString());
            Random r = new Random();
            for (int j = 0; j < n; j++)
            {
                int addKey = r.Next(99);
                listvalue.Add(addKey);
                if (tree == null)
                {
                    tree = new TreeAVL();
                    tree.insertNode(ref tree.root, addKey, tree, draw, graph, bitmap, this, ref tocDo);
                    UpdateAdd(addKey);

                    tree.root.x = pictureBox1.Width / 2;
                    tree.root.y = 45;
                    tree.root.Father = pictureBox1.Width;

                    graph.SmoothingMode = SmoothingMode.HighQuality;
                    float x = 15, y = 45;
                    draw.DrawNode(addKey, 15, 45, graph, bitmap, this);
                    pictureBox1.Image = bitmap;
                    while (x < (pictureBox1.Width / 2))
                    {
                        x += 2; if (x > pictureBox1.Width / 2) x = pictureBox1.Width / 2;

                        graph.Clear(Color.White);
                        draw.DrawNode(addKey, x, y, graph, bitmap, this);
                        Thread.Sleep(tocDo);
                        Application.DoEvents();
                        pictureBox1.Image = bitmap;
                    }

                    graph.Clear(Color.White);
                    draw.DrawTree(tree, graph, bitmap, this);
                    pictureBox1.Image = bitmap;

                    selectNode = tree.root;
                    draw.DrawSelectNode(selectNode, graph, bitmap, this);
                    pictureBox1.Image = bitmap;

                }
                else
                {
                    selectNode = tree.root;
                    int i = 0;
                    float[,] PosArr = new float[KeyArr.Length + 1, 4];
                    SavePos(tree.root, PosArr, ref i);

                    int k = tree.insertNode(ref tree.root, addKey, tree, draw, graph, bitmap, this, ref tocDo);

                    PosArr[i, 0] = (float)addKey; PosArr[i, 1] = tree.xAdd; PosArr[i, 2] = tree.yAdd; PosArr[i++, 3] = tree.xFatherAdd;

                    float[,] PosArr2 = new float[KeyArr.Length + 1, 4];

                    if (k != 0)
                    {
                        UpdateAdd(addKey);

                        GetPos();

                        i = 0;
                        SavePos(tree.root, PosArr2, ref i);
                        LoadPos(tree.root, PosArr);

                        draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);
                    }

                    graph.Clear(Color.White);
                    draw.DrawTree(tree, graph, bitmap, this);
                    pictureBox1.Image = bitmap;

                    selectNode = tree.root;
                    draw.DrawSelectNode(selectNode, graph, bitmap, this);
                    pictureBox1.Image = bitmap;

                }
            }
            sw.Stop();
            // labelX5.Text = sw.Elapsed.ToString();
            // btnSearchNode.Enabled = true;
            btnAddNode.Enabled = true;
            btnDeleteNode.Enabled = true;
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTangToc_Click(object sender, EventArgs e)
        {
            if (tocDo > 1)
            {
                tocDo -= 5;
                txbTocDo.Text = (++gtTocDo).ToString();
            }
        }

        private void btnGiamToc_Click(object sender, EventArgs e)
        {
            if (tocDo < 26)
            {
                tocDo += 5;
                txbTocDo.Text = (--gtTocDo).ToString();
            }
        }

        private void txbTocDo_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            CreateNode();
            sw.Stop();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show
                ("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (h == DialogResult.OK)
                Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            int value = Convert.ToInt32(txtValue.Text.ToString());
            listvalue.Add(value);
            //textBox1.Text = textBox1.Text +"   " + value;
            if (tree == null)
            {
                tree = new TreeAVL();
                tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);
                UpdateAdd(value);

                tree.root.x = pictureBox1.Width / 2;
                tree.root.y = 45;
                tree.root.Father = pictureBox1.Width;

                graph.SmoothingMode = SmoothingMode.HighQuality;
                float x = 15, y = 45;
                draw.DrawNode(value, 15, 45, graph, bitmap, this);
                pictureBox1.Image = bitmap;
                while (x < (pictureBox1.Width / 2))
                {
                    x += 2; if (x > pictureBox1.Width / 2) x = pictureBox1.Width / 2;

                    graph.Clear(Color.White);
                    draw.DrawNode(value, x, y, graph, bitmap, this);
                    Thread.Sleep(tocDo);
                    Application.DoEvents();
                    pictureBox1.Image = bitmap;
                }

                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;


            }
            else
            {
                selectNode = tree.root;
                int i = 0;
                float[,] PosArr = new float[KeyArr.Length + 1, 4];
                SavePos(tree.root, PosArr, ref i);

                int k = tree.insertNode(ref tree.root, value, tree, draw, graph, bitmap, this, ref tocDo);

                PosArr[i, 0] = (float)value; PosArr[i, 1] = tree.xAdd; PosArr[i, 2] = tree.yAdd; PosArr[i++, 3] = tree.xFatherAdd;

                float[,] PosArr2 = new float[KeyArr.Length + 1, 4];
                if (k != 0)
                {
                    UpdateAdd(value);

                    tree.root.x = pictureBox1.Width / 2;
                    tree.root.y = 45;
                    tree.root.Father = pictureBox1.Width;
                    tree.root.GetPos(tree.root.pLeft, this);
                    tree.root.GetPos(tree.root.pRight, this);

                    i = 0;
                    SavePos(tree.root, PosArr2, ref i);
                    LoadPos(tree.root, PosArr);

                    draw.MoveNode(ref tree, PosArr2, graph, bitmap, this, ref tocDo);

                }
          
                graph.Clear(Color.White);
                draw.DrawTree(tree, graph, bitmap, this);
                pictureBox1.Image = bitmap;

                selectNode = tree.root;
                draw.DrawSelectNode(selectNode, graph, bitmap, this);
                pictureBox1.Image = bitmap;
            }
            sw.Stop();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void postorder(Node root)
        {
            if (root == null)
            {
                return;
            }
            postorder(root.pLeft);
            postorder(root.pRight);
            textBox1.AppendText(root.key + " ");
        }
    }
}
