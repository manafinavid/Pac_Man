using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace Pac_Man
{

    public partial class Form1 : Form
    {
        List<PictureBox> albalo = new List<PictureBox>();
        Data.leaf[] leafs = new Data.leaf[83];
        Bitmap Streets;
        Bitmap Main;
        int old_positon = 0;
        int positon = 0;
        int Score;
        bool WaitForStart = false;
        List<PictureBox> pictureBoxes = new List<PictureBox>();
        bool T = true;
        bool IsOpen = true;
        int selected_option = 0;
        ghost[] ghosts = new ghost[4];
        Color Q = Color.White;
        Color P = Color.White;
        Color Z = Color.White;
        Color Z1, Z2;
        struct ghost
        {
            public int Direction;
            public Point purpos_location;
            public bool isDead;
            public PictureBox image;
        }
        public Form1()
        {
            InitializeComponent();
            {
                pictureBoxes.Add(pictureBox1);
                pictureBoxes.Add(pictureBox2);
                pictureBoxes.Add(pictureBox3);
                pictureBoxes.Add(pictureBox4);
                pictureBoxes.Add(pictureBox5);
                pictureBoxes.Add(pictureBox6);
                pictureBoxes.Add(pictureBox7);
                pictureBoxes.Add(pictureBox8);
            }
            Game.Visible = false;
            panel1.Visible = true;
            panel1.Location = new System.Drawing.Point(73, 153);
            Pacman.Visible = false;
            Data management = new Data();
            management.Write(Application.StartupPath + @"\Data.dat");
            leafs = management.Read(Application.StartupPath + @"\Data.dat");
            Create_map();
            ghosts[0].image = blueghost;
            ghosts[1].image = redghost;
            ghosts[2].image = yellowghost;
            ghosts[3].image = purpleghost;
            albalo.Add(pictureBox9);
            albalo.Add(pictureBox10);
            albalo.Add(pictureBox11);
            albalo.Add(pictureBox12);
            for (int i = 0; i < 4; i++)
            {
                Ps[i] = ghosts[i].image.Location;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (panel1.Visible)
            {
                int old = selected_option;
                switch (e.KeyData)
                {
                    case Keys.Down:
                        if (selected_option == 3)
                        {
                            selected_option = 0;
                        }
                        else
                        {
                            selected_option++;
                        }
                        show_option(old);
                        break;
                    case Keys.Up:
                        if (selected_option == 0)
                        {
                            selected_option = 3;
                        }
                        else
                        {
                            selected_option--;
                        }
                        show_option(old);
                        break;
                    case Keys.Enter:
                        switch (selected_option)
                        {
                            case 0:
                                StartGame();
                                break;
                            case 1:
                                Rank sources = new Rank();
                                sources.ShowDialog();
                                break;
                            case 2:
                                AboutBox1 about = new AboutBox1();
                                about.ShowIcon = false;
                                about.ShowInTaskbar = false;
                                about.ShowDialog();
                                break;
                            case 3:
                                Application.Exit();
                                break;
                        }
                        break;
                }
            }
            else if (WaitForStart)
            {
                if (e.KeyData == Keys.Enter)
                {

                    WaitForStart = false;
                    WaitForStartImage.Visible = false;
                    GameOver.Enabled = true;
                    fps.Enabled = true;
                    CloseAndOpen.Enabled = true;
                    Score = 0;
                    Game.Image = Properties.Resources.main;
                    this.redghost.Location = new System.Drawing.Point(242, 259);
                    this.yellowghost.Location = new System.Drawing.Point(262, 259);
                    this.purpleghost.Location = new System.Drawing.Point(285, 259);
                    this.blueghost.Location = new System.Drawing.Point(221, 259);
                    this.Pacman.Location = new System.Drawing.Point(258, 316);
                    for (int t = 0; t < 4; t++)
                    {
                        albalo[t].Visible = true;
                    }
                    ghosts[0].isDead = true;
                    ghosts[1].isDead = true;
                    ghosts[2].isDead = true;
                    ghosts[3].isDead = true;
                    Main = new Bitmap(Game.Image);
                }
                return;
            }
            else
            {
                switch (e.KeyData)
                {
                    case Keys.Left:
                        positon = 4;
                        break;
                    case Keys.Right:
                        positon = 2;
                        break;
                    case Keys.Up:
                        positon = 1;
                        break;
                    case Keys.Down:
                        positon = 3;
                        break;
                }
            }
        }
        private void StartGame()
        {
            for(int t = 0; t < 4; t++)
            {
                albalo[t].Visible = true;
            }
            Pacman.Location = new Point(258, 316);
            panel1.Visible = false;
            int i = selected_option;
            selected_option = 0;
            show_option(i);
            WaitForStartImage.Location = new System.Drawing.Point(49, 256);
            WaitForStartImage.Visible = true;
            ghosts[0].image.Visible = ghosts[1].image.Visible = ghosts[2].image.Visible = ghosts[3].image.Visible = true;
            Pacman.Visible = true;
            Game.Image = Properties.Resources.main;
            Score = 0;
            WaitForStart = true;
            Game.Visible = true;
            Streets = new Bitmap(Properties.Resources.Steers);
            Main = new Bitmap(Game.Image);
            GameOver.Enabled = true;
            ghosts[0].image.Location = new Point(221, 259);
            ghosts[1].image.Location = new Point(242, 259);
            ghosts[2].image.Location = new Point(262, 259);
            ghosts[3].image.Location = new Point(285, 259);
            path_g.Clear();
            path_g.Add(new List<int>());
            path_g.Add(new List<int>());
            path_g.Add(new List<int>());
            path_g.Add(new List<int>());
        }
        void show_option(int old)
        {
            pictureBoxes[old * 2].Visible = pictureBoxes[old * 2 + 1].Visible = false;
            pictureBoxes[selected_option * 2].Visible = pictureBoxes[selected_option * 2 + 1].Visible = true;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Game.Visible = false;
            panel1.Visible = true;
            panel1.Location = new System.Drawing.Point(73, 153);
        }
        private void CloseAndOpen_Tick(object sender, EventArgs e)
        {
            CloseAndOpen.Enabled = false;
            if (IsOpen)
            {
                Pacman.Image = closepacman;
            }
            else
            {
                Pacman.Image = openpacman;
            }
            IsOpen = !IsOpen;
            CloseAndOpen.Enabled = true;
        }
        void move()
        {
            switch (old_positon)
            {
                case 1:
                    Pacman.Location = new Point(Pacman.Location.X, Pacman.Location.Y - 1);
                    break;
                case 3:
                    Pacman.Location = new Point(Pacman.Location.X, Pacman.Location.Y + 1);
                    break;
                case 2:
                    Pacman.Location = new Point(Pacman.Location.X + 1, Pacman.Location.Y);
                    break;
                case 4:
                    Pacman.Location = new Point(Pacman.Location.X - 1, Pacman.Location.Y);
                    break;
            }
        }
        Image openpacman = Properties.Resources.packmanopen;
        Image closepacman = Properties.Resources.packmanclose;
        void turn()
        {
            if (old_positon != positon)
            {
                openpacman = Properties.Resources.packmanopen;
                closepacman = Properties.Resources.packmanclose;
                switch (positon)
                {
                    case 1:
                        openpacman.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        closepacman.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case 2:
                        break;
                    case 3:
                        openpacman.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        closepacman.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 4:
                        openpacman.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        closepacman.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        openpacman.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        closepacman.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        break;
                }
            }

        }
        private void GameOver_Tick(object sender, EventArgs e)
        {
            GameOver.Enabled = false;
            for (int i = 0; i < 540; i++)
            {
                for (int j = 0; j < 592; j++)
                {
                    if (Main.GetPixel(i, j).Name == "ffffdad5")
                    {
                        GameOver.Enabled = true;
                        return;
                    }
                }
            }
            GameOver.Enabled = false;
            EndGame();
        }
        private void EndGame()
        {

            GameOver.Enabled = false;
            fps.Enabled = false;
            CloseAndOpen.Enabled = false;
            MessageBox.Show("Game Over \nScore=" + Score);
            if(int.Parse( Properties.Settings.Default.Score)<Score)
            if(MessageBox.Show("Do you want to save this score?","New High Score", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
             fuckyou:       string player_name= Interaction.InputBox("Enter your name:");
                    if (player_name == "")
                    {
                        goto fuckyou;
                    }
                    SqlConnection sql = new SqlConnection(Properties.Settings.Default.DataConnectionString);
                    SqlCommand command = new SqlCommand("INSERT INTO Data ( player , score ) VALUES (@p,@s)", sql);
                    command.Parameters.AddWithValue("@p", player_name);
                    command.Parameters.AddWithValue("@s", Score);
                    Properties.Settings.Default.Score = Score.ToString();
                    Properties.Settings.Default.Save();
                    sql.Open();
                    command.ExecuteNonQuery();
                    sql.Close();
            }
            WaitForStart = true;
            WaitForStartImage.Visible = true;
        } 
        bool check(int pos)
        {
            switch (pos)
            {
                case 1:
                    Q = Streets.GetPixel(Pacman.Location.X + 16, Pacman.Location.Y + 13);
                    P = Streets.GetPixel(Pacman.Location.X + 18, Pacman.Location.Y + 13);
                    Z = Main.GetPixel(Pacman.Location.X + 15, Pacman.Location.Y + 13);
                    Z1 = Main.GetPixel(Pacman.Location.X + 16, Pacman.Location.Y + 13);
                    Z2 = Main.GetPixel(Pacman.Location.X + 17, Pacman.Location.Y + 13);
                    if (Q.Name != P.Name)
                    {
                        return false;
                    }
                    if (Z.Name == "ff000000" || Z.Name == "ff0080f8")
                    {
                        T = true;
                    }
                    if ((Z.Name == "ffffdad5" || Z1.Name == "ffffdad5" || Z2.Name == "ffffdad5") && T)
                    {
                        T = false;
                        Score++;
                        Text = "Pac-Man Score : " + Score;
                        for (int x = 0; x < 30; x++)
                        {
                            for (int y = -2; y < 30; y++)
                            {
                                Main.SetPixel(Pacman.Location.X + x, Pacman.Location.Y + y, Color.Black);
                            }
                        }
                        Game.Image = Main;
                    }
                    if (Q.Name == "ff22b14c")
                    {
                        return true;
                    }
                    break;
                case 3:
                    Q = Streets.GetPixel(Pacman.Location.X + 16, Pacman.Location.Y + 21);
                    P = Streets.GetPixel(Pacman.Location.X + 18, Pacman.Location.Y + 21);
                    Z = Main.GetPixel(Pacman.Location.X + 15, Pacman.Location.Y + 21);
                    Z1 = Main.GetPixel(Pacman.Location.X + 16, Pacman.Location.Y + 21);
                    Z2 = Main.GetPixel(Pacman.Location.X + 17, Pacman.Location.Y + 21);
                    if (Q.Name != P.Name)
                    {
                        return false;
                    }
                    if (Z.Name == "ff000000" || Z.Name == "ff0080f8")
                    {
                        T = true;
                    }
                    if ((Z.Name == "ffffdad5" || Z1.Name == "ffffdad5" || Z2.Name == "ffffdad5") && T)
                    {
                        T = false;
                        Score++;
                        Text = "Pac-Man Score : " + Score;
                        for (int x = 0; x < 30; x++)
                        {
                            for (int y = 0; y < 30; y++)
                            {
                                Main.SetPixel(Pacman.Location.X + x, Pacman.Location.Y + y, Color.Black);
                            }
                        }
                        Game.Image = Main;
                    }
                    if (Q.Name == "ff22b14c")
                    {
                        return true;
                    }
                    break;
                case 2:
                    Q = Streets.GetPixel(Pacman.Location.X + 21, Pacman.Location.Y + 16);
                    P = Streets.GetPixel(Pacman.Location.X + 21, Pacman.Location.Y + 18);
                    Z = Main.GetPixel(Pacman.Location.X + 37, Pacman.Location.Y + 17);
                    Z1 = Main.GetPixel(Pacman.Location.X + 37, Pacman.Location.Y + 16);
                    Z2 = Main.GetPixel(Pacman.Location.X + 37, Pacman.Location.Y + 18);
                    if (Q.Name != P.Name)
                    {
                        return false;
                    }
                    if (Z.Name == "ff000000" || Z.Name == "ff0080f8")
                    {
                        T = true;
                    }
                    if ((Z.Name == "ffffdad5" || Z1.Name == "ffffdad5" || Z2.Name == "ffffdad5") && T)
                    {
                        T = false;
                        Score++;
                        Text = "Pac-Man Score : " + Score;
                        for (int x = 0; x < 30; x++)
                        {
                            for (int y = 0; y < 30; y++)
                            {
                                Main.SetPixel(Pacman.Location.X + x, Pacman.Location.Y + y, Color.Black);
                            }
                        }
                        Game.Image = Main;
                    }
                    if (Q.Name == "ff22b14c")
                    {
                        return true;
                    }
                    break;
                case 4:
                    Q = Streets.GetPixel(Pacman.Location.X + 13, Pacman.Location.Y + 16);
                    P = Streets.GetPixel(Pacman.Location.X + 13, Pacman.Location.Y + 18);
                    Z = Main.GetPixel(Pacman.Location.X + 13, Pacman.Location.Y + 16);
                    Z1 = Main.GetPixel(Pacman.Location.X + 13, Pacman.Location.Y + 17);
                    Z2 = Main.GetPixel(Pacman.Location.X + 13, Pacman.Location.Y + 18);
                    if (Q.Name != P.Name)
                    {
                        return false;
                    }
                    if (Z.Name == "ff000000" || Z.Name == "ff0080f8")
                    {
                        T = true;
                    }
                    if ((Z.Name == "ffffdad5" || Z1.Name == "ffffdad5" || Z2.Name == "ffffdad5") && T)
                    {
                        T = false;
                        Score++;
                        Text = "Pac-Man Score : " + Score;
                        for (int x = -2; x < 30; x++)
                        {
                            for (int y = 0; y < 30; y++)
                            {
                                Main.SetPixel(Pacman.Location.X + x, Pacman.Location.Y + y, Color.Black);

                            }
                        }
                        Game.Image = Main;
                    }
                    if (Q.Name == "ff22b14c")
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        bool TTTT = false;
        Point[] Ps = new Point[4];
        private void fps_Tick(object sender, EventArgs e)
        {
            fps.Enabled = false;
            {
                fps.Enabled = false;
                if (check(positon))
                {
                    if (old_positon != positon)
                    {
                        turn();
                        old_positon = positon;
                    }
                    move();
                    for (int i = 0; i < 4; i++)
                    {
                        if (Math.Abs(Pacman.Location.X - albalo[i].Location.X) < 25 && Math.Abs(Pacman.Location.Y - albalo[i].Location.Y) < 25 && albalo[i].Visible == true)
                        {
                            albalo[i].Visible = false;
                            albalo[i].Tag = 20;
                            ball.Enabled = true;
                            ghosts[0].image.Image = ghosts[1].image.Image = ghosts[2].image.Image = ghosts[3].image.Image = Properties.Resources.white_ghost;
                            TTTT = true;
                        }
                        if (TTTT)
                        {
                            if (Math.Abs(Pacman.Location.X - ghosts[i].image.Location.X) < 25 && Math.Abs(Pacman.Location.Y - ghosts[i].image.Location.Y) < 25)
                            {
                                Score += 200;
                                Text = "Pac-Man Score : " + Score;
                                ghosts[i].isDead = true;
                                ghosts[i].image.Visible = false;
                                ghosts[i].image.Location = Ps[i];
                            }
                        }
                    }
                }
                else if (check(old_positon))
                {
                    move();
                }
                fps.Enabled = true;
            }
            {
                for (int i = 0; i < 4; i++)
                {
                    if (ghosts[i].isDead)
                    {
                        ghosts[i].image.Visible = true;
                        switch (i + 1)
                        {
                            case 1:
                                if (blueghost.Location.X == 253)
                                {
                                    if (blueghost.Location.Y == 203)
                                    {
                                        ghosts[i].isDead = false;
                                        ghosts[i].purpos_location = new Point(Tree[31].x - 17, Tree[31].y - 17);
                                    }
                                    else
                                        blueghost.Location = new Point(blueghost.Location.X, blueghost.Location.Y - 1);
                                }
                                else
                                    blueghost.Location = new Point(blueghost.Location.X + 1, blueghost.Location.Y);
                                break;
                            case 2:
                                if (redghost.Location.X == 253)
                                {
                                    if (redghost.Location.Y == 203)
                                    {
                                        ghosts[i].isDead = false;
                                        ghosts[i].purpos_location = new Point(Tree[31].x - 17, Tree[31].y - 17);
                                    }
                                    else
                                        redghost.Location = new Point(redghost.Location.X, redghost.Location.Y - 1);
                                }
                                else
                                    redghost.Location = new Point(redghost.Location.X + 1, redghost.Location.Y);
                                break;
                            case 3:
                                if (yellowghost.Location.X == 253)
                                {
                                    if (yellowghost.Location.Y == 203)
                                    {
                                        ghosts[i].isDead = false;
                                        ghosts[i].purpos_location = new Point(Tree[31].x - 17, Tree[31].y - 17);
                                    }
                                    else
                                        yellowghost.Location = new Point(yellowghost.Location.X, yellowghost.Location.Y - 1);
                                }
                                else
                                    yellowghost.Location = new Point(yellowghost.Location.X - 1, yellowghost.Location.Y);
                                break;
                            case 4:
                                if (purpleghost.Location.X == 253)
                                {
                                    if (purpleghost.Location.Y == 203)
                                    {
                                        ghosts[i].isDead = false;
                                        ghosts[i].purpos_location = new Point(Tree[31].x - 17, Tree[31].y - 17);
                                    }
                                    else
                                        purpleghost.Location = new Point(purpleghost.Location.X, purpleghost.Location.Y - 1);
                                }
                                else
                                    purpleghost.Location = new Point(purpleghost.Location.X - 1, purpleghost.Location.Y);
                                break;
                        }
                    }
                }
            }
            {
                for (int i = 0; i < 4; i++)
                {
                    if (ghosts[i].isDead == false)
                    {
                        if (ghosts[i].image.Location == ghosts[i].purpos_location)
                        {
                            if (path_g[i].Count == 1)
                            {
                                int b;
                            f: b = Rn.Next() % 83;
                                if (Math.Abs(b - path_g[i][0]) > 15)
                                {
                                    goto f;
                                }
                                List<int> vs = findway(path_g[i][0], b);
                                if (vs == null)
                                {
                                    goto f;
                                }
                                path_g[i].AddRange(vs);
                            }
                            path_g[i].RemoveAt(0);
                            ghosts[i].purpos_location = new Point(Tree[path_g[i][0]].x - 17, Tree[path_g[i][0]].y - 17);
                            ghosts[i].Direction = 0;
                        }
                        else
                        {
                            switch (ghosts[i].Direction)
                            {
                                case 0:
                                    if (ghosts[i].image.Location.X < ghosts[i].purpos_location.X)
                                    {
                                        ghosts[i].Direction = 2;
                                    }
                                    else if (ghosts[i].image.Location.X > ghosts[i].purpos_location.X)
                                    {
                                        ghosts[i].Direction = 4;
                                    }
                                    if (ghosts[i].image.Location.Y < ghosts[i].purpos_location.Y)
                                    {
                                        ghosts[i].Direction = 3;
                                    }
                                    else if (ghosts[i].image.Location.Y > ghosts[i].purpos_location.Y)
                                    {
                                        ghosts[i].Direction = 1;
                                    }
                                    break;
                                case 1:
                                    ghosts[i].image.Location = new Point(ghosts[i].image.Location.X, ghosts[i].image.Location.Y - 1);
                                    break;
                                case 2:
                                    ghosts[i].image.Location = new Point(ghosts[i].image.Location.X + 1, ghosts[i].image.Location.Y);
                                    break;
                                case 3:
                                    ghosts[i].image.Location = new Point(ghosts[i].image.Location.X, ghosts[i].image.Location.Y + 1);
                                    break;
                                case 4:
                                    ghosts[i].image.Location = new Point(ghosts[i].image.Location.X - 1, ghosts[i].image.Location.Y);
                                    break;

                            }
                        }
                    }
                    else
                    {
                        path_g[i].Clear();
                        path_g[i].Add(31);
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (Math.Abs(Pacman.Location.X - ghosts[i].image.Location.X) < 27 && Math.Abs(Pacman.Location.Y - ghosts[i].image.Location.Y) < 27 && !TTTT)
                {
                    fps.Enabled = false;
                    EndGame();
                    return;
                }
            }
            fps.Enabled = true;
        }
        public struct LEAF
        {
            public int x, y;
            public int up, down, left, right;

        }
        public List<LEAF> Tree = new List<LEAF>();
        public List<List<int>> path_g = new List<List<int>>();
        public void Create_map()
        {
            int y = leafs[0].y;

            for (int i = 0; i < 83; i++)
            {
                int o = 0;
                o = i;
                LEAF T = new LEAF() { down = -1, up = -1, left = -1, right = -1, x = leafs[i].x, y = leafs[i].y };
                if (leafs[i].left)
                {
                    var temp2 = Tree[i - 1];
                    temp2.right = i;
                    Tree[i - 1] = temp2;
                    T.left = i - 1;
                }
                if (leafs[i].up)
                {
                    int H = -1;
                    for (int c = 0; c < Tree.Count - 1; c++)
                    {
                        if (Tree[c].x == T.x)
                        {
                            H = c;
                        }
                    }
                    if (H != -1)
                    {
                        var temp2 = Tree[H];
                        temp2.down = i;
                        T.up = H;
                        Tree[H] = temp2;
                    }
                }
                Tree.Add(T);
            }
            leafs = null;
            return;
        }
        List<int> findway(int startposition, int endposition)
        {
            List<int> vs = new List<int>();
            vs.Add(startposition);
            invoid = 0;
            List<int> vs1;
            start_ = startposition;
            end_ = endposition;
            vs1 = findway(vs, endposition);
            return vs1;
        }
        int start_, end_;
        int lastmove = 0;
        bool ban = false;
        int bad_choice = -2;
        Random Rn = new Random();
        private void ball_Tick(object sender, EventArgs e)
        {
            bool OO = false ;
            for (int i = 0; i < 4; i++)
            {
                if (albalo[i].Visible == false)
                {
                    if ((int)albalo[i].Tag != 0)
                    {
                        OO = true;
                        albalo[i].Tag = (int)albalo[i].Tag - 1;
                        if ((int)albalo[i].Tag == 0)
                        {
                            redghost.Image = Properties.Resources.red_ghost;
                            yellowghost.Image = Properties.Resources.yellow_ghost;
                            blueghost.Image = Properties.Resources.blue_ghost;
                            purpleghost.Image = Properties.Resources.Purple_ghost;
                        }
                    }
                }
            }
            if (!OO)
            {
                TTTT =false;
                ball.Enabled = false;
            }
        }

        int invoid = 0;
        List<int> findway(List<int> result, int purpos)
        {
            bool bug = false;
            int p = 0;
        start:
            if (invoid > 40)
            {
                return null;
            }
            invoid++;
            if (result.Count - 1 > 2)
            {
                if (result[result.Count - 1] == result[result.Count - 1 - 2])
                {
                    bad_choice = result[result.Count - 1 - 1];
                    result.RemoveAt(result.Count - 1 - 1);
                    result.RemoveAt(result.Count - 1 - 1);
                }
            }
            if (result[result.Count - 1] != purpos)
            {
                if (Tree[result[result.Count - 1]].down != -1 && Tree[result[result.Count - 1]].down != bad_choice)
                {
                    int i = 0, j = int.MaxValue;
                    i = Math.Abs(Tree[Tree[result[result.Count - 1]].down].y - Tree[purpos].y);
                    if (result.Count - 1 != 0)
                    {
                        j = Math.Abs(Tree[result[result.Count - 1]].y - Tree[purpos].y);
                    }
                    else
                    {
                        j = Math.Abs(Tree[start_].y - Tree[purpos].y);
                    }
                    if ((i < j && !(ban && lastmove == 2) || (bug == true && lastmove != 1)))
                    {
                        if ((ban && lastmove == 2))
                        {
                            ban = false;
                        }
                        List<int> T = new List<int>();
                    a: try
                        {
                            T.AddRange(result.ToArray());
                        }
                        catch (Exception ex)
                        {
                            goto a;
                        }
                        T.Add(Tree[result[result.Count - 1]].down);
                        p++;
                        lastmove = 1;
                        List<int> vs = findway(T, purpos);
                        T = null; if (vs == null) return null;
                        if (vs[vs.Count - 1] == purpos)
                        {
                            return vs;
                        }
                    }
                }
                if (Tree[result[result.Count - 1]].up != -1 && Tree[result[result.Count - 1]].up != bad_choice)
                {
                    int i = 0, j = int.MaxValue;
                    i = Math.Abs(Tree[Tree[result[result.Count - 1]].up].y - Tree[purpos].y);
                    if (result.Count - 1 != 0)
                    {
                        j = Math.Abs(Tree[result[result.Count - 1]].y - Tree[purpos].y);
                    }
                    else
                    {
                        j = Math.Abs(Tree[start_].y - Tree[purpos].y);
                    }
                    if (i < j && !(ban && lastmove == 1) || (bug == true && lastmove != 2))
                    {
                        if ((ban && lastmove == 1))
                        {
                            ban = false;
                        }
                        List<int> T = new List<int>();
                    a: try
                        {
                            T.AddRange(result.ToArray());
                        }
                        catch (Exception ex)
                        {
                            goto a;
                        }

                        T.Add(Tree[result[result.Count - 1]].up);
                        p++;
                        lastmove = 2;
                        List<int> vs = findway(T, purpos);
                        T = null; if (vs == null) return null;
                        if (vs[vs.Count - 1] == purpos)
                        {
                            return vs;
                        }

                    }
                }
                if (Tree[result[result.Count - 1]].right != -1 && Tree[result[result.Count - 1]].right != bad_choice)
                {
                    int i = 0, j = int.MaxValue;
                    i = Math.Abs(Tree[Tree[result[result.Count - 1]].right].x - Tree[purpos].x);
                    if (result.Count - 1 != 0)
                    {
                        j = Math.Abs(Tree[result[result.Count - 1]].x - Tree[purpos].x);
                    }
                    else
                    {
                        j = Math.Abs(Tree[start_].x - Tree[purpos].x);
                    }
                    if (i < j && !(ban && lastmove == 4) || (bug == true && lastmove != 3))
                    {
                        if ((ban && lastmove == 4))
                        {
                            ban = false;
                        }
                        List<int> T = new List<int>();
                    a: try
                        {
                            T.AddRange(result.ToArray());
                        }
                        catch (Exception ex)
                        {
                            goto a;
                        }
                        T.Add(Tree[result[result.Count - 1]].right);
                        p++;
                        lastmove = 3;
                        List<int> vs = findway(T, purpos);
                        T = null; if (vs == null) return null;
                        if (vs[vs.Count - 1] == purpos)
                        {
                            return vs;
                        }
                    }
                }
                if (Tree[result[result.Count - 1]].left != -1 && Tree[result[result.Count - 1]].left != bad_choice)
                {
                    int i = 0, j = int.MaxValue;
                    i = Math.Abs(Tree[Tree[result[result.Count - 1]].left].x - Tree[purpos].x);
                    if (result.Count - 1 != 0)
                    {
                        j = Math.Abs(Tree[result[result.Count - 1]].x - Tree[purpos].x);
                    }
                    else
                    {
                        j = Math.Abs(Tree[start_].x - Tree[purpos].x);
                    }
                    if (i < j && !(ban && lastmove == 3) || (bug == true && lastmove != 4))
                    {
                        if ((ban && lastmove == 3))
                        {
                            ban = false;
                        }
                        List<int> T = new List<int>();
                    a: try
                        {
                            T.AddRange(result.ToArray());
                        }
                        catch (Exception ex)
                        {
                            goto a;
                        }
                        T.Add(Tree[result[result.Count - 1]].left);
                        p++;
                        lastmove = 4;
                        List<int> vs = findway(T, purpos);
                        T = null; if (vs == null) return null;
                        if (vs[vs.Count - 1] == purpos)
                        {
                            return vs;
                        }
                    }
                }
                if (p == 0)
                {
                    ban = true;
                    bug = true;
                    goto start;
                }

            }
            return result;
        }
    }
    class Data
    {
        public struct leaf
        {
            public int x;
            public int y;
            public bool up;
            public bool down;
            public bool left;
            public bool right;
        }
        Bitmap leafmap = new Bitmap(Properties.Resources.leef);
        public leaf[] leafs = new leaf[83];
        void find_leafs()
        {
            string Brown = Properties.Resources.brown.GetPixel(0, 0).Name;
            int x = 0;
            string wall = Properties.Resources.wall.GetPixel(0, 0).Name;

            for (int j = 0; j < 592; j++)
            {
                for (int i = 0; i < 540; i++)
                {
                    if (leafmap.GetPixel(i, j).Name == Brown)
                    {
                        leafs[x].x = i;
                        leafs[x].y = j;
                        bool a = true, b = true, c = true, d = true;
                        string temp;
                        for (int z = 1; z < 200; z++)
                        {
                            if (a && (z + i < 540))
                            {
                                temp = leafmap.GetPixel(i + z, j).Name;
                                if (temp == wall)
                                {
                                    leafs[x].right = false;
                                    a = false;
                                }
                                else if (temp == Brown)
                                {
                                    leafs[x].right = true;
                                    a = false;
                                }
                            }
                            if (b && (z < i))
                            {
                                temp = leafmap.GetPixel(i - z, j).Name;
                                if (temp == wall)
                                {
                                    leafs[x].left = false;
                                    b = false;
                                }
                                else if (temp == Brown)
                                {
                                    leafs[x].left = true;
                                    b = false;
                                }
                            }
                            if (c && (z < j))
                            {
                                temp = leafmap.GetPixel(i, j - z).Name;
                                if (temp == wall)
                                {
                                    leafs[x].up = false;
                                    c = false;
                                }
                                else if (temp == Brown)
                                {
                                    leafs[x].up = true;
                                    c = false;
                                }
                            }
                            if (d && (j + z < 592))
                            {
                                temp = leafmap.GetPixel(i, j + z).Name;
                                if (temp == wall)
                                {
                                    leafs[x].down = false;
                                    d = false;
                                }
                                else if (temp == Brown)
                                {
                                    leafs[x].down = true;
                                    d = false;
                                }
                            }
                            if (!(a || b || c || d) || z == 199)
                            {
                                x++;
                                z = 2001;
                            }
                        }
                    }
                }
            }
        }
        void write_data(string PATH)
        {
            using (Stream stream = new FileStream(PATH, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < 83; i++)
                    {
                        byte[] newBuffer;
                        newBuffer = getBytes(leafs[i]);
                        writer.Write(newBuffer);
                    }
                }
            }
        }
        byte[] getBytes(leaf str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
        public void Write(String SaveingPath)
        {
            find_leafs();
            write_data(SaveingPath);
        }
        public leaf[] Read(string SavedPath)
        {
            FileStream stream = new FileStream(SavedPath, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream, Encoding.Default);
            byte[] inputData = reader.ReadBytes((int)stream.Length);
            int i;
            leaf[] leafs = new leaf[83];
            for (i = 0; i < 1992; i += 24)
            {
                IntPtr ptr = Marshal.AllocHGlobal(24);
                Marshal.Copy(inputData, i, ptr, 24);
                leafs[i / 24] = (leaf)Marshal.PtrToStructure(ptr, typeof(leaf));
            }
            return leafs;
        }
    }
}
