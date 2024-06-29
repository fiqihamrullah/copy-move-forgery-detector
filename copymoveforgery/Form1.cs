using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
 

namespace copymoveforgery
{
    public partial class Form1 : Form
    {
        private Bitmap img;
		private DigitalPhoto foto;
		private PixelsGrabber PG;
		private PreProcessing PP;
		private PhotoViewer FW;
		private BlockDetection DB;
		private BlockMatching MB;

        private int np;

        private int iselected;

        public Form1()
        {
            InitializeComponent();
            iselected = 0;
        }

        private void printPixelofDigitalPhoto(String label,DigitalPhoto digitalphoto)
        {
            System.Diagnostics.Trace.WriteLine(label + "----------------------------------------------------------------------------"); 
            for(int i=0;i<digitalphoto.getRow();i++)
            {
                for (int j = 0; j < digitalphoto.getColomn(); j++)
                {
                    System.Diagnostics.Trace.Write(digitalphoto.getPixelOutput(i,j).ToString() + "\t");
                }
                System.Diagnostics.Trace.WriteLine(""); 
            }
        }

        private void forgeDetection()
        {
            if(pbOriginalImage.Image != null) 
			{
             //   MessageBox.Show("----Mulai--------");
             System.Diagnostics.Trace.WriteLine("Ubah ke Dalam Blok .....");
             int idx = iselected;
             int iblock = 0;
             if (idx == 0)
             {
                 foto.blockConversion(16);
                 iblock = 4;
             }
             else if (idx == 1)
             {
                 foto.blockConversion(8);
                 iblock = 2;
             }
             else if (idx == 2)
             {
                 foto.blockConversion(4);
                 iblock = 1;
             }
             DigitalPhoto[] block = foto.getBlocks();
             printPixelofDigitalPhoto("Isi Block ",block[0]);
             System.Diagnostics.Trace.WriteLine("Total Blok Yang Tercipta  " + block.Length);
             np = 10; 
	 
			 DB  = new  BlockDetection();
             DB.deteksi_block(block,iblock*iblock);
			 np=40;
             
			 MB = new BlockMatching (block,DB.getMatrik());
			 MB.lexicography_sort();
			 np=80;
	 
			 MB.forgeryAreaDetection(Int16.Parse(txtNF.Text));
			 np=100;
		
			 FW.setViewer(pbResult);
			 FW.showForgeryImage(foto,MB.get_forgearea());

			  MessageBox.Show("----Selesai--------","Pendeteksi Selesai",MessageBoxButtons.OK,MessageBoxIcon.Information);
		   }
		else
		  {
			 MessageBox.Show("Gambar belum tersedia","Gagal",MessageBoxButtons.OK,MessageBoxIcon.Warning);
		  }
        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
             openFileDialog1.ShowDialog();			 
			 img = new  Bitmap(openFileDialog1.FileName);
			 foto = new  DigitalPhoto(img.Height,img.Width);
			 pbOriginalImage.Image = img;
             pbOriginalImage.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if(pbOriginalImage.Image != null) 
			 {
				PG =  new PixelsGrabber();

                System.Diagnostics.Trace.WriteLine("---------------------------------------------------------");
                System.Diagnostics.Trace.WriteLine(">> RGB Pixel");
                System.Diagnostics.Trace.WriteLine("---------------------------------------------------------");

				PG.grabPixel(img,foto);

				PP = new PreProcessing();
                System.Diagnostics.Trace.WriteLine("---------------------------------------------------------");
                System.Diagnostics.Trace.WriteLine(">> Grayscale");
                System.Diagnostics.Trace.WriteLine("---------------------------------------------------------");
                PP.grayscale(foto);

                //Demo Wavelet

                FW = new PhotoViewer();
                FW.setViewer(pbOriginalImage);
                FW.showImage(foto); 


                viewWavelet();


				
			 }
			 else
			 {
				MessageBox.Show("Gambar belum tersedia");
			 }
        }

        private void viewWavelet()
        {
            DigitalPhoto imgWavelet = new DigitalPhoto(foto.getRow(), foto.getColomn());
            DB4Wavelet DWTL1 = new DB4Wavelet();
            DB4Wavelet DWTL2 = new DB4Wavelet();
            DWTL1.build_MatrikFilter(foto.getRow()); //16	
            
                DWTL1.featureExtraction(foto);
                DWTL2 = new DB4Wavelet();
                DWTL2.build_MatrikFilter(DWTL1.getLL().GetLength(0));
                DigitalPhoto NewFoto = new DigitalPhoto(DWTL1.getLL().GetLength(0), DWTL1.getLL().GetLength(0));
                for (int j = 0; j < DWTL1.getLL().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL1.getLL().GetLength(1); k++)
                    {
                        NewFoto.setPixelOutput(DWTL1.getLL()[j, k], j, k);
                        imgWavelet.setPixelOutput(DWTL1.getLL()[j, k], j, k);
                        
                    }
                }

                for (int j = 0; j < DWTL1.getLH().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL1.getLH().GetLength(1); k++)
                    {

                        imgWavelet.setPixelOutput(DWTL1.getLH()[j, k], j + DWTL1.getLL().GetLength(0), k);

                    }
                }

                for (int j = 0; j < DWTL1.getHL().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL1.getHL().GetLength(1); k++)
                    {

                        imgWavelet.setPixelOutput(DWTL1.getHL()[j, k], j, k + DWTL1.getLL().GetLength(0));

                    }
                }

                for (int j = 0; j < DWTL1.getHH().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL1.getHH().GetLength(1); k++)
                    {

                        imgWavelet.setPixelOutput(DWTL1.getHH()[j, k], j + DWTL1.getLL().GetLength(0), k + DWTL1.getLL().GetLength(0));

                    }
                }


                DWTL2.featureExtraction(NewFoto);
                DB4Wavelet DWT = new DB4Wavelet();
                DWT.build_MatrikFilter(DWTL2.getLL().GetLength(0));
                DigitalPhoto fotoL2 = new DigitalPhoto(DWTL2.getLL().GetLength(0), DWTL2.getLL().GetLength(0));

                for (int j = 0; j < DWTL2.getLL().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL2.getLL().GetLength(1); k++)
                    {
                        fotoL2.setPixelOutput(DWTL2.getLL()[j, k], j, k);
                        imgWavelet.setPixelOutput(DWTL2.getLL()[j, k], j, k);
                    }
                }

                for (int j = 0; j < DWTL2.getLH().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL2.getLH().GetLength(1); k++)
                    {

                        imgWavelet.setPixelOutput(DWTL2.getLH()[j, k], j + DWTL2.getLL().GetLength(0), k);
                    }
                }


                for (int j = 0; j < DWTL2.getHL().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL2.getHL().GetLength(1); k++)
                    {

                        imgWavelet.setPixelOutput(DWTL2.getHL()[j, k], j, k + DWTL2.getLL().GetLength(0));
                    }
                }

                for (int j = 0; j < DWTL2.getHH().GetLength(0); j++)
                {
                    for (int k = 0; k < DWTL2.getHH().GetLength(1); k++)
                    {

                        imgWavelet.setPixelOutput(DWTL2.getHH()[j, k], j + DWTL2.getLL().GetLength(0), k + DWTL2.getLL().GetLength(0));
                    }
                }


                DWT.featureExtraction(fotoL2);


                FW = new PhotoViewer();
                FW.setViewer(pbWavelet);
                FW.showImage(imgWavelet);                 
            
        }


        private void detectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("----Mulai--------");
            ThreadStart operasiku = new ThreadStart(this.forgeDetection);
			Thread   mythread = new Thread(operasiku);
			timer1.Enabled =true;
			mythread.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = np;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbBlok.SelectedIndex = 0;
        }

        private void cmbBlok_SelectedIndexChanged(object sender, EventArgs e)
        {
            iselected = cmbBlok.SelectedIndex;
        }
    }
}
