using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace copymoveforgery
{
    class Pair
    {
        Point Point_1;
	    Point Point_2;

        public Pair()
        {
        }

        public void set_point1(Point  Point_1)
        {
	        this.Point_1 = Point_1;
        }

        public void  set_point2(Point Point_2)
        {
	        this.Point_2 = Point_2;
        }

        public Point   get_point1()
        {
	        return Point_1;
        }

        public Point  get_point2()
        {
	        return Point_2;
        }





    }
}
