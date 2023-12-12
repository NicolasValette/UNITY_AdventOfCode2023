using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdventOfCode.Utility
{
    public class FileReader
    {
        private string _fileName;
        StreamReader _stream;
        bool _verbose = false;

        public FileReader(string fileName, bool verbose = false)
        {
            _fileName = fileName;
            _verbose = verbose;
            string path = Path.Combine(Application.streamingAssetsPath, _fileName);
            if (_verbose) Debug.Log($"Reading file : {path}");
            _stream = new StreamReader(path);
        }
        public void Close()
        {
            _stream.Close();
        }
        public string Read()
        {
            return _stream.ReadLine();
        }
    }
}