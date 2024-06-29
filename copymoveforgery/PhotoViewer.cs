using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace copymoveforgery
{
    class PhotoViewer
    {
        private PictureBox PB;
       public PhotoViewer()
        {

        }

        public void  setViewer(PictureBox PB)
        {
	        this.PB = PB;
        }

        public void  showImage(DigitalPhoto  foto)
        {
	        int row = foto.getRow();
	        int colomn = foto.getColomn();
           // MessageBox.Show(colomn.ToString());
	        Bitmap img = new  Bitmap(colomn,row);
	        for(int i=0;i<row;i++)
	        {
	         for(int j=0;j<colomn;j++)
	         {
		         int pixel = (int)foto.getPixelOutput(j,i);
		         if (pixel<0)
		         {
			        pixel=0;
		         }
		         else if(pixel>255)
		         { 
			         pixel= 255;
		         }
		        Color cr = Color.FromArgb(255,pixel,pixel,pixel);

              //   Color cr = Color.FromArgb(pixel);
               //  Color ccr = Color.FromArgb(255, cr.R, cr.G, cr.B);

		         img.SetPixel(j,i,cr);
		
	          }		 
	        }
	        PB.Image = img;
		
        }

        public void  showForgeryImage(DigitalPhoto foto,List<Pair>  pairforged)
        {
	        int row = foto.getRow();
	        int colomn = foto.getColomn();
	        Bitmap img = new  Bitmap(row,colomn);
	        for(int i=0;i<row;i++)
	        {
	         for(int j=0;j<colomn;j++)
	         {
		         int pixel = (int)foto.getPixelOutput(j,i);
		         if (pixel<0)
		         {
			        pixel=0;
		         }
		         else if(pixel>255)
		         { 
			         pixel= 255;
		         }
		         Color cr = Color.FromArgb(255,pixel,pixel,pixel);
		         img.SetPixel(j,i,cr);
		
	          }		 
	        }


	        for (int i=0;i<pairforged.Count;i++)
	        {
	 	        Point pi = pairforged[i].get_point1();
		        Point pj = pairforged[i].get_point2();
		
		        for(int j=0;j<8;j++) 
		        {
			        for (int k=0;k<8;k++)
			        {
                        int w = pi.X + j;
                        int h = pi.Y + k;
                        if (w >= foto.getColomn() || h>=foto.getRow())
                        {
                            break;
                        }
                        w = pj.X + j;
                        h = pj.Y + k;

                        if (w >= foto.getColomn() || h >= foto.getRow())
                        {
                            break;
                        }

				        int pixel = (int) foto.getPixelOutput(pi.X+j,pi.Y+k);
                        Color cr = Color.FromArgb(255,pixel,0,0);
				        img.SetPixel(pi.X+j,pi.Y+k,cr);  
				        pixel = (int) foto.getPixelOutput(pj.X+j,pj.Y+k);
                        cr = Color.FromArgb(255,pixel,0,0);
				        img.SetPixel(pj.X+j,pj.Y+k,cr);
			        }
		        }
	        }
	        PB.Image = img;
		
        }

    }
}
