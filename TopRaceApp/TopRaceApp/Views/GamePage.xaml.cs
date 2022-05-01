using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TopRaceApp.Models;
using TopRaceApp.Services;
using TopRaceApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopRaceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public int infoWidth { get; set; }
        public int infoHeight { get; set; }
        public bool DidStart { get; set; }
        public TopRaceAPIProxy Proxy { get; set; }
        public SKBitmap Crewmate1BitMap { get; set; }
        public SKBitmap Crewmate2BitMap { get; set; }
        public SKBitmap Crewmate3BitMap { get; set; }
        public SKBitmap Crewmate4BitMap { get; set; }
        public List<SKBitmap> CrewmatesBitMaps { get; set; }
        public List<SKPoint> CrewmatesSKPoints { get; set; }
        public List<bool> CrewmatesFacingLeft { get; set; }
        //public Position Crewmate1pos { get; set; }
        //public Position Crewmate2pos { get; set; }
        //public Position Crewmate3pos { get; set; }
        //public Position Crewmate4pos { get; set; }
        public List<Position> PositionsList;
        //public List<SKBitmap>[] BitmapListsArray;
        public GamePage()
        {
            Proxy = TopRaceAPIProxy.CreateProxy();
            InitializeComponent();
            PositionsList = ((App)App.Current).Positions;
            DidStart = false;
            //PositionsArray = new Position[100];
            //for (int i = 0; i < PositionsArray.Length; i++)
            //{
            //    PositionsArray[i] = ((App)App.Current).Positions.Where(p => p.Id == i).FirstOrDefault();
            //}
            //BitmapListsArray = new List<SKBitmap>[100];
            //for (int i = 0; i < BitmapListsArray.Length; i++)
            //{
            //    BitmapListsArray[i] = new List<SKBitmap>();
            //}
        }
        public void RemoveFromLists(int removedIndex)
        {
            CrewmatesBitMaps.RemoveAt(removedIndex);
            CrewmatesSKPoints.RemoveAt(removedIndex);
            CrewmatesFacingLeft.RemoveAt(removedIndex);
            BoardCanvas.InvalidateSurface();
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
        public async Task SetBitMaps()
        {
            CrewmatesBitMaps = new List<SKBitmap>();
            CrewmatesFacingLeft = new List<bool>();
            GamePageViewModel vm = (GamePageViewModel)this.BindingContext;
            List<PlayersInGame> players = vm.Players;
            try
            {
                using (Stream stream = await Proxy.GetCrewmateStream(vm.CrewmatePic1))
                using (MemoryStream memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    Crewmate1BitMap = SKBitmap.Decode(memStream);
                    BoardCanvas.InvalidateSurface();
                };
                //Crewmate1pos = players[0].CurrentPos;
                CrewmatesBitMaps.Add(Crewmate1BitMap);
                CrewmatesFacingLeft.Add(false);
                //BitmapListsArray[Crewmate1pos.Id].Add(Crewmate1BitMap);
                using (Stream stream = await Proxy.GetCrewmateStream(vm.CrewmatePic2))
                using (MemoryStream memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    Crewmate2BitMap = SKBitmap.Decode(memStream);
                    BoardCanvas.InvalidateSurface();
                };
                //Crewmate2pos = players[1].CurrentPos;
                CrewmatesBitMaps.Add(Crewmate2BitMap);
                CrewmatesFacingLeft.Add(false);
                //BitmapListsArray[Crewmate2pos.Id].Add(Crewmate2BitMap);
                if (players.Count > 2)
                {
                    using (Stream stream = await Proxy.GetCrewmateStream(vm.CrewmatePic3))
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);

                        Crewmate3BitMap = SKBitmap.Decode(memStream);
                        BoardCanvas.InvalidateSurface();
                    };
                    //Crewmate3pos = players[2].CurrentPos;
                    CrewmatesBitMaps.Add(Crewmate3BitMap);
                    CrewmatesFacingLeft.Add(false);

                    //BitmapListsArray[Crewmate3pos.Id].Add(Crewmate3BitMap);
                }
                if (players.Count > 3)
                {
                    using (Stream stream = await Proxy.GetCrewmateStream(vm.CrewmatePic4))
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);

                        Crewmate4BitMap = SKBitmap.Decode(memStream);
                        BoardCanvas.InvalidateSurface();
                    };
                    //Crewmate4pos = players[3].CurrentPos;
                    CrewmatesBitMaps.Add(Crewmate4BitMap);
                    CrewmatesFacingLeft.Add(false);
                    //BitmapListsArray[Crewmate4pos.Id].Add(Crewmate4BitMap);
                }

            }
            catch(Exception e)
            {
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
                        PrintLadder(e, startPoint, endPoint, SKColors.DarkBlue);
                    }
                    else
                    {
                        PrintLadder(e, startPoint, endPoint, SKColors.Black);
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
            GamePageViewModel vm = (GamePageViewModel)this.BindingContext;
            List<PlayersInGame> players = vm.Players;
            if (!DidStart)
            {
                infoWidth = info.Width;
                infoHeight = info.Height;
                CrewmatesSKPoints = new List<SKPoint>();
                for (int i = 0; i < players.Count; i++)
                {
                    CrewmatesSKPoints.Add(GetSKPoint(players[i].CurrentPos, info));
                }
                DidStart = true;
            }
            PrintAllCrewmates(e);
        }
        private void PrintLadder(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKPoint startPoint, SKPoint endPoint, SKColor color)
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
            SKPaint paint = new SKPaint
            {
                //Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = 10,
                StrokeCap = SKStrokeCap.Round,
            };
            canvas.DrawPoints(pointMode, ladder, paint);
        }
        private void PrintAllCrewmates(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            GamePageViewModel vm = (GamePageViewModel)this.BindingContext;
            List<PlayersInGame> Players = vm.Players;
            foreach (SKPoint point in CrewmatesSKPoints)
            {
                List<int> indexes = new List<int>();

                for (int i = 0; i < CrewmatesSKPoints.Count; i++)
                {
                    SKPoint sKPoint = CrewmatesSKPoints[i];
                    if (sKPoint.Equals(point))
                    {
                        indexes.Add(i);
                    }
                }
                if(indexes.Count == 1)
                {
                    SKBitmap crewmate = CrewmatesBitMaps[indexes[0]];
                    PrintCrewmate(e, crewmate, point, CrewmatesFacingLeft[indexes[0]]);
                }
                if(indexes.Count == 2)
                {
                    SKBitmap crewmate1 = CrewmatesBitMaps[indexes[0]]; 
                    SKBitmap crewmate2 = CrewmatesBitMaps[indexes[1]];
                    Print2Crewmates(e, crewmate1, crewmate2, point, CrewmatesFacingLeft[indexes[0]], CrewmatesFacingLeft[indexes[1]]);
                }
                if(indexes.Count == 3)
                {
                    SKBitmap crewmate1 = CrewmatesBitMaps[indexes[0]];
                    SKBitmap crewmate2 = CrewmatesBitMaps[indexes[1]]; 
                    SKBitmap crewmate3 = CrewmatesBitMaps[indexes[2]];
                    Print3Crewmates(e, crewmate1, crewmate2, crewmate3, point, CrewmatesFacingLeft[indexes[0]], CrewmatesFacingLeft[indexes[1]], CrewmatesFacingLeft[indexes[2]]);
                }
                if(indexes.Count == 4)
                {
                    SKBitmap crewmate1 = CrewmatesBitMaps[indexes[0]];
                    SKBitmap crewmate2 = CrewmatesBitMaps[indexes[1]]; 
                    SKBitmap crewmate3 = CrewmatesBitMaps[indexes[2]];
                    SKBitmap crewmate4 = CrewmatesBitMaps[indexes[3]];
                    Print4Crewmates(e, crewmate1, crewmate2, crewmate3, crewmate4, point, CrewmatesFacingLeft[indexes[0]], CrewmatesFacingLeft[indexes[1]], CrewmatesFacingLeft[indexes[2]], CrewmatesFacingLeft[indexes[3]]);
                }
            }
            
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
                StrokeWidth = 15,
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
        public SKPoint GetSKPoint(Models.Position pos, int infoWidth, int infoHeight)
        {
            int posX = pos.X;
            int posY = pos.Y;
            float x = (posX * 0.1f + 0.05f) * infoWidth;
            float y = ((9 - posY) * 0.1f + 0.05f) * infoHeight;
            return new SKPoint(x, y);
        }
        private void PrintCrewmate(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKBitmap bitmap, SKPoint point, bool isFacingLeft)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            float height = 0.095f * info.Height;
            float scale = height / bitmap.Height;
            float width = scale * bitmap.Width;
            float x = point.X - (width / 2);
            float y = point.Y - (height / 2);
            SKRect rect = new SKRect(x, y, x + width, y + height);
            if (isFacingLeft)
            {
                SKBitmap flippedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, bitmap.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(bitmap, new SKPoint());
                }            
                bitmap = flippedBitmap;
            }
            canvas.DrawBitmap(bitmap, rect);
        }
        private void Print2Crewmates(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKBitmap crewmate1, SKBitmap crewmate2, SKPoint point, bool isFacingLeft1, bool isFacingLeft2)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            float height = 0.045f * info.Height;
            float scale = height / crewmate1.Height;
            float width = scale * crewmate1.Width;
            float x = point.X - (width / 2);
            float y1 = point.Y - 0.025f * info.Height - (height / 2);
            float y2 = point.Y + 0.025f * info.Height - (height / 2);
            if (isFacingLeft1)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate1.Width, crewmate1.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate1.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate1, new SKPoint());
                }
                crewmate1 = flippedBitmap;
            }
            if (isFacingLeft2)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate2.Width, crewmate2.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate2.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate2, new SKPoint());
                }
                crewmate2 = flippedBitmap;
            }
            SKRect rect1 = new SKRect(x, y1, x + width, y1 + height);
            canvas.DrawBitmap(crewmate1, rect1);
            SKRect rect2 = new SKRect(x, y2, x + width, y2 + height);
            canvas.DrawBitmap(crewmate2, rect2);
        }
        private void Print3Crewmates(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKBitmap crewmate1, SKBitmap crewmate2, SKBitmap crewmate3, SKPoint point, bool isFacingLeft1, bool isFacingLeft2, bool isFacingLeft3)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            float height = 0.045f * info.Height;
            float scale = height / crewmate1.Height;
            float width = scale * crewmate1.Width;
            float x1 = point.X - 0.025f * info.Width - (width / 2);
            float x2 = point.X + 0.025f * info.Width - (width / 2);
            float y1 = point.Y - 0.025f * info.Height - (height / 2);
            float y2 = point.Y + 0.025f * info.Height - (height / 2);
            if (isFacingLeft1)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate1.Width, crewmate1.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate1.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate1, new SKPoint());
                }
                crewmate1 = flippedBitmap;
            }
            if (isFacingLeft2)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate2.Width, crewmate2.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate2.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate2, new SKPoint());
                }
                crewmate2 = flippedBitmap;
            }
            if (isFacingLeft3)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate3.Width, crewmate3.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate3.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate3, new SKPoint());
                }
                crewmate3 = flippedBitmap;
            }
            SKRect rect1 = new SKRect(x1, y1, x1 + width, y1 + height);
            canvas.DrawBitmap(crewmate1, rect1);
            SKRect rect2 = new SKRect(x2, y1, x2 + width, y1 + height);
            canvas.DrawBitmap(crewmate2, rect2);
            SKRect rect3 = new SKRect(x1, y2, x1 + width, y2 + height);
            canvas.DrawBitmap(crewmate3, rect3);
        }
        private void Print4Crewmates(SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e, SKBitmap crewmate1, SKBitmap crewmate2, SKBitmap crewmate3, SKBitmap crewmate4, SKPoint point, bool isFacingLeft1, bool isFacingLeft2, bool isFacingLeft3, bool isFacingLeft4)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            float height = 0.045f * info.Height;
            float scale = height / crewmate1.Height;
            float width = scale * crewmate1.Width;
            float x1 = point.X - 0.025f * info.Width - (width / 2);
            float x2 = point.X + 0.025f * info.Width - (width / 2);
            float y1 = point.Y - 0.025f * info.Height - (height / 2);
            float y2 = point.Y + 0.025f * info.Height - (height / 2);
            if (isFacingLeft1)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate1.Width, crewmate1.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate1.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate1, new SKPoint());
                }
                crewmate1 = flippedBitmap;
            }
            if (isFacingLeft2)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate2.Width, crewmate2.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate2.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate2, new SKPoint());
                }
                crewmate2 = flippedBitmap;
            }
            if (isFacingLeft3)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate3.Width, crewmate3.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate3.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate3, new SKPoint());
                }
                crewmate3 = flippedBitmap;
            }
            if (isFacingLeft4)
            {
                SKBitmap flippedBitmap = new SKBitmap(crewmate4.Width, crewmate4.Height);
                using (SKCanvas crewmateCanvas = new SKCanvas(flippedBitmap))
                {
                    crewmateCanvas.Clear();
                    crewmateCanvas.Scale(-1, 1, crewmate4.Width / 2, 0);
                    crewmateCanvas.DrawBitmap(crewmate4, new SKPoint());
                }
                crewmate4 = flippedBitmap;
            }
            SKRect rect1 = new SKRect(x1, y1, x1 + width, y1 + height);
            canvas.DrawBitmap(crewmate1, rect1);
            SKRect rect2 = new SKRect(x2, y1, x2 + width, y1 + height);
            canvas.DrawBitmap(crewmate2, rect2);
            SKRect rect3 = new SKRect(x1, y2, x1 + width, y2 + height);
            canvas.DrawBitmap(crewmate3, rect3);
            SKRect rect4 = new SKRect(x2, y2, x2 + width, y2 + height);
            canvas.DrawBitmap(crewmate4, rect4);
        }
        private int GetNewPosID(int previousPosID, int rollResult)
        {
            int newPosId = previousPosID + rollResult;
            if (newPosId > 100)
            {
                newPosId = 100 - (newPosId - 100);
            }
            return newPosId;
        }
        public void MoveCrewmate(int index, Position startPos, int rollResult ,Position endPos)
        {
            SKPoint startPoint = GetSKPoint(startPos, infoWidth, infoHeight);
            int nextPosID = GetNewPosID(startPos.Id, rollResult);
            Position nextPos = this.PositionsList.Where(p => p.Id == nextPosID).FirstOrDefault();
            SKPoint nextPoint = GetSKPoint(nextPos, infoWidth, infoHeight);
            int timesPerSpot = 50;
            double secondsForSpot = 0.1;
            SKPoint endPoint = GetSKPoint(endPos, infoWidth, infoHeight);
            if (nextPosID == endPos.Id)
            {
                if (nextPos.Y == startPos.Y + 1)
                {
                    if (startPos.Y % 2 == 0)
                    {
                        float x1 = 0.95f * infoWidth;
                        SKPoint point1 = new SKPoint(x1, startPoint.Y);
                        SKPoint point2 = new SKPoint(x1, nextPoint.Y);
                        int timeToReachPoint1 = timesPerSpot * (9 - startPos.X);
                        int timeToReachPoint2 = timesPerSpot;
                        int timeToReachNextPoint = timesPerSpot * (9 - nextPos.X);
                        int counter1 = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        bool reached1 = counter1 == timeToReachPoint1;
                        bool reached2 = counter2 == timeToReachPoint2;
                        bool reachedNext = counter3 == timeToReachNextPoint;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X + ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                SKPoint point = new SKPoint(x, startPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timeToReachPoint1;
                            }
                            if (reached1 && (!reached2))
                            {
                                counter2++;
                                float y = point1.Y - ((0.1f * infoHeight) / timesPerSpot) * counter2;
                                SKPoint point = new SKPoint(point1.X, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached2 = counter2 == timeToReachPoint2;
                                if (reached2)
                                {
                                    CrewmatesFacingLeft[index] = true;
                                }
                            }
                            if(reached1 && reached2 && (!reachedNext))
                            {
                                counter3++;
                                float x = point2.X - ((0.1f * infoWidth) / timesPerSpot) * counter3;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedNext = counter3 == timeToReachNextPoint;
                            }
                            if(reached1 && reached2 && reachedNext)
                            {
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reached2 && reachedNext) == false;
                        });
                    }
                    else
                    {
                        float x1 = 0.05f * infoWidth;
                        SKPoint point1 = new SKPoint(x1, startPoint.Y);
                        SKPoint point2 = new SKPoint(x1, nextPoint.Y);
                        int timeToReachPoint1 = timesPerSpot * (startPos.X);
                        int timeToReachPoint2 = timesPerSpot;
                        int timeToReachNextPoint = timesPerSpot * (nextPos.X);
                        int counter1 = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        bool reached1 = counter1 == timeToReachPoint1;
                        bool reached2 = counter2 == timeToReachPoint2;
                        bool reachedNext = counter3 == timeToReachNextPoint;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                SKPoint point = new SKPoint(x, startPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timeToReachPoint1;
                            }
                            if (reached1 && (!reached2))
                            {
                                counter2++;
                                float y = point1.Y - ((0.1f * infoHeight) / timesPerSpot) * counter2;
                                SKPoint point = new SKPoint(point1.X, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached2 = counter2 == timeToReachPoint2;
                                if (reached2)
                                {
                                    CrewmatesFacingLeft[index] = false;
                                }
                            }
                            if (reached1 && reached2 && (!reachedNext))
                            {
                                counter3++;
                                float x = point2.X + ((0.1f * infoWidth) / timesPerSpot) * counter3;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedNext = counter3 == timeToReachNextPoint;
                            }
                            if(reached1 && reached2 && reachedNext)
                            {
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reached2 && reachedNext) == false;
                        });
                    }
                }
                else if (startPoint.Y == nextPoint.Y)
                {
                    if (startPos.Y % 2 == 0)
                    {
                        int timersToReach = timesPerSpot * (nextPos.X - startPos.X);
                        int counter = 0;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            counter++;
                            float x = startPoint.X + ((0.1f * infoWidth) / timesPerSpot) * counter;
                            SKPoint point = new SKPoint(x, nextPoint.Y);
                            CrewmatesSKPoints.RemoveAt(index);
                            CrewmatesSKPoints.Insert(index, point);
                            BoardCanvas.InvalidateSurface();
                            if(counter == timersToReach)
                            {
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return counter < timersToReach;
                        });
                    }
                    else
                    {
                        if ((startPos.Y == 9) && (startPos.Id + rollResult > 100))
                        {
                            Position pos100 = this.PositionsList.Where(p => p.Id == 100).FirstOrDefault();
                            SKPoint point100 = GetSKPoint(pos100, infoWidth, infoHeight);
                            int timesToReach1 = timesPerSpot * (startPos.X - pos100.X);
                            int counter1 = 0;
                            bool reached1 = counter1 == timesToReach1;
                            int timesToReach2 = timesPerSpot * nextPos.X;
                            int counter2 = 0;
                            bool reached2 = counter2 == timesToReach2;
                            Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                            {
                                if (!reached1)
                                {
                                    counter1++;
                                    float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                    SKPoint point = new SKPoint(x, startPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached1 = counter1 == timesToReach1;
                                    if (reached1)
                                    {
                                        CrewmatesFacingLeft[index] = false;
                                    }
                                }
                                if (reached1 && (!reached2))
                                {
                                    counter2++;
                                    float x = point100.X + ((0.1f * infoWidth) / timesPerSpot) * counter2;
                                    SKPoint point = new SKPoint(x, nextPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached2 = counter2 == timesToReach2;
                                }
                                if(reached1 && reached2)
                                {
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, endPoint);
                                    CrewmatesFacingLeft[index] = true;
                                    BoardCanvas.InvalidateSurface();
                                }
                                return (reached1 && reached2) == false;
                            });
                        }
                        else
                        {
                            int timersToReach = timesPerSpot * (startPos.X - nextPos.X);
                            int counter = 0;
                            Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                            {
                                counter++;
                                float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter; ;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                if(counter == timersToReach)
                                {
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, endPoint);
                                    BoardCanvas.InvalidateSurface();
                                }
                                return counter < timersToReach;
                            });
                        }
                    }
                }
            }
            else if(nextPos.Id > endPos.Id)
            {
                endPoint = GetSKPoint(endPos, infoWidth, infoHeight);
                double length = Math.Pow(Math.Pow(Math.Abs(nextPos.X - endPos.X), 2) + Math.Pow(Math.Abs(nextPos.Y - endPos.Y), 2), 0.5);
                int timesForLadder = ((int)(Math.Round(length * 50, 0))) / 2;
                if (nextPos.Y == startPos.Y + 1)
                {
                    if (startPos.Y % 2 == 0)
                    {
                        float x1 = 0.95f * infoWidth;
                        SKPoint point1 = new SKPoint(x1, startPoint.Y);
                        SKPoint point2 = new SKPoint(x1, nextPoint.Y);
                        int timeToReachPoint1 = timesPerSpot * (9 - startPos.X);
                        int timeToReachPoint2 = timesPerSpot;
                        int timeToReachNextPoint = timesPerSpot * (9 - nextPos.X);
                        int counter1 = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        int counterL = 0;
                        bool reached1 = counter1 == timeToReachPoint1;
                        bool reached2 = counter2 == timeToReachPoint2;
                        bool reachedNext = counter3 == timeToReachNextPoint;
                        bool reachedL = counterL == timesForLadder;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X + ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                SKPoint point = new SKPoint(x, startPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timeToReachPoint1;
                            }
                            if (reached1 && (!reached2))
                            {
                                counter2++;
                                float y = point1.Y - ((0.1f * infoHeight) / timesPerSpot) * counter2;
                                SKPoint point = new SKPoint(point1.X, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached2 = counter2 == timeToReachPoint2;
                                if (reached2)
                                {
                                    CrewmatesFacingLeft[index] = true;
                                }
                            }
                            if (reached1 && reached2 && (!reachedNext))
                            {
                                counter3++;
                                float x = point2.X - ((0.1f * infoWidth) / timesPerSpot) * counter3;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedNext = counter3 == timeToReachNextPoint;
                            }
                            if(reached1 && reached2 && reachedNext && (!reachedL))
                            {
                                if(counterL == 0)
                                {
                                    CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                }
                                counterL++;
                                float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterL / timesForLadder));
                                float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterL / timesForLadder));
                                SKPoint point = new SKPoint(x, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedL = counterL == timesForLadder;
                            }
                            if (reached1 && reached2 && reachedNext && reachedL)
                            {
                                CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reached2 && reachedNext && reachedL) == false;
                        });
                    }
                    else
                    {
                        float x1 = 0.05f * infoWidth;
                        SKPoint point1 = new SKPoint(x1, startPoint.Y);
                        SKPoint point2 = new SKPoint(x1, nextPoint.Y);
                        int timeToReachPoint1 = timesPerSpot * (startPos.X);
                        int timeToReachPoint2 = timesPerSpot;
                        int timeToReachNextPoint = timesPerSpot * (nextPos.X);
                        int counter1 = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        int counterL = 0;
                        bool reached1 = counter1 == timeToReachPoint1;
                        bool reached2 = counter2 == timeToReachPoint2;
                        bool reachedNext = counter3 == timeToReachNextPoint;
                        bool reachedL = counterL == timesForLadder;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                SKPoint point = new SKPoint(x, startPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timeToReachPoint1;
                            }
                            if (reached1 && (!reached2))
                            {
                                counter2++;
                                float y = point1.Y - ((0.1f * infoHeight) / timesPerSpot) * counter2;
                                SKPoint point = new SKPoint(point1.X, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached2 = counter2 == timeToReachPoint2;
                                if (reached2)
                                {
                                    CrewmatesFacingLeft[index] = false;
                                }
                            }
                            if (reached1 && reached2 && (!reachedNext))
                            {
                                counter3++;
                                float x = point2.X + ((0.1f * infoWidth) / timesPerSpot) * counter3;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedNext = counter3 == timeToReachNextPoint;
                            }
                            if (reached1 && reached2 && reachedNext && (!reachedL))
                            {
                                if (counterL == 0)
                                {
                                    CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                }
                                counterL++;
                                float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterL / timesForLadder));
                                float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterL / timesForLadder));
                                SKPoint point = new SKPoint(x, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedL = counterL == timesForLadder;
                            }
                            if(reached1 && reached2 && reachedNext && reachedL)
                            {
                                CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reached2 && reachedNext && reachedL) == false;
                        });
                    }
                }
                else if (startPoint.Y == nextPoint.Y)
                {
                    if (startPos.Y % 2 == 0)
                    {
                        int timesToReach = timesPerSpot * (nextPos.X - startPos.X);
                        int counter1 = 0;
                        int counterL = 0;
                        bool reachedL = counterL == timesForLadder;
                        bool reached1 = counter1 == timesToReach;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X + ((0.1f * infoWidth) / timesPerSpot) * counter1; ;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timesToReach;
                            }
                            if(reached1 && (!reachedL))
                            {
                                if (counterL == 0)
                                {
                                    CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                }
                                counterL++;
                                float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterL / timesForLadder));
                                float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterL / timesForLadder));
                                SKPoint point = new SKPoint(x, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedL = counterL == timesForLadder;
                            }
                            if(reached1 && reachedL)
                            {
                                CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reachedL) == false;
                        });
                    }
                    else
                    {
                        if ((startPos.Y == 9) && (startPos.Id + rollResult > 100))
                        {
                            Position pos100 = this.PositionsList.Where(p => p.Id == 100).FirstOrDefault();
                            SKPoint point100 = GetSKPoint(pos100, infoWidth, infoHeight);
                            int timesToReach1 = timesPerSpot * (startPos.X - pos100.X);
                            int counter1 = 0;
                            bool reached1 = counter1 == timesToReach1;
                            int timesToReach2 = timesPerSpot * nextPos.X;
                            int counter2 = 0;
                            bool reached2 = counter2 == timesToReach2;
                            int counterL = 0;
                            bool reachedL = counterL == timesForLadder;
                            Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                            {
                                if (!reached1)
                                {
                                    counter1++;
                                    float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                    SKPoint point = new SKPoint(x, startPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached1 = counter1 == timesToReach1;
                                    if (reached1)
                                    {
                                        CrewmatesFacingLeft[index] = false;
                                    }
                                }
                                if (reached1 && (!reached2))
                                {
                                    counter2++;
                                    float x = point100.X + ((0.1f * infoWidth) / timesPerSpot) * counter2;
                                    SKPoint point = new SKPoint(x, nextPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached2 = counter2 == timesToReach2;
                                }
                                if(reached1 && reached2 && (!reachedL))
                                {
                                    if (counterL == 0)
                                    {
                                        CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                    }
                                    counterL++;
                                    float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterL / timesForLadder));
                                    float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterL / timesForLadder));
                                    SKPoint point = new SKPoint(x, y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reachedL = counterL == timesForLadder;
                                }
                                if(reached1 && reached2 && reachedL)
                                {
                                    CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, endPoint);
                                    BoardCanvas.InvalidateSurface();
                                }
                                return (reached1 && reached2 && reachedL) == false;
                            });
                        }
                        else
                        {
                            int timesToReach = timesPerSpot * (startPos.X - nextPos.X);
                            int counter1 = 0;
                            int counterL = 0;
                            bool reached1 = counter1 == timesToReach;
                            bool reachedL = counterL == timesForLadder;
                            Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                            {
                                if (!reached1)
                                {
                                    counter1++;
                                    float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1; ;
                                    SKPoint point = new SKPoint(x, nextPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached1 = counter1 == timesToReach;
                                }
                                if (reached1 && (!reachedL))
                                {
                                    if (counterL == 0)
                                    {
                                        CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                    }
                                    counterL++;
                                    float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterL / timesForLadder));
                                    float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterL / timesForLadder));
                                    SKPoint point = new SKPoint(x, y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reachedL = counterL == timesForLadder;
                                }
                                if (reached1 && reachedL)
                                {
                                    CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, endPoint);
                                    BoardCanvas.InvalidateSurface();
                                }
                                return (reached1 && reachedL) == false;
                            });
                        }
                    }
                }
            }
            else if(nextPos.Id < endPos.Id)
            {
                endPoint = GetSKPoint(endPos, infoWidth, infoHeight);
                double length = Math.Pow(Math.Pow(Math.Abs(nextPos.X - endPos.X), 2) + Math.Pow(Math.Abs(nextPos.Y - endPos.Y), 2), 0.5);
                int timesForSnake = ((int)(Math.Round(length * 50, 0)))/2;
                if (nextPos.Y == startPos.Y + 1)
                {
                    if (startPos.Y % 2 == 0)
                    {
                        float x1 = 0.95f * infoWidth;
                        SKPoint point1 = new SKPoint(x1, startPoint.Y);
                        SKPoint point2 = new SKPoint(x1, nextPoint.Y);
                        int timeToReachPoint1 = timesPerSpot * (9 - startPos.X);
                        int timeToReachPoint2 = timesPerSpot;
                        int timeToReachNextPoint = timesPerSpot * (9 - nextPos.X);
                        int counter1 = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        int counterS = 0;
                        bool reached1 = counter1 == timeToReachPoint1;
                        bool reached2 = counter2 == timeToReachPoint2;
                        bool reachedNext = counter3 == timeToReachNextPoint;
                        bool reachedS = counterS == timesForSnake;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X + ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                SKPoint point = new SKPoint(x, startPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timeToReachPoint1;
                            }
                            if (reached1 && (!reached2))
                            {
                                counter2++;
                                float y = point1.Y - ((0.1f * infoHeight) / timesPerSpot) * counter2;
                                SKPoint point = new SKPoint(point1.X, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached2 = counter2 == timeToReachPoint2;
                                if (reached2)
                                {
                                    CrewmatesFacingLeft[index] = true;
                                }
                            }
                            if (reached1 && reached2 && (!reachedNext))
                            {
                                counter3++;
                                float x = point2.X - ((0.1f * infoWidth) / timesPerSpot) * counter3;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedNext = counter3 == timeToReachNextPoint;
                            }
                            if (reached1 && reached2 && reachedNext && (!reachedS))
                            {
                                if(counterS == 0)
                                {
                                    CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                }
                                counterS++;
                                float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterS / timesForSnake));
                                float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterS / timesForSnake));
                                SKPoint point = new SKPoint(x, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedS = counterS == timesForSnake;
                            }
                            if(reached1 && reached2 && reachedNext && reachedS)
                            {
                                CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reached2 && reachedNext && reachedS) == false;
                        });
                    }
                    else
                    {
                        float x1 = 0.05f * infoWidth;
                        SKPoint point1 = new SKPoint(x1, startPoint.Y);
                        SKPoint point2 = new SKPoint(x1, nextPoint.Y);
                        int timeToReachPoint1 = timesPerSpot * (startPos.X);
                        int timeToReachPoint2 = timesPerSpot;
                        int timeToReachNextPoint = timesPerSpot * (nextPos.X);
                        int counter1 = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        int counterS = 0;
                        bool reached1 = counter1 == timeToReachPoint1;
                        bool reached2 = counter2 == timeToReachPoint2;
                        bool reachedNext = counter3 == timeToReachNextPoint;
                        bool reachedS = counterS == timesForSnake;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                SKPoint point = new SKPoint(x, startPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timeToReachPoint1;
                            }
                            if (reached1 && (!reached2))
                            {
                                counter2++;
                                float y = point1.Y - ((0.1f * infoHeight) / timesPerSpot) * counter2;
                                SKPoint point = new SKPoint(point1.X, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached2 = counter2 == timeToReachPoint2;
                                if (reached2)
                                {
                                    CrewmatesFacingLeft[index] = false;
                                }
                            }
                            if (reached1 && reached2 && (!reachedNext))
                            {
                                counter3++;
                                float x = point2.X + ((0.1f * infoWidth) / timesPerSpot) * counter3;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedNext = counter3 == timeToReachNextPoint;
                            }
                            if (reached1 && reached2 && reachedNext && (!reachedS))
                            {
                                if (counterS == 0)
                                {
                                    CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                }
                                counterS++;
                                float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterS / timesForSnake));
                                float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterS / timesForSnake));
                                SKPoint point = new SKPoint(x, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedS = counterS == timesForSnake;
                            }
                            if (reached1 && reached2 && reachedNext && reachedS)
                            {
                                CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reached2 && reachedNext && reachedS) == false;
                        });
                    }
                }
                else if (startPoint.Y == nextPoint.Y)
                {
                    if (startPos.Y % 2 == 0)
                    {
                        int timesToReach = timesPerSpot * (nextPos.X - startPos.X);
                        int counter1 = 0;
                        int counterS = 0;
                        bool reachedS = counterS == timesForSnake;
                        bool reached1 = counter1 == timesToReach;
                        Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                        {
                            if (!reached1)
                            {
                                counter1++;
                                float x = startPoint.X + ((0.1f * infoWidth) / timesPerSpot) * counter1; ;
                                SKPoint point = new SKPoint(x, nextPoint.Y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reached1 = counter1 == timesToReach;
                            }
                            if (reached1 && (!reachedS))
                            {
                                if (counterS == 0)
                                {
                                    CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                }
                                counterS++;
                                float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterS / timesForSnake));
                                float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterS / timesForSnake));
                                SKPoint point = new SKPoint(x, y);
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, point);
                                BoardCanvas.InvalidateSurface();
                                reachedS = counterS == timesForSnake;
                            }
                            if (reached1 && reachedS)
                            {
                                CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                CrewmatesSKPoints.RemoveAt(index);
                                CrewmatesSKPoints.Insert(index, endPoint);
                                BoardCanvas.InvalidateSurface();
                            }
                            return (reached1 && reachedS) == false;
                        });
                    }
                    else
                    {
                        if ((startPos.Y == 9) && (startPos.Id + rollResult > 100))
                        {
                            Position pos100 = this.PositionsList.Where(p => p.Id == 100).FirstOrDefault();
                            SKPoint point100 = GetSKPoint(pos100, infoWidth, infoHeight);
                            int timesToReach1 = timesPerSpot * (startPos.X - pos100.X);
                            int counter1 = 0;
                            bool reached1 = counter1 == timesToReach1;
                            int timesToReach2 = timesPerSpot * nextPos.X;
                            int counter2 = 0;
                            bool reached2 = counter2 == timesToReach2;
                            int counterS = 0;
                            bool reachedS = counterS == timesForSnake;
                            Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                            {
                                if (!reached1)
                                {
                                    counter1++;
                                    float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1;
                                    SKPoint point = new SKPoint(x, startPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached1 = counter1 == timesToReach1;
                                    if (reached1)
                                    {
                                        CrewmatesFacingLeft[index] = false;
                                    }
                                }
                                if (reached1 && (!reached2))
                                {
                                    counter2++;
                                    float x = point100.X + ((0.1f * infoWidth) / timesPerSpot) * counter2;
                                    SKPoint point = new SKPoint(x, nextPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached2 = counter2 == timesToReach2;
                                }
                                if (reached1 && reached2 && (!reachedS))
                                {
                                    if (counterS == 0)
                                    {
                                        CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                    }
                                    counterS++;
                                    float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterS / timesForSnake));
                                    float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterS / timesForSnake));
                                    SKPoint point = new SKPoint(x, y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reachedS = counterS == timesForSnake;
                                }
                                if (reached1 && reached2 && reachedS)
                                {
                                    CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, endPoint);
                                    BoardCanvas.InvalidateSurface();
                                }
                                return (reached1 && reached2 && reachedS) == false;
                            });
                        }
                        else
                        {
                            int timesToReach = timesPerSpot * (startPos.X - nextPos.X);
                            int counter1 = 0;
                            int counterS = 0;
                            bool reached1 = counter1 == timesToReach;
                            bool reachedS = counterS == timesForSnake;
                            Device.StartTimer(TimeSpan.FromSeconds(secondsForSpot / timesPerSpot), () =>
                            {
                                if (!reached1)
                                {
                                    counter1++;
                                    float x = startPoint.X - ((0.1f * infoWidth) / timesPerSpot) * counter1; ;
                                    SKPoint point = new SKPoint(x, nextPoint.Y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reached1 = counter1 == timesToReach;
                                }
                                if (reached1 && (!reachedS))
                                {
                                    if (counterS == 0)
                                    {
                                        CrewmatesFacingLeft[index] = endPos.X < nextPos.X;
                                    }
                                    counterS++;
                                    float x = nextPoint.X + ((endPoint.X - nextPoint.X) * ((float)counterS / timesForSnake));
                                    float y = nextPoint.Y + ((endPoint.Y - nextPoint.Y) * ((float)counterS / timesForSnake));
                                    SKPoint point = new SKPoint(x, y);
                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, point);
                                    BoardCanvas.InvalidateSurface();
                                    reachedS = counterS == timesForSnake;
                                }
                                if (reached1 && reachedS)
                                {
                                    CrewmatesFacingLeft[index] = endPos.Y % 2 != 0;

                                    CrewmatesSKPoints.RemoveAt(index);
                                    CrewmatesSKPoints.Insert(index, endPoint);
                                    BoardCanvas.InvalidateSurface();
                                }
                                return (reached1 && reachedS) == false;
                            });
                        }
                    }
                }
            }

        }
        public double GetLength(SKPoint point1, SKPoint point2)
        {
            return Math.Pow(Math.Pow(point1.X + point2.X, 2) + Math.Pow(point1.Y + point2.Y, 2), 0.5);
        }
        
    }
}