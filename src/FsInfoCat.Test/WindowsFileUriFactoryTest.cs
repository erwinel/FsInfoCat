using FsInfoCat.Test.Helpers;
using FsInfoCat.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class WindowsFileUriFactoryTest
    {
        public static IEnumerable<TestCaseData> GetFsPathRegexTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FsPathRegex");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFsPathRegexTestCases))]
        public string FsPathRegexTest(string input, bool base64Encoded)
        {
            Match match = WindowsFileUriFactory.FS_PATH_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FsPathRegex", base64Encoded, "a", "h", "p");
        }

        public static IEnumerable<TestCaseData> GetFsPathPatternTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FsPathPattern");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFsPathPatternTestCases))]
        public string FsPathPatternTest(string input, bool base64Encoded)
        {
            Match match = Regex.Match(input, WindowsFileUriFactory.PATTERN_ABS_FS_PATH);
            return UrlHelperTest.ToTestReturnValueXml(match, "FsPathPattern", base64Encoded);
        }

        public static IEnumerable<TestCaseData> GetFormatDetectionRegexTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FormatGuess");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFormatDetectionRegexTestCases))]
        public string FormatDetectionRegexTest(string input, bool base64Encoded)
        {
            Match match = WindowsFileUriFactory.FORMAT_DETECTION_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FormatGuess", base64Encoded, "f", "u", "h", "p", "x");
        }

        public static IEnumerable<TestCaseData> GetFileUriStrictTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FileUriStrict");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFileUriStrictTestCases))]
        public string FileUriStrictTest(string input, bool base64Encoded)
        {
            Match match = WindowsFileUriFactory.FILE_URI_STRICT_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FileUriStrict", base64Encoded, "a", "h", "p");
        }

        public static IEnumerable<TestCaseData> GetIpv6AddressRegexTestCases()
        {
            return UrlHelperTest.GetHostNameTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Ipv6AddressRegex");
                return new TestCaseData(element.GetAttribute("Value"))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIpv6AddressRegexTestCases))]
        public string Ipv6AddressRegexTest(string input)
        {
            Match match = WindowsFileUriFactory.LOCAL_IPV6_ADDRESS_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "Ipv6AddressRegex", "ipv6", "unc");
        }

        public static IEnumerable<TestCaseData> GetHostNameRegexTestCases()
        {
            return UrlHelperTest.GetHostNameTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("HostNameRegex");
                return new TestCaseData(element.GetAttribute("Value"))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetHostNameRegexTestCases))]
        public string HostNameRegexTest(string input)
        {
            Match match = WindowsFileUriFactory.HOST_NAME_W_UNC_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "HostNameRegex", "ipv2", "ipv6", "unc", "dns");
        }

        public static IEnumerable<TestCaseData> GetIsWellFormedFileUriStringTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("IsWellFormedFileUriString");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"),
                        Enum.Parse(typeof(UriKind), expected.GetAttribute("Kind")))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(XmlConvert.ToBoolean(expected.InnerText));
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIsWellFormedFileUriStringTestCases))]
        public bool IsWellFormedFileUriStringTest(string uriString, UriKind kind)
        {
            bool result = WindowsFileUriFactory.INSTANCE.IsWellFormedFileUriString(uriString, kind);
            return result;
        }

        public static IEnumerable<TestCaseData> GetParseUriOrPathTestCases()
        {
            return UrlHelperTest.GetUriTestData().Where(e => !(e.SelectSingleNode("ParseUriOrPath") is null)).Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("ParseUriOrPath");
                bool preferFsPath = XmlConvert.ToBoolean(expected.GetAttribute("PreferFsPath"));
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"),
                        preferFsPath, base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.SelectSingleNode("Expected").OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetParseUriOrPathTestCases))]
        public string ParseUriOrPathTest(string source, bool preferFsPath, bool base64Encoded)
        {
            FileStringFormat result = WindowsFileUriFactory.INSTANCE.ParseUriOrPath(source, preferFsPath, out string hostName, out string path);
            if (base64Encoded)
            {
                UTF8Encoding encoding = new UTF8Encoding(false, true);
                return XmlBuilder.NewDocument("Expected")
                    .WithAttribute("HostName", (string.IsNullOrEmpty(hostName)) ? hostName : Convert.ToBase64String(encoding.GetBytes(hostName)))
                    .WithAttribute("Path", (string.IsNullOrEmpty(path)) ? path : Convert.ToBase64String(encoding.GetBytes(path)))
                    .WithInnerText(result.ToString("F")).OuterXml;
            }
            return XmlBuilder.NewDocument("Expected")
                .WithAttribute("HostName", hostName)
                .WithAttribute("Path", path)
                .WithInnerText(result.ToString("F")).OuterXml;
        }
    }
}
