using WebApp.Models.ViewModels;

namespace UnitTests.ViewModels
{
    public class PaginatedListTests
    {
        [Fact]
        public void CreateFromList_WithValidParameters_ReturnsCorrectPaginatedList()
        {
            // Arrange
            var sourceList = CreateTestItems(25);
            int pageIndex = 2;
            int pageSize = 10;

            // Act
            var result = PaginatedList<string>.CreateFromList(sourceList, pageIndex, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(3, result.TotalPages); // 25 items / 10 per page = 3 pages
            Assert.Equal(10, result.Items.Count); // Page 2 should have 10 items
            Assert.True(result.HasPreviousPage);
            Assert.True(result.HasNextPage);
        }

        [Fact]
        public void CreateFromList_FirstPage_HasNoPreviousPage()
        {
            // Arrange
            var sourceList = CreateTestItems(15);
            int pageIndex = 1;
            int pageSize = 10;

            // Act
            var result = PaginatedList<string>.CreateFromList(sourceList, pageIndex, pageSize);

            // Assert
            Assert.False(result.HasPreviousPage);
            Assert.True(result.HasNextPage);
            Assert.Equal(10, result.Items.Count);
        }

        [Fact]
        public void CreateFromList_LastPage_HasNoNextPage()
        {
            // Arrange
            var sourceList = CreateTestItems(15);
            int pageIndex = 2;
            int pageSize = 10;

            // Act
            var result = PaginatedList<string>.CreateFromList(sourceList, pageIndex, pageSize);

            // Assert
            Assert.True(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
            Assert.Equal(5, result.Items.Count); // Last page has 5 remaining items
        }

        [Fact]
        public void CreateFromList_SinglePage_HasNeitherPreviousNorNext()
        {
            // Arrange
            var sourceList = CreateTestItems(5);
            int pageIndex = 1;
            int pageSize = 10;

            // Act
            var result = PaginatedList<string>.CreateFromList(sourceList, pageIndex, pageSize);

            // Assert
            Assert.False(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
            Assert.Equal(5, result.Items.Count);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public void CreateFromList_EmptyList_ReturnsEmptyPaginatedList()
        {
            // Arrange
            var emptyList = new List<string>();
            int pageIndex = 1;
            int pageSize = 10;

            // Act
            var result = PaginatedList<string>.CreateFromList(emptyList, pageIndex, pageSize);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalPages);
            Assert.False(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }

        [Fact]
        public void CreateFromList_PageIndexBeyondRange_ReturnsEmptyPage()
        {
            // Arrange
            var sourceList = CreateTestItems(10);
            int pageIndex = 5; // Beyond available pages
            int pageSize = 10;

            // Act
            var result = PaginatedList<string>.CreateFromList(sourceList, pageIndex, pageSize);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public void CreateFromList_CorrectItemsOnEachPage()
        {
            // Arrange
            var sourceList = new List<string> { "Item1", "Item2", "Item3", "Item4", "Item5" };
            int pageSize = 2;

            // Act - Page 1
            var page1 = PaginatedList<string>.CreateFromList(sourceList, 1, pageSize);
            
            // Act - Page 2
            var page2 = PaginatedList<string>.CreateFromList(sourceList, 2, pageSize);
            
            // Act - Page 3
            var page3 = PaginatedList<string>.CreateFromList(sourceList, 3, pageSize);

            // Assert
            Assert.Equal(new[] { "Item1", "Item2" }, page1.Items);
            Assert.Equal(new[] { "Item3", "Item4" }, page2.Items);
            Assert.Equal(new[] { "Item5" }, page3.Items);
        }

        [Theory]
        [InlineData(10, 3, 4)] // 10 items, 3 per page = 4 pages
        [InlineData(15, 5, 3)] // 15 items, 5 per page = 3 pages
        [InlineData(20, 7, 3)] // 20 items, 7 per page = 3 pages
        [InlineData(1, 10, 1)] // 1 item, 10 per page = 1 page
        public void CreateFromList_TotalPagesCalculation_IsCorrect(int itemCount, int pageSize, int expectedTotalPages)
        {
            // Arrange
            var sourceList = CreateTestItems(itemCount);

            // Act
            var result = PaginatedList<string>.CreateFromList(sourceList, 1, pageSize);

            // Assert
            Assert.Equal(expectedTotalPages, result.TotalPages);
        }

        private List<string> CreateTestItems(int count)
        {
            return Enumerable.Range(1, count)
                           .Select(i => $"Item{i}")
                           .ToList();
        }
    }
}