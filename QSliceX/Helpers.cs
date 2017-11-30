using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QSliceX {
    static class Helpers {
        public static void Save<T>(Stream stream, T obj) where T : class {
            var writer = new XmlSerializer(typeof(T));
            writer.Serialize(stream, obj);
        }

        public static T Load<T>(Stream stream) where T : class {
            var reader = new XmlSerializer(typeof(T));
            return reader.Deserialize(stream) as T;
        }
    }
}
