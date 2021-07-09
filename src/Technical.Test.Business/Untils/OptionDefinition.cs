using MongoDB.Bson;
using MongoDB.Driver;

namespace Technical.Test.Business.Untils
{
    public class OptionDefinition
    {
        public int? Page { get; set; }
        public int? Limit { get; set; }
        public SortDefinition<BsonDocument> Sort { get; set; }
        public string Projections { get; set; }
        public bool IgnoreLimit { get; set; }

        public int? _limit { get { return IgnoreLimit ? null : Limit == null || Limit == 0 ? 1 : Limit > 20 ? 20 : Limit; } }
        public int? _skip { get { return IgnoreLimit ? null : Page == null || Page == 0 ? null : _limit * (Page - 1); } }

        public ProjectionDefinition<BsonDocument> _projections
        {
            get
            {
                ProjectionDefinition<BsonDocument> newProject = null;

                if (!string.IsNullOrEmpty(Projections))
                {
                    foreach (string projection in Projections.Split(","))
                    {
                        if (newProject == null)
                        {
                            newProject = Builders<BsonDocument>.Projection.Include(projection.Trim());
                        }
                        else
                        {
                            newProject = newProject.Include(projection.Trim());
                        }
                    }
                }

                return newProject;
            }
        }
    }
}
