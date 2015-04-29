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

            //Now we are going to deserialized of file stream and write the output out.
            Stream fileRead = File.Open(filename, FileMode.Open);

            //deserialize the file stream and reassigned the return object to our dataObject holder.
            dataObject = (DataObjectSerializable)form.Deserialize(fileRead);
            //print out the value to see whether its the same with our first assigned value.
            Console.WriteLine("Name :" + dataObject.Name);
            Console.WriteLine("Age :" + dataObject.Age);
            //to pause our console from exit. request the console to read for key input.
            Console.ReadKey();
        }


    }
}
