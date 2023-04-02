
namespace XFrame.Core
{
    [BaseModule]
    public class ParserModule : SingletonModule<ParserModule>
    {
        public StringParser STRING { get; private set; }
        public IntParser INT { get; private set; }
        public FloatParser FLOAT { get; private set; }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            STRING = new StringParser();
            INT = new IntParser();
            FLOAT = new FloatParser();
        }
    }
}
