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
                ICryptor cryptor = CryptoModule.Inst.New("x1df2eop", "3sfd2ds4");
                cryptor.BeginEncrypt();
                StreamWriter writer = cryptor.Writer;
                writer.Write("test");
                writer.Write(1);
                writer.Write(true);
                byte[] buffer = cryptor.EndEncrypt();
                cryptor.Dispose();

                Log.Debug("encrypt count " + buffer.Length);
                cryptor = CryptoModule.Inst.New("x1df2eop", "3sfd2ds4");
                cryptor.BeginDecrypty(buffer);
                cryptor.EndDecrypty();
                StreamReader reader = cryptor.Reader;
                Log.Debug(reader.ReadToEnd());
                cryptor.Dispose();
            });
        }
    }
}
