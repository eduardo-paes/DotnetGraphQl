using Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Seeds
{
    public static class CampusSeeder
    {
        public static void Seed(MigrationBuilder builder)
        {
            string campusesFile = Path.Combine(AppContext.BaseDirectory, "Seeds", "Data", "campuses.txt");
            foreach (string campusName in File.ReadAllLines(campusesFile))
            {
                // Save campus
                AddCampus(builder, new Campus(Guid.NewGuid(), campusName.Trim()));
            }
        }

        private static void AddCampus(MigrationBuilder builder, Campus m)
        {
            builder.InsertData(
                table: "Campuses",
                columns: new[] { "Id", "DeletedAt", "Name" },
                values: new object[,]
                {
                    { m.Id, m.DeletedAt, m.Name },
                },
                schema: "public");
        }
    }
}