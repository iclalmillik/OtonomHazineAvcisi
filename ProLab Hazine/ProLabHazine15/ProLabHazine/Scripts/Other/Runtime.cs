using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProLabHazine
{
    public class Runtime
    {
        #region PROPS, OBJECTS AND VALUES

        public static int totalStepValue;
        public static int minimumStepvalue;

        public static int CollectedTresuresvalue;
        private List<Tresure> tresures;
       
        AnaForm anaForm;
        Character character;

        public List<DynamicObstacle> discoveredDynamicObstacles;
        public List<StaticObstacle> discoveredStaticObstacles;
        public List<Tresure> discoveredTresures;

        #endregion

        #region CTOR

        public Runtime(List<Tresure> _tresures,AnaForm _anaForm, Character _character)
        {
            discoveredStaticObstacles = new List<StaticObstacle>();
            discoveredDynamicObstacles = new List<DynamicObstacle>();
            discoveredTresures = new List<Tresure>();

            tresures = _tresures;
            anaForm = _anaForm;
            character = _character;
        }

        #endregion

        #region METHOTS

        public bool CollectTresures(int nextXLocation, int nextYLocation)
        {
            bool isGameOver = false;

            foreach (Tresure tresure in tresures)
                if (tresure.Bounds.Contains(nextXLocation, nextYLocation)) // !
                {
                    string obstacleInfo = $"tresure found:{tresure.Location.XLocation},{tresure.Location.YLocation},{tresure.tresureName} ";
                    if (!anaForm.lstBoxGameObjects.Items.Contains(obstacleInfo))
                    {
                        discoveredTresures.Add(tresure);

                        anaForm.lstBoxGameObjects.Items.Add(obstacleInfo);
                        CollectedTresuresvalue++;
                    }

                    anaForm.lblCollectedTresuresNumber.Text = $"Toplam hazine sayisi: {CollectedTresuresvalue}";

                    if (CollectedTresuresvalue == anaForm.totalTresures)
                    {
                        isGameOver = true;
                        anaForm.lblCollectedLastTresure.Text = obstacleInfo;
                        anaForm.lblLastTresureStepValue.Text = $"Son hazineye ulaşıldıginda adim sayisi: {totalStepValue}";
                    }
                  
                    tresures.Remove(tresure);
                    break;
                }

            return isGameOver;
        }

        public void DiscoverStaticObstacle(StaticObstacle obstacle)
        {
            string obstacleInfo = $"static obstacle found:{obstacle.Location.XLocation},{obstacle.Location.YLocation},{obstacle.obstacleName} ";
            if (!anaForm.lstBoxGameObjects.Items.Contains(obstacleInfo))
            {
                discoveredStaticObstacles.Add(obstacle);
                character.FindObstaclePoints();

                anaForm.lstBoxGameObjects.Items.Add(obstacleInfo);
            }

        }

        public void DiscoverDynamicObstacle(DynamicObstacle obstacle)
        {
            string obstacleInfo = $"Dynamic obstacle found:{obstacle.Location.XLocation},{obstacle.Location.YLocation},{obstacle.obstacleName} ";
            if (!anaForm.lstBoxGameObjects.Items.Contains(obstacleInfo))
            {
                discoveredDynamicObstacles.Add(obstacle);
                character.FindObstaclePoints();

                anaForm.lstBoxGameObjects.Items.Add(obstacleInfo);
            }

        }

        #endregion
    }
}
