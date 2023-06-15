using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DDHelpers.EntityFramework.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Implementa relação ManyToMany entre duas tabelas especificando o nome da tabela alvo.
        /// </summary>
        /// <typeparam name="T1">O objeto a ser utilizado para configuração.</typeparam>
        /// <typeparam name="TRelatedEntity">O objeto a ser utilizado para a relação.</typeparam>
        /// <param name="modelBuilder">A instância corrente do ModelBuilder.</param>
        /// <param name="toTableName">O nome da tabela relacional alvo.</param>
        /// <param name="hasManyNavigationExpression">A propriedade relacional do objeto de configuração.</param>
        /// <param name="withManyNavigationExpression">A propriedade relacional do objeto de relação.</param>
        public static void SetManyToMany<T1, TRelatedEntity>(this ModelBuilder modelBuilder,
            string toTableName,
            Expression<Func<T1, IEnumerable<TRelatedEntity>?>>? hasManyNavigationExpression,
            Expression<Func<TRelatedEntity, IEnumerable<T1>?>> withManyNavigationExpression)
            where T1 : class where TRelatedEntity : class
        {
            modelBuilder.Entity<T1>()
                .HasMany(navigationExpression: hasManyNavigationExpression)
                .WithMany(withManyNavigationExpression)
                .UsingEntity(join => join.ToTable(toTableName));
        }
    }
}