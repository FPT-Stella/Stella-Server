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
    public class SubjectComboRepository : Repository<SubjectCombo>, ISubjectComboRepository
    {
        private static readonly FilterDefinition<SubjectCombo> NotDeletedFilter =
            Builders<SubjectCombo>.Filter.Eq(c => c.DelFlg, false);

        public SubjectComboRepository(IMongoDatabase database) : base(database, "SubjectCombos")
        {
            CreateIndexes();
        }

        /// <summary>
        /// Creates indexes for the SubjectCombos collection
        /// </summary>
        private void CreateIndexes()
        {
            // Create index for program_id (for filtering by program)
            _collection.Indexes.CreateOne(new CreateIndexModel<SubjectCombo>(
                Builders<SubjectCombo>.IndexKeys.Ascending(c => c.ProgramId),
                new CreateIndexOptions { Background = true }));

            // Create compound index for program_id + combo_name (for uniqueness within a program)
            _collection.Indexes.CreateOne(new CreateIndexModel<SubjectCombo>(
                Builders<SubjectCombo>.IndexKeys
                    .Ascending(c => c.ProgramId)
                    .Ascending(c => c.ComboName),
                new CreateIndexOptions { Unique = true, Background = true }));

            // Create text index on text fields for full-text search
            _collection.Indexes.CreateOne(new CreateIndexModel<SubjectCombo>(
                Builders<SubjectCombo>.IndexKeys
                    .Text(c => c.ComboName)
                    .Text(c => c.Description)
                    .Text(c => c.ProgramOutcome),
                new CreateIndexOptions { Background = true }));
        }

        /// <summary>
        /// Gets Subject Combos by Program ID
        /// </summary>
        public async Task<List<SubjectCombo>> GetByProgramIdAsync(Guid programId)
        {
            var filter = Builders<SubjectCombo>.Filter.Eq(c => c.ProgramId, programId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Gets Subject Combo by combo name
        /// </summary>
        public async Task<SubjectCombo?> GetByComboNameAsync(string comboName)
        {
            var filter = Builders<SubjectCombo>.Filter.Eq(c => c.ComboName, comboName) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if a Subject Combo with the specified name exists within a program
        /// </summary>
        public async Task<bool> IsComboNameExistedInProgramAsync(Guid programId, string comboName)
        {
            var filter = Builders<SubjectCombo>.Filter.Eq(c => c.ProgramId, programId) &
                        Builders<SubjectCombo>.Filter.Eq(c => c.ComboName, comboName) &
                        NotDeletedFilter;

            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        /// <summary>
        /// Searches Subject Combos with pagination
        /// </summary>
        public async Task<PagedResult<SubjectCombo>> SearchComboAsync(
            string searchTerm,
            Guid? programId,
            PaginationParams paginationParams)
        {
            // Define searchable fields
            var searchableFields = new[] { "combo_name", "description", "program_outcome" };

            // If program ID is provided, filter by it
            if (programId.HasValue)
            {
                // First create a combined filter with program ID
                var programFilter = Builders<SubjectCombo>.Filter.Eq(c => c.ProgramId, programId.Value) &
                                    NotDeletedFilter;

                // Use the base SearchAsync with initial filter
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    // No search term, just filter by program ID
                    var results = await FilterByAsync(c => c.ProgramId == programId.Value);
                    var totalCount = results.Count();

                    return new PagedResult<SubjectCombo>
                    {
                        CurrentPage = paginationParams.PageNumber,
                        PageSize = paginationParams.PageSize,
                        TotalCount = totalCount,
                        TotalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize),
                        Items = results.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                      .Take(paginationParams.PageSize)
                    };
                }
                else
                {
                    // Use the search method with custom filter
                    return await SearchAsync(
                        searchTerm,
                        paginationParams,
                        searchableFields,
                        false);
                }
            }
            else
            {
                // No program ID filter, use standard search
                return await SearchAsync(
                    searchTerm,
                    paginationParams,
                    searchableFields,
                    false);
            }
        }

        /// <summary>
        /// Deletes all Subject Combos for a specific program
        /// </summary>
        public async Task DeleteByProgramIdAsync(Guid programId)
        {
            var filter = Builders<SubjectCombo>.Filter.Eq(c => c.ProgramId, programId) & NotDeletedFilter;
            var update = Builders<SubjectCombo>.Update
                .Set(c => c.DelFlg, true)
                .Set(c => c.UpdDate, DateTime.UtcNow);

            await _collection.UpdateManyAsync(filter, update);
        }
    }
}
