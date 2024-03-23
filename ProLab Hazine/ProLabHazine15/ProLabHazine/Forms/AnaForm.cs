using ProLabHazine.Scripts;
using ProLabHazine.Scripts.TresureScripts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProLabHazine
{
    public partial class AnaForm : Form
    {
        #region PROPS, OBJECTS AND VARIABLES

        private bool ısGridDrawed = false;

        public int mapXSize;
        public int mapYSize;

        public int cellWidth;
        public int cellHeight;

        public int totalTresures = 0;

        internal static bool[,] fogMap;

        private Character Player;
        public Runtime runtime;

        private Graphics graphics;
        private Random random = new Random();
        private Timer timer;

        public static List<StaticObstacle> staticObstacles = new List<StaticObstacle>();
        public static List<DynamicObstacle> dynamicObstacles = new List<DynamicObstacle>();
        private List<Tresure> tresures = new List<Tresure>();

        #endregion

        #region CTOR

        public AnaForm(int xSize, int ySize)
        {
            InitializeComponent();

            SetPctrBoxAndMainFormDisplay();

            mapXSize = xSize;
            mapYSize = ySize;

            cellWidth = pctrBoxMap.Width / mapXSize;
            cellHeight = pctrBoxMap.Height / mapYSize;

            CreateGameObjects();

            fogMap = new bool[mapXSize, mapYSize];

            InitializeFogMap();

            pctrBoxMap.Paint += new PaintEventHandler(DrawGrid);

            runtime = new Runtime(tresures, this, Player);
        }

        #endregion

        #region AUXILIARY METOTHS
        public void StartGame()
        {
            SetDynamicObstaclesTimerSettings();
            SetCharacterTimerSettings();
            DrawFog();
        }

        private void SetDynamicObstaclesTimerSettings()
        {
            timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_TickForDynamicObstacles;
            timer.Start();
        }

        private void SetCharacterTimerSettings()
        {
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Timer_TickForCharacter;
            timer.Start();
        }

        private void SetPctrBoxAndMainFormDisplay()
        {
            this.Width = 1800;
            this.Height = 1000;

            tabControl1.Width = 1750;
            tabControl1.Height = 950;

            pctrBoxMap.Width = 1500;
            pctrBoxMap.Height = 850;
        }

        private void Timer_TickForCharacter(object sender, EventArgs e)
        {
            Player.Move(fogMap, staticObstacles, dynamicObstacles, tresures);
            UpdateFogAndWalkableArea();

            if(Runtime.CollectedTresuresvalue == totalTresures)
            {
                GameOverRemoveFog();
                Player.ClosestTresures(Player.characterStartLocation);
                pctrBoxMap.Invalidate();
                timer.Stop();
            }

            pctrBoxMap.Invalidate();
        }

        private void GameOverRemoveFog()
        {
            for(int i = 0; i < mapXSize; i++)
            {
                for(int j = 0; j < mapYSize; j++)
                {
                    fogMap[i, j] = false;
                }
            }

            pctrBoxMap.Paint -= new PaintEventHandler(DrawMapWithFog); 
            pctrBoxMap.Paint += new PaintEventHandler(DrawGameOver); 
            pctrBoxMap.Invalidate(); 
        }

        private void DrawGameOver(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Izgarayı geri çiz
            for (int i = 0; i < mapXSize; i++)
            {
                for (int j = 0; j < mapYSize; j++)
                {
                    int x = i * cellWidth;
                    int y = j * cellHeight;
                    graphics.DrawRectangle(Pens.Black, x, y, cellWidth, cellHeight);
                    
                }
            }

            foreach (Tresure tresure in Player.tresuresForTest)
            {
                // Hazineleri tekrar çiz
                string imagePath = string.Empty;

                if (tresure is GoldenChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "GoldenChest.jpg");
                else if (tresure is SilverChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "SilverChest.jpg");
                else if (tresure is EmeraldChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "EmeraldChest.jpg");
                else if (tresure is CopperChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "CopperChest.jpg");

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    Rectangle rect = new Rectangle(tresure.Location.XLocation * cellWidth,
                                                    tresure.Location.YLocation * cellHeight,
                                                    cellWidth * tresure.tresureXSize,
                                                    cellHeight * tresure.tresureYSize);
                    Image image = Image.FromFile(imagePath);
                    graphics.DrawImage(image, rect);
                }
            }

            int totalCount = Player.shortestPathLocations.Count;
            int index = 0;

            Runtime.minimumStepvalue = totalCount;

            foreach (Location location in Player.shortestPathLocations)
            {
                double colorMultiple = 1.0 * index / totalCount;

                Color color = InterpolateColor(Color.Green, Color.Blue, colorMultiple);

                Rectangle cellRect = new Rectangle(location.XLocation * cellWidth, location.YLocation * cellHeight, cellWidth, cellHeight);
                
                SolidBrush brush = new SolidBrush(color);
                g.FillRectangle(brush, cellRect);
                index++;
            }
            lblMinimunStepValue.Text = $"En kısa yol için adım sayısı: {Runtime.minimumStepvalue}";
        }

        private Color InterpolateColor(Color startColor, Color endColor, double colorMultiple)
        {
            int r = (int)(startColor.R + (endColor.R - startColor.R) * colorMultiple);
            int g = (int)(startColor.G + (endColor.G - startColor.G) * colorMultiple);
            int b = (int)(startColor.B + (endColor.B - startColor.B) * colorMultiple);

            return Color.FromArgb(r, g, b);
        }

        private void Timer_TickForDynamicObstacles(object sender, EventArgs e)
        {
            foreach (DynamicObstacle dynamicObstacle in dynamicObstacles)
                dynamicObstacle.Move();

            if (Runtime.CollectedTresuresvalue == totalTresures)
                timer.Stop();

            pctrBoxMap.Invalidate();
        }

        private bool CheckCollisionWithExistingObjects(int x, int y, int entityXSize, int entityYSize)
        {

            foreach (StaticObstacle obstacle in staticObstacles)
            {
                if (obstacle.Bounds.IntersectsWith(new Rectangle(x, y, entityXSize + 3, entityYSize + 3)))
                    return true;
            }

            foreach (Tresure tresure in tresures)
            {
                if (tresure.Bounds.IntersectsWith(new Rectangle(x, y, entityXSize + 3, entityYSize + 3)))
                    return true;
            }

            foreach (DynamicObstacle dynamicObstacle in dynamicObstacles)
            {
                if (dynamicObstacle.Bounds.IntersectsWith(new Rectangle(x, y, entityXSize + 3, entityYSize + 3)))
                    return true;
            }

            if (Player.Bounds.IntersectsWith(new Rectangle(x, y, entityXSize, entityYSize)))
                return true;

            return false;
        }

        #region METOTH ABOUT CREATING GAME OBJECTS

        private void CreateGameObjects()
        {
            CreatePlayer();
            CreateStaticObstacles();
            CreateDynamicObstacles();
            CreateTresures();
        }

        private void CreatePlayer()
        {
            Player = new Character(1, "Player", random.Next(0, mapXSize), random.Next(0, mapYSize),
                1, 1, this, tresures, staticObstacles, dynamicObstacles);
        }

        #region METOTHS ABOUT TRESURES

        private void CreateTresures()
        {
            tresures.Clear();

            totalTresures = 20; // 4 adet her türden toplam 16 hazine

            for (int i = 0; i < totalTresures / 4; i++)
            {
                CreateRandomTresure(typeof(GoldenChest));
                CreateRandomTresure(typeof(SilverChest));
                CreateRandomTresure(typeof(EmeraldChest));
                CreateRandomTresure(typeof(CopperChest));
            }
        }

        private void CreateRandomTresure(Type tresureType)
        {
            int tresureXPosition;
            int tresureYPosition;
            int tresureSize = 3; // Hazineler her zaman 3 x 3

            do
            {
                tresureXPosition = random.Next(0, mapXSize - 5);
                tresureYPosition = random.Next(0, mapYSize - 5);
            } while (CheckCollisionWithExistingObjects(tresureXPosition, tresureYPosition, tresureSize, tresureSize));

            if (tresureType == typeof(GoldenChest))
                tresures.Add(new GoldenChest(tresureXPosition, tresureYPosition, tresureSize, tresureSize));
            else if (tresureType == typeof(SilverChest))
                tresures.Add(new SilverChest(tresureXPosition, tresureYPosition, tresureSize, tresureSize));
            else if (tresureType == typeof(EmeraldChest))
                tresures.Add(new EmeraldChest(tresureXPosition, tresureYPosition, tresureSize, tresureSize));
            else if (tresureType == typeof(CopperChest))
                tresures.Add(new CopperChest(tresureXPosition, tresureYPosition, tresureSize, tresureSize));
        }

        #endregion

        #region METOTHS ABOUT DYNAMIC OBSTACLES

        private void CreateDynamicObstacles()
        {
            dynamicObstacles.Clear();

            int totalObstacles = 2;

            for (int i = 0; i < totalObstacles; i++)
            {
                CreateRandomDynamicObstacle(0);
                CreateRandomDynamicObstacle(1);
            }
        }

        private void CreateRandomDynamicObstacle(int obstacleType)
        {
            int obstacleXPosition = 0;
            int obstacleYPosition = 0;
            int obstacleRouteXSize = 0;
            int obstacleRouteYSize = 0;

            if (obstacleType == 0) { obstacleRouteXSize = 3; obstacleRouteYSize = 2; }
            else { obstacleRouteXSize = 2; obstacleRouteYSize = 5; }

            do
            {
                obstacleXPosition = random.Next(0, mapXSize - obstacleRouteXSize - 3);
                obstacleYPosition = random.Next(0, mapYSize - obstacleRouteYSize - 3);

            } while (CheckCollisionWithExistingObjects(obstacleXPosition, obstacleYPosition, obstacleRouteXSize, obstacleRouteYSize));

            if (obstacleType == 0) // Bee
                dynamicObstacles.Add(new Bee(obstacleXPosition, obstacleYPosition, obstacleRouteXSize, obstacleRouteYSize));
            else if (obstacleType == 1)// Bird
                dynamicObstacles.Add(new Bird(obstacleXPosition, obstacleYPosition, obstacleRouteXSize, obstacleRouteYSize));
        }

        #endregion

        #region METOTHS ABOUT STATIC OBSTACLES

        private void CreateStaticObstacles()
        {
            staticObstacles.Clear();

            for (int i = 0; i < 8; i++)
            {
                CreateRandomStaticObstacle(0);
                CreateRandomStaticObstacle(1);
            }

            for (int i = 0; i < 2; i++)
            {
                CreateRandomStaticObstacle(2);
                CreateRandomStaticObstacle(3);
            }
        }

        private void CreateRandomStaticObstacle(int obstacleType)
        {
            int seasonType = random.Next(0, 2); // 0: Summer, 1: Winter

            int obstacleXPosition;
            int obstacleYPosition;
            int obstacleXSize = 0;
            int obstacleYSize = 0;

            if (obstacleType == 0) // rock
            {
                obstacleXSize = random.Next(2, 4);
                obstacleYSize = obstacleXSize;
            }
            else if (obstacleType == 1) // Tree
            {
                obstacleXSize = random.Next(2, 6);
                obstacleYSize = obstacleXSize;
            }
            else if (obstacleType == 2) // Mountain
            {
                obstacleXSize = 15;
                obstacleYSize = 15;
            }
            else if (obstacleType == 3) // Wall
            {
                obstacleXSize = 10;
                obstacleYSize = 1;
            }

            do
            {
                if (seasonType == 0) // Summer
                    obstacleXPosition = random.Next(mapXSize / 2, mapXSize - obstacleXSize - 3);
                else // Winter
                    obstacleXPosition = random.Next(0, mapXSize / 2);

                obstacleYPosition = random.Next(0, mapYSize - obstacleYSize - 3);
            } while (CheckCollisionWithExistingObjects(obstacleXPosition, obstacleYPosition, obstacleXSize, obstacleYSize));

            if (obstacleType == 0) // Rock
                staticObstacles.Add(new Rock(obstacleXPosition, obstacleYPosition, obstacleXSize, obstacleYSize, seasonType));
            else if (obstacleType == 1) // Tree
                staticObstacles.Add(new Tree(obstacleXPosition, obstacleYPosition, obstacleXSize, obstacleYSize, seasonType));
            else if (obstacleType == 2) // Mountain
                staticObstacles.Add(new Mountain(obstacleXPosition, obstacleYPosition, obstacleXSize, obstacleYSize, seasonType));
            else if (obstacleType == 3) // Wall
                staticObstacles.Add(new Wall(obstacleXPosition, obstacleYPosition, obstacleXSize, obstacleYSize, seasonType));
        }

        #endregion

        #endregion

        #endregion

        #region DRAW METHOTS

        private void DrawGrid(object sender, PaintEventArgs e)
        {
            if (mapXSize == 0 || mapYSize == 0) return;

            graphics = e.Graphics;

            graphics.Clear(Color.White);

            if(!ısGridDrawed)
            {
                for (int i = 0; i < mapXSize; i++)
                {
                    for (int j = 0; j < mapYSize; j++)
                    {
                        int x = i * cellWidth;
                        int y = j * cellHeight;
                        graphics.DrawRectangle(Pens.Black, x, y, cellWidth, cellHeight);
                    }
                }

                ısGridDrawed = true;
            }
            

            DrawImages();
        }

        #region DRAW GAME OBJECTS

        private void DrawImages()
        {
            Rectangle playerRect = new Rectangle(Player.Location.XLocation * cellWidth, Player.Location.YLocation * cellHeight, cellWidth * Player.Bounds.Width, cellHeight * Player.Bounds.Height);
            Image playerImage = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Sprites", "Player.jpg"));
            graphics.DrawImage(playerImage, playerRect);

            DrawStaticObstacles();
            DrawDynamicObstacles();
            DrawTresures();
        }

        private void DrawStaticObstacles()
        {
            foreach (StaticObstacle obstacle in staticObstacles)
            {
                string imagePath = string.Empty;

                if (obstacle is Rock && obstacle.seasonType == 0)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "RockSummer.jpg");
                else if (obstacle is Rock && obstacle.seasonType == 1)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "RockWinter.jpg");
                else if (obstacle is Mountain && obstacle.seasonType == 0)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "MountainSummer.jpg");
                else if (obstacle is Mountain && obstacle.seasonType == 1)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "MountainWinter.jpg");
                else if (obstacle is Wall && obstacle.seasonType == 0)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "WallSummer.jpg");
                else if (obstacle is Wall && obstacle.seasonType == 1)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "WallWinter.jpg");
                else if (obstacle is Tree && obstacle.seasonType == 0)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "TreeSummer.jpg");
                else if (obstacle is Tree && obstacle.seasonType == 1)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "TreeWinter.jpg");

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    Rectangle rect = new Rectangle(obstacle.Location.XLocation * cellWidth,
                                                    obstacle.Location.YLocation * cellHeight,
                                                    cellWidth * obstacle.obstacleXSize,
                                                    cellHeight * obstacle.obstacleYSize);
                    Image image = Image.FromFile(imagePath);
                    graphics.DrawImage(image, rect);
                }
            }
        }

        private void DrawDynamicObstacles()
        {
            foreach (DynamicObstacle dynamicObstacle in dynamicObstacles)
            {
                string imagePath = string.Empty;

                if (dynamicObstacle is Bee)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "Bee.jpg");
                else if (dynamicObstacle is Bird)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "Bird.jpg");

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    Rectangle routeRect = new Rectangle(dynamicObstacle.Location.XLocation * cellWidth,
                                                    dynamicObstacle.Location.YLocation * cellHeight,
                                                    cellWidth * dynamicObstacle.obstacleXSize,
                                                    cellHeight * dynamicObstacle.obstacleYSize);

                    Rectangle spriteRect = new Rectangle(dynamicObstacle.SpriteLocation.XLocation * cellWidth,
                                dynamicObstacle.SpriteLocation.YLocation * cellHeight,
                                cellWidth * dynamicObstacle.obstacleSpriteXSize,
                                cellHeight * dynamicObstacle.obstacleSpriteYSize);

                    Image image = Image.FromFile(imagePath);

                    graphics.FillRectangle(Brushes.Red, routeRect);
                    graphics.DrawImage(image, spriteRect);
                }
            }
        }

        private void DrawTresures()
        {
            foreach (Tresure tresure in tresures)
            {
                // Hazine görüntülerini çiz
                string imagePath = string.Empty;

                if (tresure is GoldenChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "GoldenChest.jpg");
                else if (tresure is SilverChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "SilverChest.jpg");
                else if (tresure is EmeraldChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "EmeraldChest.jpg");
                else if (tresure is CopperChest)
                    imagePath = Path.Combine(Environment.CurrentDirectory, "Sprites", "CopperChest.jpg");

                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    Rectangle rect = new Rectangle(tresure.Location.XLocation * cellWidth,
                                                    tresure.Location.YLocation * cellHeight,
                                                    cellWidth * tresure.tresureXSize,
                                                    cellHeight * tresure.tresureYSize);
                    Image image = Image.FromFile(imagePath);
                    graphics.DrawImage(image, rect);
                }
            }
        }

        #endregion

        #region DRAW FOG

        private void DrawFog()
        {
            pctrBoxMap.Paint += new PaintEventHandler(DrawMapWithFog);
            UpdateFogAndWalkableArea();
        }

        private void DrawMapWithFog(object sender, PaintEventArgs e)
        {
            DrawFog(e.Graphics);
        }

        private void InitializeFogMap()
        {
            for (int i = 0; i < mapXSize; i++)
            {
                for (int j = 0; j < mapYSize; j++)
                {
                    fogMap[i, j] = true;
                }
            }
        }

        public void UpdateFogAndWalkableArea()
        {
            int playerX = Player.Location.XLocation;
            int playerY = Player.Location.YLocation;

            for (int i = playerX - 3; i <= playerX + 2 + Player.Bounds.Width; i++)
            {
                for (int j = playerY - 3; j <= playerY + 2 + Player.Bounds.Height; j++)
                {
                    if (i >= 0 && i < mapXSize && j >= 0 && j < mapYSize)
                    {
                        fogMap[i, j] = false;

                        foreach (StaticObstacle staticObstacle in staticObstacles)
                        {
                            if ((i >= staticObstacle.Location.XLocation && i <= staticObstacle.Location.XLocation + staticObstacle.Bounds.Width &&
                                j >= staticObstacle.Location.YLocation && j <= staticObstacle.Location.YLocation + staticObstacle.Bounds.Height))
                            {
                                for (int a = staticObstacle.Location.XLocation; a < staticObstacle.Location.XLocation + staticObstacle.Bounds.Width; a++)
                                {
                                    for (int b = staticObstacle.Location.YLocation; b < staticObstacle.Location.YLocation + staticObstacle.Bounds.Height; b++)
                                    {
                                        fogMap[a, b] = false;
                                    }
                                }

                                runtime.DiscoverStaticObstacle(staticObstacle);
                                break;
                            }
                        }

                        foreach (DynamicObstacle dynamicObstacle in dynamicObstacles)
                        {
                            if ((i >= dynamicObstacle.Location.XLocation && i <= dynamicObstacle.Location.XLocation + dynamicObstacle.Bounds.Width &&
                                j >= dynamicObstacle.Location.YLocation && j <= dynamicObstacle.Location.YLocation + dynamicObstacle.Bounds.Height))
                            {
                                for (int a = dynamicObstacle.Location.XLocation; a < dynamicObstacle.Location.XLocation + dynamicObstacle.Bounds.Width; a++)
                                {
                                    for (int b = dynamicObstacle.Location.YLocation; b < dynamicObstacle.Location.YLocation + dynamicObstacle.Bounds.Height; b++)
                                    {
                                        fogMap[a, b] = false;
                                    }
                                }

                                runtime.DiscoverDynamicObstacle(dynamicObstacle);
                                break;
                            }
                        }

                        foreach (Tresure tresure in tresures)
                        {
                            if (i >= tresure.Location.XLocation && i <= tresure.Location.XLocation + tresure.Bounds.Width &&
                                j >= tresure.Location.YLocation && j <= tresure.Location.YLocation + tresure.Bounds.Height)
                            {
                                for (int a = tresure.Location.XLocation; a < tresure.Location.XLocation + tresure.Bounds.Width; a++)
                                {
                                    for (int b = tresure.Location.YLocation; b < tresure.Location.YLocation + tresure.Bounds.Height; b++)
                                    {
                                        fogMap[a, b] = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawFog(Graphics g)
        {
            for (int i = 0; i < mapXSize; i++)
            {
                for (int j = 0; j < mapYSize; j++)
                {
                    if (fogMap[i, j])
                    {
                        Rectangle cellRect = new Rectangle(i * cellWidth, j * cellHeight, cellWidth, cellHeight);
                        g.FillRectangle(Brushes.LightGray, cellRect);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
