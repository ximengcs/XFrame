using System.Text;
using XFrame.Core;
using XFrame.Modules.Crypto;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    [TestClass]
    public class CryptorTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                ICryptor cryptor = XModule.Crypto.New("x1df2eop", "3sfd2ds4");
                cryptor.BeginEncrypt();
                StreamWriter writer = cryptor.Writer;
                byte[] buffer1 = Encoding.UTF8.GetBytes("test");
                writer.BaseStream.Write(buffer1, 0, buffer1.Length);
                writer.Write("test");
                writer.Write(1);
                writer.Write(true);
                byte[] buffer = cryptor.EndEncrypt();
                cryptor.Dispose();

                Log.Debug("encrypt count " + buffer.Length);
                cryptor = XModule.Crypto.New("x1df2eop", "3sfd2ds4");
                cryptor.BeginDecrypty(buffer);
                byte[] buffer3 = cryptor.EndDecrypty();
                StreamReader reader = cryptor.Reader;

                Log.Debug($"{reader.ReadToEnd()} ({reader.EndOfStream})");
                Log.Debug(Encoding.UTF8.GetString(buffer3));
                cryptor.Dispose();
            });
        }
    }
}
