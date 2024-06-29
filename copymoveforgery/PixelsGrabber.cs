using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace copymoveforgery
{
    class PixelsGrabber
    {
        private Bitmap img;
	    private DigitalPhoto foto;
        public PixelsGrabber()
        {
	 
        }

        public PixelsGrabber(DigitalPhoto foto)
        {
	        this.foto = foto;
        }

        public void  grabPixel(Bitmap img, DigitalPhoto foto)
        {
	        int row = foto.getRow();
	        int colomn = foto.getColomn();

	        for(int i=0;i<row;i++)
	        {

	             for(int j=0;j<colomn;j++)
	             {
			        Color c = img.GetPixel(j,i);
			        foto.setPixelInput(Color.FromArgb(0,c.R,c.G,c.B).ToArgb(),j,i);

                    String strpxl = c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString();
                    System.Diagnostics.Trace.Write(strpxl + "\t");
	             }
                 System.Diagnostics.Trace.WriteLine("");
	        }

        }

    }
}
