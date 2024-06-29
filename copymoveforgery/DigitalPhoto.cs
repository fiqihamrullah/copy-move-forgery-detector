using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace copymoveforgery
{
    class DigitalPhoto
    {
       private int colomn;
	   private int row;
	   private int pixel;
	   private int[,] citraInput;
	   private double[,] citraOutput;
	   private DigitalPhoto[] block;
	   private int positionX;
	   private int positionY;

       public DigitalPhoto(int row, int colomn)
       {

           	citraInput=new int[row,colomn];
	        citraOutput= new double[row,colomn];
	        this.row = row;
	        this.colomn = colomn;
	
       }

	   public DigitalPhoto(int row, int colomn, int positionX, int positionY)
       {
            citraInput=  new int[row,colomn];
            citraOutput = new double[row, colomn];
	        this.row = row;
	        this.colomn = colomn;
	        this.positionX = positionX;
	        this.positionY = positionY;
       }

	   public void setRow(int row)
       {
           this.row = row;
       }

       public void setColomn(int colomn)
       {
           this.colomn = colomn;
       }

       public void setPixelInput(int pixel, int x, int y)
       {
           citraInput[y, x] = pixel;
       }

       public void setPixelOutput(double pixel, int x, int y)
       {
          citraOutput[y, x] = pixel;
       }

       public int getRow()
       {
           return this.row;
       }

       public int getColomn()
       {
           return this.colomn;

       }
       public int get_positionX()
       {
           return positionX;
       }

       public int get_positionY()
       {
           return positionY;
       }

       public int getPixelInput(int x, int y)
       {
           return citraInput[y, x];

       }

       public double getPixelOutput(int x, int y)
       {
           return citraOutput[y, x];
       }

       public void blockConversion(int b)
       {
            int iAwal= 0;
	        int jAwal= -1;
	        int nX = colomn-(b+1);
	        int nY = row-(b+1);
	      
	        int sumBlock = nX * nY;
            block =  new DigitalPhoto[sumBlock];
	        for (int h=0; h<sumBlock; h++)
	        {  
                
		         if (iAwal % nX==0) 
		         {
			           jAwal +=1;  
			           iAwal = 0;
		         }
                     
                 block[h] = new  DigitalPhoto(b,b,iAwal,jAwal); //bentuk blockny
		      
		         for(int j=jAwal; j<b+jAwal; j++)
		         {
		           for (int i= iAwal; i<b+iAwal; i++)
		            {			
			
				        block[h].setPixelOutput(citraOutput[j,i],i-iAwal,j-jAwal); //pindahi nilai pixel 
			        }
		        }
		         iAwal+=1;   
	        }
       }

       public DigitalPhoto[] getBlocks()
       {
           return block;
       }

    }
}
