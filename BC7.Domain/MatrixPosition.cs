using System;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class MatrixPosition
    {
        public Guid Id { get; private set; }
        public Guid? UserMultiAccountId { get; private set; }
        public Guid? ParentId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public int MatrixLevel { get; private set; }
        public int Left { get; set; } // this public set is only for Update TODO later think how to replace public set for updates
        public int Right { get; set; } // this public set is only for Update
        public int DepthLevel { get; private set; }

        /// <summary>
        /// This ctor is only for Update Left and Right values
        /// </summary>
        public MatrixPosition()
        {
        }
        
        public MatrixPosition(Guid id, Guid? userMultiAccountId, Guid? parentId, int matrixLevel, int left, int right, int depthLevel)
        {
            ValidateDomain(id, matrixLevel, left, right, depthLevel);

            Id = id;
            UserMultiAccountId = userMultiAccountId;
            ParentId = parentId;
            MatrixLevel = matrixLevel;
            Left = left;
            Right = right;
            DepthLevel = depthLevel;
            CreatedAt = DateTime.UtcNow;
        }

        public void AssignMultiAccount(Guid userMultiAccountId)
        {
            if (userMultiAccountId == Guid.Empty)
            {
                throw new DomainException($"Invalid {nameof(userMultiAccountId)}.");
            }

            UserMultiAccountId = userMultiAccountId;
        }

        private static void ValidateDomain(Guid id, int matrixLevel, int left, int right, int depthLevel)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException($"Invalid {nameof(id)}.");
            }
            if (matrixLevel < 0)
            {
                throw new DomainException($"Invalid {nameof(matrixLevel)}.");
            }
            if (left < 1)
            {
                throw new DomainException($"Invalid {nameof(left)}.");
            }
            if (left >= right)
            {
                throw new DomainException("Left value cannot be equal or greater than right value");
            }
            if (depthLevel < 0)
            {
                throw new DomainException($"Invalid {nameof(depthLevel)}.");
            }
        }
    }
}
