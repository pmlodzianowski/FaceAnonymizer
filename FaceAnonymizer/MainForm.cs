using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace faceMaskDetector
{
    public partial class MainForm : Form
    {
        private VideoCapture _capture;
        private Mat _frame;
        private ImageBox _imgbox;
        private Size _frameSize;
        private Detector _detector;

        public MainForm()
        {
            InitializeComponent();
            InitVideoCapture();
            InitImageBox();
            StartVideoCapture();
            ClientSize = _frameSize;
            Disposed += MainForm_Disposed;
        }


        private void InitVideoCapture()
        {
            _detector = new Detector();
            _frame = new Mat();
            _capture = new VideoCapture();
            _capture.ImageGrabbed += ProcessFrame;

            var width = Convert.ToInt32(_capture.Get(Emgu.CV.CvEnum.CapProp.FrameWidth));
            var height = Convert.ToInt32(_capture.Get(Emgu.CV.CvEnum.CapProp.FrameHeight));
            _frameSize = new Size(width, height);
        }

        private void InitImageBox()
        {
            _imgbox = new ImageBox
            {
                Size = _frameSize,
                Location = new Point(0, 0)
            };
            Controls.Add(_imgbox);
        }

        private void StartVideoCapture()
        {
            if (_capture is object)
            {
                try
                {
                    _capture.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            _imgbox?.Dispose();
            _capture?.Stop();
            _capture?.Dispose();
            _frame?.Dispose();
            _detector?.Dispose();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture is object && _capture.Ptr != IntPtr.Zero)
            {
                var oldFrame = _frame;
                _capture.Retrieve(_frame, 0);
                using (oldFrame)
                {
                    _frame = _detector?.Run(oldFrame);
                }
                _imgbox.Image = _frame;
            }
        }
    }
}
