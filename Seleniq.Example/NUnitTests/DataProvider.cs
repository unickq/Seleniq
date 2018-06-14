using System.Collections;
using NUnit.Framework;

namespace Seleniq.Example.NUnitTests
{
    public class DataProvider
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData("github", 0, "github");
                yield return new TestCaseData("youtube", 0, "youtube");
            }
        }
    }
}