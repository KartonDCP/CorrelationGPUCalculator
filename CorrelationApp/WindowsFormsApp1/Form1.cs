﻿using System;using System.Collections.Generic;using System.ComponentModel;using System.Data;using System.Drawing;using System.IO;using System.Linq;using System.Text;using System.Threading.Tasks;using System.Windows.Forms;using WindowsFormsApp1.Math.Statistics;using WindowsFormsApp1.Utils;using MathNet.Numerics.Statistics;namespace WindowsFormsApp1{    public partial class Form1 : Form    {        private List<double[]> _fileArrays;        private volatile bool isRedAlready = false;        public Form1()        {            InitializeComponent();            _fileArrays = new List<double[]>();        }        private void ButtonLoadClick(object sender, EventArgs e)        {            _fileArrays = new List<double[]>();            isRedAlready = false;            using (var ofd = new System.Windows.Forms.OpenFileDialog())            {                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)                {                    TryInit(ofd.FileName);                }                else                {                    MessageBox.Show("Не выбран файл!");                }            }        }        private void TryInit(string filePath)        {            Task.Run(() =>            {                var lines = File.ReadLines(filePath).Skip(4);                char fileSeparator = GetCurrentSeparator(lines.First());                InitArrays(lines.Count(), lines.First().Split(fileSeparator).Length);                VolatileRead(lines, fileSeparator);                isRedAlready = true;            });        }        void InitArrays(int rowLength, int rows)        {            for (int i = 0; i < rows; i++)            {                _fileArrays.Add(new double[rowLength]);            }        }        private char GetCurrentSeparator(string line1)        {            foreach (var sep in " \t,")            {                if (line1.Contains(sep) && line1.Split(sep).Length > 1)                {                    return sep;                }            }            throw new FileLoadException("Bad file format!");        }        private void VolatileRead(IEnumerable<string> lines, char separator)        {            for (int i = 0; i < lines.Count(); i++)            {                var splitted = lines.ElementAt(i).Split(separator);                for (int j = 0; j < splitted.Length; j++)                {                    _fileArrays.ElementAt(j)[i] = Double.Parse(splitted[j]);                }            }        }        public static double[] Rank(IEnumerable<double> series) => series == null ? new double[0] : ArrayStatistics.RanksInplace(series.ToArray<double>());                private void CalculateClick(object sender, EventArgs e)        {            List<double[]> listOfRanks = new List<double[]>();            foreach (var signal in _fileArrays)            {                listOfRanks.Add(Rank(signal));            }            double [,] correlationMatrix = new double[listOfRanks.Count, listOfRanks.Count];            for (int i = 0; i < listOfRanks.Count; i++)            {                for (int j = 0; j < listOfRanks.Count; j++)                {                    correlationMatrix[i, j] = Correlations.Spearmanr(listOfRanks[i], listOfRanks[j]);                }            }            for (int i = 0; i < listOfRanks.Count; i++)            {                for (int j = 0; j < listOfRanks.Count; j++)                {                    Console.Write(correlationMatrix[i, j] + "\t\t\t");                }                Console.WriteLine();            }        }    }}