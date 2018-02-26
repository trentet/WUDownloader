using System;
using System.Collections.Generic;


namespace WUDownloader
{
    class Schema
    {
        private string tableName;
        private Dictionary<string, Type> dictionary = new Dictionary<string, Type>();

        public string TableName { get => tableName; set => tableName = value; }
        public Dictionary<string, Type> Dictionary { get => dictionary; set => dictionary = value; }
    }
}
