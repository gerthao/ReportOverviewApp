using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportOverviewApp.Helpers
{
    public class SearchTokenizer
    {
        private string entry;
        private StringBuilder buffer;
        private Dictionary<string, Mode> tokens;
        public enum Mode { Normal, And, Or, Not, Exact };
        private Mode mode;
        public SearchTokenizer() {
            tokens = new Dictionary<string, Mode>();
            buffer = new StringBuilder();
        }
        public SearchTokenizer(string input) : this()
            => entry = input != null? input.Trim() : input;
        public void Tokenize()
        {
            List<string> data = entry.Split(' ').ToList();
            foreach(string token in data)
            {
                HandleMode(token);
                buffer.Append(token);
            }
        }
        public void HandleToken(string token)
        {
            tokens.Add(token, mode);
        }
        public void HandleMode(string token)
        {
            switch (token)
            {
                case "{and}":
                    mode = Mode.And;
                    break;
                case "{or}":
                    mode = Mode.Or;
                    break;
                case "{not}":
                    mode = Mode.Not;
                    break;
                case "{exact}":
                    mode = Mode.Exact;
                    break;
                default:
                    mode = Mode.Normal;
                    break;
            }
        }
        public void Tokenize(string input)
        {
            entry = input;
            this.Tokenize();
        }
        public Dictionary<string, Mode> GetSearchTokens() => tokens;
        public void And() { }
        public void Or() { }
        public void Exact() { }
        public void Not() { }
    }
}
