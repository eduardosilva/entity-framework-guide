using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework_guide.Core.Infrastructure.DataAccess
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Get registro from cache or attach
        /// </summary>
        /// <typeparam name="T">T used in DbSet</typeparam>
        /// <param name="collection">DbSet</param>
        /// <param name="searchLocalQuery">Criteria to search record in DbSet</param>
        /// <param name="getAttachItem">Function to attach new data in DbSet</param>
        /// <example>
        /// var country = Context.Countries.GetLocalOrAttach(c => c.Id == CountryId, () => new Country { Id = CountryId });
        /// </example>
        /// <remarks>
        /// See: https://gist.github.com/eduardosilva/58d1f672335a6788b9cbb2c2f4e747d3
        /// </remarks>
        /// <returns>
        /// Return data from cache or new data attached
        /// </returns>
        public static T GetLocalOrAttach<T>(this DbSet<T> collection, Func<T, bool> searchLocalQuery, Func<T> getAttachItem) where T : class
        {
            T localEntity = collection.Local.FirstOrDefault(searchLocalQuery);

            if (localEntity == null)
            {
                localEntity = getAttachItem();
                collection.Attach(localEntity);
            }

            return localEntity;
        }
    }
}
