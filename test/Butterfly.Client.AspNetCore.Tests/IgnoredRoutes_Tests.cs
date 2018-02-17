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
            config.IgnoredRoutesRegexPatterns = new string[] {"[home]"};
            var routes = new string[] {"/home/index", "/user/about"};
            var result = Regex.IsMatch(config.IgnoredRoutesRegexPatterns[0], routes[0]);
            Assert.True(result);
            result = Regex.IsMatch(config.IgnoredRoutesRegexPatterns[0], routes[1]);
            Assert.False(result);
        }
    }
}