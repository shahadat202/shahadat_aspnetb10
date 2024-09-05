using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Web.Migrations.BlogDb
{
    /// <inheritdoc />
    public partial class UpdateGetBlogPostSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetBlogPosts 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@Title nvarchar(max) = '%',
                	@PostDateFrom datetime = NULL,
                	@PostDateTo datetime = NULL,
                	@Body nvarchar(max) = '%',
                	@CategoryId uniqueidentifier = NULL,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @sql nvarchar(2000);
                	Declare @countsql nvarchar(2000);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	-- Collecting Total
                	Select @Total = count(*) from BlogPosts;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = count(*) from BlogPosts bp inner join 
                					Categories c on bp.CategoryId = c.Id where 1 = 1 ';

                	SET @countsql = @countsql + ' AND bp.Title LIKE ''%'' + @xTitle + ''%''' 

                	SET @countsql = @countsql + ' AND bp.Body LIKE ''%'' + @xBody + ''%''' 

                	IF @PostDateFrom IS NOT NULL
                	SET @countsql = @countsql + ' AND bp.PostDate >= @xPostDateFrom'

                	IF @PostDateTo IS NOT NULL
                	SET @countsql = @countsql + ' AND bp.PostDate <= @xPostDateTo' 

                	IF @CategoryId IS NOT NULL
                	SET @countsql = @countsql + ' AND bp.CategoryId = @xCategoryId' 

                	SELECT @countparamlist = '@xTitle nvarchar(max),
                		@xBody nvarchar(max),
                		@xPostDateFrom datetime,
                		@xPostDateTo datetime,
                		@xCategoryId uniqueidentifier,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Title,
                		@Body,
                		@PostDateFrom,
                		@PostDateTo,
                		@CategoryId,
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	SET @sql = 'select bp.Id, bp.Title, bp.Body, bp.PostDate, c.Name as CategoryName from BlogPosts bp inner join 
                					Categories c on bp.CategoryId = c.Id where 1 = 1 ';

                	SET @sql = @sql + ' AND bp.Title LIKE ''%'' + @xTitle + ''%''' 

                	SET @sql = @sql + ' AND bp.Body LIKE ''%'' + @xBody + ''%''' 

                	IF @PostDateFrom IS NOT NULL
                	SET @sql = @sql + ' AND bp.PostDate >= @xPostDateFrom'

                	IF @PostDateTo IS NOT NULL
                	SET @sql = @sql + ' AND bp.PostDate <= @xPostDateTo' 

                	IF @CategoryId IS NOT NULL
                	SET @sql = @sql + ' AND bp.CategoryId = @xCategoryId' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xTitle nvarchar(max),
                		@xBody nvarchar(max),
                		@xPostDateFrom datetime,
                		@xPostDateTo datetime,
                		@xCategoryId uniqueidentifier,
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@Title,
                		@Body,
                		@PostDateFrom,
                		@PostDateTo,
                		@CategoryId,
                		@PageIndex,
                		@PageSize;

                	print @sql;
                	print @countsql;

                END
                GO
                
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetBlogPosts]";
            migrationBuilder.DropTable(sql);
        }
    }
}
