using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Freelance.Core.Models;
using Moq;

namespace Freelance.Tests
{
    public static class Extensions
    {
        public static Mock<DbSet<T>> GetMockSet<T>(this ICollection<T> list) where T : class
        {
            var queryable = list.AsQueryable();
            var mockList = new Mock<DbSet<T>>(MockBehavior.Loose);
            mockList.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new DbAsyncEnumerator<T>(list.AsQueryable().GetEnumerator()));

            mockList.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new DbAsyncQueryProvider<T>(list.AsQueryable().Provider));
            mockList.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockList.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockList.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockList.Setup(m => m.Include(It.IsAny<string>())).Returns(mockList.Object);
            mockList.Setup(m => m.Add(It.IsAny<T>())).Returns((T a) => { list.Add(a); return a; });
            mockList.Setup(m => m.Remove(It.IsAny<T>())).Returns((T a) => { list.Remove(a); return a; });

            return mockList;
        }
    }
}