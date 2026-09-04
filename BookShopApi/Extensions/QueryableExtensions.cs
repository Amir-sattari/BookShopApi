using System.Linq.Expressions;
using BookShopApi.Dtos.Common;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Extensions
{
    // Shared query pipeline for list endpoints. Entity maps (BookQueryableExtensions, later User) stay in this file.
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Expression<Func<T, bool>>? predicate)
        {
            return predicate is null ? query : query.Where(predicate);
        }

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, bool enabled, Expression<Func<T, bool>> predicate)
        {
            return enabled ? query.Where(predicate) : query;
        }

        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? term,
            params Expression<Func<T, string>>[] properties)
        {
            return query.ApplySearch(term, extraPredicates: null, properties);
        }

        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? term,
            IEnumerable<Expression<Func<T, bool>>>? extraPredicates,
            params Expression<Func<T, string>>[] properties)
        {
            if (string.IsNullOrWhiteSpace(term))
                return query;

            var text = term.Trim();
            var parameter = Expression.Parameter(typeof(T), "e");
            var contains = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
            var value = Expression.Constant(text);
            Expression? body = null;

            foreach (var property in properties)
            {
                var member = new ParameterReplacer(property.Parameters[0], parameter).Visit(property.Body)!;
                var call = Expression.Call(member, contains, value);
                body = body is null ? call : Expression.OrElse(body, call);
            }

            if (extraPredicates is not null)
            {
                foreach (var predicate in extraPredicates)
                {
                    var replaced = new ParameterReplacer(predicate.Parameters[0], parameter).Visit(predicate.Body)!;
                    body = body is null ? replaced : Expression.OrElse(body, replaced);
                }
            }

            if (body is null)
                return query;

            return query.Where(Expression.Lambda<Func<T, bool>>(body, parameter));
        }

        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query,
            string? sortKey,
            string? sortDirection,
            IReadOnlyDictionary<string, Func<IQueryable<T>, bool, IQueryable<T>>> selectors,
            Func<IQueryable<T>, IQueryable<T>>? defaultSort = null)
        {
            var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
            var key = sortKey?.Trim().ToLowerInvariant();

            if (!string.IsNullOrEmpty(key) && selectors.TryGetValue(key, out var apply))
                return apply(query, descending);

            return defaultSort?.Invoke(query) ?? query;
        }

        public static IQueryable<T> OrderByDirection<T, TKey>(
            this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector,
            bool descending)
        {
            return descending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        public static IQueryable<T> ThenByDirection<T, TKey>(
            this IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector,
            bool descending = false)
        {
            if (query is not IOrderedQueryable<T> ordered)
                return query.OrderByDirection(keySelector, descending);

            return descending
                ? ordered.ThenByDescending(keySelector)
                : ordered.ThenBy(keySelector);
        }

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PagedQuery paging,
            CancellationToken cancellationToken = default)
        {
            paging.Normalize();

            var totalCount = await query.CountAsync(cancellationToken);

            if (!paging.IsPaged)
            {
                var allItems = await query.ToListAsync(cancellationToken);
                return new PagedResult<T>
                {
                    Items = allItems,
                    TotalCount = totalCount,
                    Page = 1,
                    PageSize = totalCount
                };
            }

            var page = paging.Page!.Value;
            var pageSize = paging.PageSize!.Value;
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private sealed class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _from;
            private readonly ParameterExpression _to;

            public ParameterReplacer(ParameterExpression from, ParameterExpression to)
            {
                _from = from;
                _to = to;
            }

            protected override Expression VisitParameter(ParameterExpression node) =>
                node == _from ? _to : base.VisitParameter(node);
        }
    }

    public static class BookQueryableExtensions
    {
        public static readonly IReadOnlyDictionary<string, Func<IQueryable<Book>, bool, IQueryable<Book>>> SortSelectors =
            new Dictionary<string, Func<IQueryable<Book>, bool, IQueryable<Book>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["title"] = (query, descending) => query.OrderByDirection(b => b.Title, descending).ThenByDirection(b => b.Id),
                ["author"] = (query, descending) => query.OrderByDirection(b => b.Author, descending).ThenByDirection(b => b.Id),
                ["price"] = (query, descending) => query.OrderByDirection(b => b.Price, descending).ThenByDirection(b => b.Id),
                ["quantity"] = (query, descending) => query.OrderByDirection(b => b.Quantity, descending).ThenByDirection(b => b.Id),
                ["createdat"] = (query, descending) => query.OrderByDirection(b => b.CreatedAt, descending).ThenByDirection(b => b.Id),
                ["discountpercentage"] = SortByDiscountPercentage
            };

        public static IQueryable<Book> ApplySearch(this IQueryable<Book> query, string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return query;

            var text = term.Trim();
            Expression<Func<Book, bool>>[] extraPredicates =
            [
                b => b.BookCategories.Any(bc => bc.Category.Name.Contains(text))
            ];

            return QueryableExtensions.ApplySearch(
                query,
                text,
                extraPredicates,
                b => b.Title,
                b => b.Author,
                b => b.Translator);
        }

        public static IQueryable<Book> ApplySort(
            this IQueryable<Book> query,
            string? sortKey,
            string? sortDirection,
            bool applyDefault = false)
        {
            return QueryableExtensions.ApplySort(
                query,
                sortKey,
                sortDirection,
                SortSelectors,
                applyDefault ? q => q.OrderByDescending(b => b.Id) : null);
        }

        private static IQueryable<Book> SortByDiscountPercentage(IQueryable<Book> query, bool descending)
        {
            var now = DateTime.UtcNow;
            return query
                .OrderByDirection(
                    b => b.BookDiscounts
                        .Where(d => d.IsActive && d.Percentage > 0 && (d.StartDate == null || d.StartDate <= now) && (d.EndDate == null || d.EndDate >= now))
                        .OrderByDescending(d => d.Id)
                        .Select(d => d.Percentage)
                        .FirstOrDefault(),
                    descending)
                .ThenByDirection(b => b.Id);
        }
    }
}
