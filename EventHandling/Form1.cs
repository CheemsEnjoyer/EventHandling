using EventHandling.objects;
using System;
using static System.Formats.Asn1.AsnWriter;
namespace EventHandling
{
    public partial class Form1 : Form
    {
        MyRectangle myRect;
        List<BaseObject> objects = new();
        Player player;
        Goal goal;
        Goal goal1;
        Marker marker;
        private Random random = new Random();
        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] ����� ��������� � {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            timer1.Tick += TimerTick;
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            goal = new Goal(pbMain.Width / 2, pbMain.Height / 2, 0);
            goal1 = new Goal(pbMain.Width / 2, pbMain.Height / 2, 0);
            goal.TimerEnded += HandleTimerEnd;
            goal1.TimerEnded += HandleTimerEnd;
            objects.Add(marker);
            objects.Add(player);
            objects.Add(goal);
            objects.Add(goal1);
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            updatePlayer();
            UpdateScoreDisplay();
            // ������������� �����������
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            // �������� �������
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                // �� ���� �� ������ ���������� ������ dx, dy
                // ��� ������ ���������, ������ ���� ������ ����������
                // ������� ����������� ������ � �������
                // 0.5 ������ ����������� ������� �������� �� ����
                // � ������� ���� ������������ �������� ��������
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // ����������� ���� �������� ������ 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // ���������� ������,
            // ����� �����, ����� ����� ��������� ������� ��������� ����������� ����������
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // �������� ������� ������ � ������� ������� ��������
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void pbMain_Click(object sender, EventArgs e)
        {

        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // ��� ������� �������� ������� �� ����� ���� �� ��� �� ������
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // � ������� �� ������ ��������� � objects
            }

            // � ��� ��� � ��������
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void UpdateScoreDisplay()
        {
            int score = player.score; 
            lblScope.Text = $"����: {score}";
        }

        private void TimerTick(object sender, EventArgs e)
        {
            goal.UpdateTimer();
            goal1.UpdateTimer();
            pbMain.Invalidate();
        }

        private void HandleTimerEnd()
        {
            goal.X = random.Next(0, 300);
            goal.Y = random.Next(0, 300);
        }
    }
}
