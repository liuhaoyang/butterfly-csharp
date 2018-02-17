using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Butterfly.Client.AspNetCore.Tests
{
    public class IgnoredRoutes_Tests
    {
        [Fact]
        public void IgnoredRoutes_Test()
        {
            var config = new ButterflyConfig();
            config.IgnoredRoutesRegexPatterns = new string[] {"/favicon.ico"};
            var result = Regex.IsMatch(config.IgnoredRoutesRegexPatterns[0], "/favicon.ico");
            Assert.True(result);
        }

        [Fact]
        public void IgnoredRoutes_Regex_Test()
        {
            var config = new ButterflyConfig();
            config.IgnoredRoutesRegexPatterns = new string[] {"/status"};
            var routes = new string[] {"/status", "/administration/status"};
            var result = IsMatch(config.IgnoredRoutesRegexPatterns, routes[0]);
            Assert.True(result);
            result = result = IsMatch(config.IgnoredRoutesRegexPatterns, routes[1]);
            Assert.True(result);
        }

        private bool IsMatch(string[] patterns, string path)
        {
            if (patterns == null || patterns.Any(x => Regex.IsMatch(path, x)))
            {
                return true;
            }

            return false;
        }

    }
}