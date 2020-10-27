﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conteque.Shoppingendly.Modules.UserAccess.Domain.Users;
using FluentAssertions;
using NetArchTest.Rules;

namespace Conteque.Shoppingendly.Modules.UserAccess.ArchTests.Base
{
    public abstract class TestBase
    {
        protected static Assembly DomainAssembly => typeof(User).Assembly;

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            types.Should().BeNullOrEmpty();
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}