﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration.Functions
{
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    using Moq;
    using Xunit;

    public class ModificationFunctionsConfigurationTests
    {
        [Fact]
        public void Can_clone_configuration()
        {
            var modificationFunctionsConfiguration = new ModificationFunctionsConfiguration();

            var modificationFunctionConfiguration = new ModificationFunctionConfiguration();

            modificationFunctionsConfiguration.InsertFunction(modificationFunctionConfiguration);
            modificationFunctionsConfiguration.UpdateFunction(modificationFunctionConfiguration);
            modificationFunctionsConfiguration.DeleteFunction(modificationFunctionConfiguration);

            var clone = modificationFunctionsConfiguration.Clone();

            Assert.NotSame(modificationFunctionsConfiguration, clone);
            Assert.NotSame(modificationFunctionConfiguration, clone.InsertModificationFunctionConfiguration);
            Assert.NotSame(modificationFunctionConfiguration, clone.UpdateModificationFunctionConfiguration);
            Assert.NotSame(modificationFunctionConfiguration, clone.DeleteModificationFunctionConfiguration);
        }

        [Fact]
        public void Configure_should_call_configure_function_configurations()
        {
            var modificationFunctionsConfiguration = new ModificationFunctionsConfiguration();

            var mockModificationFunctionConfiguration = new Mock<ModificationFunctionConfiguration>();

            modificationFunctionsConfiguration.InsertFunction(mockModificationFunctionConfiguration.Object);
            modificationFunctionsConfiguration.UpdateFunction(mockModificationFunctionConfiguration.Object);
            modificationFunctionsConfiguration.DeleteFunction(mockModificationFunctionConfiguration.Object);

            var entitySet = new EntitySet();
            entitySet.ChangeEntityContainerWithoutCollectionFixup(new EntityContainer());

            var storageModificationFunctionMapping
                = new StorageModificationFunctionMapping(
                    entitySet,
                    new EntityType(),
                    new EdmFunction(),
                    new StorageModificationFunctionParameterBinding[0],
                    null,
                    null);

            modificationFunctionsConfiguration.Configure(
                new StorageEntityTypeModificationFunctionMapping(
                    new EntityType(),
                    storageModificationFunctionMapping,
                    storageModificationFunctionMapping,
                    storageModificationFunctionMapping));

            mockModificationFunctionConfiguration
                .Verify(m => m.Configure(storageModificationFunctionMapping), Times.Exactly(3));
        }

        [Fact]
        public void Configure_association_set_should_call_configure_function_configurations()
        {
            var modificationFunctionsConfiguration = new ModificationFunctionsConfiguration();

            var mockModificationFunctionConfiguration = new Mock<ModificationFunctionConfiguration>();

            modificationFunctionsConfiguration.InsertFunction(mockModificationFunctionConfiguration.Object);
            modificationFunctionsConfiguration.DeleteFunction(mockModificationFunctionConfiguration.Object);

            var entitySet = new EntitySet();
            entitySet.ChangeEntityContainerWithoutCollectionFixup(new EntityContainer());

            var storageModificationFunctionMapping
                = new StorageModificationFunctionMapping(
                    entitySet,
                    new EntityType(),
                    new EdmFunction(),
                    new StorageModificationFunctionParameterBinding[0],
                    null,
                    null);

            modificationFunctionsConfiguration.Configure(
                new StorageAssociationSetModificationFunctionMapping(
                    new AssociationSet("AS", new AssociationType()),
                    storageModificationFunctionMapping,
                    storageModificationFunctionMapping));

            mockModificationFunctionConfiguration
                .Verify(m => m.Configure(storageModificationFunctionMapping), Times.Exactly(2));
        }

        [Fact]
        public void IsCompatible_should_check_compatibility_of_insert_configuration()
        {
            var modificationFunctionsConfiguration1 = new ModificationFunctionsConfiguration();
            var modificationFunctionsConfiguration2 = new ModificationFunctionsConfiguration();

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            var modificationFunctionConfiguration1 = new ModificationFunctionConfiguration();
            var modificationFunctionConfiguration2 = new ModificationFunctionConfiguration();

            modificationFunctionsConfiguration1.InsertFunction(modificationFunctionConfiguration1);

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            modificationFunctionsConfiguration2.InsertFunction(modificationFunctionConfiguration2);

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            modificationFunctionConfiguration1.HasName("Foo");

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            modificationFunctionConfiguration2.HasName("Bar");

            Assert.False(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));
        }

        [Fact]
        public void IsCompatible_should_check_compatibility_of_delete_configuration()
        {
            var modificationFunctionsConfiguration1 = new ModificationFunctionsConfiguration();
            var modificationFunctionsConfiguration2 = new ModificationFunctionsConfiguration();

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            var modificationFunctionConfiguration1 = new ModificationFunctionConfiguration();
            var modificationFunctionConfiguration2 = new ModificationFunctionConfiguration();

            modificationFunctionsConfiguration1.DeleteFunction(modificationFunctionConfiguration1);

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            modificationFunctionsConfiguration2.DeleteFunction(modificationFunctionConfiguration2);

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            modificationFunctionConfiguration1.HasName("Foo");

            Assert.True(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));

            modificationFunctionConfiguration2.HasName("Bar");

            Assert.False(modificationFunctionsConfiguration1.IsCompatibleWith(modificationFunctionsConfiguration2));
        }
    }
}
