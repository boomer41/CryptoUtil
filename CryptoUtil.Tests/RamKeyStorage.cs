using System.Collections.Generic;

namespace CryptoUtil.Tests
{

    internal class RamKeyStorage : IKeyStorage
    {

        private readonly Dictionary<string, string> _map = new Dictionary<string, string>();

        public string RetrieveKey(string keyId)
        {
            lock (_map)
            {
                return _map.TryGetValue(keyId, out var key) ? key : null;
            }
        }

        public void SaveKey(string keyId, string keyData)
        {
            lock (_map)
            {
                _map[keyId] = keyData;
            }
        }

    }

}