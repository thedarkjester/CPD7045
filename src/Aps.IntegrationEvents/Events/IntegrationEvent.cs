using System;
using Seterlund.CodeGuard;

namespace Aps.Integration.Events
{
    public class IntegrationEvent
    {
        private int rowVersion;
        private readonly string nameSpaceName;
        private readonly byte[] serializedEvent;
        DateTime occuredDate;

        public byte[] SerializedEvent
        {
            get { return serializedEvent; }
        }

        public string NameSpaceName
        {
            get { return nameSpaceName; }
        }

        public int RowVersion
        {
            get { return rowVersion; }
        }

        public DateTime OccuredDate {
            get
            {
                return occuredDate;
            }
        }

        private IntegrationEvent()
        {

        }

        public IntegrationEvent(string nameSpaceName, byte[] serializedEvent)
        {
            Guard.That(nameSpaceName).IsNotEmpty();
            Guard.That(serializedEvent).IsNotNull();
            Guard.That(serializedEvent).IsTrue(bytes => bytes.Length > 0,"Length must be > 0");

            this.nameSpaceName = nameSpaceName;
            this.serializedEvent = serializedEvent;
            occuredDate = DateTime.UtcNow;
        }

   

        public void SetRowVersion(int version)
        {
            this.rowVersion = version;
        }
    }
}