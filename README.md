# Copy-Move Forgery Detector

This application is built to detect forgery related to Copy-Move / pixel cloning in images. It implements image processing methods such as grayscale conversion, image blocking, image transformation using Wavelet DB4 L2, and lexicographical sorting. Each image block is transformed into a wavelet matrix and sorted using the lexicographical method. Each image block is compared using an exact-match (Block Matching) approach within a certain regional range (neighborhood) and filtered by a certain similarity frequency. The highest frequency of likelihood in each image region will be selected as the suspected Copy-Move region.


[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org)

Feel Free to use for learning purpose. 

## **Result**

 


## Run Locally

Clone the project

```bash
  git clone https://github.com/fiqihamrullah/copy-move-forgery-detector.git
```

Go to the project directory, run file solusion (*.sln) then build and run 
