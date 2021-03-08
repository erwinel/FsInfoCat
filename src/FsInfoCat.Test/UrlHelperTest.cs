using System;
using NUnit.Framework;
using FsInfoCat.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FsInfoCat.Test.Helpers;
using System.Text.RegularExpressions;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class UrlHelperTest
    {
        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> GetWindowsFsPathRegexTestCases()
        {
            foreach (var item in (new[]
            {
                new { Value = "SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET", Description = "FQDN Host" },
                new { Value = "MyHost", Description = "AName host" },
                new { Value = "192.168.1.1", Description = "IPV2 host" },
                new { Value = "::1", Description = "Simple IPV6 host" },
                new { Value = "1:2:3:4:5:6:7:8", Description = "Full IPV6 host" },
                new { Value = "::3:4:5:6:7:8", Description = "Leading partial IPV6 host" },
                new { Value = "1:2:3:4:5:6::", Description = "Trailing partial IPV6 host" },
                new { Value = "1:2::6:7:8", Description = "Inner partial IPV6 host" }
            }).SelectMany(host => (new[]
            {
                new { Value = @"\test azure share", Description = "URN" },
                new { Value = @"\test.azure#share", Description = "URN with unacceptable URL character" },
                new { Value = @"\test azure share/", Description = "URN with trailing slash" },
                new { Value = @"\", Description = "URN with root path" },
                new { Value = @"", Description = "URN with no path" },
                new { Value = @"/test azure share\", Description = "URN with slashes and trailing backslash" },
                new { Value = @"/", Description = "URN with root slash" }
            }).SelectMany(a => (new[] { new { Value = "//", Name = "2 slashes" }, new { Value = "\\\\", Name = "2 backslashes" } }).Select(s =>
                new { Value = @$"{s.Value}{host.Value}{a.Value}", Description = $"{s.Name}; {host.Description} {a.Description}", Host = host.Value, Path = a.Value }
            ))))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, null, item.Host, item.Path));
            foreach (var item in (new[]
            {
                new { Value = "A", Description = "UC A" },
                new { Value = "a", Description = "LC a" },
                new { Value = "Z", Description = "UC Z" },
                new { Value = "z", Description = "LC z" }
            }).SelectMany(drive => (new[]
            {
                new { Value = @"\test azure share", Description = "Drive" },
                new { Value = @"\test.azure#share", Description = "Drive with unacceptable URL character" },
                new { Value = @"\test azure share/", Description = "Drive with trailing slash" },
                new { Value = @"\", Description = "Drive with root path" },
                new { Value = @"", Description = "Drive with no path" },
                new { Value = @"/test azure share\", Description = "Drive with slashes and trailing backslash" },
                new { Value = @"/", Description = "Drive with root slash" }
            }).Select(a =>
                new { Value = @$"{drive.Value}:{a.Value}", Description = $"{drive.Description} {a.Description}", Drive = drive.Value, Path = $"{drive.Value}:{a.Value}" }
            )))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, $"{item.Drive}:", null, item.Path));
            foreach (var item in (new[]
            {
                new { Value = @"test azure share", Description = "Relative path" },
                new { Value = @"test azure share/", Description = "Relative path leading backslash with trailing slash" },
                new { Value = @"test azure share\", Description = "Relative path leading backslash with trailing backslash" },
                new { Value = @"\test azure share", Description = "Relative path with leading backslash" },
                new { Value = @"\test.azure#share", Description = "Relative path with unacceptable URL character" },
                new { Value = @"\test azure share/", Description = "Relative path leading backslash with trailing slash" },
                new { Value = @"\", Description = "Relative path with root only" },
                new { Value = @"", Description = "Empty path" },
                new { Value = @"/test azure share\", Description = "Relative path with leading slash and trailing backslash" },
                new { Value = @"/", Description = "Relative path as root slash" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, null, null, item.Value));

            foreach (var item in (new[]
            {
                new { Value = @"\\-host\test azure share", Description = "Invalid host name #1" },
                new { Value = @"\\SERVICENOWDIAG479 NET\test azure share", Description = "Invalid host name #2" },
                new { Value = @"\\host \test azure share", Description = "Invalid host name #3" },
                new { Value = @"\\192.168..255\test azure share", Description = "Invalid IPV2 address" },
                new { Value = @"\\1:2:3:4:5:6x:7:8\test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = @"\\::3::6:7:8\test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = @"\\:1\test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = @"\\\test azure share", Description = "Mising host #1" },
                new { Value = @"\\\", Description = "Mising host #2" },
                new { Value = @"\\", Description = "Mising host #3" },
                new { Value = @"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\test<share", Description = "URN with invalid file name char" },
                new { Value = @"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\/test azure share", Description = "URN with invalid separator" },
                new { Value = @"C://test azure share", Description = "Drive with invalid separator" },
                new { Value = @"1://test azure share", Description = "Invalid drive #1" },
                new { Value = @"://test azure share", Description = "Invalid drive #2" },
                new { Value = @"AA://test azure share", Description = "Invalid drive #3" },
                new { Value = @"C:/test azure>share", Description = "Drive with invalid file name char" },
                new { Value = @"test azure share\\", Description = "Relative with invalid separator" },
                new { Value = @"/test azure;share", Description = "Drive with invalid file name char" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(false, null, null, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetWindowsFsPathRegexTestCases))]
        public IFuncInvocationResult<string, string, string, bool> WindowsFsPathRegexTest(string uri)
        {
            Match match = UriHelper.Windows.FS_PATH_REGEX.Match(uri);
            if (match.Success)
                return new FuncInvocationResult<string, string, string, bool>(true,
                    (match.Groups["a"].Success) ? match.Groups["a"].Value : null,
                    (match.Groups["h"].Success) ? match.Groups["h"].Value : null,
                    (match.Groups["p"].Success) ? match.Groups["p"].Value : null);
            return new FuncInvocationResult<string, string, string, bool>(false, null, null, null);
        }

        public static IEnumerable<TestCaseData> GetWindowsFsPathPatternTestCases()
        {
            foreach (var item in (new[]
            {
                new { Value = "SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET", Description = "FQDN Host" },
                new { Value = "MyHost", Description = "AName host" },
                new { Value = "192.168.1.1", Description = "IPV2 host" },
                new { Value = "::1", Description = "Simple IPV6 host" },
                new { Value = "1:2:3:4:5:6:7:8", Description = "Full IPV6 host" },
                new { Value = "::3:4:5:6:7:8", Description = "Leading partial IPV6 host" },
                new { Value = "1:2:3:4:5:6::", Description = "Trailing partial IPV6 host" },
                new { Value = "1:2::6:7:8", Description = "Inner partial IPV6 host" }
            }).SelectMany(host => (new[]
            {
                new { Value = @"\test azure share", Description = "URN" },
                new { Value = @"\test.azure#share", Description = "URN with unacceptable URL character" },
                new { Value = @"\test azure share/", Description = "URN with trailing slash" },
                new { Value = @"\", Description = "URN with root path" },
                new { Value = @"", Description = "URN with no path" },
                new { Value = @"/test azure share\", Description = "URN with slashes and trailing backslash" },
                new { Value = @"/", Description = "URN with root slash" }
            }).SelectMany(a => (new[] { new { Value = "//", Name = "2 slashes" }, new { Value = "\\\\", Name = "2 backslashes" } }).Select(s =>
                new { Value = @$"{s.Value}{host.Value}{a.Value}", Description = $"{s.Name}; {host.Description} {a.Description}" }
            ))).SelectMany(v => (new[] { new { Value = v.Value, Description = v.Description }, new { Value = $" {v.Value}", Description = $"{v.Description} and leading space" } })))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(true, item.Value));
            foreach (var item in (new[]
            {
                new { Value = "A", Description = "UC A" },
                new { Value = "a", Description = "LC a" },
                new { Value = "Z", Description = "UC Z" },
                new { Value = "z", Description = "LC z" },
            }).SelectMany(drive => (new[]
            {
                new { Value = @"\test azure share", Description = "Drive" },
                new { Value = @"\test.azure#share", Description = "Drive with unacceptable URL character" },
                new { Value = @"\test azure share/", Description = "Drive with trailing slash" },
                new { Value = @"\", Description = "Drive with root path" },
                new { Value = @"", Description = "Drive with no path" },
                new { Value = @"/test azure share\", Description = "Drive with slashes and trailing backslash" },
                new { Value = @"/", Description = "Drive with root slash" }
            }).Select(a =>
                new { Value = @$"{drive.Value}:{a.Value}", Description = $"{drive.Description} {a.Description}" }
            )).SelectMany(v => (new[] { new { Value = v.Value, Description = v.Description }, new { Value = $" {v.Value}", Description = $"{v.Description} and leading space" } })))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(true, item.Value));
            foreach (var item in (new[]
            {
                new { Value = @"test azure share", Description = "Relative path" },
                new { Value = @"test azure share/", Description = "Relative path leading backslash with trailing slash" },
                new { Value = @"test azure share\", Description = "Relative path leading backslash with trailing backslash" },
                new { Value = @"\test azure share", Description = "Relative path with leading backslash" },
                new { Value = @"\test.azure#share", Description = "Relative path with unacceptable URL character" },
                new { Value = @"\test azure share/", Description = "Relative path leading backslash with trailing slash" },
                new { Value = @"\", Description = "Relative path with root only" },
                new { Value = @"", Description = "Empty path" },
                new { Value = @"/test azure share\", Description = "Relative path with leading slash and trailing backslash" },
                new { Value = @"/", Description = "Relative path as root slash" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(true, item.Value));

            foreach (var item in (new[]
            {
                new { Value = @"\\-host\test azure share", Description = "Invalid host name #1" },
                new { Value = @"\\SERVICENOWDIAG479 NET\test azure share", Description = "Invalid host name #2" },
                new { Value = @"\\host \test azure share", Description = "Invalid host name #3" },
                new { Value = @"\\192.168..255\test azure share", Description = "Invalid IPV2 address" },
                new { Value = @"\\1:2:3:4:5:6x:7:8\test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = @"\\::3::6:7:8\test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = @"\\:1\test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = @"\\\test azure share", Description = "Mising host #1" },
                new { Value = @"\\\", Description = "Mising host #2" },
                new { Value = @"\\", Description = "Mising host #3" },
                new { Value = @"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\test<share", Description = "URN with invalid file name char" },
                new { Value = @"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\/test azure share", Description = "URN with invalid separator" },
                new { Value = @"C://test azure share", Description = "Drive with invalid separator" },
                new { Value = @"1:/test azure share", Description = "Invalid drive #1" },
                new { Value = @":/test azure share", Description = "Invalid drive #2" },
                new { Value = @"AA:/test azure share", Description = "Invalid drive #3" },
                new { Value = @"C:/test azure>share", Description = "Drive with invalid file name char" },
                new { Value = @"test azure share\\", Description = "Relative with invalid separator" },
                new { Value = @"/test azure;share", Description = "Drive with invalid file name char" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(false, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetWindowsFsPathPatternTestCases))]
        public IFuncInvocationResult<string, bool> WindowsFsPathPatternTest(string uri)
        {
            Match match = Regex.Match(uri, UriHelper.Windows.PATTERN_ABS_FS_PATH);
            if (match.Success)
                return new FuncInvocationResult<string, bool>(true, match.Value);
            return new FuncInvocationResult<string, bool>(false, null);
        }

        public static IEnumerable<TestCaseData> GetLinuxFsPathRegexTestCases()
        {
            foreach (var item in (new[]
            {
                new { Value = "SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET", Description = "FQDN Host" },
                new { Value = "MyHost", Description = "AName host" },
                new { Value = "192.168.1.1", Description = "IPV2 host" },
                new { Value = "::1", Description = "Simple IPV6 host" },
                new { Value = "1:2:3:4:5:6:7:8", Description = "Full IPV6 host" },
                new { Value = "::3:4:5:6:7:8", Description = "Leading partial IPV6 host" },
                new { Value = "1:2:3:4:5:6::", Description = "Trailing partial IPV6 host" },
                new { Value = "1:2::6:7:8", Description = "Inner partial IPV6 host" },
            }).SelectMany(host => (new[]
            {
                new { Value = "/test azure share", Description = "URN" },
                new { Value = "/test.azure#share", Description = "URN with unacceptable URL character" },
                new { Value = "/test azure share/", Description = "URN with trailing slash" },
                new { Value = "/", Description = "URN with root path" },
                new { Value = "", Description = "URN with no path" }
            }).Select(a =>
                new { Value = @$"//{host.Value}{a.Value}", Description = $"{host.Description} {a.Description}", Abs = (a.Value.Length > 0) ? "/" : null, Host = host.Value, Path = a.Value }
            )))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, item.Abs, item.Host, item.Path));
            foreach (var item in (new[]
            {
                new { Value = "A", Description = "UC A" },
                new { Value = "a", Description = "LC a" },
                new { Value = "Z", Description = "UC Z" },
                new { Value = "z", Description = "LC z" }
            }).SelectMany(drive => (new[]
            {
                new { Value = @"/test azure share", Description = "Drive" },
                new { Value = @"/test.azure#share", Description = "Drive with unacceptable URL character" },
                new { Value = @"/test azure share/", Description = "Drive with trailing slash" },
                new { Value = @"/", Description = "Drive with root path" },
                new { Value = @"", Description = "Drive with no path" }
            }).Select(a =>
                new { Value = @$"{drive.Value}:{a.Value}", Description = $"{drive.Description} {a.Description}" }
            )))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, null, null, item.Value));
            foreach (var item in (new[]
            {
                new { Value = @"/test azure share", Description = "Relative path leading slash" },
                new { Value = @"/test azure share/", Description = "Relative path leading and trailing slash" },
                new { Value = @"/test.azure#share", Description = "Relative path with unacceptable URL character" },
                new { Value = @"/", Description = "Relative path with root only" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, "/", null, item.Value));
            foreach (var item in (new[]
            {
                new { Value = @"test azure share", Description = "Relative path" },
                new { Value = @"test azure share/", Description = "Relative path trailing slash" },
                new { Value = @"", Description = "Empty path" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(true, null, null, item.Value));

            foreach (var item in (new[]
            {
                new { Value = "//-host/test azure share", Description = "Invalid host name #1" },
                new { Value = "//SERVICENOWDIAG479 NET/test azure share", Description = "Invalid host name #2" },
                new { Value = "//host /test azure share", Description = "Invalid host name #3" },
                new { Value = "//192.168..255/test azure share", Description = "Invalid IPV2 address" },
                new { Value = "//1:2:3:4:5:6x:7:8/test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = "//::3::6:7:8/test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = "//:1/test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = "///test azure share", Description = "Mising host #1" },
                new { Value = "///", Description = "Mising host #2" },
                new { Value = "//", Description = "Mising host #3" },
                new { Value = "//SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/test\u0000share", Description = "URN with invalid file name char" },
                new { Value = "//SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET//test azure share", Description = "URN with invalid separator" },
                new { Value = "C://test azure share", Description = "Drive with invalid separator" },
                new { Value = "C:/test azure\u0000share", Description = "Drive with invalid file name char" },
                new { Value = "test azure share//", Description = "Relative with invalid separator" },
                new { Value = "/test azure\u0000share", Description = "Relative with invalid file name char" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathRegex: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, bool>(false, null, null, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetLinuxFsPathRegexTestCases))]
        public IFuncInvocationResult<string, string, string, bool> LinuxFsPathRegexTest(string uri)
        {
            Match match = UriHelper.Linux.FS_PATH_REGEX.Match(uri);
            if (match.Success)
                return new FuncInvocationResult<string, string, string, bool>(true,
                    (match.Groups["a"].Success) ? match.Groups["a"].Value : null,
                    (match.Groups["h"].Success) ? match.Groups["h"].Value : null,
                    (match.Groups["p"].Success) ? match.Groups["p"].Value : null);
            return new FuncInvocationResult<string, string, string, bool>(false, null, null, null);
        }

        public static IEnumerable<TestCaseData> GetLinuxFsPathPatternTestCases()
        {
            foreach (var item in (new[]
            {
                new { Value = "SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET", Description = "FQDN Host" },
                new { Value = "MyHost", Description = "AName host" },
                new { Value = "192.168.1.1", Description = "IPV2 host" },
                new { Value = "::1", Description = "Simple IPV6 host" },
                new { Value = "1:2:3:4:5:6:7:8", Description = "Full IPV6 host" },
                new { Value = "::3:4:5:6:7:8", Description = "Leading partial IPV6 host" },
                new { Value = "1:2:3:4:5:6::", Description = "Trailing partial IPV6 host" },
                new { Value = "1:2::6:7:8", Description = "Inner partial IPV6 host" }
            }).SelectMany(host => (new[]
            {
                new { Value = "/test azure share", Description = "URN" },
                new { Value = "/test.azure#share", Description = "URN with unacceptable URL character" },
                new { Value = "/test azure share/", Description = "URN with trailing slash" },
                new { Value = "/", Description = "URN with root path" },
                new { Value = "", Description = "URN with no path" },
                new { Value = "/test azure share/", Description = "URN with slashes and trailing backslash" }
            }).SelectMany(a => (new[] { new { Value = "//", Name = "2 slashes" }, new { Value = "//", Name = "2 backslashes" } }).Select(s =>
                new { Value = $"{s.Value}{host.Value}{a.Value}", Description = $"{s.Name}; {host.Description} {a.Description}" }
            ))).SelectMany(v => (new[] { new { Value = v.Value, Description = v.Description }, new { Value = $" {v.Value}", Description = $"{v.Description} and leading space" } })))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(true, item.Value));
            foreach (var item in (new[]
            {
                new { Value = "A", Description = "UC A" },
                new { Value = "a", Description = "LC a" },
                new { Value = "Z", Description = "UC Z" },
                new { Value = "z", Description = "LC z" }
            }).SelectMany(drive => (new[]
            {
                new { Value = "/test azure share", Description = "Drive" },
                new { Value = "/test.azure#share", Description = "Drive with unacceptable URL character" },
                new { Value = "/test azure share/", Description = "Drive with trailing slash" },
                new { Value = "/", Description = "Drive with root path" },
                new { Value = "", Description = "Drive with no path" },
                new { Value = "/test azure share/", Description = "Drive with slashes and trailing backslash" }
            }).Select(a =>
                new { Value = $"{drive.Value}:{a.Value}", Description = $"{drive.Description} {a.Description}" }
            )).SelectMany(v => (new[] { new { Value = v.Value, Description = v.Description }, new { Value = $" {v.Value}", Description = $"{v.Description} and leading space" } })))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(false, null));
            foreach (var item in (new[]
            {
                new { Value = "test azure share", Description = "Relative path" },
                new { Value = "test azure share/", Description = "Relative path with trailing slash" },
                new { Value = "", Description = "Empty path" },
                new { Value = @"\test azure share/", Description = "Relative path with leading backslash and trailing slash" },
                new { Value = "\\", Description = "Relative path as root baclslash" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(false, null));
            foreach (var item in (new[]
            {
                new { Value = "/test azure share", Description = "Rooted path" },
                new { Value = "/test azure share/", Description = "Rooted path with trailing slash" },
                new { Value = "/test.azure#share", Description = "Rooted path with unacceptable URL character" },
                new { Value = "/", Description = "Rooted path with slash only" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(true, item.Value));

            foreach (var item in (new[]
            {
                new { Value = @"\\MyHost/test azure share", Description = "Invalid URN separator #1" },
                new { Value = "//MyHost\u0000test azure share", Description = "Invalid URN separator #2" },
                new { Value = "//-host/test azure share", Description = "Invalid host name #1" },
                new { Value = "//SERVICENOWDIAG479 NET/test azure share", Description = "Invalid host name #2" },
                new { Value = "//host /test azure share", Description = "Invalid host name #3" },
                new { Value = "//192.168..255/test azure share", Description = "Invalid IPV2 address" },
                new { Value = "//1:2:3:4:5:6x:7:8/test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = "//::3::6:7:8/test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = "//:1/test azure share", Description = "Invalid IPV6 address #1" },
                new { Value = "///test azure share", Description = "Mising host #1" },
                new { Value = "///", Description = "Mising host #2" },
                new { Value = "//", Description = "Mising host #3" },
                new { Value = "//SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/test\u0000share", Description = "URN with invalid file name char" },
                new { Value = "//SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET//test azure share", Description = "URN with invalid separator" },
                new { Value = "C://test azure share", Description = "Drive with invalid separator" },
                new { Value = "1://test azure share", Description = "Invalid drive #1" },
                new { Value = "://test azure share", Description = "Invalid drive #2" },
                new { Value = "AA://test azure share", Description = "Invalid drive #3" },
                new { Value = "C:/test azure\u0000share", Description = "Drive with invalid file name char" },
                new { Value = "test azure share//", Description = "Relative with invalid separator" },
                new { Value = "/test azure\u0000share", Description = "Rooted with invalid file name char" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"LinuxFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, bool>(false, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetLinuxFsPathPatternTestCases))]
        public IFuncInvocationResult<string, bool> LinuxFsPathPatternTest(string uri)
        {
            Match match = Regex.Match(uri, UriHelper.Linux.PATTERN_ABS_FS_PATH);
            if (match.Success)
                return new FuncInvocationResult<string, bool>(true, match.Value);
            return new FuncInvocationResult<string, bool>(false, null);
        }

        public static IEnumerable<TestCaseData> GetWindowsFormatGuessRegexTestCases()
        {
            yield return new TestCaseData("file://mysite.org/dir/file.txt")
                .SetDescription("WindowsFormatGuess: file URL")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file://mysite.org/dir/file.txt", // f
                    null, // u
                    "mysite.org", // h
                    "/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData(@"file:\\mysite.org\dir\file.txt")
                .SetDescription("WindowsFormatGuess: file URL with alt separators")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    @"file:\\mysite.org\dir\file.txt", // f
                    null, // u
                    "mysite.org", // h
                    @"\dir\file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("file://mysite.org/dir/")
                .SetDescription("WindowsFormatGuess: file URL, trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file://mysite.org/dir/", // f
                    null, // u
                    "mysite.org", // h
                    "/dir/", // p
                    null // x
                ));
            yield return new TestCaseData("file:///C:/dir/file.txt")
                .SetDescription("WindowsFormatGuess: file drive URL")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file:///C:/dir/file.txt", // f
                    null, // u
                    null, // h
                    "/C:/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("file:\\\\\\C:\\dir\\file.txt")
                .SetDescription("WindowsFormatGuess: file drive URL with alt separators")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file:\\\\\\C:\\dir\\file.txt", // f
                    null, // u
                    null, // h
                    "\\C:\\dir\\file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("file:///C:/dir/")
                .SetDescription("WindowsFormatGuess: file drive URL, trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file:///C:/dir/", // f
                    null, // u
                    null, // h
                    "/C:/dir/", // p
                    null // x
                ));
            yield return new TestCaseData("file://C:/dir/file.txt")
                .SetDescription("WindowsFormatGuess: file URL bad host")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file://C:/dir/file.txt", // f
                    null, // u
                    "C:", // h
                    "/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData(@"\\mysite.org\dir\file.txt")
                .SetDescription("WindowsFormatGuess: URN string")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    @"\\mysite.org\dir\file.txt", // u
                    "mysite.org", // h
                    @"\dir\file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("//mysite.org/dir/file.txt")
                .SetDescription("WindowsFormatGuess: URN with alt separators")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    "//mysite.org/dir/file.txt", // u
                    "mysite.org", // h
                    "/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData(@"\\mysite.org\dir\")
                .SetDescription("WindowsFormatGuess: URN string with trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    @"\\mysite.org\dir\", // u
                    "mysite.org", // h
                    @"\dir\", // p
                    null // x
                ));
            yield return new TestCaseData(@"\\mysite.org\")
                .SetDescription("WindowsFormatGuess: URN string with root only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    @"\\mysite.org\", // u
                    "mysite.org", // h
                    @"\", // p
                    null // x
                ));
            yield return new TestCaseData(@"\\mysite.org")
                .SetDescription("WindowsFormatGuess: URN string with no path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    @"\\mysite.org", // u
                    "mysite.org", // h
                    @"", // p
                    null // x
                ));
            yield return new TestCaseData(@"C:\mysite.org\file.txt")
                .SetDescription("WindowsFormatGuess: UC Drive path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"C:\mysite.org\file.txt", // p
                    null // x
                ));
            yield return new TestCaseData(@"z:\mysite.org\file.txt")
                .SetDescription("WindowsFormatGuess: LC Drive path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"z:\mysite.org\file.txt", // p
                    null // x
                ));
            yield return new TestCaseData(@"c:\")
                .SetDescription("WindowsFormatGuess: Drive root")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"c:\", // p
                    null // x
                ));
            yield return new TestCaseData(@"Z:")
                .SetDescription("WindowsFormatGuess: Drive only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"Z:", // p
                    null // x
                ));
            yield return new TestCaseData(@"C:\mysite.org\dir\")
                .SetDescription("WindowsFormatGuess: Drive with trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"C:\mysite.org\dir\", // p
                    null // x
                ));
            yield return new TestCaseData(@"C:\")
                .SetDescription("WindowsFormatGuess: Drive with root path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"C:\", // p
                    null // x
                ));
            yield return new TestCaseData(@"C:")
                .SetDescription("WindowsFormatGuess: Drive with colon only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"C:", // p
                    null // x
                ));
            yield return new TestCaseData(@"C:/mysite.org\dir/")
                .SetDescription("WindowsFormatGuess: Drive with alt separators")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    @"C:/mysite.org\dir/", // p
                    null // x
                ));
            yield return new TestCaseData("http://tempuri.org/my/file.txt")
                .SetDescription("WindowsFormatGuess: Alt scheme")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    "tempuri.org", // h
                    "/my/file.txt", // p
                    "http://tempuri.org/my/file.txt" // x
                ));
            yield return new TestCaseData("xx:")
                .SetDescription("WindowsFormatGuess: Alt scheme only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    "", // p
                    "xx:" // x
                ));

            foreach (var item in (new[]
            {
                new { Value = "", Description = "Empty path" },
                new { Value = "/", Description = "Relative path slash only" },
                new { Value = @"\mysite.org\dir\file.txt", Description = "Relative path with leading backslash" },
                new { Value = @"mysite.org\dir\file.txt", Description = "Relative path" },
                new { Value = "//", Description = "Missing host #1" },
                new { Value = "///", Description = "Missing host #1" },
                new { Value = "///test", Description = "Missing host #3" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription($"WindowsFsPathPattern: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(false,  null,  null,  null, null, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetWindowsFormatGuessRegexTestCases))]
        public IFuncInvocationResult<string, string, string, string, string, bool> WindowsFormatGuessRegexTest(string uri)
        {
            Match match = UriHelper.Windows.FORMAT_GUESS_REGEX.Match(uri);
            if (match.Success)
                return new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    (match.Groups["f"].Success) ? match.Groups["f"].Value : null,
                    (match.Groups["u"].Success) ? match.Groups["u"].Value : null,
                    (match.Groups["h"].Success) ? match.Groups["h"].Value : null,
                    (match.Groups["p"].Success) ? match.Groups["p"].Value : null,
                    (match.Groups["x"].Success) ? match.Groups["x"].Value : null);
            return new FuncInvocationResult<string, string, string, string, string, bool>(false, null, null, null, null, null);
        }

        public static IEnumerable<TestCaseData> GetLinuxFormatGuessRegexTestCases()
        {
            yield return new TestCaseData("file://mysite.org/dir/file.txt")
                .SetDescription("LinuxFormatGuess: file URL")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file://mysite.org/dir/file.txt", // f
                    null, // u
                    "mysite.org", // h
                    "/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("file://mysite.org/dir/")
                .SetDescription("LinuxFormatGuess: file URL, trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file://mysite.org/dir/", // f
                    null, // u
                    "mysite.org", // h
                    "/dir/", // p
                    null // x
                ));
            yield return new TestCaseData("file:///C:/dir/file.txt")
                .SetDescription("LinuxFormatGuess: file drive URL")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file:///C:/dir/file.txt", // f
                    null, // u
                    null, // h
                    "/C:/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("file:///C:/dir/")
                .SetDescription("LinuxFormatGuess: file drive URL, trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file:///C:/dir/", // f
                    null, // u
                    null, // h
                    "/C:/dir/", // p
                    null // x
                ));
            yield return new TestCaseData("file://C:/dir/file.txt")
                .SetDescription("LinuxFormatGuess: file URL bad host")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    "file://C:/dir/file.txt", // f
                    null, // u
                    "C:", // h
                    "/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("//mysite.org/dir/file.txt")
                .SetDescription("LinuxFormatGuess: URN")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    "//mysite.org/dir/file.txt", // u
                    "mysite.org", // h
                    "/dir/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("//mysite.org/dir/")
                .SetDescription("LinuxFormatGuess: URN string with trailing slash")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    "//mysite.org/dir/", // u
                    "mysite.org", // h
                    "/dir/", // p
                    null // x
                ));
            yield return new TestCaseData("//mysite.org/")
                .SetDescription("LinuxFormatGuess: URN string with root only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    "//mysite.org/", // u
                    "mysite.org", // h
                    "/", // p
                    null // x
                ));
            yield return new TestCaseData("//mysite.org")
                .SetDescription("LinuxFormatGuess: URN string with no path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    "//mysite.org", // u
                    "mysite.org", // h
                    "", // p
                    null // x
                ));
            yield return new TestCaseData("/mysite.org/file.txt")
                .SetDescription("LinuxFormatGuess: Rooted path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    "/mysite.org/file.txt", // p
                    null // x
                ));
            yield return new TestCaseData("/")
                .SetDescription("LinuxFormatGuess: Root only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    "/", // p
                    null // x
                ));
            yield return new TestCaseData("http://tempuri.org/my/file.txt")
                .SetDescription("LinuxFormatGuess: Alt scheme")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    "tempuri.org", // h
                    "/my/file.txt", // p
                    "http://tempuri.org/my/file.txt" // x
                ));
            yield return new TestCaseData("xx:")
                .SetDescription("LinuxFormatGuess: Alt scheme only")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    null, // h
                    "", // p
                    "xx:" // x
                ));
            yield return new TestCaseData(@"C:\mysite.org\file.txt")
                .SetDescription("Drive path")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u  
                    @"\mysite.org\file.txt", // h
                    "", // p
                    @"C:\mysite.org\file.txt" // x
                ));
            yield return new TestCaseData(@"file:\\mysite.org\dir\file.txt")
                .SetDescription("File URL with alt separators")
                .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    null, // f
                    null, // u
                    @"\\mysite.org\dir\file.txt", // h
                    "", // p
                    @"file:\\mysite.org\dir\file.txt" // x
                ));

            foreach (var item in (new[]
            {
                new { Value = "", Description = "Empty path" },
                new { Value = @"mysite.org\dir\file.txt", Description = "Relative path" },
                new { Value = "//", Description = "Missing host #1" },
                new { Value = "///", Description = "Missing host #1" },
                new { Value = "///test", Description = "Missing host #3" }
            }))
                yield return new TestCaseData(item.Value)
                    .SetDescription("LinuxFormatGuess: {item.Description}")
                    .Returns(new FuncInvocationResult<string, string, string, string, string, bool>(false, null, null, null, null, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetLinuxFormatGuessRegexTestCases))]
        public IFuncInvocationResult<string, string, string, string, string, bool> LinuxFormatGuessRegexTest(string uri)
        {
            Match match = UriHelper.Linux.FORMAT_GUESS_REGEX.Match(uri);
            if (match.Success)
                return new FuncInvocationResult<string, string, string, string, string, bool>(true,
                    (match.Groups["f"].Success) ? match.Groups["f"].Value : null,
                    (match.Groups["u"].Success) ? match.Groups["u"].Value : null,
                    (match.Groups["h"].Success) ? match.Groups["h"].Value : null,
                    (match.Groups["p"].Success) ? match.Groups["p"].Value : null,
                    (match.Groups["x"].Success) ? match.Groups["x"].Value : null);
            return new FuncInvocationResult<string, string, string, string, string, bool>(false, null, null, null, null, null);
        }

        public static IEnumerable<TestCaseData> GetAuthorityCaseInsensitiveEqualsTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative), new Uri("", UriKind.Relative))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: 2 Empty Uri values")
                .Returns(true);
            yield return new TestCaseData(new Uri(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\testazureshare", UriKind.Absolute),
                    new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: 2 different original strings and case, same absolute URI")
                .Returns(true);
            yield return new TestCaseData(new Uri("http://tempuri.org/TEST", UriKind.Absolute), new Uri("http://tempuri.org/test", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Same host, different path case")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/TEST", UriKind.Absolute), new Uri("HTTP://TEMPURI.ORG/test", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Alternating cases")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org:80", UriKind.Absolute), new Uri("http://tempuri.org:75", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Same host, different port")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/", UriKind.Absolute), new Uri("http://tempuri.org", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Same host, one  omitting root slash")
                .Returns(true);
            yield return new TestCaseData(new Uri("HTTP://tempuri.org/", UriKind.Absolute), new Uri("http://tempuri.org", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Different case scheme")
                .Returns(true);
            yield return new TestCaseData(new Uri("http://tempuri.org/", UriKind.Absolute), new Uri("https://tempuri.org", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Alternating scheme")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://user@tempuri.org/", UriKind.Absolute), new Uri("http://used@tempuri.org", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Alternating username")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/?test=one", UriKind.Absolute), new Uri("http://tempuri.org?test=two", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Alternating query")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/?test=one", UriKind.Absolute), new Uri("http://tempuri.org?test=ONE", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Alternating query case")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/?test=one#six", UriKind.Absolute), new Uri("http://tempuri.org?test=one#two", UriKind.Absolute))
                .SetDescription("AuthorityCaseInsensitiveEqualsTest: Alternating fragment")
                .Returns(false);
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetAuthorityCaseInsensitiveEqualsTestCases))]
        public bool AuthorityCaseInsensitiveEqualsTest(Uri x, Uri y) => x.AuthorityCaseInsensitiveEquals(y);

        public static IEnumerable<TestCaseData> GetAsRelativeUriStringTestCases()
        {
            yield return new TestCaseData("")
                .SetDescription("AsRelativeUriStringTest: 2 Empty Uri values")
                .Returns("");
            yield return new TestCaseData(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\testazureshare")
                .SetDescription("AsRelativeUriStringTest: UNC path")
                .Returns("/testazureshare");
            yield return new TestCaseData("http://tempuri.org/TEST Path")
                .SetDescription("AsRelativeUriStringTest: Absolute URI")
                .Returns("/TEST%20Path");
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetAsRelativeUriStringTestCases))]
        public string AsRelativeUriStringTest(string text)
        {
            return text.AsRelativeUriString();
        }

        public static IEnumerable<TestCaseData> GetAsUserNameComponentEncodedTestCases()
        {
            yield return new TestCaseData("")
                .SetDescription("AsUserNameComponentEncodedTest: 2 Empty Uri values")
                .Returns("");
            yield return new TestCaseData("!")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("!");
            yield return new TestCaseData("@")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%40");
            yield return new TestCaseData("#")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%23");
            yield return new TestCaseData("$")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("$");
            yield return new TestCaseData("%")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%25");
            yield return new TestCaseData("^")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%5E");
            yield return new TestCaseData("&")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("&");
            yield return new TestCaseData("*")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("*");
            yield return new TestCaseData("(")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("(");
            yield return new TestCaseData(")")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns(")");
            yield return new TestCaseData("_")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("_");
            yield return new TestCaseData("+")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("+");
            yield return new TestCaseData("-")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("-");
            yield return new TestCaseData("=")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("=");
            yield return new TestCaseData("[")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("[");
            yield return new TestCaseData("]")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("]");
            yield return new TestCaseData("\\")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%5C");
            yield return new TestCaseData(";")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns(";");
            yield return new TestCaseData("'")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("'");
            yield return new TestCaseData(".")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns(".");
            yield return new TestCaseData("/")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%2F");
            yield return new TestCaseData(">")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%3E");
            yield return new TestCaseData("?")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%3F");
            yield return new TestCaseData(":")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%3A");
            yield return new TestCaseData("\"")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%22");
            yield return new TestCaseData("{")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%7B");
            yield return new TestCaseData("}")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%7D");
            yield return new TestCaseData("|")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%7C");
            yield return new TestCaseData(@"!@#$%^&*()_+-=[]\;'./>?:""{}|")
                .SetDescription("AsUserNameComponentEncodedTest: UNC path")
                .Returns("!%40%23$%25%5E&*()_+-=[]%5C;'.%2F%3E%3F%3A%22%7B%7D%7C");
            yield return new TestCaseData("http://tempuri.org/TEST Path")
                .SetDescription("AsUserNameComponentEncodedTest: Absolute URI")
                .Returns("http%3A%2F%2Ftempuri.org%2FTEST%20Path");
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetAsUserNameComponentEncodedTestCases))]
        public string AsUserNameComponentEncodedTest(string text) => text.AsUserNameComponentEncoded();

        public static IEnumerable<TestCaseData> GetAsPasswordComponentEncodedTestCases()
        {
            yield return new TestCaseData("")
                .SetDescription("AsPasswordComponentEncodedTest: 2 Empty Uri values")
                .Returns("");
            yield return new TestCaseData("!")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("!");
            yield return new TestCaseData("@")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%40");
            yield return new TestCaseData("#")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%23");
            yield return new TestCaseData("$")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("$");
            yield return new TestCaseData("%")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%25");
            yield return new TestCaseData("^")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%5E");
            yield return new TestCaseData("&")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("&");
            yield return new TestCaseData("*")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("*");
            yield return new TestCaseData("(")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("(");
            yield return new TestCaseData(")")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns(")");
            yield return new TestCaseData("_")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("_");
            yield return new TestCaseData("+")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("+");
            yield return new TestCaseData("-")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("-");
            yield return new TestCaseData("=")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("=");
            yield return new TestCaseData("[")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("[");
            yield return new TestCaseData("]")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("]");
            yield return new TestCaseData("\\")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%5C");
            yield return new TestCaseData(";")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns(";");
            yield return new TestCaseData("'")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("'");
            yield return new TestCaseData(".")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns(".");
            yield return new TestCaseData("/")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%2F");
            yield return new TestCaseData(">")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%3E");
            yield return new TestCaseData("?")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%3F");
            yield return new TestCaseData(":")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns(":");
            yield return new TestCaseData("\"")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%22");
            yield return new TestCaseData("{")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%7B");
            yield return new TestCaseData("}")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%7D");
            yield return new TestCaseData("|")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%7C");
            yield return new TestCaseData(@"!@#$%^&*()_+-=[]\;'./>?:""{}|")
                .SetDescription("AsPasswordComponentEncodedTest: UNC path")
                .Returns("!%40%23$%25%5E&*()_+-=[]%5C;'.%2F%3E%3F:%22%7B%7D%7C");
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetAsPasswordComponentEncodedTestCases))]
        public string AsPasswordComponentEncodedTest(string text) => text.AsPasswordComponentEncoded();

        public static IEnumerable<TestCaseData> GetGetUserNameAndPasswordTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative))
                .SetDescription("GetUserNameAndPasswordTest: Empty Uri")
                .Returns(new Tuple<string, string>(null, null));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute))
                .SetDescription("GetUserNameAndPasswordTest: User and password")
                .Returns(new Tuple<string, string>("me", "thepw"));
            yield return new TestCaseData(new Uri("http://justuser@host.com", UriKind.Absolute))
                .SetDescription("GetUserNameAndPasswordTest: Only user")
                .Returns(new Tuple<string, string>("justuser", null));
            yield return new TestCaseData(new Uri("http://:justpw@host.com", UriKind.Absolute))
                .SetDescription("GetUserNameAndPasswordTest: Password with empty user")
                .Returns(new Tuple<string, string>("", "justpw"));
            yield return new TestCaseData(new Uri("http://host.com", UriKind.Absolute))
                .SetDescription("GetUserNameAndPasswordTest: No user or pw")
                .Returns(new Tuple<string, string>(null, null));
            yield return new TestCaseData(null)
                .SetDescription("GetUserNameAndPasswordTest: null value")
                .Returns(new Tuple<string, string>(null, null));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource(nameof(GetGetUserNameAndPasswordTestCases))]
        public Tuple<string, string> GetUserNameAndPasswordTest(Uri uri)
        {
            string userName = uri.GetUserNameAndPassword(out string password);
            return new Tuple<string, string>(userName, password);
        }

        public static IEnumerable<TestCaseData> GetTrySetUserInfoComponentTestCases()
        {
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), "new", "pw")
                .SetDescription("TrySetUserInfoComponentTest: Replace username password")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new:pw@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://justuser@host.com", UriKind.Absolute), "xyz", "pdq")
                .SetDescription("TrySetUserInfoComponentTest: Add username and password")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://xyz:pdq@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://:justpw@host.com", UriKind.Absolute), "xyz", "justpw")
                .SetDescription("TrySetUserInfoComponentTest: Set username")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://xyz:justpw@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), "me", "thepw")
                .SetDescription("TrySetUserInfoComponentTest: Setting same password")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new:pw@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), null, null)
                .SetDescription("TrySetUserInfoComponentTest: Remove")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), null, "thepw")
                .SetDescription("TrySetUserInfoComponentTest: Remove by setting user to null")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), "me", null)
                .SetDescription("TrySetUserInfoComponentTest: Remove just pw")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://host.com", UriKind.Absolute), null, null)
                .SetDescription("TrySetUserInfoComponentTest: Remove nothing")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("thepw@host.com", UriKind.Relative), "new", "pw")
                .SetDescription("TrySetUserInfoComponentTest: Replace username password")
                .Returns(new Tuple<bool, Uri>(false, new Uri("thepw@host.com", UriKind.Relative)));
            yield return new TestCaseData(new Uri("file:///NoHost", UriKind.Absolute), "new", "pw")
                .SetDescription("TrySetUserInfoComponentTest: Replace username password")
                .Returns(new Tuple<bool, Uri>(false, new Uri("file:///NoHost", UriKind.Absolute)));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource(nameof(GetTrySetUserInfoComponentTestCases))]
        public Tuple<bool, Uri> TrySetUserInfoComponentTest(Uri uri, string userName, string password)
        {
            bool returnValue = uri.TrySetUserInfoComponent(userName, password, out Uri result);
            return new Tuple<bool, Uri>(returnValue, result);
        }

        public static IEnumerable<TestCaseData> GetTrySetHostComponentTestCases()
        {
            yield return new TestCaseData(new Uri("http://me:thepw@host.com:8080/test?qry=true#mark", UriKind.Absolute), "new", (int?)null)
                .SetDescription("TrySetHostComponentTest: Replace between user info and path")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me:thepw@new/test?qry=true#mark", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("file://mysite/mypath", UriKind.Absolute), null, (int?)null)
                .SetDescription("TrySetHostComponentTest: Remove")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file:///mypath", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("file:///mypath", UriKind.Absolute), "mysite", (int?)null)
                .SetDescription("TrySetHostComponentTest: Insert")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://mysite/mypath", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com:8080", UriKind.Absolute), "new", 9090)
                .SetDescription("TrySetHostComponentTest: Replace after user info and no path, with port")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me:thepw@new:9090", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com:8080", UriKind.Absolute), "new", (int?)null)
                .SetDescription("TrySetHostComponentTest: Replace after user info and no path")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me:thepw@new", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://host.com:8080/test?qry=true#mark", UriKind.Absolute), "new", 9000)
                .SetDescription("TrySetHostComponentTest: Replace between scheme and path, with port")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new:9000/test?qry=true#mark", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://host.com:8080/test?qry=true#mark", UriKind.Absolute), "new", (int?)null)
                .SetDescription("TrySetHostComponentTest: Replace between scheme and path")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new/test?qry=true#mark", UriKind.Absolute)));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource(nameof(GetTrySetHostComponentTestCases))]
        public Tuple<bool, Uri> TrySetHostComponentTest(Uri uri, string hostName, int? port)
        {
            bool returnValue = uri.TrySetHostComponent(hostName, port, out Uri result);
            return new Tuple<bool, Uri>(returnValue, result);
        }

        //public static IEnumerable<TestCaseData> GetTrySetPathComponentTestCases()
        //{
        //    string[] authorityComponents = new string[] { "", "http://tempuri.org", "https://login:pw@tempuri:8080", "file://", "file://MyShare" };
        //    var pathComponents = (new string[] { "", "Temp", "My Dir" }).SelectMany(s => new string[] { s, $"{s}/", $"{s}/t.data", $"{s}/My File.txt", $"{s}/Sub Dir/" })
        //        .Select(s => new { Raw = s, Escaped = Uri.EscapeUriString(s) }); 
        //    string[] queryComponents = new string[] { "", "?", "?key=value" };
        //    string[] fragmentComponents = new string[] { "", "#", "#fragment" };
        //    return pathComponents.Concat(pathComponents.Where(p => !p.Raw.Equals(p.Escaped)).Select(p => new { Raw = p.Escaped, Escaped = p.Escaped })).SelectMany(newPath =>
        //        authorityComponents.SelectMany(authority =>
        //            ((authorityComponents.Length > 0) ? pathComponents.Select(p =>
        //                (p.Escaped.Length == 0 || p.Escaped.StartsWith('/')) ? p.Escaped : $"/{p.Escaped}") : pathComponents.Select(p => p.Escaped)
        //            ).SelectMany(originalPath =>
        //                queryComponents.SelectMany(oldQuery => queryComponents.SelectMany(newQuery => fragmentComponents.SelectMany(oldFragment => fragmentComponents.Select(newFragment =>
        //                    new TestCaseData(
        //                        new Uri($"{authority}{originalPath}{oldQuery}{oldFragment}", (authority.Length > 0) ? UriKind.Absolute : UriKind.Relative),
        //                        $"{newPath.Raw}{newQuery}{newFragment}"
        //                    ).Returns(new Tuple<bool, Uri>(true, new Uri(
        //                        (newPath.Escaped.Length == 0 || newPath.Escaped.StartsWith('/')) ?
        //                            ((newFragment.Length > 0 || newQuery.Length > 0) ?
        //                                $"{authority}{newPath.Escaped}{newQuery}{newFragment}" :
        //                                $"{authority}{newPath.Escaped}{oldQuery}{oldFragment}") :
        //                            ((newFragment.Length > 0 || newQuery.Length > 0) ?
        //                                $"{authority}/{newPath.Escaped}{newQuery}{newFragment}" :
        //                                $"{authority}/{newPath.Escaped}{oldQuery}{oldFragment}"),
        //                        (authority.Length > 0) ? UriKind.Absolute : UriKind.Relative
        //                    )))
        //                ))))
        //            )
        //        ).Concat(queryComponents.SelectMany(q => fragmentComponents.Select(f =>
        //            new TestCaseData(null, $"{newPath.Raw}").Returns(new Tuple<bool, Uri>(false, null))
        //        )))
        //    ).Concat(authorityComponents.SelectMany(a => pathComponents.SelectMany(p => queryComponents.SelectMany(q => fragmentComponents.Select(f =>
        //        new TestCaseData(new Uri(
        //            (p.Escaped.Length == 0 || p.Escaped.StartsWith("/")) ? $"{a}{p.Escaped}{q}{f}" : $"{a}/{p.Escaped}{q}{f}",
        //            (a.Length > 0) ? UriKind.Absolute : UriKind.Relative
        //        ), null).Returns(new Tuple<bool, Uri>(true, new Uri($"{a}{q}{f}", (a.Length > 0) ? UriKind.Absolute : UriKind.Relative)))
        //    ))))).Select(t => t.SetArgDisplayNames("uri", "path"));
        //}

        //[Test, Property("Priority", 2)]
        //[TestCaseSource(nameof(GetTrySetPathComponentTestCases))]
        //public static Tuple<bool, Uri> TrySetPathComponentTest(Uri uri, string path)
        //{
        //    bool returnValue = uri.TrySetPathComponent(path, out Uri result);
        //    return new Tuple<bool, Uri>(returnValue, result);
        //}

        public static IEnumerable<TestCaseData> GetTrySetTrailingEmptyPathSegmentTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/", UriKind.Relative)));

            yield return new TestCaseData(new Uri("/", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/", UriKind.Relative)));

            yield return new TestCaseData(new Uri("\\", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test/", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test/", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test/", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test\\", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test/", UriKind.Relative)));

            yield return new TestCaseData(new Uri("#", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/#", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test#", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test/#", UriKind.Relative)));

            yield return new TestCaseData(new Uri("/Test#", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/Test/#", UriKind.Relative)));

            yield return new TestCaseData(new Uri("?", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/?", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test?", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test/?", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test/?", UriKind.Relative), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test/?", UriKind.Relative)));

            yield return new TestCaseData(new Uri("file:///", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file:///", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/MyShare", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/MyShare/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/MyShare/", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/MyShare/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/MyShare/myfile.txt", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/MyShare/myfile.txt/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/MyShare/myfile.txt/", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/MyShare/myfile.txt/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://tempuri.org/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org/", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://tempuri.org/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org/home.htm", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://tempuri.org/home.htm/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org/home.htm/", UriKind.Absolute), true)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://tempuri.org/home.htm/", UriKind.Absolute)));



            yield return new TestCaseData(new Uri("", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("", UriKind.Relative)));

            yield return new TestCaseData(new Uri("/", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("", UriKind.Relative)));

            yield return new TestCaseData(new Uri("\\", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test/", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test\\", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test", UriKind.Relative)));

            yield return new TestCaseData(new Uri("#", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("#", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test#", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test#", UriKind.Relative)));

            yield return new TestCaseData(new Uri("/Test#", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("/Test#", UriKind.Relative)));

            yield return new TestCaseData(new Uri("?", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("?", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test?", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test?", UriKind.Relative)));

            yield return new TestCaseData(new Uri("Test/?", UriKind.Relative), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("Test?", UriKind.Relative)));

            yield return new TestCaseData(new Uri("file:///", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(false, new Uri("file:///", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(false, new Uri("file://MySite/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/MyShare", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/MyShare", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("file://MySite/MyShare/", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://MySite/MyShare", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(false, new Uri("http://tempuri.org/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org/", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(false, new Uri("http://tempuri.org/", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org/home.htm", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://tempuri.org/home.htm", UriKind.Absolute)));

            yield return new TestCaseData(new Uri("http://tempuri.org/home.htm/", UriKind.Absolute), false)
                .SetDescription("uri: \"\", shouldHaveTrailingSlash: true")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://tempuri.org/home.htm", UriKind.Absolute)));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource(nameof(GetTrySetTrailingEmptyPathSegmentTestCases))]
        public static Tuple<bool, Uri> TrySetTrailingEmptyPathSegmentTest(Uri uri, bool shouldHaveTrailingSlash)
        {
            bool returnValue = uri.TrySetTrailingEmptyPathSegment(shouldHaveTrailingSlash, out Uri result);
            return new Tuple<bool, Uri>(returnValue, result);
        }

        public static IEnumerable<TestCaseData> GetAsNormalizedTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative))
                .SetDescription("AsNormalizedTest: Empty Uri")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri("?", UriKind.Relative))
                .SetDescription("AsNormalizedTest: Relative with empty query")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri("#", UriKind.Relative))
                .SetDescription("AsNormalizedTest: Relative with empty fragment")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri("?#", UriKind.Relative))
                .SetDescription("AsNormalizedTest: Relative with empty query and fragment")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\testazureshare", UriKind.Absolute))
                .SetDescription("AsNormalizedTest: Upper case UNC host file URI")
                .Returns(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return new TestCaseData(new Uri("FILE://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE?#", UriKind.Absolute))
                .SetDescription("AsNormalizedTest: URL totally upper case")
                .Returns(new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            foreach (ValueAndExpectedResult<Uri, Uri> testData in GetFileUriValues())
                yield return new TestCaseData(testData.Value)
                    .SetDescription(testData.Description)
                    .Returns(testData.ExpectedResult);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource(nameof(GetAsNormalizedTestCases))]
        public Uri AsNormalizedTest(Uri uri) => uri.AsNormalized();

        public class ValueAndDescription<T>
        {
            public ValueAndDescription(T value, string description)
            {
                Value = value;
                Description = description;
            }

            public T Value { get; }
            public string Description { get; }
        }

        public class ValueAndExpectedResult<TValue, TResult> : ValueAndDescription<TValue>
        {
            public ValueAndExpectedResult(TValue value, TResult expectedResult, string description) : base(value, description)
            {
                ExpectedResult = expectedResult;
            }

            public TResult ExpectedResult { get; }
        }

        public class SerialNumberValueAndDescription<T> : ValueAndDescription<T>
        {
            public SerialNumberValueAndDescription(uint serialNumber, byte? ordinal, T value, string description)
                : base(value, description)
            {
                SerialNumber = serialNumber;
                Ordinal = ordinal;
            }

            public uint SerialNumber { get; }
            public byte? Ordinal { get; }
        }

        public class UuidValueAndDescription<T> : ValueAndDescription<T>
        {
            public UuidValueAndDescription(Guid uuid, T value, string description)
                : base(value, description)
            {
                UUID = uuid;
            }

            public Guid UUID { get; }
        }

        public class SerialNumberUrlAndDescription : SerialNumberValueAndDescription<Uri>
        {
            public SerialNumberUrlAndDescription(uint serialNumber, byte? ordinal, Uri value, string description) : base(serialNumber, ordinal, value, description)
            {
            }

            public static SerialNumberUrlAndDescription Create(ValueAndDescription<uint> serialNumber, ValueAndDescription<byte> ordinal)
            {
                if (serialNumber is null)
                    throw new ArgumentNullException(nameof(serialNumber));
                if (ordinal is null)
                {
                    string id = $"{(serialNumber.Value << 8).ToString("x4")}-{(serialNumber.Value & 0xffff).ToString("x4")}";
                    return new SerialNumberUrlAndDescription(serialNumber.Value, null,
                        new Uri($"urn:volume:id:{id}", UriKind.Absolute),
                        $"serialNumber = {id} ({serialNumber.Description})");
                }
                string snId = serialNumber.Value.ToString("x8");
                string ordId = ordinal.Value.ToString("x2");
                    return new SerialNumberUrlAndDescription(serialNumber.Value, ordinal.Value,
                        new Uri($"urn:volume:id:{snId}-{ordId}", UriKind.Absolute),
                        $"serialNumber = {snId} ({serialNumber.Description}), ordinal = {ordId} ({ordinal.Description})");
            }
        }

        public static IEnumerable<ValueAndDescription<Guid>> GetUuidValues()
        {
            yield return new ValueAndDescription<Guid>(new Guid("39adc116-682d-11eb-ae93-0242ac130002"), "Version 1 UUID");
            yield return new ValueAndDescription<Guid>(new Guid("77e419fa-2146-35f0-92a3-721b6b2536a9"), "Version 3 UUID");
            yield return new ValueAndDescription<Guid>(new Guid("552841f8-3407-4608-b1d5-c129039539e2"), "Version 4 UUID");
            yield return new ValueAndDescription<Guid>(Guid.Empty, "Emtpy UUID");
        }

        public static IEnumerable<ValueAndDescription<uint>> GetSerialNumberValues()
        {
            yield return new ValueAndDescription<uint>(0U, "Zero value");
            yield return new ValueAndDescription<uint>((uint)int.MaxValue, "Bit-wise equivalent of Int32.MaxValue");
            yield return new ValueAndDescription<uint>(uint.MaxValue, "Bit-wise equivalent of -1");
            yield return new ValueAndDescription<uint>(0xe5b42303U, "Random value > Int32.MaxValue");
        }

        public static IEnumerable<ValueAndDescription<byte>> GetOrdinalValues()
        {
            yield return new ValueAndDescription<byte>(0x00, "Zero value");
            yield return new ValueAndDescription<byte>(0x07, "Value 7");
            yield return new ValueAndDescription<byte>(0xA0, "Value 10");
            yield return new ValueAndDescription<byte>(0xFF, "Max value");
        }

        public static IEnumerable<ValueAndExpectedResult<Uri, Uri>> GetFileUriValues()
        {
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri(@"\\servicenowdiag479.file.core.windows.net\testazureshare", UriKind.Absolute),
                new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute),
                "From UNC path");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri(@"C:\Users\lerwi\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Administrative Tools", UriKind.Absolute),
                new Uri(@"file:///C:/Users/lerwi/AppData/Roaming/Microsoft/Windows/Start%20Menu/Programs/Administrative%20Tools", UriKind.Absolute),
                "From program files folder");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri("file:///my/folder", UriKind.Absolute),
                new Uri("file:///my/folder", UriKind.Absolute),
                "Local without explicit host name");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri("file://192.168.1.1/my/folder", UriKind.Absolute),
                new Uri("file://192.168.1.1/my/folder", UriKind.Absolute),
                "IPV2 address");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri("file://[fe80::1dee:91b0:4872:1f9]/my/folder", UriKind.Absolute),
                new Uri("file://[fe80::1dee:91b0:4872:1f9]/my/folder", UriKind.Absolute),
                "IPV6 address");
        }

        public static IEnumerable<ValueAndDescription<Uri>> GetHttpUriValues()
        {
            yield return new ValueAndDescription<Uri>(new Uri("https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/complex-data-model?view=aspnetcore-5.0&tabs=visual-studio#many-to-many-relationships", UriKind.Absolute),
                "Path, query and fragment");
            yield return new ValueAndDescription<Uri>(new Uri("http://tempuri.org", UriKind.Absolute), "Host only");
            yield return new ValueAndDescription<Uri>(new Uri("http://user:pw@erwinefamily.net/feed/v2", UriKind.Absolute), "Username and PW");
            yield return new ValueAndDescription<Uri>(new Uri("file://test@myhost.com", UriKind.Absolute), "Username only");
        }

        public static IEnumerable<ValueAndDescription<Uri>> GetRelativeUriValues()
        {
            yield return new ValueAndDescription<Uri>(new Uri("", UriKind.Absolute), "Empty");
            yield return new ValueAndDescription<Uri>(new Uri("/", UriKind.Absolute), "Root");
            yield return new ValueAndDescription<Uri>(new Uri("MyFolder/", UriKind.Absolute), "No root");
            yield return new ValueAndDescription<Uri>(new Uri("/one/two/three?key=X&value=7#mark", UriKind.Absolute), "Multi-segment path with query and fragment");
        }

        public static IEnumerable<SerialNumberValueAndDescription<Uri>> GetVolumeSerialNumberUriValues() => GetSerialNumberValues().Select(sn =>
        {
            string id = $"{(sn.Value << 8).ToString("x4")}-{(sn.Value & 0xffff).ToString("x4")}";
            return new SerialNumberValueAndDescription<Uri>(sn.Value, null, new Uri($"urn:volume:id:{id}", UriKind.Absolute), sn.Description);
        });

        public static IEnumerable<SerialNumberUrlAndDescription> GetVolumeSerialNumberAndOrdinalUriValues() => GetSerialNumberValues().SelectMany(sn =>
            GetOrdinalValues().Select(o => SerialNumberUrlAndDescription.Create(sn, o))
        );

        public static IEnumerable<UuidValueAndDescription<Uri>> GetVolumeUUIDUriValues() => GetUuidValues().Select(uuid =>
        {
            return new UuidValueAndDescription<Uri>(uuid.Value, new Uri($"urn:uuid:{uuid.Value.ToString("d").ToLower()}", UriKind.Absolute), uuid.Description);
        });
    }
}
