using Microsoft.Extensions.Configuration;
using System.Net;

namespace Service.BoardCommander.DataBuilder
{
    public interface IDataBuilderService
    {
        byte[] RegisterColor(int group, string address, string color, int registerNumber);
        byte[] RegisterValue(int group, string address, string value, int registerNumber);
        byte[] RegisterClock(string address, int linesC);
    }
    public class DataBuilderService : IDataBuilderService
    {
        private readonly IConfiguration _configuration;

        public DataBuilderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] RegisterClock(string address, int linesC)
        {
            int num = 0;
            byte[] array = new byte[1024];
            int num2 = (int)DateTime.Now.DayOfWeek;
            num2++;
            byte b = 0;
            byte b2 = (byte)linesC;
            byte b3 = (byte)int.Parse(address);
            byte b4 = 3;
            byte b5 = 4;
            array[num++] = b;
            array[num++] = b2;
            array[num++] = b3;
            array[num++] = b4;
            int b6 = 0;
            byte b7 = SERST(0, 1, 0, 0, b6, 0, 0, 0);
            array[num++] = b7;
            string text = DateTime.Now.ToString("HHmmssddMMyy");
            text = text.Insert(10, "0" + num2.ToString());
            for (int i = 0; i < 3; i++)
            {
                array[num++] = Convert.ToByte('0');
            }
            for (int j = 0; j < text.Length; j++)
            {
                array[num++] = Convert.ToByte(text[j]);
            }
            array[num++] = b5;
            byte b8 = 0;
            for (int k = 0; k < num; k++)
            {
                b8 ^= array[k];
            }
            array[num++] = b8;
            byte[] array2 = new byte[num];
            Array.Copy(array, 0, array2, 0, num);
            return array2;
        }

        public byte[] RegisterColor(int group, string address, string color, int registerNumber)
        {
            int num = 0;
            byte[] array = new byte[1024];
            byte b = (byte)int.Parse(address);
            byte b2 = 1;
            byte b3 = (byte)group;
            byte b4 = 30;
            byte b5 = 4;
            array[num++] = b2;
            array[num++] = b;
            array[num++] = b3;
            array[num++] = b4;
            registerNumber += 48;

            array[num++] = Convert.ToByte(registerNumber);

            var currentColor = _configuration.GetSection("Boards").GetSection("Colors").Get<string[]>().FirstOrDefault(c => c == color);
            if (currentColor == null)
            {

            }
            array[num++] = Convert.ToByte(currentColor[0]);
            array[num++] = b5;
            byte b6 = 0;
            for (int i = 0; i < num; i++)
            {
                b6 ^= array[i];
            }

            array[num++] = b6;
            byte[] array2 = new byte[num];
            Array.Copy(array, 0, array2, 0, num);
            return array2;
        }

        public byte[] RegisterValue(int group, string address, string value, int registerNumber)
        {
            int num = 0;
            byte[] array = new byte[1024];
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
            byte b = 1;
            byte b2 = (byte)int.Parse(address);
            byte b3 = 12;
            byte b4 = (byte)group;
            byte b5 = 4;
            array[num++] = b;
            array[num++] = b2;
            array[num++] = b4;
            array[num++] = b3;
            registerNumber += 48;
            array[num++] = Convert.ToByte(registerNumber);
            foreach(var val in value)
                array[num++] = Convert.ToByte(val);
            
            array[num++] = b5;
            byte b6 = 0;
            for (int j = 0; j < num; j++)
            {
                b6 ^= array[j];
            }
            array[num++] = b6;
            byte[] array2 = new byte[num];
            Array.Copy(array, 0, array2, 0, num);
            return array2;
        }

        private byte SERST(int b7, int b6, int b5, int b4, int b3, int b2, int b1, int b0)
        {
            b7 = 1;
            b6 = 1;
            b5 = 0;
            b0 = 0;
            return Convert.ToByte(b7 * 128 + b6 * 64 + b5 * 32 + b4 * 16 + b3 * 8 + b2 * 4 + b1 * 2 + b0);
        }

    }
}