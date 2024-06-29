using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace copymoveforgery
{
    class DB4Wavelet
    {
        private DigitalPhoto foto;
	    private double[,] low_pass;
	    private double[,] high_pass;
	    private double[,] LL;
	    private double[,] HL;
	    private double[,] LH;
	    private double[,] HH;

	  


        public  DB4Wavelet()
        {
	
        }

        public void build_MatrikFilter(int b)
        {	
	        int row_filter = b/2;
	        int colomn_filter = b;
	        low_pass = new double[row_filter, colomn_filter];
            high_pass = new double[row_filter, colomn_filter];
	
	        for(int i=0;i<row_filter;i++){
		        for(int j=0;j<colomn_filter;j++){
				        if (j==i*2||j==((i*2)+1))
				        {
					        low_pass[i,j]= 0.5;
				        }
				        else{
					        low_pass[i,j]= 0;
				        } 
		        }
	        }

	        for(int i=0;i<row_filter;i++){
		        for(int j=0;j<colomn_filter;j++){
				        if(j== i*2)
				        {
					        high_pass[i,j]= 0.5;
				        }
				        else if(j==(i*2)+1)
				        {
					        high_pass[i,j]= -0.5;
				        }
				        else 
				        {
					        high_pass[i,j]= 0;
				        }
				 
		        }
	        }
        }

        public void  printMatrix(String label,double[,] matrix)
        {
            String mat="";
	        System.Diagnostics.Trace.WriteLine(label);
	        System.Diagnostics.Trace.WriteLine("==========================================");
	        for(int i=0;i<matrix.GetLength(0);i++)
	        {
		        mat= "";
              for(int j=0;j<matrix.GetLength(1);j++)
	          {
			        mat += matrix[i,j] + "\t";
	          }
	          System.Diagnostics.Trace.WriteLine(mat);
	        }
        }


         public  void  featureExtraction(DigitalPhoto foto)
            {
	            int colomn = foto.getColomn();
	            int row = foto.getRow();
	            double[,] matrik_transpose;
	            matrik_transpose= new  double[row,colomn];//tempat menampung value transpose
	            double[,] hsl_lowpass;
	            hsl_lowpass= new  double[row,colomn];
	            double[,] hsl_highpass;
	            hsl_highpass= new double[row,colomn];
	            double[,] temp_rowlow;
	            temp_rowlow= new  double[row,colomn/2];
	            double[,] temp_rowhigh;
	            temp_rowhigh= new double[row,colomn/2];

	            LL= new  double[row/2,colomn/2];
	            LH= new  double[row/2,colomn/2];
	            HL= new  double[row/2,colomn/2];
                HH = new double[row / 2, colomn / 2];
	
           
	            for (int i=0;i<row;i++)
	            {
		            for(int j=0;j<colomn;j++)
		            {  
			            matrik_transpose[j,i] = foto.getPixelOutput(i,j);		
		            }
	            }
	          
	            for (int i=0;i<row/2;i++)
	            { 
		            for(int j=0;j<colomn;j++)
		            {
			            hsl_lowpass[i,j]=0;
			            for (int k=0;k<colomn;k++)
			            {	
				            hsl_lowpass[i,j]+= low_pass[i,k] * matrik_transpose[k,j];  //dekomposition row	
			            }
		            }
	            }
	         
              for (int i=0;i<row/2;i++)
	            {
		            for(int j=0;j<colomn;j++)
		            {
			            temp_rowlow[j,i]= hsl_lowpass[i,j];
		            }
	            }
	          
	            for (int x=0;x<row/2;x++){ 
		            for(int y=0;y<colomn/2;y++){
			            LL[x,y]=0;
			            for (int z=0;z<colomn;z++)
			            {	
				            LL[x,y]+= low_pass[x,z] * temp_rowlow[z,y]; 
				
			            }
		            }
	            }
	           
	            for (int x=0;x<row/2;x++){ 
		            for(int y=0;y<colomn/2;y++){
			            LH[x,y]=0;
			            for (int z=0;z<colomn;z++)
			            {	
				            LH[x,y]+=  high_pass[x,z] * temp_rowlow[z,y]; 
			            }
		            }
	            }
	          
	            for (int i=0;i<row/2;i++)
	            { 
		            for(int j=0;j<colomn;j++)
		            {
			            hsl_highpass[i,j]=0;
			            for (int k=0;k<colomn;k++)
			            {	
				            hsl_highpass[i,j]+= high_pass[i,k] * matrik_transpose[k,j];  //dekomposition row
			            }
		            }
	            }
	
            
              for (int i=0;i<row/2;i++)
	            {
		            for(int j=0;j<colomn;j++)
		            {
			            temp_rowhigh[j,i]= hsl_highpass[i,j];
		            }
	            }
   
          
	            for (int x=0;x<row/2;x++){ 
		            for(int y=0;y<colomn/2;y++){
			            HL[x,y]=0;
			            for (int z=0;z<colomn;z++)
			            {	
				            HL[x,y]+= low_pass[x,z] * temp_rowhigh[z,y]; 
			            }
		            }
	            }
    
           
	            for (int x=0;x<row/2;x++)
                { 
		            for(int y=0;y<colomn/2;y++)
                    {
			            HH[x,y]=0;
			            for (int z=0;z<colomn;z++)
			            {	
				            HH[x,y]+=  high_pass[x,z] * temp_rowhigh[z,y]; 
				
			            }
		            }
	            }
            } 

            public  double[,]   getLL()
            {
	            return LL;	
            }

            public  double[,]  getHL()
            {
	            return HL;	
            }

            public  double[,] getLH()
            {
	            return LH;	
            }

            public  double[,]  getHH()
            {
	            return HH;	
            }


 

         
	
	

    }
}
