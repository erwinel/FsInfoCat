using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalRecordedTVPropertySetUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            dbContext.RejectChanges();
        }

        [TestMethod("RecordedTVPropertySet Add/Remove Tests")]
        [Ignore]
        public void RecordedTVPropertySetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.RecordedTVPropertySet target = new();
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();
            // DEFERRED: Validate default values
            Assert.IsNull(target.ChannelNumber);
            Assert.IsNull(target.EpisodeName);
            Assert.IsNull(target.IsDTVContent);
            Assert.IsNull(target.IsHDContent);
            Assert.IsNull(target.NetworkAffiliation);
            Assert.IsNull(target.OriginalBroadcastDate);
            Assert.IsNull(target.ProgramDescription);
            Assert.IsNull(target.StationCallSign);
            Assert.IsNull(target.StationName);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Guid Id")]
        [Ignore]
        public void RecordedTVPropertySetIdTestMethod()
        {
            Local.RecordedTVPropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("RecordedTVPropertySet ChannelNumber Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.ChannelNumber: BIGINT \"ChannelNumber\" IS NULL OR (\"ChannelNumber\">=0 AND \"ChannelNumber\"<4294967296)")]
        [Ignore]
        public void RecordedTVPropertySetChannelNumberTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { ChannelNumber = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.ChannelNumber), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ChannelNumber);

            expected = default; // DEFERRED: Set valid value
            target.ChannelNumber = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ChannelNumber);

            expected = default; // DEFERRED: Set invalid value
            target.ChannelNumber = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.ChannelNumber), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ChannelNumber);
        }

        [TestMethod("RecordedTVPropertySet EpisodeName Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.EpisodeName: NVARCHAR(1024)")]
        [Ignore]
        public void RecordedTVPropertySetEpisodeNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { EpisodeName = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.EpisodeName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.EpisodeName);

            expected = default; // DEFERRED: Set valid value
            target.EpisodeName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.EpisodeName);

            expected = default; // DEFERRED: Set invalid value
            target.EpisodeName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.EpisodeName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.EpisodeName);
        }

        [TestMethod("RecordedTVPropertySet IsDTVContent Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.IsDTVContent: BIT")]
        [Ignore]
        public void RecordedTVPropertySetIsDTVContentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            bool? expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { IsDTVContent = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.IsDTVContent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.IsDTVContent);

            expected = default; // DEFERRED: Set valid value
            target.IsDTVContent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.IsDTVContent);

            expected = default; // DEFERRED: Set invalid value
            target.IsDTVContent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.IsDTVContent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.IsDTVContent);
        }

        [TestMethod("RecordedTVPropertySet IsHDContent Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.IsHDContent: BIT")]
        [Ignore]
        public void RecordedTVPropertySetIsHDContentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            bool? expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { IsHDContent = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.IsHDContent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.IsHDContent);

            expected = default; // DEFERRED: Set valid value
            target.IsHDContent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.IsHDContent);

            expected = default; // DEFERRED: Set invalid value
            target.IsHDContent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.IsHDContent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.IsHDContent);
        }

        [TestMethod("RecordedTVPropertySet NetworkAffiliation Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.NetworkAffiliation: NVARCHAR(256)")]
        [Ignore]
        public void RecordedTVPropertySetNetworkAffiliationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { NetworkAffiliation = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.NetworkAffiliation), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.NetworkAffiliation);

            expected = default; // DEFERRED: Set valid value
            target.NetworkAffiliation = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.NetworkAffiliation);

            expected = default; // DEFERRED: Set invalid value
            target.NetworkAffiliation = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.NetworkAffiliation), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.NetworkAffiliation);
        }

        [TestMethod("RecordedTVPropertySet OriginalBroadcastDate Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.OriginalBroadcastDate: DATETIME")]
        [Ignore]
        public void RecordedTVPropertySetOriginalBroadcastDateTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            System.DateTime? expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { OriginalBroadcastDate = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.OriginalBroadcastDate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.OriginalBroadcastDate);

            expected = default; // DEFERRED: Set valid value
            target.OriginalBroadcastDate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.OriginalBroadcastDate);

            expected = default; // DEFERRED: Set invalid value
            target.OriginalBroadcastDate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.OriginalBroadcastDate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.OriginalBroadcastDate);
        }

        [TestMethod("RecordedTVPropertySet ProgramDescription Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.ProgramDescription: TEXT")]
        [Ignore]
        public void RecordedTVPropertySetProgramDescriptionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { ProgramDescription = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.ProgramDescription), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ProgramDescription);

            expected = default; // DEFERRED: Set valid value
            target.ProgramDescription = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ProgramDescription);

            expected = default; // DEFERRED: Set invalid value
            target.ProgramDescription = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.ProgramDescription), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ProgramDescription);
        }

        [TestMethod("RecordedTVPropertySet StationCallSign Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.StationCallSign: NVARCHAR(32)")]
        [Ignore]
        public void RecordedTVPropertySetStationCallSignTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { StationCallSign = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.StationCallSign), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.StationCallSign);

            expected = default; // DEFERRED: Set valid value
            target.StationCallSign = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.StationCallSign);

            expected = default; // DEFERRED: Set invalid value
            target.StationCallSign = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.StationCallSign), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.StationCallSign);
        }

        [TestMethod("RecordedTVPropertySet StationName Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "RecordedTVPropertySet.StationName: NVARCHAR(256)")]
        [Ignore]
        public void RecordedTVPropertySetStationNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.RecordedTVPropertySet target = new() { StationName = expected };
            EntityEntry<Local.RecordedTVPropertySet> entityEntry = dbContext.RecordedTVPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.StationName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.StationName);

            expected = default; // DEFERRED: Set valid value
            target.StationName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.StationName);

            expected = default; // DEFERRED: Set invalid value
            target.StationName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RecordedTVPropertySet.StationName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.RecordedTVPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.StationName);
        }
    }
}
