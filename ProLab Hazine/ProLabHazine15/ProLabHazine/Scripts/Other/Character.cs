using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProLabHazine
{
    public class Character
    {
        #region PROPS, VALUES AND OBJECTS

        public int ID { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public Rectangle Bounds { get; set; }

        private int sayac = 0;
        public Location characterStartLocation = null;

        public bool isGameOver = false;

        AnaForm anaForm;

        public HashSet<Location> visitedArea = new HashSet<Location>();
        public HashSet<Location> shortestPathLocations = new HashSet<Location>();
        HashSet<Point> discoveredObstaclePositions;

        Runtime runtime;

        public List<Tresure> tresuresForTest = new List<Tresure>();

        List<Tresure> tresures;
        List<StaticObstacle> staticObstacles;
        List<DynamicObstacle> dynamicObstacles;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            None
        }
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        #endregion

        #region CTOR
        public Character(int id, string name, int x, int y, int playerXSize, int playerYSize, AnaForm _anaForm,
            List<Tresure> _tresures, List<StaticObstacle> _staticObstacles, List<DynamicObstacle> _dynamicObstacles)
        {
            ID = id;
            Name = name;
            Location = new Location(x, y);

            characterStartLocation = new Location(x, y);

            Bounds = new Rectangle(x, y, playerXSize, playerYSize);
            anaForm = _anaForm;

            tresures = _tresures;
            staticObstacles = _staticObstacles;
            dynamicObstacles = _dynamicObstacles;

            runtime = new Runtime(_tresures, _anaForm, this);

            discoveredObstaclePositions = new HashSet<Point>();
        }

        #endregion

        #region AUXILIARY METHOTS

        #region CLOSEST PATH

        public void ClosestTresures(Location startLocation)
        {
            while (runtime.discoveredTresures.Count != 0)
            {
                Tresure closestTresure = null;
                Location closestTresureLocation = new Location(-1, -1);
               double distanceBetweenLocations = double.MaxValue;

                foreach (Tresure tresure in runtime.discoveredTresures)
                {
                    if (distanceBetweenLocations > CalculateDistance(startLocation, tresure.Location))
                    {
                        distanceBetweenLocations = CalculateDistance(startLocation, tresure.Location);
                        closestTresureLocation = tresure.Location;
                        closestTresure = tresure;
                    }
                }

                if (closestTresure != null)
                {
                    FindClosetsPath(startLocation, closestTresureLocation);

                    startLocation.XLocation = closestTresure.Location.XLocation;
                    startLocation.YLocation = closestTresure.Location.YLocation;

                    tresuresForTest.Add(closestTresure);
                    runtime.discoveredTresures.Remove(closestTresure);
                }
            }
        }

        public void FindClosetsPath(Location startLocation, Location closestTresureLocation)
        {
            shortestPathLocations.Add(closestTresureLocation);

            Location currentLocation = closestTresureLocation;
            while (currentLocation.XLocation != startLocation.XLocation || currentLocation.YLocation != startLocation.YLocation)
            {
                Location nextLocation = GetNextClosestLocation(currentLocation, startLocation);
                shortestPathLocations.Add(nextLocation);
                currentLocation = nextLocation;
            }

            foreach (Location location in shortestPathLocations)
            {
                if(!shortestPathLocations.Any(loc => loc.XLocation == location.XLocation && loc.YLocation == location.YLocation))
                    shortestPathLocations.Add(location);
            }
        }

        private Location GetNextClosestLocation(Location currentLocation, Location startLocation)
        {
            int currentX = currentLocation.XLocation;
            int currentY = currentLocation.YLocation;

            if (currentX < startLocation.XLocation)
                currentX++;
            else if (currentX > startLocation.XLocation)
                currentX--;
            else if (currentY < startLocation.YLocation)
                currentY++;
            else if (currentY > startLocation.YLocation)
                currentY--;

            return new Location(currentX, currentY);
        }

        #endregion

        #region CONTROLL METHOTS

        private bool IsValidMove(int nextXLocation, int nextYLocation) // ?
        {
            if (nextXLocation < 0 || nextYLocation < 0 || (nextXLocation) >= AnaForm.fogMap.GetLength(0) || (nextYLocation) >= AnaForm.fogMap.GetLength(1))
                return false;

            foreach (StaticObstacle obstacle in staticObstacles)
                if (obstacle.Bounds.Contains(nextXLocation, nextYLocation))
                {
                    runtime.DiscoverStaticObstacle(obstacle);

                    return false;
                }

            foreach (DynamicObstacle obstacle in dynamicObstacles)
                if (obstacle.Bounds.Contains(nextXLocation, nextYLocation))
                {
                    runtime.DiscoverDynamicObstacle(obstacle);

                    return false;
                }

            return true;
        }

        private static bool IsInsideMap(int x, int y, int width, int height)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private bool IsPositionBlocked(int x, int y)
        {
            Point playerPosition = new Point(Location.XLocation, Location.YLocation);

            if (playerPosition.Equals(new Point(x, y)) || discoveredObstaclePositions.Contains(new Point(x, y)))
                return true;

            return false;
        }

        private bool IsAnyObstacleBetweenTheTargetAndPlayer(int targetPositionX, int targetPositionY)
        {
            Point targetPosition = new Point(targetPositionX, targetPositionY);
            Point playerPosition = new Point(Location.XLocation, Location.YLocation);

            if (targetPosition.Equals(playerPosition))
                return false;

            int dx = Math.Sign(targetPositionX - playerPosition.X);
            int dy = Math.Sign(targetPositionY - playerPosition.Y);

            int x = playerPosition.X;
            int y = playerPosition.Y;

            while (x != targetPositionX || y != targetPositionY)
            {
                if (IsPositionBlocked(x + dx, y + dy))
                {
                    sayac++;
                    return true;
                }

                if (x != targetPositionX)
                    x += dx;

                if (y != targetPositionY)
                    y += dy;
            }

            return false;
        }

        #endregion

        #region CALCULATION METHOTS

        private bool IsSmokeAreaBigEnough(int targetX, int targetY)
        {
            for (int i = targetX - 1; i <= targetX + 1; i++)
            {
                for (int j = targetY - 1; j <= targetY + 1; j++)
                {
                    if (IsInsideMap(i, j, anaForm.mapXSize, anaForm.mapYSize) && !AnaForm.fogMap[i, j])
                        return false;
                }
            }

            return true;
        }
        public static double CalculateDistance(Location point1, Location point2)
        {
            int deltaX = point2.XLocation - point1.XLocation;
            int deltaY = point2.YLocation - point1.YLocation;

            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            return distance;
        }
        
        #endregion

        #endregion

        #region DIRACTION AND ROTATION METHOTS

        public Direction FindNearestFoggyDirection()
        {
            int startX = Location.XLocation;
            int startY = Location.YLocation;

            int width = AnaForm.fogMap.GetLength(0);
            int height = AnaForm.fogMap.GetLength(1);

            List<Direction> availableDirections = new List<Direction>();
            List<Direction> reserveAvailableDirections = new List<Direction>();

            for (int radius = 1; radius <= Math.Max(width, height); radius++)
            {
                Point tresurePoint = RotateForTresure();

                if (tresurePoint.X != -1)
                {
                    availableDirections.Clear();

                    if (Location.XLocation < tresurePoint.X)
                        availableDirections.Add(Direction.Right);
                    else if (Location.XLocation > tresurePoint.X)
                        availableDirections.Add(Direction.Left);

                    if (Location.YLocation < tresurePoint.Y)
                        availableDirections.Add(Direction.Down);
                    else if (Location.YLocation > tresurePoint.Y)
                        availableDirections.Add(Direction.Up);

                    Random random = new Random();
                    int index = random.Next(0, availableDirections.Count);
                    return availableDirections[index];
                }

                for (int i = startX - radius; i <= startX + radius; i++)
                {
                    for (int j = startY - radius; j <= startY + radius; j++)
                    {
                        if (IsInsideMap(i, j, width, height) && AnaForm.fogMap[i, j])
                        {
                            if (IsValidMove(i, j) && !IsAnyObstacleBetweenTheTargetAndPlayer(i, j) )
                            {
                                if(IsSmokeAreaBigEnough(i, j))
                                {
                                    if (i < startX)
                                        availableDirections.Add(Direction.Left);
                                    else if (i > startX)
                                        availableDirections.Add(Direction.Right);

                                    if (j < startY)
                                        availableDirections.Add(Direction.Up);
                                    else if (j > startY)
                                        availableDirections.Add(Direction.Down);
                                }
                                else
                                {
                                    if (i < startX)
                                        reserveAvailableDirections.Add(Direction.Left);
                                    else if (i > startX)
                                        reserveAvailableDirections.Add(Direction.Right);

                                    if (j < startY)
                                        reserveAvailableDirections.Add(Direction.Up);
                                    else if (j > startY)
                                        reserveAvailableDirections.Add(Direction.Down);
                                }
                            }
                        }
                    }
                }

                if (availableDirections.Count > 0)
                {
                    Random random = new Random();
                    int index = random.Next(0, availableDirections.Count);
                    return availableDirections[index];
                }
            }

            if (availableDirections.Count == 0)
            {
                if (reserveAvailableDirections.Count != 0)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(0, reserveAvailableDirections.Count);
                    return reserveAvailableDirections[randomIndex];
                }
                else
                {
                    List<Direction> allDirections = new List<Direction> { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
                    Random random = new Random();
                    int randomIndex = random.Next(0, allDirections.Count);
                    return allDirections[randomIndex];
                }
            }

            return Direction.None;
        }

        private Point RotateForTresure()
        {
            Point tresurePoint = new Point(-1, -1);
            Tresure tresureWithLowestPriority = null;

            foreach (Tresure tresure in tresures)
            {
                if (!AnaForm.fogMap[tresure.Location.XLocation, tresure.Location.YLocation])
                {
                    if (tresureWithLowestPriority == null || tresure.tresurePriority < tresureWithLowestPriority.tresurePriority)
                    {
                        tresurePoint.X = tresure.Location.XLocation;
                        tresurePoint.Y = tresure.Location.YLocation;
                        tresureWithLowestPriority = tresure;
                    }
                }
            }

            return tresurePoint;
        }

        public void FindObstaclePoints()
        {
            foreach (StaticObstacle obstacle in anaForm.runtime.discoveredStaticObstacles)
            {
                for (int i = obstacle.Location.XLocation; i < obstacle.Location.XLocation + obstacle.Bounds.Width; i++)
                {
                    for (int j = obstacle.Location.YLocation; j < obstacle.Location.YLocation + obstacle.Bounds.Height; j++)
                    {
                        discoveredObstaclePositions.Add(new Point(i, j));
                    }
                }
            }

            foreach (DynamicObstacle obstacle in anaForm.runtime.discoveredDynamicObstacles)
            {
                for (int i = obstacle.Location.XLocation; i < obstacle.Location.XLocation + obstacle.Bounds.Width; i++)
                {
                    for (int j = obstacle.Location.YLocation; j < obstacle.Location.YLocation + obstacle.Bounds.Height; j++)
                    {
                        discoveredObstaclePositions.Add(new Point(i, j));
                    }
                }
            }
        }

        public void Move(bool[,] fogMap, List<StaticObstacle> staticObstacles, List<DynamicObstacle> dynamicObstacles, List<Tresure> tresures)
        {
            if (anaForm.runtime != null)
                runtime = anaForm.runtime;

            if (Runtime.CollectedTresuresvalue == anaForm.totalTresures)
                return;

            int newX = Location.XLocation;
            int newY = Location.YLocation;

            switch (FindNearestFoggyDirection())
            {
                case Direction.Up:
                    newY -= 1;
                    break;
                case Direction.Down:
                    newY += 1;
                    break;
                case Direction.Left:
                    newX -= 1;
                    break;
                case Direction.Right:
                    newX += 1;
                    break;
            }

            if (IsValidMove(newX, newY))
            {
                Location.XLocation = newX;
                Location.YLocation = newY;
                Runtime.totalStepValue++;
                anaForm.lblTotalStepValue.Text = $"Toplam adım sayısı: {Runtime.totalStepValue}";
            }

            isGameOver = runtime.CollectTresures(newX, newY);

                for (int i = Location.XLocation; i <= Location.XLocation + Bounds.Width; i++)
                    for (int j = Location.YLocation; j <= Location.YLocation + Bounds.Height; j++)
                        visitedArea.Add(new Location(i, j));
        }

        #endregion
    }
}

