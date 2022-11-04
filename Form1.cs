using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Simple_Face_Detection
{
    public partial class Form1 : Form
    {

        private readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Detect_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog fileDialog = new OpenFileDialog() { Filter = "JPEG|*.jpg" , Multiselect = false })
            {
                if(fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(fileDialog.FileName);
                    if (img == null) return;
                    int orientationId = 0x0112;
                    if (img.PropertyIdList.Contains(orientationId))
                    {
                        var orientation = (int)img.GetPropertyItem(orientationId).Value[0];
                        var rotateFlip = RotateFlipType.RotateNoneFlipNone;
                        switch (orientation)
                        {
                            case 1: rotateFlip = RotateFlipType.RotateNoneFlipNone; break;
                            case 2: rotateFlip = RotateFlipType.RotateNoneFlipX; break;
                            case 3: rotateFlip = RotateFlipType.Rotate180FlipNone; break;
                            case 4: rotateFlip = RotateFlipType.Rotate180FlipX; break;
                            case 5: rotateFlip = RotateFlipType.Rotate90FlipX; break;
                            case 6: rotateFlip = RotateFlipType.Rotate90FlipNone; break;
                            case 7: rotateFlip = RotateFlipType.Rotate270FlipX; break;
                            case 8: rotateFlip = RotateFlipType.Rotate270FlipNone; break;
                            default: rotateFlip = RotateFlipType.RotateNoneFlipNone; break;
                        }
                        if (rotateFlip != RotateFlipType.RotateNoneFlipNone)
                        {
                            img.RotateFlip(rotateFlip);
                            img.RemovePropertyItem(orientationId);
                        }
                    }

                    pic.Image = img;
                    var bitmap = new Bitmap(pic.Image);
                    Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bitmap);
                    Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.4, 0);
                    foreach(Rectangle rectangle in rectangles)
                    {
                        using(Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Pen pen = new Pen(Color.Red,1))
                            {
                                graphics.DrawRectangle(pen, rectangle);
                            }
                        }
                    }
                    pic.Image = bitmap;
                }
            }
        }

        private void CorrectExifOrientation(Image image)
        {
            if (image == null) return;
            int orientationId = 0x0112;
            if (image.PropertyIdList.Contains(orientationId))
            {
                var orientation = (int)image.GetPropertyItem(orientationId).Value[0];
                var rotateFlip = RotateFlipType.RotateNoneFlipNone;
                switch (orientation)
                {
                    case 1: rotateFlip = RotateFlipType.RotateNoneFlipNone; break;
                    case 2: rotateFlip = RotateFlipType.RotateNoneFlipX; break;
                    case 3: rotateFlip = RotateFlipType.Rotate180FlipNone; break;
                    case 4: rotateFlip = RotateFlipType.Rotate180FlipX; break;
                    case 5: rotateFlip = RotateFlipType.Rotate90FlipX; break;
                    case 6: rotateFlip = RotateFlipType.Rotate90FlipNone; break;
                    case 7: rotateFlip = RotateFlipType.Rotate270FlipX; break;
                    case 8: rotateFlip = RotateFlipType.Rotate270FlipNone; break;
                    default: rotateFlip = RotateFlipType.RotateNoneFlipNone; break;
                }
                if (rotateFlip != RotateFlipType.RotateNoneFlipNone)
                {
                    image.RotateFlip(rotateFlip);
                    image.RemovePropertyItem(orientationId);
                }
            }
        }
    }
}
