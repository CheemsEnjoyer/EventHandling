using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandling.objects
{
    internal class Goal : BaseObject
    {
        private Random random = new Random();
        public event Action TimerEnded;
        private int timerDuration = 60; // Продолжительность таймера в секундах
        private int currentTime; // Текущее время таймера
        private PointF timerTextLocation;
        public Goal(float x, float y, float angle) : base(x, y, angle)
        {
            this.X = random.Next(0, 300);
            this.Y = random.Next(0, 300);
        }
        public override void Render(Graphics g)
        {
            base.Render(g); // Вызываем базовый метод для отрисовки самого объекта Goal

            if (isVisible)
            {
                // Отрисовка шарика
                g.FillEllipse(
                    new SolidBrush(Color.GreenYellow),
                    -15, -15,
                    30, 30
                );
                g.DrawEllipse(
                    new Pen(Color.Black, 2),
                    -15, -15,
                    30, 30
                );

                g.DrawString(
                    currentTime.ToString(),
                    new Font("Verdana", 8), // шрифт и размер
                    new SolidBrush(Color.Green), // цвет шрифта
                    10, 10 // точка в которой нарисовать текст
                );
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        private bool isVisible = true;

        public void SetVisible(bool visible)
        {
            isVisible = visible;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            if (obj is Player)
            {
                // Исчезновение Goal
                SetVisible(false);
                // Генерация случайных координат для Goal
                this.X = random.Next(0, 300);
                this.Y = random.Next(0, 300);
                ResetTimer();
                SetVisible(true);
            }
        }
        public void UpdateTimer()
        {
            // Обновление времени таймера
            currentTime--;
            if (currentTime <= 0)
            {
                // Если время истекло, переместите Goal и сгенерируйте событие
                ResetTimer();
                TimerEnded?.Invoke();
            }
        }

        private void ResetTimer()
        {
            // Сброс таймера и перемещение Goal в случайное место
            currentTime = timerDuration;
            this.X = random.Next(0, 300);
            this.Y = random.Next(0, 300);
        }
        public void SetTimerTextLocation(PointF location)
        {
            timerTextLocation = location;
        }
    }
}
