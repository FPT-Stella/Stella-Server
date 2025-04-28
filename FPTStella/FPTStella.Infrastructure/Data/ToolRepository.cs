using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class ToolRepository : Repository<Tools>, IToolRepository
    {
        private static readonly FilterDefinition<Tools> NotDeletedFilter =
            Builders<Tools>.Filter.Eq(s => s.DelFlg, false);
        public ToolRepository(IMongoDatabase database) : base(database, "Tools")
        {
            CreateIndexes();
        }
        private void CreateIndexes()
        {
            // Create unique index on tool name
            _collection.Indexes.CreateOne(new CreateIndexModel<Tools>(
                Builders<Tools>.IndexKeys.Ascending(t => t.ToolName),
                new CreateIndexOptions { Unique = true, Background = true }));

            // Create text index for searching on tool name and description
            _collection.Indexes.CreateOne(new CreateIndexModel<Tools>(
                Builders<Tools>.IndexKeys.Text(t => t.ToolName).Text(t => t.Description),
                new CreateIndexOptions { Background = true }));
        }
        /// <summary>
        /// Gets a tool by its name
        /// </summary>
        public async Task<Tools?> GetByToolNameAsync(string toolName)
        {
            var filter = Builders<Tools>.Filter.Eq(t => t.ToolName, toolName) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if a tool with the specified name already exists
        /// </summary>
        public async Task<bool> IsToolNameExistedAsync(string toolName)
        {
            var filter = Builders<Tools>.Filter.Eq(t => t.ToolName, toolName) & NotDeletedFilter;
            return await _collection.CountDocumentsAsync(filter) > 0;
        }
        /// <summary>
        /// Searches for tools with pagination and filtering by name or description
        /// </summary>
        public async Task<PagedResult<Tools>> SearchToolsAsync(
            string? searchTerm = null,
            PaginationParams? paginationParams = null)
        {
            // Use provided pagination params or create default
            var pagingParams = paginationParams ?? new PaginationParams();

            // Start with just the not deleted filter
            var filter = NotDeletedFilter;

            // Add search term filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchFilter = Builders<Tools>.Filter.Or(
                    Builders<Tools>.Filter.Regex(t => t.ToolName,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Tools>.Filter.Regex(t => t.Description,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")));

                filter = filter & searchFilter;
            }

            // Get total count for pagination
            var totalCount = await _collection.CountDocumentsAsync(filter);

            // Get paginated results
            var items = await _collection.Find(filter)
                .Sort(Builders<Tools>.Sort.Ascending(t => t.ToolName))
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Limit(pagingParams.PageSize)
                .ToListAsync();

            // Create and return paged result
            return new PagedResult<Tools>
            {
                CurrentPage = pagingParams.PageNumber,
                PageSize = pagingParams.PageSize,
                TotalCount = (int)totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagingParams.PageSize),
                Items = items
            };
        }
        /// <summary>
        /// Gets all tools with optional filtering by tool name
        /// </summary>
        public async Task<List<Tools>> GetToolsByNameContainingAsync(string? toolName = null)
        {
            var filter = NotDeletedFilter;

            // Add name filter if provided
            if (!string.IsNullOrWhiteSpace(toolName))
            {
                filter = filter & Builders<Tools>.Filter.Regex(t => t.ToolName,
                    new MongoDB.Bson.BsonRegularExpression(toolName, "i"));
            }

            return await _collection.Find(filter)
                .Sort(Builders<Tools>.Sort.Ascending(t => t.ToolName))
                .ToListAsync();
        }
    }
}
