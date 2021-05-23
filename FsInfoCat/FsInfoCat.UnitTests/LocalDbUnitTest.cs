using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            _testContext = testContext;
            if (Services.ServiceProvider is null)
            {
                string path = Services.GetAppDataPath(typeof(LocalDbUnitTest).Assembly, AppDataPathLevel.Application);
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Services.Initialize(services => Local.LocalDbContext.ConfigureServices(services, typeof(LocalDbUnitTest).Assembly, null));
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            string path = Services.GetAppDataPath(typeof(LocalDbUnitTest).Assembly, AppDataPathLevel.Application);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        [TestMethod]
        public void SymbolicNameTestMethod()
        {
            using (var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>())
            {
                Local.SymbolicName item = new Local.SymbolicName
                {
                    Name = "Test",
                    CreatedOn = DateTime.Now.AddDays(1),
                    ModifiedOn = DateTime.Now
                };
                Local.FileSystem fileSystem = new Local.FileSystem
                {
                    DisplayName = "Again"
                };
                item.FileSystem = fileSystem;
                Assert.IsTrue(item.IsNew());
                Assert.AreEqual(Guid.Empty, item.Id);
                dbContext.SymbolicNames.Add(item);
                dbContext.FileSystems.Add(fileSystem);
                dbContext.SaveChanges();
                Assert.IsFalse(item.IsNew());
                Assert.AreNotEqual(Guid.Empty, item.Id);
            }
        }
    }
}
