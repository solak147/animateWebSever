using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;

namespace Animate.Data.SliderCaptha
{
    public class Captcha
    {

        public static Captcha64Model GenerateBase64()
        {
            CaptchaModel model = Captcha.Generate();
            if (model != null)
            {
                return new Captcha64Model()
                {
                    X = model.X,
                    Y = model.Y,
                    Background = ImageToBase64(model.Background, ImageFormat.Jpeg),
                    Slide = ImageToBase64(model.Slide, ImageFormat.Png)
                };
            }
            else
            {
                return null;
            }
        }

        /// &lt;summary&gt;
        /// 生成验证码
        /// &lt;/summary&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static CaptchaModel Generate()
        {
            Bitmap image = BgImage();
            if (image != null)
            {
                int l = Config.l;
                int d = Config.d;
                int width = image.Width;
                int height = image.Height;
                int x = RandomNext(width / 3, width - d - l - 10);//初始x
                int y = RandomNext(10 + d, height - l - 10); ;//初始y
                GraphicsPath path = GetSliderPath(x, y);
                Graphics g = GetGraphics(image);

                //水印
                if (Config.showWatermark)
                {
                    Font font = new Font("宋体", 12, FontStyle.Bold);
                    SizeF size = g.MeasureString(Config.watermarkText, font);
                    Point Plogo = new Point((int)(width - size.Width - 5), (int)(height - size.Height - 5));
                    Color color = image.GetPixel(Plogo.X, Plogo.Y);
                    SolidBrush bru = new SolidBrush(AntiColor(color));
                    g.DrawString(Config.watermarkText, font, bru, Plogo);
                }

                Pen pen = new Pen(Color.FromArgb(200, 255, 255, 255), 2);
                g.DrawPath(pen, path);
                Image slider = CaptureSlider(image, path, x, width, height);
                SolidBrush brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                g.FillPath(brush, path);
                g.Save();
                g.Dispose();
                return new CaptchaModel()
                {
                    X = x,
                    Y = y,
                    Background = image,
                    Slide = slider
                };
            }
            return null;
        }




        /// &lt;summary&gt;
        /// 获取图片Graphics
        /// &lt;/summary&gt;
        /// &lt;param name="image"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        private static Graphics GetGraphics(Image image)
        {
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.High;
            return g;
        }

        /// &lt;summary&gt;
        /// 获取滑块path
        /// &lt;/summary&gt;
        private static GraphicsPath GetSliderPath(int x, int y)
        {
            int l = Config.l;
            int r = Config.r;
            int b = Config.b;
            int c = Config.c;
            int d = Config.d;
            int blod = Config.blod;
            GraphicsPath path = new GraphicsPath(FillMode.Winding);
            Point Pa = new Point(x, y);
            Point Pb = new Point(x + l / 2 - b, y - c + blod);
            Point Pd = new Point(x + l, y);
            Point Pe = new Point(Pd.X + c - blod, y + l / 2 - b);
            Point Pg = new Point(Pd.X, y + l);
            Point Ph = new Point(x, y + l);
            Point Pj = new Point(x + c - blod, Pe.Y);
            path.AddLine(Pa, Pb);
            path.AddArc(x + l / 2 - r, y - d, d, d, 130f, 280f);
            path.AddLines(new Point[] { Pd, Pe });
            path.AddArc(x + l, y + l / 2 - r, d, d, 220f, 280f);
            path.AddLines(new Point[] { Pg, Ph });
            path.AddArc(x, y + l / 2 - r, d, d, 140f, -280f);
            path.AddLine(Pj, Pa);
            return path;
        }


        /// &lt;summary&gt;
        /// 获取滑块区域
        /// &lt;/summary&gt;
        /// &lt;param name="image"&gt;&lt;/param&gt;
        /// &lt;param name="path"&gt;&lt;/param&gt;
        /// &lt;param name="x"&gt;&lt;/param&gt;
        /// &lt;param name="width"&gt;&lt;/param&gt;
        /// &lt;param name="height"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        private static Image CaptureSlider(Image image, GraphicsPath path, int x, int width, int height)
        {
            Bitmap concave = new Bitmap(image.Width, image.Height);
            Graphics g = GetGraphics(concave);
            TextureBrush brush = new TextureBrush(image);
            g.Clear(Color.Transparent);
            g.FillPath(brush, path);
            g.Dispose();
            return CaptureImage(concave, x, height);
        }


        /// &lt;summary&gt;
        /// 裁剪图片
        /// &lt;/summary&gt;
        /// &lt;param name="fromImage"&gt;&lt;/param&gt;
        /// &lt;param name="offsetX"&gt;&lt;/param&gt;
        /// &lt;param name="width"&gt;&lt;/param&gt;
        /// &lt;param name="height"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        private static Image CaptureImage(Image fromImage, int offsetX, int height)
        {
            int width = Config.l + Config.d + Config.blod;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = GetGraphics(bitmap);
            g.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, 0, width, height), GraphicsUnit.Pixel);
            g.Dispose();
            return bitmap;
        }


        /// &lt;summary&gt;
        /// 生成随机数
        /// &lt;/summary&gt;
        /// &lt;param name="min"&gt;&lt;/param&gt;
        /// &lt;param name="max"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        private static int RandomNext(int min, int max)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(min, max);
        }


        /// &lt;summary&gt;
        /// 取反色
        /// &lt;/summary&gt;
        /// &lt;param name="color"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static Color AntiColor(Color color)
        {
            if (color.R > 128 && color.G > 128 && color.B > 128)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }

        }

        /// &lt;summary&gt;
        /// 获取背景图
        /// &lt;/summary&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        private static Bitmap BgImage()
        {
            WebClient web = new WebClient();
            int num = RandomNext(1, 20);
            Stream stream = web.OpenRead($"http://00x1.com/images/Pic/{num}.jpg");
            Bitmap bitmap = (Bitmap)Image.FromStream(stream);
            return bitmap;
        }



        /// &lt;summary&gt;
        /// base64转图片
        /// &lt;/summary&gt;
        /// &lt;param name="base64string"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static Bitmap Base64ToImage(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        /// &lt;summary&gt;
        /// 图片转base64
        /// &lt;/summary&gt;
        /// &lt;param name="image"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            if (image == null) return string.Empty;
            string strbaser64 = "";
            try
            {
                string head = "";
                string formatName = ImgFormat.NameFromGuid(format);
                head = $"data:image/{formatName.ToLower()};base64,";
                MemoryStream ms = new MemoryStream();
                image.Save(ms, format);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = head +Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }

    }
}
