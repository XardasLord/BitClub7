﻿using System;

namespace BC7.Business.Models
{
    public class MatrixPositionModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? UserMultiAccountId { get; set; }
        public string MultiAccountName { get; set; }
        public int MatrixLevel { get; set; }
        public int DepthLevel { get; set; }
    }
}
