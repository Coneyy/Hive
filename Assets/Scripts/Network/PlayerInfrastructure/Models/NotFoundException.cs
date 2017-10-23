using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Network.PlayerInfrastructure.Models
{
    class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException()
        {

        }
    }

    class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }

        public AlreadyExistsException()
        {

        }
    }
}
