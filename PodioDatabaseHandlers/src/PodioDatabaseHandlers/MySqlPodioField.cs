using System;
namespace BrickBridge.Lambda.MySql
{
    public class MySqlPodioField
    {
        public int PodioFieldId { get; set; }
        public string ExternalId { get; set; }
        public string Type { get; set; }
		public string Name { get; set; }
    }
}
