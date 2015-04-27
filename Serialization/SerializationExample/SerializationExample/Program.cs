using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SerializationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //File path
            string filename = "D:\\SelfLearning\\c#\\Serialization\\file.dat";
            
            //Create a file stream and file.
            Stream fileStream = File.Open(filename, FileMode.Create);
            
            //Create data object.
            DataObjectSerializable dataObject = new DataObjectSerializable();
            //Set the data object data.
            dataObject.Name = "http://ubuntuanakramli.blogspot.com/";
            dataObject.Age = 2015 - 2011;
            
            //Serialize the object
            IFormatter form = new BinaryFormatter();
            form.Serialize(fileStream, dataObject);
            fileStream.Close();
        }


    }
}
