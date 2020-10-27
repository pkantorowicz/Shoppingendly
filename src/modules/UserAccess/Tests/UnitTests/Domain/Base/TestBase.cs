using System;
using System.Collections.Generic;
using System.Linq;
using Conteque.Liberey.Domain.BusinessRules;
using Conteque.Liberey.Domain.Core.Aggregates;
using Conteque.Liberey.Domain.Core.Entities;
using Conteque.Liberey.Domain.Events.DomainEvents;
using Conteque.Liberey.Domain.Exceptions;
using FluentAssertions;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base
{
    public abstract class TestBase
    {
        public static T AssertPublishedDomainEvent<T>(Entity aggregate)
            where T : IDomainEvent
        {
            var domainEvent = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().SingleOrDefault();

            if (domainEvent == null)
            {
                throw new Exception($"{typeof(T).Name} event not published");
            }

            return domainEvent;
        }

        public static T AssertPublishedDomainEvent<T>(AggregateRoot aggregate)
            where T : IDomainEvent
        {
            var domainEvent = aggregate.GetDomainEvents().OfType<T>().SingleOrDefault();

            if (domainEvent == null)
            {
                throw new Exception($"{typeof(T).Name} event not published");
            }

            return domainEvent;
        }

        public static void AssertDomainEventNotPublished<T>(AggregateRoot aggregate)
            where T : IDomainEvent
        {
            var domainEvent = aggregate.GetDomainEvents().OfType<T>().SingleOrDefault();
            domainEvent.Should().BeNull();
        }

        public static List<T> AssertPublishedDomainEvents<T>(Entity aggregate)
            where T : IDomainEvent
        {
            var domainEvents = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().ToList();

            if (!domainEvents.Any())
            {
                throw new Exception($"{typeof(T).Name} event not published");
            }

            return domainEvents;
        }

        public static List<T> AssertPublishedDomainEvents<T>(AggregateRoot aggregate)
            where T : IDomainEvent
        {
            var domainEvents = aggregate.GetDomainEvents().OfType<T>().ToList();

            if (!domainEvents.Any())
            {
                throw new Exception($"{typeof(T).Name} event was not published");
            }

            return domainEvents;
        }

        protected static void AssertBrokenRule<TRule>(Action action)
            where TRule : class, IBusinessRule
        {
            action
                .Should()
                .Throw<BusinessRuleValidationException>()
                .Which.BrokenRule.Should().BeOfType<TRule>();
        }
    }
}