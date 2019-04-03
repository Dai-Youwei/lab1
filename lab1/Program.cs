using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Program
    {
        //将字符串转化为二维 码
        public void StringToQrCode(string str)
        {
            //校正，M表示15%校正
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(str);
            //根据字符串信息在指定位置输出 █
            for (int j = 0; j < qrCode.Matrix.Width; j++)
            {
                for (int i = 0; i < qrCode.Matrix.Height; i++)
                {
                    Console.Write(qrCode.Matrix[i, j] ? "  " : "█");
                }
                Console.WriteLine();
            }
        }


        //将文件中的每一行转化为二维码并保存在bin目录下
        public static void QrCodeToPng(string fileName1, string str, int lineNum)

        {
            //校正，M表示15%校正
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            //生成二维码
            QrCode qrCode = qrEncoder.Encode(str);
            //生成显示二维码的渲染器
            GraphicsRenderer graphicsRenderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            //将二维码保存为png，保存至当前目录下，并输出提示消息
            MemoryStream ms = new MemoryStream();

            graphicsRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

            Image img = Image.FromStream(ms);
            //得到图片编号
            string num = "00";
            num = num + lineNum;
            img.Save(num.Substring(num.Length - 3, 3) + ".png");
            Console.WriteLine("已经生成图片" + num.Substring(num.Length - 3, 3));

        }

        static void Main(string[] args)
        {
            Program pro1 = new Program();
            Console.Write("请输入长度小于50的字符串或文件名(文件名格式：-fD://test.txt)：");
            //得到用户输入
            string input = Console.ReadLine();
            int index = input.IndexOf("-f");
            //判断字符串长度，若不符合要求则输出提示信息
            if (input.Length > 50)
            {
                Console.WriteLine("输入的字符串长度需小于50！");
            }
            //用户输入为字符串而非文件名
            else if (index == -1)
            {
                pro1.StringToQrCode(input);
                Console.WriteLine(@"按任意键停止…");
                Console.ReadKey();
            }
            //用户输入为文件
            else
            {
                //得到文件名
                string fileName1 = input.Substring(index + 2, input.Length - index - 2);
                //判断文件是否存在
                if (File.Exists(fileName1))
                {
                    string[] str = File.ReadAllLines(fileName1);
                    for (int i = 0; i < str.Length; i++)
                    {
                        QrCodeToPng(fileName1, str[i], i + 1);
                    }
                }
                //如果不存在则输出提示信息
                else
                {
                    Console.WriteLine("文件不存在！");
                }
            }
        }
    }
}
