using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatGetProductSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetProducts 
                    @PageIndex int,
                    @PageSize int, 
                    @OrderBy nvarchar(50),
                    @Title nvarchar(max) = '%',
                    @Quantity int = NULL,
                    @MinLevel int = NULL,
                    @Tag nvarchar(50) = NULL,
                    @PriceFrom decimal(18, 2) = NULL,
                    @PriceTo decimal(18, 2) = NULL,
                    @DateFrom datetime = NULL,
                    @DateTo datetime = NULL,
                    @Total int output,
                    @TotalDisplay int output
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Collecting Total Count
                    SELECT @Total = COUNT(*) FROM Products;

                    -- Dynamic SQL for filtering and counting
                    DECLARE @sql nvarchar(MAX);
                    DECLARE @countsql nvarchar(MAX);
                    DECLARE @paramList nvarchar(MAX);

                    SET @countsql = '
                        SELECT @TotalDisplay = COUNT(*) 
                        FROM Products i 
                        WHERE 1 = 1';

                    -- Filters for counting
                    IF @Title IS NOT NULL
                        SET @countsql = @countsql + ' AND i.Title LIKE ''%'' + @xTitle + ''%'' ';

                    IF @Quantity IS NOT NULL
                        SET @countsql = @countsql + ' AND i.Quantity = @xQuantity';

                    IF @MinLevel IS NOT NULL
                        SET @countsql = @countsql + ' AND i.MinLevel >= @xMinLevel';

                    IF @Tag IS NOT NULL
                        SET @countsql = @countsql + ' AND i.Tag LIKE ''%'' + @xTag + ''%'' ';

                    IF @PriceFrom IS NOT NULL
                        SET @countsql = @countsql + ' AND i.Price >= @xPriceFrom';

                    IF @PriceTo IS NOT NULL
                        SET @countsql = @countsql + ' AND i.Price <= @xPriceTo';

                    IF @DateFrom IS NOT NULL
                        SET @countsql = @countsql + ' AND i.InsertedDate >= @xDateFrom';

                    IF @DateTo IS NOT NULL
                        SET @countsql = @countsql + ' AND i.InsertedDate <= @xDateTo';

                    -- Execute count query
                    SET @paramList = '@xTitle nvarchar(max), 
                                        @xQuantity int, 
                                        @xMinLevel int, 
                                        @xTag nvarchar(50), 
                                        @xPriceFrom decimal(18, 2), 
                                        @xPriceTo decimal(18, 2), 
                                        @xDateFrom datetime, 
                                        @xDateTo datetime,
                                        @TotalDisplay int output';

                    EXEC sp_executesql @countsql, @paramList, 
                        @xTitle = @Title, 
                        @xQuantity = @Quantity, 
                        @xMinLevel = @MinLevel, 
                        @xTag = @Tag, 
                        @xPriceFrom = @PriceFrom, 
                        @xPriceTo = @PriceTo, 
                        @xDateFrom = @DateFrom, 
                        @xDateTo = @DateTo, 
                        @TotalDisplay = @TotalDisplay OUTPUT;

                    -- Dynamic SQL for paginated data retrieval
                    SET @sql = '
                        SELECT * 
                        FROM Products i
                        WHERE 1 = 1 ';

                    -- Filters for data
                    IF @Title IS NOT NULL
                        SET @sql = @sql + ' AND i.Title LIKE ''%'' + @xTitle + ''%'' ';

                    IF @Quantity IS NOT NULL
                        SET @sql = @sql + ' AND i.Quantity = @xQuantity';

                    IF @MinLevel IS NOT NULL
                        SET @sql = @sql + ' AND i.MinLevel >= @xMinLevel';

                    IF @Tag IS NOT NULL
                        SET @sql = @sql + ' AND i.Tag LIKE ''%'' + @xTag + ''%'' ';

                    IF @PriceFrom IS NOT NULL
                        SET @sql = @sql + ' AND i.Price >= @xPriceFrom';

                    IF @PriceTo IS NOT NULL
                        SET @sql = @sql + ' AND i.Price <= @xPriceTo';

                    IF @DateFrom IS NOT NULL
                        SET @sql = @sql + ' AND i.InsertedDate >= @xDateFrom';

                    IF @DateTo IS NOT NULL
                        SET @sql = @sql + ' AND i.InsertedDate <= @xDateTo';

                    -- Pagination and ordering
                    SET @sql = @sql + ' ORDER BY ' + QUOTENAME(@OrderBy) + ' 
                                        OFFSET @PageSize * (@PageIndex - 1) ROWS 
                                        FETCH NEXT @PageSize ROWS ONLY';

                    -- Execute paginated data query
                    SET @paramList = '@xTitle nvarchar(max),
                                        @xQuantity int, 
                                        @xMinLevel int, 
                                        @xTag nvarchar(50), 
                                        @xPriceFrom decimal(18, 2), 
                                        @xPriceTo decimal(18, 2), 
                                        @xDateFrom datetime, 
                                        @xDateTo datetime, 
                                        @PageIndex int, 
                                        @PageSize int';

                    EXEC sp_executesql @sql, @paramList, 
                        @xTitle = @Title, 
                        @xQuantity = @Quantity, 
                        @xMinLevel = @MinLevel, 
                        @xTag = @Tag, 
                        @xPriceFrom = @PriceFrom, 
                        @xPriceTo = @PriceTo, 
                        @xDateFrom = @DateFrom, 
                        @xDateTo = @DateTo, 
                        @PageIndex = @PageIndex, 
                        @PageSize = @PageSize;
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.DropTable(sql);
        }
    }
}
