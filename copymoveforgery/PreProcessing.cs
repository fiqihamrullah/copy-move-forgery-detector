using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; 

namespace copymoveforgery
{
    class PreProcessing
    {

            public PreProcessing()
            {

            }
            public void  grayscale(DigitalPhoto  foto)
            {
	            int row = foto.getRow();
	            int colomn = foto.getColomn();
	            for(int i=0;i<row;i++)
	            {

	             for(int j=0;j<colomn;j++)
	             {
		             int pixel = foto.getPixelInput(j,i);
		             Color c = Color.FromArgb(pixel);
		             int gray = (c.R+c.G+c.B)/3;
                     System.Diagnostics.Trace.Write(gray.ToString() + "\t");
		             foto.setPixelOutput(gray,j,i);
	              }
                 System.Diagnostics.Trace.WriteLine("");
	            }
            }

    }
}
