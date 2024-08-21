﻿namespace XuongMay.ModelViews.ProductModelViews
{
    public class UpdateProductModelView
    {
        public required string ProductName { get; set; }
        public required string ProductSize { get; set; }
        public required string Status { get; set; }
        public required Guid CategoryId { get; set; }  // Changed to Guid
    }
}
