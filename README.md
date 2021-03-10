# TinyPngCompressor
A simple Web Site utilizing TinyPNG API to optimize local repository images

## How To Use

1. Clone the repository or [download the zip](https://github.com/pepinho24/TinyPngCompressor/archive/main.zip)
2. Open the .sln project in Visual Studio
3. Build solution
4. Run the page
5. Enter path to the folder containing the documentation repository (e.g. `d:\work\ajax-docs`)
6. Enter the TinyPNG API obtained from https://tinypng.com/developers
7. Press the button and wait for the optimization to finish
8. Copy the content of the `Compressed` folder inside the project to the folder containing the documentation repository (e.g. `d:\work\ajax-docs`)
9. Commit the optimized files according to team procedures

## Important Notes

1. The compressions.xml contains a list of all optimized images
2. The error.xml file contains the path and error message for the images that failed optimization
  - For example, if a .bmp or .gif file is renamed has its extention changed to .jpg or .png, the image will not be optimized 
3. For testing purposes, the Default.aspx.cs gets only the top 5 files sorted by size. To optimize all images, comment out [Line 68 and uncomment the next line](https://github.com/pepinho24/TinyPngCompressor/blob/main/TinyPngCompressor/Default.aspx.cs#L68-L69)
