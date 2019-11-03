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



    }
}
