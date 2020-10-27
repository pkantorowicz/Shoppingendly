using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conteque.Liberey.Domain.Core.Entities;
using Conteque.Liberey.Domain.Events.DomainEvents;

namespace Conteque.Shoppingendly.Modules.UserAccess.UnitTests.Domain.Base
{
    public static class DomainEventsTestHelper
    {
        public static IEnumerable<IDomainEvent> GetAllDomainEvents(Entity aggregate)
        {
            var domainEvents = new List<IDomainEvent>();

            if (aggregate.GetDomainEvents() != null)
            {
                domainEvents.AddRange(aggregate.GetDomainEvents());
            }

            var fields = aggregate.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Concat(aggregate.GetType().BaseType
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)).ToArray();

            foreach (var field in fields)
            {
                var isEntity = typeof(Entity).IsAssignableFrom(field.FieldType);

                if (isEntity)
                {
                    var entity = field.GetValue(aggregate) as Entity;
                    domainEvents.AddRange(GetAllDomainEvents(entity).ToList());
                }

                if (field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                {
                    if (field.GetValue(aggregate) is IEnumerable enumerable)
                    {
                        foreach (var en in enumerable)
                        {
                            if (en is Entity entityItem)
                            {
                                domainEvents.AddRange(GetAllDomainEvents(entityItem));
                            }
                        }
                    }
                }
            }

            return domainEvents;
        }
    }
}