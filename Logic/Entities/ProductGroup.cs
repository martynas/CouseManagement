using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Logic.Entities
{
    public class ProductGroup : AbstractEntity
    {

        private string heading;
        private string code;

        public ProductGroup()
        {
        }

        public string Heading
        {
            get { return heading; }
            internal set { heading = value; }
        }

    }
}
