using SerializationExample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionDecrpytion
{
    class Program
    {
        /*
         * Secret key string.
         */
         static string secretKey = "password";
        /*
        * Byte array key. For encryption and decryption purpose. 
        */
        readonly static byte[] Key = Encoding.UTF8.GetBytes(secretKey);
        /*
        * Byte array iv. For encryption and decryption purpose. 
        */
        readonly static byte[] IV = Encoding.UTF8.GetBytes(secretKey);

        /*
         * Our dataObject holder.
         */
        static DataObjectSerializable dataObject;
        /*
         * DES Crypto Service Provider.
         */
        static DESCryptoServiceProvider des;
        /*
         * 
         */
        static ICryptoTransform desencrypt;

        static void Main(string[] args)
        {

            //File path
            string filename = "D:\\SelfLearning\\c#\\Serialization\\file.dat";

            //Create data object.
            dataObject = new DataObjectSerializable();
            //Set the data object data.
            dataObject.Name = "http://ubuntuanakramli.blogspot.com/";
            dataObject.Age = 2015 - 2011;

            //encrypt and serialize.
            encryptNserialize(filename);

            //decrpyt and deserialize , store result to dataObject.
            dataObject = decryptNdeserialize(filename);

            //print out the result.
            Console.WriteLine("Name" + dataObject.Name);
            Console.WriteLine("Age"  + dataObject.Age);

            Console.ReadKey();
        }

        /*
         * Encrypt and Serialize method.
         */
        public static void encryptNserialize(string filename)
        {
            //Instantiate a memory stream object.
            Stream serializedStream = new MemoryStream();
            //Instantiate binary formatter object.
            IFormatter formatterEn = new BinaryFormatter();
            //First serialize our data object to memory stream.
            formatterEn.Serialize(serializedStream, dataObject);

            //reset back out stream to Position 0. this is due to the serialization process, the stream data position has reach
            //the last position. this is important else we might face the  Exception as 'Binary stream '0' does not contain a valid BinaryHeader.
            serializedStream.Seek(0, SeekOrigin.Begin);

            //instantiate a file stream object.
            FileStream fsWrite = new FileStream(filename,
                               FileMode.Create,
                               FileAccess.Write);
            //create a byte array with the lenght of our serialized stream.
            byte [] byteArray = new byte[serializedStream.Length];

            //read the serialized stream and store its byte array value to our byteArray variable.
            serializedStream.Read(byteArray, 0, byteArray.Length);
  
            //instantiate a des descrytor service provider type object.
            des = new DESCryptoServiceProvider();
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.CBC;
            des.Key = Key;
            des.IV = IV;

            //create the Encryptor.
            desencrypt = des.CreateEncryptor();

            //instantiate a crypto stream object, construct with our file stream, encryptor. note that the mode we use is Write.
            CryptoStream cryptStream = new CryptoStream(fsWrite,
            desencrypt,
            CryptoStreamMode.Write);

            //write the byteArray to our filestream via crpytStream.
            cryptStream.Write(byteArray, 0, byteArray.Length);

            //flush and close our streams.
            cryptStream.FlushFinalBlock();
            cryptStream.Close();
            serializedStream.Flush();
            serializedStream.Close();
            fsWrite.Close();
        }

       /*
       * Decrypt and deserialize function.
       */
        public static DataObjectSerializable decryptNdeserialize(string filename)
        {
            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(filename,
                                           FileMode.Open,
                                           FileAccess.Read);

            //create des crypto service provider object.
            des = new DESCryptoServiceProvider();
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.CBC;
            des.Key = Key;
            des.IV = IV;

            //create Descryptor.
            desencrypt = des.CreateDecryptor();
            //Construct the cryptostream with filestream that we use to store data that we read from a file.
            CryptoStream cryptStream = new CryptoStream(fsread,
            desencrypt,
            CryptoStreamMode.Read);

            //create byte array object with length of our filestream.
            byte[] byteArray = new byte[fsread.Length];

            //store the byte in cryptStream to our byteArray.
            cryptStream.Read(byteArray, 0, byteArray.Length);
            
            //create a new memory stream.
            MemoryStream memoryStream = new MemoryStream();

            //write the byteArray data to memory stream.
            memoryStream.Write(byteArray, 0, byteArray.Length);
    
            //create formatter.
            IFormatter formatter = new BinaryFormatter();

            //reposition our memory stream to position 0.
            memoryStream.Seek(0, SeekOrigin.Begin);

            //assign the deserializated object to our data object.
            DataObjectSerializable dataObject = (DataObjectSerializable)formatter.Deserialize(memoryStream);
            // flush and close all stream.
            cryptStream.Flush();
            cryptStream.Close();
            fsread.Close();
            memoryStream.Flush();
            memoryStream.Close();

            return dataObject;
        }
    }
}
