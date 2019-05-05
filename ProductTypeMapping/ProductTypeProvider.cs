using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.ProductTypeMapping
{
    public class ProductTypeProvider
    {
        private const string DefaultType = "Default good";
        private static Dictionary<string, string> _productTypeMapping;
        static ProductTypeProvider() {
            _productTypeMapping = new Dictionary<string, string>
            {
                { "Pampers", "Child" },
                { "Lipstic", "Cosmetics" },
                { "Bread", "Food" }
            };
        }

        public static string GetProductType(string key) {
            if (_productTypeMapping.ContainsKey(key)) {
                return _productTypeMapping[key];
            }
            return DefaultType;
        }
    }
}
