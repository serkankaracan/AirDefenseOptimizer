using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirDefenseOptimizer.FuzzyLogic
{
    public class FuzzyLogicController
    {
        private readonly FuzzyLogicService _fuzzyLogicService;

        public FuzzyLogicController()
        {
            _fuzzyLogicService = new FuzzyLogicService();
        }

        public double CalculateOptimalEngagement(double speed, double distance)
        {
            return _fuzzyLogicService.CalculateThreatLevel(speed, distance);
        }
    }

}
