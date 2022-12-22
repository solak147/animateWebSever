namespace Animate.Data.SliderCaptha
{
    public class Config
    {
        /// &lt;summary&gt;
        /// 矩形宽
        /// &lt;/summary&gt;
        public static int l = 42;
        /// &lt;summary&gt;
        /// 圆形半径
        /// &lt;/summary&gt;
        public static int r = 9;
        /// &lt;summary&gt;
        /// 圆形直径
        /// &lt;/summary&gt;
        public static int d = r * 2;
        /// &lt;summary&gt;
        /// 计算圆形与矩形交接三角形边
        /// &lt;/summary&gt;
        public static int a = (int)(r * Math.Sin(Math.PI * (50 / 180f)));
        public static int b = (int)(r * Math.Cos(Math.PI * (50 / 180f)));
        public static int c = r - a;
        /// &lt;summary&gt;
        /// 滑块边框
        /// &lt;/summary&gt;
        public static int blod = 2;
        /// &lt;summary&gt;
        /// 水印
        /// &lt;/summary&gt;
        public static string watermarkText = "SC.NET";
        /// &lt;summary&gt;
        /// 是否显示水印
        /// &lt;/summary&gt;
        public static bool showWatermark = true;

    }
}
