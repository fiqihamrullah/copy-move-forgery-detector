using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace copymoveforgery
{
    class BlockDetection
    {
        private DigitalPhoto[] block;
	    private DB4Wavelet DWT;
	    private double[,] matrik_block;

        public BlockDetection()
        {

        }


        private void sort(double[] vectorsubband)
        {
            for (int i = 0; i < vectorsubband.Length - 1; i++)
            {
                for (int j = i + 1; j < vectorsubband.Length; j++)
                {
                    if (vectorsubband[i] > vectorsubband[j])
                    {
                        double temp = vectorsubband[i];
                        vectorsubband[i] = vectorsubband[j];
                        vectorsubband[j] = temp;

                    }
                }
            }
        }

        private void fillVector(double[] fromvector, double[] toVector, int idxlast)
        {
            for (int i = 0; i < fromvector.Length; i++)
            {
                toVector[i + idxlast] = fromvector[i];
            }
        }

        private void printMatrix(String label, double[,] matrix)
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

       
	    public double[] vektor_koefisienwavelet(DB4Wavelet DWT,int ivector)
        {
            //menggabungkan semua fitur koefesien dalam 1 vektor
            double[] vector_row = new double[ivector];//16 jumlah fitur koefesien dwt
            int idx_kol = 0;	
	        // System::Diagnostics::Trace::WriteLine("Banyanya Dimensi " + DWT->getLL()->GetLength(0).ToString());
	        for(int i=0; i<DWT.getLL().GetLength(0);i++)
	                 {
		                for(int j=0; j<DWT.getLL().GetLength(1);j++)
		                {
			                vector_row[idx_kol] = DWT.getLL()[i,j];
			                idx_kol++;
		                }
	                 }
                //-------------------------------------------------------------------------------------//
	                for(int i=0; i<DWT.getHL().GetLength(0);i++)
	                 {
		                for(int j=0; j<DWT.getHL().GetLength(1);j++)
		                {
			                vector_row[idx_kol] = DWT.getHL()[i,j];
			                //str += vector_row[idx_kol] +";";
			                idx_kol++;
		                }
	                 }
                 //-------------------------------------------------------------------------------------//
	                 for(int i=0; i<DWT.getLH().GetLength(0);i++)
	                 {
		                for(int j=0; j<DWT.getLH().GetLength(1);j++)
		                {
			                vector_row[idx_kol] = DWT.getLH()[i,j];
			                //str += vector_row[idx_kol] +";";
			                idx_kol++;
		                }
	                 }
                 //-------------------------------------------------------------------------------------//
	                 for(int i=0; i<DWT.getHH().GetLength(0);i++)
	                 {
		                for(int j=0; j<DWT.getHH().GetLength(1);j++)
		                {
			                vector_row[idx_kol] = DWT.getHH()[i,j];
			                //str += vector_row[idx_kol] +";";
			                idx_kol++;
		                }
	                 }
                return vector_row;
        }

	    public void deteksi_block(DigitalPhoto[] block,int iblock)
        {
            int Q = 256; //nilai Q dari jurnal	 

            matrik_block = new double[block.GetLength(0), iblock]; 
	        DB4Wavelet DWTL1 = new DB4Wavelet();
	        DB4Wavelet DWTL2 = new DB4Wavelet();
	        DWTL1.build_MatrikFilter(block[0].getRow()); //16	
	        for(int i=0; i<block.GetLength(0);i++)
	            {
		            DWTL1.featureExtraction(block[i]);
		            DWTL2 = new  DB4Wavelet();
		            DWTL2.build_MatrikFilter(DWTL1.getLL().GetLength(0)); 
		            DigitalPhoto  NewFoto = new  DigitalPhoto(DWTL1.getLL().GetLength(0), DWTL1.getLL().GetLength(0));
				            for(int j=0;j<DWTL1.getLL().GetLength(0);j++)
				            {
					            for(int k=0;k<DWTL1.getLL().GetLength(1);k++)
					            {
						            NewFoto.setPixelOutput(DWTL1.getLL()[j,k],j,k);
						
					             }
				            }
				            DWTL2.featureExtraction(NewFoto);
				            DWT = new  DB4Wavelet();
				            DWT.build_MatrikFilter(DWTL2.getLL().GetLength(0));
				            DigitalPhoto fotoL2 = new  DigitalPhoto(DWTL2.getLL().GetLength(0), DWTL2.getLL().GetLength(0));
				
				            for(int j=0;j<DWTL2.getLL().GetLength(0);j++)
				            {
					            for(int k=0;k<DWTL2.getLL().GetLength(1);k++)
					            {
						            fotoL2.setPixelOutput(DWTL2.getLL()[j,k],j,k);
						          
						
					             }
				            }
		            DWT.featureExtraction(fotoL2);
                    double[] vektor_row = vektor_koefisienwavelet(DWT,iblock); //return nilai koef

                    
                    if (i == 0)
                    {
                        System.Diagnostics.Trace.WriteLine(" Hasil DWT Lv. 2-----------------------------------------");
                        for (int ii = 0; ii < vektor_row.Length; ii++)
                        {
                            System.Diagnostics.Trace.WriteLine(vektor_row[ii]);
                        }
                    }

                    for (int j = 0; j < iblock; j++)//16
		            {
		              matrik_block[i,j] =  vektor_row[j]/Q;  
			            
		            }

                    if (i == 0)
                    {
                        System.Diagnostics.Trace.WriteLine("Hasil DWT Ternormalisasi (Setelah dibagi dengan Q) --------------------------");
                        for (int ii = 0; ii < vektor_row.Length; ii++)
                        {
                            System.Diagnostics.Trace.WriteLine(matrik_block[i,ii]);
                        }
                    }

	            } 	

        }

        public double[,] getMatrik()
        {
            return matrik_block;
        }

    }
}
