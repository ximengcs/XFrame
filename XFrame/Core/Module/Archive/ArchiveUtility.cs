using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Crypto;

namespace XFrame.Modules.Archives
{
    public class ArchiveUtility
    {
        public static IArchiveUtilityHelper Helper { get; set; } = new DefaultArchiveUtilityHelper();

        public static byte[] ReadBytes(string path)
        {
            byte[] result;
            if (XConfig.ArchiveEncrypt)
            {
                byte[] data = Helper.ReadAllBytes(path);
                ICryptor cryptor = XModule.Crypto.New();
                cryptor.BeginDecrypty(data);
                result = cryptor.EndDecrypty();
                cryptor.Dispose();
            }
            else
            {
                result = Helper.ReadAllBytes(path);
            }
            return result;
        }

        public static void WriteBytes(string path, byte[] buffer)
        {
            if (XConfig.ArchiveEncrypt)
            {
                ICryptor cryptor = XModule.Crypto.New();
                cryptor.BeginEncrypt();
                cryptor.Writer.BaseStream.Write(buffer, 0, buffer.Length);
                byte[] data = cryptor.EndEncrypt();
                Helper.WriteAllBytes(path, data);
                cryptor.Dispose();
            }
            else
            {
                Helper.WriteAllBytes(path, buffer);
            }
        }

        public static string ReadText(string path)
        {
            string result;
            if (XConfig.ArchiveEncrypt)
            {
                byte[] data = Helper.ReadAllBytes(path);
                ICryptor cryptor = XModule.Crypto.New();
                cryptor.BeginDecrypty(data);
                cryptor.EndDecrypty();
                result = cryptor.Reader.ReadToEnd();
                cryptor.Dispose();
            }
            else
            {
                result = Helper.ReadAllText(path);
            }
            return result;
        }

        public static void WriteText(string path, string text)
        {
            if (XConfig.ArchiveEncrypt)
            {
                ICryptor cryptor = XModule.Crypto.New();
                cryptor.BeginEncrypt();
                cryptor.Writer.Write(text);
                byte[] data = cryptor.EndEncrypt();
                Helper.WriteAllBytes(path, data);
                cryptor.Dispose();
            }
            else
            {
                Helper.WriteAllText(path, text);
            }
        }
    }
}
