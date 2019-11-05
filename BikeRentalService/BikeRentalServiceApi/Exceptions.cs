using System;

namespace BikeRentalServiceApi
{
    public class Exceptions
    {
        public class CustomerNotExistingException : Exception
        {
            public CustomerNotExistingException(string message) : base(message) { }

            public CustomerNotExistingException(string message, Exception innerException) : base(message, innerException) { }

            public CustomerNotExistingException() { }
        }
        public class BikeNotExistingException : Exception
        {
            public BikeNotExistingException(string message) : base(message) { }

            public BikeNotExistingException(string message, Exception innerException) : base(message, innerException) { }

            public BikeNotExistingException() { }
        }
        public class RentalNotExistingException : Exception
        {
            public RentalNotExistingException(string message) : base(message) { }

            public RentalNotExistingException(string message, Exception innerException) : base(message, innerException) { }

            public RentalNotExistingException() { }
        }
        public class RentalAlreadyEndedException : Exception
        {
            public RentalAlreadyEndedException(string message) : base(message) { }

            public RentalAlreadyEndedException(string message, Exception innerException) : base(message, innerException) { }

            public RentalAlreadyEndedException() { }
        }
        public class RentalNotEndedException : Exception
        {
            public RentalNotEndedException(string message) : base(message) { }

            public RentalNotEndedException(string message, Exception innerException) : base(message, innerException) { }

            public RentalNotEndedException() { }
        }
        public class TotalAmountNotGreaterThanZeroException : Exception
        {
            public TotalAmountNotGreaterThanZeroException(string message) : base(message) { }

            public TotalAmountNotGreaterThanZeroException(string message, Exception innerException) : base(message, innerException) { }

            public TotalAmountNotGreaterThanZeroException() { }
        }

        public class BikeInRentalException : Exception
        {
            public BikeInRentalException(string message) : base(message) { }

            public BikeInRentalException(string message, Exception innerException) : base(message, innerException) { }

            public BikeInRentalException() { }
        }

        public class BikeAlreadyInRentalException : Exception
        {
            public BikeAlreadyInRentalException(string message) : base(message) { }

            public BikeAlreadyInRentalException(string message, Exception innerException) : base(message, innerException) { }

            public BikeAlreadyInRentalException() { }
        }
        public class CustomerAlreadyInRentalException : Exception
        {
            public CustomerAlreadyInRentalException(string message) : base(message) { }

            public CustomerAlreadyInRentalException(string message, Exception innerException) : base(message, innerException) { }

            public CustomerAlreadyInRentalException() { }
        }



    }
}
