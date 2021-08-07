using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;

public class Detector : IDisposable
{
    private readonly CascadeClassifier _cascadeClassifier = new CascadeClassifier(@"Resources\haarcascades\haarcascade_frontalface_alt_tree.xml");
    public Mat Run(Mat image)
    {
        var img = new Mat();
        image.CopyTo(img);
        using (var grayImage = new Mat())
        {
            CvInvoke.CvtColor(img, grayImage, ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(grayImage, grayImage);

            var detectedFaceCollection = _cascadeClassifier.DetectMultiScale(grayImage);
            foreach (var face in detectedFaceCollection)
            {
                using (var mat = new Mat(img, face))
                {
                    CvInvoke.GaussianBlur(mat, mat, new Size(0, 0), 15d);
                }
            }
            return img;
        }
    }

    public void Dispose()
    {
        _cascadeClassifier?.Dispose();
    }
}
