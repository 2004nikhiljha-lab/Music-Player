using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ModernMusicPlayer
{
    public class ModernMusicPlayerForm : Form
    {
        private Panel sidePanel;
        private Panel mainPanel;
        private Panel playerPanel;
        private Label titleLabel;
        private Timer animationTimer;
        private float progress = 0;
        private int selectedIndex = 0;
        private string[] menuItems = { "Home", "Discover", "Library", "Playlists", "Settings" };
        private string[] songs = { "Midnight Echoes", "Neon Dreams", "Electric Pulse", "Cosmic Wave" };
        private Button playButton;
        private bool isPlaying = false;

        public ModernMusicPlayerForm()
        {
            InitializeComponents();
            SetupUI();
        }

        private void InitializeComponents()
        {
            this.Text = "Modern Music Player";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#0a0e27");
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;

            animationTimer = new Timer();
            animationTimer.Interval = 50;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void SetupUI()
        {
            sidePanel = new Panel
            {
                Width = 250,
                Dock = DockStyle.Left,
                BackColor = ColorTranslator.FromHtml("#121738")
            };
            sidePanel.Paint += SidePanel_Paint;

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#0a0e27")
            };
            mainPanel.Paint += MainPanel_Paint;

            playerPanel = new Panel
            {
                Height = 100,
                Dock = DockStyle.Bottom,
                BackColor = ColorTranslator.FromHtml("#1a1f3a")
            };
            playerPanel.Paint += PlayerPanel_Paint;

            playButton = new Button
            {
                Size = new Size(60, 60),
                Location = new Point(570, 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorTranslator.FromHtml("#6C5CE7"),
                ForeColor = Color.White,
                Text = "▶",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            playButton.FlatAppearance.BorderSize = 0;
            playButton.Click += PlayButton_Click;
            playerPanel.Controls.Add(playButton);

            Button closeBtn = new Button
            {
                Text = "✕",
                Size = new Size(40, 40),
                Location = new Point(this.Width - 45, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14),
                Cursor = Cursors.Hand
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => Application.Exit();
            this.Controls.Add(closeBtn);

            this.Controls.Add(mainPanel);
            this.Controls.Add(sidePanel);
            this.Controls.Add(playerPanel);

            for (int i = 0; i < menuItems.Length; i++)
            {
                int index = i;
                Panel menuItem = CreateMenuItem(menuItems[i], 50 + i * 60, index);
                sidePanel.Controls.Add(menuItem);
            }
        }

        private Panel CreateMenuItem(string text, int y, int index)
        {
            Panel item = new Panel
            {
                Size = new Size(230, 50),
                Location = new Point(10, y),
                BackColor = index == selectedIndex ? ColorTranslator.FromHtml("#6C5CE7") : Color.Transparent,
                Cursor = Cursors.Hand
            };

            Label lbl = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = false,
                Size = new Size(200, 50),
                Location = new Point(15, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            item.Controls.Add(lbl);
            item.Click += (s, e) => { selectedIndex = index; this.Invalidate(true); };
            lbl.Click += (s, e) => { selectedIndex = index; this.Invalidate(true); };

            return item;
        }

        private void SidePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Label logo = new Label
            {
                Text = "♪ PULSE",
                ForeColor = ColorTranslator.FromHtml("#6C5CE7"),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            if (!sidePanel.Controls.Contains(logo))
                sidePanel.Controls.Add(logo);
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawString("Trending Now", new Font("Segoe UI", 24, FontStyle.Bold), 
                Brushes.White, new PointF(40, 30));

            for (int i = 0; i < songs.Length; i++)
            {
                DrawSongCard(g, songs[i], 40 + (i % 2) * 520, 100 + (i / 2) * 220, i);
            }
        }

        private void DrawSongCard(Graphics g, string title, int x, int y, int index)
        {
            Rectangle cardRect = new Rectangle(x, y, 480, 180);
            
            using (LinearGradientBrush brush = new LinearGradientBrush(
                cardRect, 
                ColorTranslator.FromHtml("#1a1f3a"),
                ColorTranslator.FromHtml("#2d3561"),
                LinearGradientMode.Horizontal))
            {
                g.FillRoundedRectangle(brush, cardRect, 20);
            }

            Color[] colors = { 
                ColorTranslator.FromHtml("#FF6B6B"),
                ColorTranslator.FromHtml("#4ECDC4"),
                ColorTranslator.FromHtml("#FFD93D"),
                ColorTranslator.FromHtml("#6C5CE7")
            };

            Rectangle albumRect = new Rectangle(x + 20, y + 20, 140, 140);
            using (SolidBrush albumBrush = new SolidBrush(colors[index]))
            {
                g.FillRoundedRectangle(albumBrush, albumRect, 15);
            }

            g.DrawString(title, new Font("Segoe UI", 16, FontStyle.Bold),
                Brushes.White, new PointF(x + 180, y + 30));
            
            g.DrawString("Artist Name", new Font("Segoe UI", 11),
                new SolidBrush(ColorTranslator.FromHtml("#8892B2")), new PointF(x + 180, y + 65));
            
            g.DrawString("3:45", new Font("Segoe UI", 10),
                new SolidBrush(ColorTranslator.FromHtml("#8892B2")), new PointF(x + 180, y + 95));

            Rectangle playRect = new Rectangle(x + 400, y + 130, 40, 40);
            using (SolidBrush playBrush = new SolidBrush(ColorTranslator.FromHtml("#6C5CE7")))
            {
                g.FillEllipse(playBrush, playRect);
            }
            g.DrawString("▶", new Font("Segoe UI", 12), Brushes.White, 
                new PointF(x + 413, y + 137));
        }

        private void PlayerPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawString("Now Playing: Midnight Echoes", new Font("Segoe UI", 12, FontStyle.Bold),
                Brushes.White, new PointF(20, 15));
            
            g.DrawString("Artist Name", new Font("Segoe UI", 9),
                new SolidBrush(ColorTranslator.FromHtml("#8892B2")), new PointF(20, 40));

            Rectangle progressBar = new Rectangle(20, 70, this.Width - 40, 6);
            using (SolidBrush bgBrush = new SolidBrush(ColorTranslator.FromHtml("#2d3561")))
            {
                g.FillRoundedRectangle(bgBrush, progressBar, 3);
            }

            Rectangle progressFill = new Rectangle(20, 70, (int)((this.Width - 40) * progress), 6);
            using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                progressFill,
                ColorTranslator.FromHtml("#6C5CE7"),
                ColorTranslator.FromHtml("#A29BFE"),
                LinearGradientMode.Horizontal))
            {
                g.FillRoundedRectangle(progressBrush, progressFill, 3);
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            isPlaying = !isPlaying;
            playButton.Text = isPlaying ? "⏸" : "▶";
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                progress += 0.005f;
                if (progress > 1) progress = 0;
            }
            playerPanel.Invalidate();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModernMusicPlayerForm());
        }
    }

    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }
    }
}