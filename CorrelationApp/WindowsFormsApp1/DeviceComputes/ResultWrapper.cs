﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DeviceComputes
{
    public abstract class ResultWrapper
    {
        private readonly string _outputFolder;
        private readonly string _prevName;

        public ResultWrapper(string outputFolder, string prevName)
        {
            this._outputFolder = outputFolder;
            _prevName = prevName;

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
        }

        public async void WriteMatrixesToFile(List<double[,]> matrixes, int batchSize, int shiftStep)
        {
            string filename = $"{_outputFolder}//{shiftStep}_{batchSize}_{_prevName}";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var matrix in matrixes)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    for (int i = 0; i < matrix.GetLength(1); i++)
                    {
                        if (i != 0)
                        {
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.Append(matrix[i, j].ToString());
                    }

                    stringBuilder.Append("\n");
                }

                stringBuilder.Append("\n\r\n\r");
            }

            Task.Run(() => { File.WriteAllText(filename, stringBuilder.ToString()); });
        }
    }
}