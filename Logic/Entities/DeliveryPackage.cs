using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    /// <summary>
    /// Delivery package.
    /// </summary>
    public class DeliveryPackage : AbstractEntity
    {
        private string heading;
        private int groupSize;
        private int trainingSessions;

        public DeliveryPackage()
        {
        }

        #region Properties
        public string Heading
        {
            get { return heading; }
            set { heading = value; }
        }
        public int GroupSize
        {
            get { return groupSize; }
            set { groupSize = value; }
        }
        public int TrainingSessions
        {
            get { return trainingSessions; }
            set { trainingSessions = value; }
        }
        #endregion
    }
}
