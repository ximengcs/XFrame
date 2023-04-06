using System.IO;
using XFrame.Modules.Config;
using XFrame.Modules.Crypto;

namespace XFrame.Modules.Archives
{
    internal class ArchiveUtility
    {
        public static byte[] ReadBytes(string path)
        {
            byte[] result;
            if (XConfig.ArchiveEncrypt)
            {
                byte[] data = File.ReadAllBytes(path);
                ICryptor cryptor = CryptoModule.Inst.New();
                cryptor.BeginDecrypty(data);
                result = cryptor.EndDecrypty();
                cryptor.Dispose();
            }
            else
            {
                result = File.ReadAllBytes(path);
            }
            return result;
        }

        public static void WriteBytes(string path, byte[] buffer)
        {
            if (XConfig.ArchiveEncrypt)
            {
                ICryptor cryptor = CryptoModule.Inst.New();
                cryptor.BeginEncrypt();
                cryptor.Writer.BaseStream.Write(buffer, 0, buffer.Length);
                byte[] data = cryptor.EndEncrypt();
                File.WriteAllBytes(path, data);
                cryptor.Dispose();
            }
            else
            {
                File.WriteAllBytes(path, buffer);
            }
        }

        public static string ReadText(string path)
        {
            string result;
            if (XConfig.ArchiveEncrypt)
            {
                byte[] data = File.ReadAllBytes(path);
                ICryptor cryptor = CryptoModule.Inst.New();
                cryptor.BeginDecrypty(data);
                cryptor.EndDecrypty();
                result = cryptor.Reader.ReadToEnd();
                cryptor.Dispose();
            }
            else
            {
                result = File.ReadAllText(path);
            }
            return result;
        }

        public static void WriteText(string path, string text)
        {
            if (XConfig.ArchiveEncrypt)
            {
                ICryptor cryptor = CryptoModule.Inst.New();
                cryptor.BeginEncrypt();
                cryptor.Writer.Write(text);
                byte[] data = cryptor.EndEncrypt();
                File.WriteAllBytes(path, data);
                cryptor.Dispose();
            }
            else
            {
                File.WriteAllText(path, text);
            }
        }
    }
}
