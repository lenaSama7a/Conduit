using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Conduit.Db.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Bio", "Email", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, null, "lenasama7a@gmail.com", "13579L$l", "lenaSamaha" },
                    { 2, null, "saulBellow@gmail.com", "246810Saul%", "saulBellow" },
                    { 3, "I'm From Italy", "SidneySheldon@gmail.com", "Sidney1&", "SidneySheldon" },
                    { 4, null, "ErnestHemingway@gmail.com", "12246E-r", "ErnestHemingway" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "Tag", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "The most visited urban park in the United States.", "#central_Park", "Central Park", 1 },
                    { 2, "A 102-story skyscraper located in Midtown Manhattan.", "#Empire_State_Building", "Empire State Building", 1 },
                    { 3, "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", "#Cathedral", "Cathedral", 2 },
                    { 4, "The the finest example of railway architecture in Belgium.", "#Central_Station", "Antwerp Central Station", 2 },
                    { 5, "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.", "#Eiffel_Tower", "Eiffel Tower", 3 },
                    { 6, "The world's largest museum.", "#Louvre", "The Louvre", 4 }
                });

            migrationBuilder.InsertData(
                table: "Follows",
                columns: new[] { "FolloweeId", "FollowerId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "Body", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "This is beautiful article", 1 },
                    { 2, 3, "Amazing!", 2 },
                    { 3, 5, "Eiffel Tower is the best", 3 },
                    { 4, 6, "I love this museum", 4 }
                });

            migrationBuilder.InsertData(
                table: "FavoriteArticles",
                columns: new[] { "ArticleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 1 },
                    { 1, 3 },
                    { 2, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FavoriteArticles",
                keyColumns: new[] { "ArticleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "FavoriteArticles",
                keyColumns: new[] { "ArticleId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "FavoriteArticles",
                keyColumns: new[] { "ArticleId", "UserId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "FavoriteArticles",
                keyColumns: new[] { "ArticleId", "UserId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FolloweeId", "FollowerId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FolloweeId", "FollowerId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FolloweeId", "FollowerId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FolloweeId", "FollowerId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FolloweeId", "FollowerId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
