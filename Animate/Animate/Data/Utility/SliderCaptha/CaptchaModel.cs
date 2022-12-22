using System.Drawing;

namespace Animate.Data.SliderCaptha
{
    public class CaptchaModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Image Background { get; set; }
        public Image Slide { get; set; }
    }
    public class Captcha64Model
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Background { get; set; }
        public string Slide { get; set; }
    }
}
