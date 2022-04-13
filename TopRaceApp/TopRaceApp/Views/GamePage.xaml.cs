using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopRaceApp.Models;
using TopRaceApp.DTOs;
using TopRaceApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace TopRaceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();        
        }
        public void SetBorder()
        {
            GamePageViewModel vm = (GamePageViewModel)this.BindingContext;
            List<PlayersInGame> players = vm.Players;
            if(players[0].Id == ((App)App.Current).currentPlayerInGame.Id)
            {
                Crewmate1Frame.BorderColor = Xamarin.Forms.Color.Red;
            }
            if (players[1].Id == ((App)App.Current).currentPlayerInGame.Id)
            {
                Crewmate2Frame.BorderColor = Xamarin.Forms.Color.Red;
            }
            if (players.Count > 2 && players[2].Id == ((App)App.Current).currentPlayerInGame.Id)
            {
                Crewmate3Frame.BorderColor = Xamarin.Forms.Color.Red;
            }
            if (players.Count > 3 && players[3].Id == ((App)App.Current).currentPlayerInGame.Id)
            {
                Crewmate4Frame.BorderColor = Xamarin.Forms.Color.Red;
            }
        }
        private void BoardCanvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            #region board
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SKPath path = new SKPath();
                    path.MoveTo(j * 0.1f * info.Width, i * 0.1f * info.Height);
                    path.LineTo((0.1f + j * 0.1f) * info.Width, i * 0.1f * info.Height);
                    path.LineTo((0.1f + j * 0.1f) * info.Width, (0.1f + i * 0.1f) * info.Height);
                    path.LineTo(j * 0.1f * info.Width, (0.1f + i * 0.1f) * info.Height);
                    path.LineTo(j * 0.1f * info.Width, i * 0.1f * info.Height);
                    path.Close();
                    SKPaint fillPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.Green,
                    };
                    if ((i + j) % 2 == 0)
                    {
                        fillPaint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = SKColor.Parse("9EEEA8"),
                        };
                    }

                    int posNum = 10 * (9 - i) + (9 - j) + 1;
                    if (i % 2 == 1)
                        posNum = 10 * (9 - i) + j + 1;
                    if (posNum == 100)
                    {
                        fillPaint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = SKColors.Pink,
                        };

                    }
                    canvas.DrawPath(path, fillPaint);

                    string text = posNum.ToString();
                    float textX = j * 0.1f * info.Width;
                    float textY = (0.1f + i * 0.1f) * info.Height;
                    SKPaint textPaint = new SKPaint
                    {
                        TextSize = 40,
                        Color = SKColors.Black,
                    };
                    canvas.DrawText(text, textX, textY, textPaint);

                }
            }
            #endregion
            SKPaint ladderPaint1 = new SKPaint
            {
                //Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkBlue,
                StrokeWidth = 10,
                StrokeCap = SKStrokeCap.Round,
            }; 
            SKPaint ladderPaint2 = new SKPaint
            {
                //Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 10,
                StrokeCap = SKStrokeCap.Round,
            };

            int ladderCounter = 0;
            int snakeCounter = 0;
            Mover[,] board = ((GamePageViewModel)this.BindingContext).Board;
            foreach(Mover m in board)
            {
                SKPoint startPoint = GetSKPoint(m.StartPos, info);
                SKPoint endPoint = GetSKPoint(m.EndPos, info);
                if (m.IsLadder)
                {
                    if(ladderCounter % 2 == 0)
                    {
                        PrintLadder(e, startPoint, endPoint, ladderPaint1);
                    }
                    else
                    {
                        PrintLadder(e, startPoint, endPoint, ladderPaint2);
                    }
                    ladderCounter++;
                }
                if (m.IsSnake)
                {
                    if(snakeCounter % 4 == 0)
                    {
                        PrintSnake(e, startPoint, endPoint, SKColors.Yellow);
                    }
                    else if(snakeCounter % 4 == 1)
                    {
                        PrintSnake(e, startPoint, endPoint, SKColors.Orange);
                    }
                    else if (snakeCounter % 4 == 2)
                    {
                        PrintSnake(e, startPoint, endPoint, SKColors.Purple);
                    }
                    else
                    {
                        PrintSnake(e, startPoint, endPoint, SKColors.Red);
                    }
                    snakeCounter++;
                }
            }
            
            
           
        }
        private void PrintLadder(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKPoint startPoint, SKPoint endPoint, SKPaint paint)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            int width = info.Width;
            int height = info.Height;
            double startX = ((double)startPoint.X);
            double startY = ((double)startPoint.Y);
            double endX = ((double)endPoint.X);
            double endY = ((double)endPoint.Y);
            double m1 = (endY - startY) / (endX - startX);
            double m2 = (-1) / m1;
            double d = 0.025;
            double engle = Math.Atan(m2);
            double x1 = startX + Math.Cos(engle) * d * width;
            double y1 = startY + Math.Sin(engle) * d * height;
            double x2 = startX - Math.Cos(engle) * d * width;
            double y2 = startY - Math.Sin(engle) * d * height;

            double x3 = endX + Math.Cos(engle) * d * width;
            double y3 = endY + Math.Sin(engle) * d * height;
            double x4 = endX - Math.Cos(engle) * d * width;
            double y4 = endY - Math.Sin(engle) * d * height;
            SKPoint point1 = new SKPoint
            {
                X = (float)x1,
                Y = (float)y1,
            };
            SKPoint point2 = new SKPoint
            {
                X = (float)x2,
                Y = (float)y2,
            };
            SKPoint point3 = new SKPoint
            {
                X = (float)x3,
                Y = (float)y3,
            };
            SKPoint point4 = new SKPoint
            {
                X = (float)x4,
                Y = (float)y4,
            };
            List<SKPoint> ladderList = new List<SKPoint>();
            ladderList.Add(point1);
            ladderList.Add(point3);
            ladderList.Add(point2);
            ladderList.Add(point4);
            double topDiffX = x3 - x1;
            double topDiffY = y3 - y1;
            double lowDiffX = x4 - x2;
            double lowDiffY = y4 - y2;
            double toplength = Math.Pow(((Math.Pow(topDiffX / width, 2)) + Math.Pow(topDiffY / height, 2)), 0.5);
            int num = (int)Math.Round((toplength / 0.06), 0);
            for (int i = 1; i < num; i++)
            {
                SKPoint topPoint = new SKPoint
                {
                    X = (float)(x1 + (topDiffX / num) * i),
                    Y = (float)(y1 + (topDiffY / num) * i),
                };
                SKPoint lowPoint = new SKPoint
                {
                    X = (float)(x2 + (lowDiffX / num) * i),
                    Y = (float)(y2 + (lowDiffY / num) * i),
                };
                ladderList.Add(topPoint);
                ladderList.Add(lowPoint);
            }
            SKPoint[] ladder = ladderList.ToArray();
            SKPointMode pointMode = SKPointMode.Lines;
            canvas.DrawPoints(pointMode, ladder, paint);
        }
        
        private void PrintSnake(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKPoint startPoint, SKPoint endPoint, SKColor color)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            int width = info.Width;
            int height = info.Height;
            double startX = ((double)startPoint.X);
            double startY = ((double)startPoint.Y);
            double endX = ((double)endPoint.X);
            double endY = ((double)endPoint.Y);
            double midX = (startX + endX) / 2;
            double midY = (startY + endY) / 2;
            double diffX = midX - startX;
            double diffY = midY - startY;
            double x1 = midX + diffY;
            double y1 = midY + diffX;
            double x2 = midX - diffY;
            double y2 = midY - diffX;
            if (x1 > width || x2 > width || x2 < 0 || x1 < 0 ||
                y1 > height || y2 > height || y2 < 0 || y1 < 0)
            {
                x1 = midX + (2 * diffY) / 3;
                y1 = midY + (2 * diffX) / 3;
                x2 = midX - (2 * diffY) / 3;
                y2 = midY - (2 * diffX) / 3;
            }
            if (x1 > width || x2 > width || x2 < 0 || x1 < 0 ||
                y1 > height || y2 > height || y2 < 0 || y1 < 0)
            {
                x1 = midX + (1 * diffY) / 2;
                y1 = midY + (1 * diffX) / 2;
                x2 = midX - (1 * diffY) / 2;
                y2 = midY - (1 * diffX) / 2;
            }
            SKPoint point1 = new SKPoint
            {
                X = (float)x1,
                Y = (float)y1,
            };
            SKPoint point2 = new SKPoint
            {
                X = (float)x2,
                Y = (float)y2,
            };
            SKPoint[] snake = new SKPoint[4];
            snake[0] = startPoint;
            snake[1] = point1;
            snake[2] = point2;
            snake[3] = endPoint;
            SKPath snakePath = new SKPath();
            snakePath.MoveTo(snake[0]);
            snakePath.CubicTo(snake[1], snake[2], snake[3]);
            SKPaint linePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = 20,
                StrokeCap = SKStrokeCap.Round,
            };
            SKPaint headPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = color,
            };
            canvas.DrawPath(snakePath, linePaint);
            canvas.DrawCircle(snake[0], 0.03f * info.Width, headPaint);
        }
        public SKPoint GetSKPoint(Models.Position pos, SKImageInfo info)
        {
            int posX = pos.X;
            int posY = pos.Y;
            float x = (posX * 0.1f + 0.05f) * info.Width;
            float y = ((9 - posY) * 0.1f + 0.05f) * info.Height;
            return new SKPoint(x, y);
        }
    }
}