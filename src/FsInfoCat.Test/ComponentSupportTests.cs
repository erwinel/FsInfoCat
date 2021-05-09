using FsInfoCat.Desktop.Model.ComponentSupport;
using FsInfoCat.Desktop.Model.Validation;
using FsInfoCat.Test.ComponentSupport;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace FsInfoCat.Test
{
    public class ComponentSupportTests
    {
        private readonly ILogger<ComponentSupportTests> _logger = TestHelper.LoggerFactory.CreateLogger<ComponentSupportTests>();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EqualityComparerAssumptionsTest()
        {
            TestComponent testComponentX = new TestComponent(1, 2);
            IEqualityComparer testComponentComparer = EqualityComparer<TestComponent>.Default;
            bool actual = testComponentComparer.Equals(testComponentX, testComponentX);
            Assert.That(actual, Is.True);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(0));

            testComponentX = new TestComponent(1, 2);
            TestComponent testComponentY = new TestComponent(1, 2);
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.False);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(1));

            testComponentX = new TestGenericEquatableComponent(1, 2);
            testComponentY = new TestGenericEquatableComponent(1, 2);
            testComponentComparer = EqualityComparer<TestGenericEquatableComponent>.Default;
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.True);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(1));

            testComponentX = new TestBaseEquatableComponent(1, 2);
            testComponentY = new TestBaseEquatableComponent(1, 2);
            testComponentComparer = EqualityComparer<TestBaseEquatableComponent>.Default;
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.False);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(1));
            Assert.That(testComponentX.GetTypedEqualsInvocationCount(), Is.EqualTo(0));

            testComponentX = new TestEquatableComponent(1, 2);
            testComponentY = new TestEquatableComponent(1, 2);
            testComponentComparer = EqualityComparer<TestEquatableComponent>.Default;
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.True);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(0));
            Assert.That(testComponentX.GetTypedEqualsInvocationCount(), Is.EqualTo(1));

            testComponentX = new TestGenericComparableComponent(1, 2);
            testComponentY = new TestGenericComparableComponent(1, 2);
            testComponentComparer = EqualityComparer<TestGenericComparableComponent>.Default;
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.False);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(1));
            Assert.That(testComponentX.GetComparisonInvocationCount(), Is.EqualTo(0));

            testComponentX = new TestBaseComparableComponent(1, 2);
            testComponentY = new TestBaseComparableComponent(1, 2);
            testComponentComparer = EqualityComparer<TestBaseComparableComponent>.Default;
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.False);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(1));
            Assert.That(testComponentX.GetComparisonInvocationCount(), Is.EqualTo(0));

            testComponentX = new TestComparableComponent(1, 2);
            testComponentY = new TestComparableComponent(1, 2);
            testComponentComparer = EqualityComparer<TestComparableComponent>.Default;
            actual = testComponentComparer.Equals(testComponentX, testComponentY);
            Assert.That(actual, Is.False);
            Assert.That(testComponentX.GetGenericEqualsInvocationCount(), Is.EqualTo(1));
            Assert.That(testComponentX.GetComparisonInvocationCount(), Is.EqualTo(0));
        }

        [Test]
        public void ModelDescriptorTest()
        {
            Type type = typeof(TestComponent);
            ModelDescriptorBuilder<TestComponent> builder1 = new ModelDescriptorBuilder<TestComponent>();
            ModelDescriptor<TestComponent> modelDescriptor1 = builder1.Build();
            Assert.That(modelDescriptor1, Is.Not.Null);
            Assert.That(modelDescriptor1.SimpleName, Is.EqualTo(type.Name));
            Assert.That(modelDescriptor1.FullName, Is.EqualTo(type.FullName));
            Assert.That(modelDescriptor1.Properties, Is.Not.Null);
            Assert.That(modelDescriptor1.Properties.Count, Is.EqualTo(3));

            IModelPropertyDescriptor<TestComponent> pd1 = modelDescriptor1[nameof(TestComponent.Denominator)];
            Assert.That(pd1, Is.Not.Null);
            Assert.That(pd1.IsReadOnly, Is.True);
            Assert.That(pd1.Category, Is.EqualTo(System.ComponentModel.CategoryAttribute.Default.Category));
            Assert.That(pd1.AreStandardValuesExclusive, Is.False);
            Assert.That(pd1.AreStandardValuesSupported, Is.False);
            Assert.That(pd1.Description, Is.EqualTo(TestComponent.DESCRIPTION_DENOMINATOR));
            Assert.That(pd1.DisplayName, Is.EqualTo(nameof(TestComponent.Denominator)));
            Assert.That(pd1.Name, Is.EqualTo(nameof(TestComponent.Denominator)));
            Assert.That(pd1.Owner, Is.SameAs(modelDescriptor1));
            Type expectedType = typeof(int);
            Assert.That(pd1.PropertyType, Is.EqualTo(expectedType));
            Assert.That(pd1.SupportsChangeEvents, Is.False);
            object actual = pd1.ConvertFromInvariantString("6");
            Assert.That(actual, Is.InstanceOf(expectedType));
            Assert.That(actual, Is.EqualTo(6));

            pd1 = modelDescriptor1[nameof(TestComponent.Numerator)];
            Assert.That(pd1, Is.Not.Null);
            Assert.That(pd1.IsReadOnly, Is.False);
            Assert.That(pd1.Category, Is.EqualTo(System.ComponentModel.CategoryAttribute.Behavior.Category));
            Assert.That(pd1.AreStandardValuesExclusive, Is.False);
            Assert.That(pd1.AreStandardValuesSupported, Is.False);
            Assert.That(pd1.Description, Is.Empty);
            Assert.That(pd1.DisplayName, Is.EqualTo(nameof(TestComponent.Numerator)));
            Assert.That(pd1.Name, Is.EqualTo(nameof(TestComponent.Numerator)));
            Assert.That(pd1.Owner, Is.SameAs(modelDescriptor1));
            expectedType = typeof(int);
            Assert.That(pd1.PropertyType, Is.EqualTo(expectedType));
            Assert.That(pd1.SupportsChangeEvents, Is.False);
            actual = pd1.ConvertFromInvariantString("12");
            Assert.That(actual, Is.InstanceOf(expectedType));
            Assert.That(actual, Is.EqualTo(12));

            pd1 = modelDescriptor1[nameof(TestComponent.Value)];
            Assert.That(pd1, Is.Not.Null);
            Assert.That(pd1.IsReadOnly, Is.True);
            Assert.That(pd1.Category, Is.EqualTo(System.ComponentModel.CategoryAttribute.Default.Category));
            Assert.That(pd1.AreStandardValuesExclusive, Is.False);
            Assert.That(pd1.AreStandardValuesSupported, Is.False);
            Assert.That(pd1.Description, Is.Empty);
            Assert.That(pd1.DisplayName, Is.EqualTo(TestComponent.DISPLAY_NAME_VALUE));
            Assert.That(pd1.Name, Is.EqualTo(nameof(TestComponent.Value)));
            Assert.That(pd1.Owner, Is.SameAs(modelDescriptor1));
            expectedType = typeof(double);
            Assert.That(pd1.PropertyType, Is.EqualTo(expectedType));
            Assert.That(pd1.SupportsChangeEvents, Is.False);
            actual = pd1.ConvertFromInvariantString("17");
            Assert.That(actual, Is.InstanceOf(expectedType));
            Assert.That(actual, Is.EqualTo(17.0));
        }

        [Test]
        public void DbConnectionStringModelDescriptorBuilderTest()
        {
            Type type = typeof(SqlConnectionStringBuilder);
            DbConnectionStringModelDescriptorBuilder<SqlConnectionStringBuilder> builder = new DbConnectionStringModelDescriptorBuilder<SqlConnectionStringBuilder>();
            ModelDescriptor<SqlConnectionStringBuilder> modelDescriptor = builder.Build();
            Assert.That(modelDescriptor, Is.Not.Null);
            Assert.That(modelDescriptor.SimpleName, Is.EqualTo(type.Name));
            Assert.That(modelDescriptor.FullName, Is.EqualTo(type.FullName));
            Assert.That(modelDescriptor.Properties, Is.Not.Null);
            Assert.That(modelDescriptor.Properties.Count, Is.EqualTo(39));
            IModelPropertyDescriptor<SqlConnectionStringBuilder> modelPropertyDescriptor = modelDescriptor[nameof(SqlConnectionStringBuilder.ConnectionString)];
            Assert.That(modelPropertyDescriptor, Is.Not.Null);
            Assert.That(modelPropertyDescriptor.ValidationAttributes.OfType<RequiredAttribute>().Any(), Is.True);
        }

        [Test]
        public void ValidationAttributeListTest()
        {
            ValidationAttributeList target = new ValidationAttributeList();
            RequiredAttribute item0 = new RequiredAttribute { AllowEmptyStrings = true };
            target.Add(item0);
            Assert.That(target.Count, Is.EqualTo(1));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(((RequiredAttribute)target[0]).AllowEmptyStrings, Is.True);

            item0 = new RequiredAttribute { AllowEmptyStrings = false };
            target.Add(item0);
            Assert.That(target.Count, Is.EqualTo(1));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(((RequiredAttribute)target[0]).AllowEmptyStrings, Is.False);

            StringLengthAttribute item1 = new StringLengthAttribute(100);
            target.Add(item1);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(((RequiredAttribute)target[0]).AllowEmptyStrings, Is.False);
            Assert.That(((StringLengthAttribute)target[1]).MaximumLength, Is.EqualTo(100));

            target.Add(item0);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[1], Is.SameAs(item0));
            Assert.That(target[0], Is.SameAs(item1));
            Assert.That(((RequiredAttribute)target[1]).AllowEmptyStrings, Is.False);
            Assert.That(((StringLengthAttribute)target[0]).MaximumLength, Is.EqualTo(100));

            CustomValidationAttribute item2 = new CustomValidationAttribute(typeof(TestValidator), nameof(TestValidator.IsValidDenominator));

            target.Add(item2);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[1], Is.SameAs(item0));
            Assert.That(target[0], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));
            Assert.That(((RequiredAttribute)target[1]).AllowEmptyStrings, Is.False);
            Assert.That(((StringLengthAttribute)target[0]).MaximumLength, Is.EqualTo(100));
            Assert.That(((CustomValidationAttribute)target[2]).Method, Is.EqualTo(nameof(TestValidator.IsValidDenominator)));
            Assert.That(((CustomValidationAttribute)target[2]).ValidatorType, Is.EqualTo(typeof(TestValidator)));

            CustomValidationAttribute item3 = new CustomValidationAttribute(typeof(TestValidator), nameof(TestValidator.IsValidNumerator));

            target.Add(item3);
            Assert.That(target.Count, Is.EqualTo(4));
            Assert.That(target[1], Is.SameAs(item0));
            Assert.That(target[0], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));
            Assert.That(target[3], Is.SameAs(item3));
            Assert.That(((RequiredAttribute)target[1]).AllowEmptyStrings, Is.False);
            Assert.That(((StringLengthAttribute)target[0]).MaximumLength, Is.EqualTo(100));
            Assert.That(((CustomValidationAttribute)target[2]).Method, Is.EqualTo(nameof(TestValidator.IsValidDenominator)));
            Assert.That(((CustomValidationAttribute)target[2]).ValidatorType, Is.EqualTo(typeof(TestValidator)));
            Assert.That(((CustomValidationAttribute)target[3]).Method, Is.EqualTo(nameof(TestValidator.IsValidNumerator)));
            Assert.That(((CustomValidationAttribute)target[3]).ValidatorType, Is.EqualTo(typeof(TestValidator)));
        }
    }
}
