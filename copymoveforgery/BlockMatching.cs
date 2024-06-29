using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace copymoveforgery
{
    class BlockMatching
    {
        	
	        private DigitalPhoto[] block;
	        private double[,] matrik;
	        private Point[] position;
	        private List<Pair> lstpair;
	        private List<Pair> pairforged; 

            public BlockMatching(DigitalPhoto[] block, double[,]  matrik)
            {
                this.block = block;
	            this.matrik = matrik;
	            position =  new Point[block.GetLength(0)]; //mengambil position asli/ori
	            for(int i =0;i<block.GetLength(0);i++)
	            {
                  position[i]=  new Point();
	              position[i].X = block[i].get_positionX();
	              position[i].Y = block[i].get_positionY(); 

	            }
            }

            private int compare(int row_a, int row_b)
            {
                int value = 0;
                for (int x = 0; x < matrik.GetLength(1); x++)
                {
                    if (matrik[row_a, x] > matrik[row_b, x])
                    {
                        value = 1;
                        break;
                    }
                    if (matrik[row_a, x] < matrik[row_b, x])
                    {
                        value = -1;
                        break;
                    }
                }
                return value;
            }

             

            
	        public void lexicography_sort()
            {
                double[] temp = new double[4]; //jumlah colomn
                Point temp_position;
	            for(int i=0; i<block.GetLength(0)-1; i++)
	            {	
		            for(int j=i+1; j<block.GetLength(0); j++)
		            {
		
			            if (compare(i,j)==1)    //jika i lebih besar dari j
			            {
                   
				            for(int k=0; k<4;k++)
				            {  //memasukkan data colomn
					            temp[k] = matrik[i,k];
					            matrik[i,k] = matrik[j,k];
					            matrik[j,k] = temp[k];
				            }
				
			            temp_position =  new Point(position[i].X,position[i].Y);
			            position[i].X = position[j].X;
			            position[j].X = temp_position.X;

			            position[i].Y = position[j].Y;
			            position[j].Y = temp_position.Y;

                       }
			
		            }				
	            }
            }

	       

	        public void forgeryAreaDetection(int Nf)
            {
                
                lstpair = new  List<Pair>();
                List<Point> lstpoint;
                lstpoint = new  List<Point>();
                 Point p_baru;
                
                 double Nn = 50;

                 for(int i=0;i<position.GetLength(0);i++)
                 {
	                 for(int j=i+1; j<position.GetLength(0);j++)
	                 {
		                 if(Math.Abs (i-j) < Nn )
		                 {
	      	                Pair pr = new Pair();
			                if (compare(i,j)==0) //2 blok dianggap mirip/ sama semua nilai vektor
			                {
		                       pr.set_point1(position[i]);
		                       pr.set_point2(position[j]);
                               lstpair.Add(pr);
			                }

		                 }
		                 else{
                               break;
		                 }
	                 }
                 }

                System.Diagnostics.Trace.WriteLine("Bervalue Membentuk pasangan sebanyak  "  + lstpair.Count.ToString()); 
                ////seleksi koordinat terpilih dengan beberapa kondisi
                 int max_x = 0;
                 int max_y = 0;
                 for(int i=0;i<lstpair.Count;i++)
                 {
	                 Point  pi = lstpair[i].get_point1();
	                 Point pj = lstpair[i].get_point2();
	                 p_baru = new   Point();
	                 if((pi.X - pj.X)>0)
	                 {
		                 p_baru.X = pi.X - pj.X;
		                 p_baru.Y = Math.Abs (pi.Y - pj.Y);
	                 }
	                 else if ((pi.X - pj.X)<0)
	                 {
		                 p_baru.X = pj.X - pi.X;
		                 p_baru.Y = Math.Abs (pi.Y - pj.Y);
	                 }
	                 else if (pi.X == pj.X)
	                 {
		                 p_baru.X = 0;
		                 p_baru.Y = Math.Abs (pi.Y - pj.Y);
	                 }
	                 
                    lstpoint.Add(p_baru);

                     //mencari koordinat terrow 
	                if(max_x < p_baru.X)
	                {
		                max_x = p_baru.X;
	                }
	                if(max_y < p_baru.Y)
	                {
		                max_y = p_baru.Y;
	                }
                 }

                System.Diagnostics.Trace.WriteLine("Offset Bervalue Dibentuk .... " ); 
                //Menghitung Frekuensi kemunculan koordinat/offset
                int[,] frekuensi = new int[max_x+1, max_y+1];
                for(int i=0;i<max_x;i++)
                {
	                for(int j=0;j<max_y;j++)
	                {
		                frekuensi[i,j]=0;			//inisialisasi
	                }
                }
             

 
                for(int i=0;i<lstpoint.Count;i++)
                {
	                int x = lstpoint[i].X;
	                int y = lstpoint[i].Y;
	                frekuensi[x,y]++;
	                //System::Diagnostics::Trace::WriteLine("FREKUENSI="+frekuensi[x,y].ToString());
                }
                System.Diagnostics.Trace.WriteLine("jumlah list="+lstpoint.Count.ToString());
                System.Diagnostics.Trace.WriteLine("Frekuensi Offset Bervalue Dihitung .... " );
                //membuang koordinat yg tdk sesuai dgn syarat (pair<Nf)
                    pairforged= new  List<Pair>();
                    for(int i=0; i<lstpoint.Count; i++){
	                    int x = lstpoint[i].X;
	                    int y = lstpoint[i].Y;
                      // System::Diagnostics::Trace::WriteLine("Frekuensi =" +frekuensi[x,y].ToString());
	                    if (frekuensi[x,y]>=Nf)
	                    {
		                    pairforged.Add(lstpair[i]); 
	                    }
                    }
            lstpair.Clear();
                 
	               
            }

            public List<Pair>  get_forgearea()
            {
                return pairforged;
            }

    }
}
